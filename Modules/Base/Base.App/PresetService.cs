using System;
using System.Collections.Concurrent;
using Base.DAL;
using System.Linq;
using System.Threading.Tasks;
using Base.Events;
using Base.Extensions;
using Base.Security;
using Base.UI;
using Base.UI.Service;
using Base.Utils.Common.Caching;
using AppContext = Base.Ambient.AppContext;

namespace Base.App
{
    public class PresetService<T> : IPresetService<T>
        where T : Preset
    {
        private const string key_user_categories = "user_categories";

        private readonly IExtendedCacheWrapper _cacheWrapper;
        private readonly IPresetFactory<T> _presetFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        
        public PresetService(IExtendedCacheWrapper cacheWrapper, IPresetFactory<T> presetFactory, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _cacheWrapper = cacheWrapper;
            _presetFactory = presetFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public static CacheAccessor<T> PresetGroup = new CacheAccessor<T>();

        public void OnEvent(IChangeObjectEvent<IUserCategory> evnt)
        {
            if (evnt.Original == null) return;
            _cacheWrapper.TryRemove(PresetGroup, evnt.Original.ID.ToString());
        }

        public void OnEvent(IChangeObjectEvent<PresetRegistor> evnt)
        {
            //NOTE: если это глоб.пресет
            if (evnt.Modified.UserID == null)
            {
                _cacheWrapper.TryRemove(PresetGroup, key_user_categories);
            }
        }

        private string GetGlobalKey(string ownerName)
        {
            return $"{ownerName}:{typeof(T).FullName}";
        }

        private string GetUserKey(string ownerName)
        {
            return $"{GetGlobalKey(ownerName)}:user-{AppContext.SecurityUser.ID}";
        }

        private string GetUserCatKey(string ownerName)
        {
            return $"{GetGlobalKey(ownerName)}:cat-{AppContext.SecurityUser.CategoryInfo.ID}";
        }

        private string GetDefaultKey(string ownerName)
        {
            return $"{GetGlobalKey(ownerName)}:default";
        }

        public virtual async Task<T> GetAsync(string ownerName)
        {

            if (string.IsNullOrEmpty(ownerName))
                throw new ArgumentNullException(ownerName);

            _cacheWrapper.GetOrAdd(PresetGroup, null, () => null);

            T preset;

            string userkey = GetUserKey(ownerName);

            if (_cacheWrapper.TryGet(PresetGroup, userkey, out preset))
                if (preset != null) return preset;

            preset = await GetUserPresetAsync(ownerName);

            if (preset != null)
            {
                _cacheWrapper.AddOrUpdate(PresetGroup, userkey, preset, PresetGroup.GetDependencyKey(null));
                return preset;
            }

            string usercatkey = GetUserCatKey(ownerName);
            string catId = AppContext.SecurityUser.CategoryInfo.ID.ToString();

            _cacheWrapper.GetOrAdd(PresetGroup, key_user_categories, () => null, PresetGroup.GetDependencyKey(null));

            _cacheWrapper.GetOrAdd(PresetGroup, catId, () => null, PresetGroup.GetDependencyKey(key_user_categories));

            preset = await _cacheWrapper.GetOrAddAsync(PresetGroup, usercatkey,
                () => GetUserCategoryPresetAsync(ownerName), PresetGroup.GetDependencyKey(catId));

            if (preset != null)
                return preset;

            return GetDefaultPreset(ownerName);
        }

        private async Task<PresetRegistor> GetUserPresetRegistorAsync(IUnitOfWork uofw, string ownerName)
        {
            var presetdb = await uofw.GetRepository<PresetRegistor>().All()
               .Where(x => x.UserID == AppContext.SecurityUser.ID).Where(typeof(T), ownerName)
               .FirstOrDefaultAsync();

            return presetdb;
        }

        private async Task<T> GetUserPresetAsync(string ownerName)
        {
            using (var uofw = _unitOfWorkFactory.CreateSystem())
            {
                var presetdb = await GetUserPresetRegistorAsync(uofw, ownerName);

                return (T)presetdb?.Preset;
            }
        }

        public async Task<T> GetUserCategoryPresetAsync(string ownerName)
        {
            using (var uofw = _unitOfWorkFactory.CreateSystem())
            {
                var repository = uofw.GetRepository<UserCategory>();
                var userCategory = repository.Find(AppContext.SecurityUser.CategoryInfo.ID);

                if (userCategory != null)
                {
                    var preset = (await userCategory.Presets
                        .Where(x => x.ObjectID != null)
                        .Select(x => x.Object).AsQueryable()
                        .Where(typeof(T), ownerName)
                        .FirstOrDefaultAsync())?.Preset;

                    if (preset == null)
                    {
                        if (userCategory.sys_all_parents != null)
                        {
                            int[] parents = userCategory.sys_all_parents
                                .Split(HCategory.Seperator).Select(HCategory.IdToInt).ToArray();

                            string type = typeof(T).GetTypeName();

                            preset = (await repository.All()
                                .Where(x => parents.Contains(x.ID) && x.Presets.Any(p => p.Object.Type == type && p.Object.For == ownerName))
                                .ToListAsync())
                                .OrderByDescending(x => x.Level)
                                    .SelectMany(x => x.Presets)
                                    .Select(x => x.Object)
                                    .FirstOrDefault()?.Preset;
                        }
                    }

                    return (T)preset;
                }
            }

            return null;
        }

        public async Task<T> SaveAsync(T preset)
        {
            using (var uofw = _unitOfWorkFactory.CreateSystem())
            {
                var repository = uofw.GetRepository<PresetRegistor>();

                var presetdb = await GetUserPresetRegistorAsync(uofw, preset.For) ?? new PresetRegistor()
                {
                    Type = typeof(T).GetTypeName(),
                    For = preset.For,
                    UserID = AppContext.SecurityUser.ID,
                    UserLogin = AppContext.SecurityUser.Login
                };

                presetdb.Preset = preset;

                if (presetdb.ID == 0)
                    repository.Create(presetdb);
                else
                    repository.Update(presetdb);


                await uofw.SaveChangesAsync();

                _cacheWrapper.AddOrUpdate(PresetGroup, GetUserKey(preset.For), preset);

                return preset;
            }
        }

        public async Task DeleteAsync(Preset preset)
        {
            using (var uofw = _unitOfWorkFactory.CreateSystem())
            {
                var presetdb = await GetUserPresetRegistorAsync(uofw, preset.For);

                if (presetdb != null)
                {
                    if (presetdb.UserID != AppContext.SecurityUser.ID)
                        throw new AccessDeniedException();


                    uofw.GetRepository<PresetRegistor>().Delete(presetdb);
                    await uofw.SaveChangesAsync();

                    _cacheWrapper.TryRemove(PresetGroup, GetUserKey(preset.For));
                }
            }
        }

        private readonly ConcurrentDictionary<string, T> _defaultPresets = new ConcurrentDictionary<string, T>();

        public T GetDefaultPreset(string ownerName)
        {
            return _defaultPresets.GetOrAdd(ownerName, x =>
            {
                var preset = _presetFactory.Create(ownerName);

                preset.For = ownerName;

                return preset;
            });
        }

        public void DefaultPresetClear(string ownerName)
        {
            T preset;
            _defaultPresets.TryRemove(ownerName, out preset);
        }

        public void PresetClear()
        {
            //NOTE: очистка всего кэша
            _cacheWrapper.TryRemove(PresetGroup, null);
        }

        //sib

        private string GetUserKey(string ownerName, int userID)
        {
            return $"{GetGlobalKey(ownerName)}:user-{userID}";
        }

        public void Delete(PresetRegistor presetdb)
        {
            using (var uofw = _unitOfWorkFactory.CreateSystem())
            {
                var pr =  uofw.GetRepository<PresetRegistor>()
                    .Filter(f => f.ID == presetdb.ID)
                    .FirstOrDefault();
                uofw.GetRepository<PresetRegistor>().Delete(pr);
                uofw.SaveChanges();
                if (presetdb.UserID != null)
                    _cacheWrapper.TryRemove(PresetGroup, GetUserKey(presetdb.Preset.For, presetdb.UserID.Value));
            }
        }

        //end sib
    }

    public static class PresetServiceExtensions
    {
        public static IQueryable<PresetRegistor> Where(this IQueryable<PresetRegistor> q, Type presetType, string ownerName)
        {
            string type = presetType.GetTypeName();
            return q.Where(x => x.Type == type && x.For == ownerName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Base.Attributes;
using Base.DAL;
using Base.Entities.Complex;
using Base.Service;
using Base.Utils.Common.Caching;
using Base.Utils.Common.Maybe;

namespace Base.UI.Service
{
    public class UiEnumService : BaseObjectService<UiEnum>, IUiEnumService
    {
        private readonly ISimpleCacheWrapper _cacheWrapper;        

        public UiEnumService(IBaseObjectServiceFacade facade, ISimpleCacheWrapper cacheWrapper)
            : base(facade)
        {
            _cacheWrapper = cacheWrapper;
        }

        public override UiEnum Update(IUnitOfWork unitOfWork, UiEnum obj)
        {
            try
            {
                _cacheWrapper.TryRemove(CacheAccessor, Type.GetType(obj.Type).GetTypeName());
            }
            catch { }

            return base.Update(unitOfWork, obj);
        }

        public override void Delete(IUnitOfWork unitOfWork, UiEnum obj)
        {
            try
            {
                _cacheWrapper.TryRemove(CacheAccessor, Type.GetType(obj.Type).GetTypeName());
            }
            catch { }

            base.Delete(unitOfWork, obj);
        }

        public new IQueryable<BaseObject> GetAll(IUnitOfWork unitOfWork, bool? hidden)
        {
            return base.GetAll(unitOfWork, hidden).ToList().AsQueryable();
        }

        protected override IObjectSaver<UiEnum> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<UiEnum> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver).SaveOneToMany(x => x.Values);
        }

        private IEnumerable<Type> GetTypes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var types = new List<Type> { };

            foreach (var assembly in assemblies)
            {
                try
                {
                    types.AddRange(assembly.GetTypes()
                        .Where(t => t.IsEnum && t.IsDefined(typeof(UiEnumAttribute), false)));
                }
                catch { }
            }

            return types;
        }

        private List<UiEnumValue> GetValues(Type enumType)
        {
            var res = new List<UiEnumValue>();

            foreach (var val in Enum.GetValues(enumType))
            {
                var attr = enumType.GetField(val.ToString()).GetCustomAttribute<UiEnumValueAttribute>();

                res.Add(new UiEnumValue()
                {
                    Value = ((int)((object)val)).ToString(),
                    Title = attr.With(x => x.Title) ?? val.ToString(),
                    Icon = new Icon()
                    {
                        Value = attr.With(x => x.Icon),
                        Color = attr.With(x => x.Color)
                    }
                });
            }

            return res;
        }

        public void Sync()
        {
            var types = GetTypes();

            using (var uof = UnitOfWorkFactory.CreateSystem())
            {
                var repository = uof.GetRepository<UiEnum>();

                var dbEnums = repository.All().Where(x => !x.Hidden).ToList();

                var enums = types as Type[] ?? types.ToArray();

                foreach (string type in dbEnums.Select(x => x.Type).Except(enums.Select(m => m.GetTypeName())))
                {
                    var en = dbEnums.FirstOrDefault(x => x.Type == type);
                    en.Values.Clear();
                    repository.Delete(en);
                }

                foreach (var enumType in enums.Distinct())
                {
                    var dbenum = dbEnums.FirstOrDefault(x => x.Type == enumType.GetTypeName());

                    var values = GetValues(enumType);

                    if (dbenum == null)
                    {
                        repository.Create(new UiEnum()
                        {
                            Type = enumType.GetTypeName(),
                            Title = enumType.Name,
                            Values = values
                        });
                    }
                    else
                    {
                        foreach (string val in dbenum.Values.Select(x => x.Value).Except(values.Select(x => x.Value)).ToList())
                        {
                            dbenum.Values.Remove(dbenum.Values.FirstOrDefault(x => x.Value == val));
                        }

                        foreach (string val in values.Select(x => x.Value).Except(dbenum.Values.Select(x => x.Value)).ToList())
                        {
                            dbenum.Values.Add(values.FirstOrDefault(x => x.Value == val));
                        }

                        repository.Update(dbenum);
                    }
                }

                uof.SaveChanges();
            }
        }


        private static readonly CacheAccessor<UiEnum> CacheAccessor = new CacheAccessor<UiEnum>();

        public UiEnum GetEnum(Type type)
        {
            if (type == null)
                throw new Exception("source: UiEnumService.GetEnum; error: type is null");

            if (!type.IsDefined(typeof(UiEnumAttribute), false))
                throw new Exception(
                    $"source: UiEnumService.GetEnum; error: [{type.GetTypeName()}] is not UiEnumAttribute");


            string typeName = type.GetTypeName();

            return _cacheWrapper.GetOrAdd(CacheAccessor, typeName, () =>
            {
                var res = new UiEnum()
                {
                    Type = typeName
                };

                using (var uof = UnitOfWorkFactory.CreateSystem())
                {
                    var uienum = 
                        uof.GetRepository<UiEnum>().All()
                            .Where(x => !x.Hidden)
                                .FirstOrDefault(x => x.Type == typeName);

                    if (uienum != null)
                    {
                        res.Title = uienum.Title;
                        res.Values = new List<UiEnumValue>();

                        foreach (var uiEnumValue in uienum.Values)
                        {
                            res.Values.Add(new UiEnumValue()
                            {
                                Value = uiEnumValue.Value,
                                Title = uiEnumValue.Title,
                                Icon = uiEnumValue.Icon
                            });
                        }
                    }
                    else
                    {
                        res.Title = type.Name;
                        res.Values = GetValues(type);
                    }
                }

                return res;
            });

        }
    }
}

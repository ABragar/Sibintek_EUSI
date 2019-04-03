using Base;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using CorpProp.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Base.Extensions;
using CorpProp.Entities.Subject;
using CorpProp.Services.Subject;
using AppContext = Base.Ambient.AppContext;
using CorpProp.Common;
using System.Data;
using CorpProp.Entities.Import;
using CorpProp.Helpers.Import.Extentions;
using Base.Security;
using CorpProp.Helpers;
using Base.Identity.Entities;
using Identity = Base.Identity.Core;
using Base.Security.Service.Abstract;

namespace CorpProp.Services.Security
{
    public interface ISibUserService : IBaseObjectService<SibUser>, IExcelImportEntity
    {

    }

    internal class RequestOnSibUser
    {
        private readonly ISibUserService _sibUserService;
        private readonly ISocietyService _societyService;

        public RequestOnSibUser(ISibUserService sibUserService, ISocietyService societyService)
        {
            _sibUserService = sibUserService;
            _societyService = societyService;
        }

        private bool TryUnsetOtherUsersResponsableOnRequest(IUnitOfWork unitOfWork, SibUser sibUser)
        {
            if (sibUser == null)
                return false;
            if (!sibUser.ResponsibleOnRequest)
            {
                var responsibleOnRequest = unitOfWork.GetRepository<SibUser>().Find(sibUser.ID)?.ResponsibleOnRequest;
                if (responsibleOnRequest != null)
                {
                    if (responsibleOnRequest.Value)
                    {
                        sibUser.ResponsibleOnRequest = true;
                    }
                }
                return false;
            }
            var otherResponsable = from user in unitOfWork.GetRepository<SibUser>().All()
                                   where user.SocietyID == sibUser.SocietyID
                                   where user.ResponsibleOnRequest
                                   where user.ID != sibUser.ID
                                   select user;
            otherResponsable.ForEach(user => user.ResponsibleOnRequest = false);

            if (sibUser.Society?.ID != null)
            {
                var repo = unitOfWork.GetRepository<Society>();
                var society = repo.Find(sibUser.Society.ID); //_societyService.Get(unitOfWork, );
                if (society != null)
                {
                    society.ResponsableForResponseID = sibUser.ID;
                    unitOfWork.SaveChanges();
                }
            }
            return true;
        }

        private SibUser SelectResponsableOnRequestOrDefault(IReadOnlyCollection<SibUser> collection)
        {
            return collection.FirstOrDefault(user => user.ResponsibleOnRequest);
        }

        public void Create(IUnitOfWork unitOfWork, SibUser obj)
        {
            TryUnsetOtherUsersResponsableOnRequest(unitOfWork, obj);
        }

        public void CreateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<SibUser> collection)
        {
            TryUnsetOtherUsersResponsableOnRequest(unitOfWork, SelectResponsableOnRequestOrDefault(collection));
        }

        public void UpdateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<SibUser> collection)
        {
            TryUnsetOtherUsersResponsableOnRequest(unitOfWork, SelectResponsableOnRequestOrDefault(collection));
        }

        public void Update(IUnitOfWork unitOfWork, SibUser obj)
        {
            TryUnsetOtherUsersResponsableOnRequest(unitOfWork, obj);
        }
    }

    public class SibUserService : BaseObjectService<SibUser>, ISibUserService 
    {
        private readonly IAccessErrorDescriber _accessErrorDescriber;
        private readonly RequestOnSibUser _requestOnSibUser;
        private readonly ILoginProvider _login_provider;
        

        public SibUserService(
            IBaseObjectServiceFacade facade
            , IAccessErrorDescriber accessErrorDescriber
            , ISocietyService societyService
            , ILoginProvider login_provider) : base(facade)
        {
            _accessErrorDescriber = accessErrorDescriber;
            _requestOnSibUser = new RequestOnSibUser(this, societyService);
            _login_provider = login_provider;
        }

        public ILoginProvider LoginProvider { get { return _login_provider; } }

        public override IQueryable<SibUser> GetAll(IUnitOfWork unitOfWork, bool? hidden = null)
        {
            return base.GetAll(unitOfWork, hidden);
        }

        public override SibUser Get(IUnitOfWork unitOfWork, int id)
        {
            if (!AppContext.SecurityUser.IsAdmin && AppContext.SecurityUser.ProfileInfo.ID != id)
                throw new Exception(_accessErrorDescriber.AccessDenied());

            return base.Get(unitOfWork, id);
        }

        public override SibUser Update(IUnitOfWork unitOfWork, SibUser obj)
        {

            if (!AppContext.SecurityUser.IsAdmin && AppContext.SecurityUser.ProfileInfo.ID != obj.ID)
                throw new Exception(_accessErrorDescriber.AccessDenied());
            using (var newUOW = UnitOfWorkFactory.CreateSystem())
            {
                _requestOnSibUser.Update(newUOW, obj);
            }
            return base.Update(unitOfWork, obj);
        }

        public override SibUser Create(IUnitOfWork unitOfWork, SibUser obj)
        {
            _requestOnSibUser.Create(unitOfWork, obj);
            return base.Create(unitOfWork, obj);
        }

        public override IReadOnlyCollection<SibUser> CreateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<SibUser> collection)
        {
            throw new NotImplementedException();
        }

        public override IReadOnlyCollection<SibUser> UpdateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<SibUser> collection)
        {
            throw new NotImplementedException();
        }

        public override void Delete(IUnitOfWork unitOfWork, SibUser obj)
        {
            if (!AppContext.SecurityUser.IsAdmin && AppContext.SecurityUser.ProfileInfo.ID != obj.ID)
                throw new Exception(_accessErrorDescriber.AccessDenied());

            base.Delete(unitOfWork, obj);
        }

        protected override IObjectSaver<SibUser> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<SibUser> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Image)
                //.SaveOneToMany(x => x.Phones) //В профиле SibUser нет такой коллекции
                //.SaveOneToMany(x => x.Emails) //В профиле SibUser нет такой коллекции

                .SaveOneObject(x => x.Society)
                .SaveOneObject(x => x.SocietyDept)
                .SaveOneObject(x => x.User)
                
                .SaveOneObject(x => x.Boss)
                .SaveOneObject(x => x.Vice)

                //.SaveManyToMany(x => x.ExecutorTasks)
               
                ;
        }


        /// <summary>
        /// Импорт Профилей пользователей из файла Excel.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="histUofw"></param>
        /// <param name="table"></param>
        /// <param name="error"></param>
        /// <param name="count"></param>
        /// <param name="history"></param>
        public void Import(
            IUnitOfWork uofw
            , IUnitOfWork histUofw
            , DataTable table
            , Dictionary<string, string> colsNameMapping
            , ref int count
            , ref ImportHistory history)
        {
            try
            {
                string err = "";

                //при работе с аккаунтами, нужен ISystemTransactionUnitOfWork
                using (var tranUow = UnitOfWorkFactory.CreateSystemTransaction())
                {
                    //пропускаем первые 9 строк файла не считая строки названия колонок.
                    int start = ImportHelper.GetRowStartIndex(table);
                    for (int i = start; i < table.Rows.Count; i++)
                    {
                        var row = table.Rows[i];
                        ImportObject(tranUow, row, colsNameMapping, ref err, ref count, ref history);
                        count++;
                    }
                    if (history.ImportErrorLogs.Count == 0)
                        tranUow.Commit();
                }

            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        public void ImportObject(
           IUnitOfWork uow
           , DataRow row
           , Dictionary<string, string> colsNameMapping
           , ref string error
           , ref int count
           , ref ImportHistory history)
        {
            try
            {

               

                var catName = ImportHelper.GetValueByName(uow, typeof(string), row, "Category", colsNameMapping) as string;
                UserCategory cat = FindOrCreateUserCategory(uow, catName);                

                var login = ImportHelper.GetValueByName(uow, typeof(string), row, "SysName", colsNameMapping) as string;
                var user = FindOrCreateUser(uow, login, cat);

                var profile = uow.GetRepository<SibUser>()
                    .Filter(f => !f.Hidden && f.User != null && f.User.SysName == login)
                    .FirstOrDefault();

                if (profile == null)                
                    profile = uow.GetRepository<SibUser>().Create(new SibUser() { User = user });                   

                profile.FillObject(uow, typeof(SibUser),
                           row, row.Table.Columns, ref error, ref history, colsNameMapping);

                var deptName = ImportHelper.GetValueByName(uow, typeof(string), row, "SocietyDept", colsNameMapping) as string;
                profile.SetSocietyDept(uow, deptName);
                

                if (user != null && user.ProfileId != profile.ID)
                    user.Profile = profile;
                uow.SaveChanges();
            }
            catch (Exception ex)
            {

                history.ImportErrorLogs.AddError(ex);
            }
        }

        private UserCategory FindOrCreateUserCategory(IUnitOfWork uow, string catName)
        {
            UserCategory cat = null;
            if (!String.IsNullOrEmpty(catName))
            {
                cat = uow.GetRepository<UserCategory>()
                .Filter(f => !f.Hidden && f.Name == catName)
                .FirstOrDefault();


                if (cat == null)
                    cat = uow.GetModifiedEntities<UserCategory>()
                    .Where(f => f.Key != null && f.Key.Name == catName).Select(s => s.Key).FirstOrDefault();

                if (cat == null)
                {
                    var oid = Guid.NewGuid().ToString();
                    cat = uow.GetRepository<UserCategory>()
                        .Create(new UserCategory()
                        {
                            Name = catName,
                            SysName = oid,
                            ProfileMnemonic = nameof(SibUser)
                        });
                    uow.SaveChanges();                    
                }
                    
            }
            return cat;
        }

        private User FindOrCreateUser(IUnitOfWork uow, string login, UserCategory cat)
        {
            //Если нет категории, то нельзя создать пользователя.
            if (cat == null) return null;

            var user = uow.GetRepository<User>()
                   .Filter(f => !f.Hidden && f.SysName == login)
                   .FirstOrDefault();

            if (user == null)
                user = uow.GetModifiedEntities<User>()
                    .Where(f => f.Key != null && f.Key.SysName == login)
                    .Select(s => s.Key)
                    .FirstOrDefault();

            if (user == null)
            {
                user = uow.GetRepository<User>()
                   .Create(new User()
                   {
                       SysName = login,
                       CategoryID = cat.ID

                   });

                uow.SaveChanges();                

                AccountEntry acc = new AccountEntry()
                {                    
                    IsUser = true,
                    UserName = login,
                    UserId = user.ID
                };
                //по умолчанию
                var pass = "123456Ss";
                LoginProvider.AttachSystemPassword(uow, user.ID, login, pass);                
            }

            return user;
        }

        public void CancelImport(
             IUnitOfWork uofw
            , ref ImportHistory history
            )
        {
            throw new NotImplementedException();
        }
    }
}

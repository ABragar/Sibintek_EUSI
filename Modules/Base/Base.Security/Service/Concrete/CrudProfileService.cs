using System;
using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Extensions;
using Base.Service;
using AppContext = Base.Ambient.AppContext;

namespace Base.Security.Service
{
    public class CrudProfileService<T> : BaseObjectService<T>, ICrudProfileService<T> where T : BaseProfile, new()
    {
        private readonly IAccessErrorDescriber _accessErrorDescriber;

        public CrudProfileService(IBaseObjectServiceFacade facade, IAccessErrorDescriber accessErrorDescriber) : base(facade)
        {
            _accessErrorDescriber = accessErrorDescriber;
        }

        public override IQueryable<T> GetAll(IUnitOfWork unitOfWork, bool? hidden = null)
        {
            return AppContext.SecurityUser.IsAdmin ? base.GetAll(unitOfWork, hidden) : base.GetAll(unitOfWork, hidden).Where(x => x.ID == AppContext.SecurityUser.ProfileInfo.ID);
        }

        public override T Get(IUnitOfWork unitOfWork, int id)
        {
            if (!AppContext.SecurityUser.IsAdmin && AppContext.SecurityUser.ProfileInfo.ID != id)
                throw new Exception(_accessErrorDescriber.AccessDenied());

            return base.Get(unitOfWork, id);
        }

        public override T Update(IUnitOfWork unitOfWork, T obj)
        {

            if (!AppContext.SecurityUser.IsAdmin && AppContext.SecurityUser.ProfileInfo.ID != obj.ID)
                throw new Exception(_accessErrorDescriber.AccessDenied());

            return base.Update(unitOfWork, obj);
        }

        public override IReadOnlyCollection<T> CreateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<T> collection)
        {
            throw new NotImplementedException();
        }

        public override IReadOnlyCollection<T> UpdateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<T> collection)
        {
            throw new NotImplementedException();
        }

        public override void Delete(IUnitOfWork unitOfWork, T obj)
        {
            if (!AppContext.SecurityUser.IsAdmin && AppContext.SecurityUser.ProfileInfo.ID != obj.ID)
                throw new Exception(_accessErrorDescriber.AccessDenied());

            var repositoryProfileEmail = unitOfWork.GetRepository<ProfileEmail>();

            repositoryProfileEmail.All().Where(x => x.BaseProfileID == obj.ID).ForEach(x => repositoryProfileEmail.Delete(x));

            var repositoryProfilePhone = unitOfWork.GetRepository<ProfilePhone>();

            repositoryProfilePhone.All().Where(x => x.BaseProfileID == obj.ID).ForEach(x => repositoryProfilePhone.Delete(x));

            unitOfWork.GetRepository<T>().Delete(obj);

            unitOfWork.SaveChanges();
        }

        protected override IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<T> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Image)
                .SaveOneToMany(x => x.Phones)
                .SaveOneToMany(x => x.Emails);
        }
    }
}
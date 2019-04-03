using System;
using System.Linq;
using Base.DAL;
using Base.Security;
using Base.Service;
using Base.Support.Entities;
using Base.Support.Services.Abstract;
using AppContext = Base.Ambient.AppContext;

namespace Base.Support.Services.Concrete
{
    public class BaseSupportService<T> : BaseObjectService<T>, IBaseSupportService<T>
        where T : BaseSupport, new()
    {
        public BaseSupportService(IBaseObjectServiceFacade facade)
            : base(facade)
        {
        }

        protected override IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<T> objectSaver)
        {
            if (objectSaver.IsNew)
            {
                objectSaver.Dest.CreatorID = AppContext.SecurityUser.ID;
                objectSaver.Dest.CreateDate = DateTime.Now;
            }
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneToMany(x => x.AttachFiles, x => x.SaveOneObject(z => z.Object));
        }

        public override IQueryable<T> GetAll(IUnitOfWork unitOfWork, bool? hidden)
        {
            return base.GetAll(unitOfWork, hidden).OrderByDescending(x => x.CreateDate);
        }
    }
}

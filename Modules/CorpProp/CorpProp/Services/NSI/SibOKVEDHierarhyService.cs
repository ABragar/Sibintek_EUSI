using Base.Service;
using CorpProp.Entities.NSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;

namespace CorpProp.Services.NSI
{
    
    public interface ISibOKVEDHierarhyService : IBaseCategoryService<SibOKVED>
    {

    }

    public class SibOKVEDHierarhyService : BaseCategoryService<SibOKVED>, ISibOKVEDHierarhyService
    {
        public SibOKVEDHierarhyService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<SibOKVED> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<SibOKVED> objectSaver)
        {
            if (objectSaver != null && objectSaver.Src != null)
            {
                objectSaver.Src.Title = objectSaver.Src.Code + " " + objectSaver.Src.Name;
                objectSaver.Dest.Title = objectSaver.Src.Title;
            }             
            
            return base.GetForSave(unitOfWork, objectSaver) ;
        }

    }
}

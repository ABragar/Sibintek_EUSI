using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Contact.Entities;
using Base.Contact.Service.Abstract;
using Base.DAL;
using Base.Service;

namespace Base.Contact.Service.Concrete
{
    public class FamilyMemberService : BaseObjectService<FamilyMember>, IFamilyMemberService
    {
        public FamilyMemberService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<FamilyMember> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<FamilyMember> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver).SaveOneObject(x => x.Relative) ;
        }
    }
}

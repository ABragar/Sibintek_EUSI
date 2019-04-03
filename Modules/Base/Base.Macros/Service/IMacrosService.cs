using System.Collections.Generic;
using Base.DAL;
using Base.Macros.Entities;
using Base.Security;

namespace Base.Macros
{
    public interface IMacrosService
    {
        bool CheckBranch(IUnitOfWork uow, BaseObject obj, IEnumerable<ConditionItem> inits);
        void InitializeObject(ISecurityUser securityUser, BaseObject src, BaseObject dest, IEnumerable<InitItem> inits);
    }

}

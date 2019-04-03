using System;
using Base.Service.Crud;

namespace Base.BusinessProcesses.Entities
{
    public interface IWFObjectService : IBaseObjectCrudService
    {
        [Obsolete]
        void BeforeInvoke(BaseObject obj);
        void OnActionExecuting(ActionExecuteArgs args);        
    }
}
using Base.Service;
using Base.Support.Entities;

namespace Base.Support.Services.Concrete
{
    public class SupportRequestService : BaseSupportService<SupportRequest>
    {
        public SupportRequestService(IBaseObjectServiceFacade facade) 
            : base(facade)
        {
        }
    }
}
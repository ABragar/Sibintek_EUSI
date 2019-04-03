using Base.CRM.Entities;
using Base.CRM.Services.Abstract;
using Base.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;

namespace Base.CRM.Services.Concrete
{
    public class DiscountBaseService<T> : BaseObjectService<T>, IDiscountService<T> where T : DiscountBase<BaseObject>
    {
        public DiscountBaseService(IBaseObjectServiceFacade facade) : base(facade)
        {

        }      
    }
}

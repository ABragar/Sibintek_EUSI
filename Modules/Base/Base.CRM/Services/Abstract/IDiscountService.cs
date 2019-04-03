using Base.CRM.Entities;
using Base.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.CRM.Services.Abstract
{
    public interface IDiscountService<T> : IBaseObjectService<T> 
        where T : DiscountBase<BaseObject>
    {
    }
}

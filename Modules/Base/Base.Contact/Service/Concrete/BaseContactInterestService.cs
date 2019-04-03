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
    public class BaseContactInterestService<T> : BaseObjectService<T>, IBaseContactInterestService<T>
        where T : ContactInterest, new()

    {
        public BaseContactInterestService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }
   
    }
}

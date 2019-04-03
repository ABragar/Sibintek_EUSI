using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Support.Entities;

namespace Base.Support.Events
{
    public class SupportEventArgs
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public BaseSupport Object { get; set; }

        public SupportEventArgs(IUnitOfWork unitOfWork, BaseSupport obj)
        {
            UnitOfWork = unitOfWork;
            Object = obj;
        }
    }
}

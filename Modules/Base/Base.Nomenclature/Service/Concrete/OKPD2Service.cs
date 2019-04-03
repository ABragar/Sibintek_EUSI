using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Nomenclature.Entities;
using Base.Service;

namespace Base.Nomenclature.Service.Concrete
{
    public interface IOKPD2Service : IBaseCategoryService<OKPD2>
    {

    }
    public class OKPD2Service:BaseCategoryService<OKPD2>,IOKPD2Service
    {
        public OKPD2Service(IBaseObjectServiceFacade facade) : base(facade)
        {
        }
    }
}

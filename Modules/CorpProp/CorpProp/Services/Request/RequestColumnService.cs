using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Base.DAL;
using Base.Service;
using CorpProp.Entities.Request;

namespace CorpProp.Services.Request
{
    public class RequestColumnService: BaseObjectService<RequestColumn>
    {
        public RequestColumnService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public override RequestColumn Create(IUnitOfWork unitOfWork, RequestColumn obj)
        {
            return base.Create(unitOfWork, obj);
        }
    }
}

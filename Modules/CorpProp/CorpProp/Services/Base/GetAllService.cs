using Base;
using Base.DAL;
using Base.Enums;
using Base.Events;
using Base.Security;
using Base.Service;
using Base.Service.Crud;
using Base.Service.Log;
using Base.Utils.Common.Attributes;
using CorpProp.Common;
using CorpProp.Entities.Base;
using CorpProp.Entities.Security;
using CorpProp.Extentions;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using AppContext = Base.Ambient.AppContext;

namespace CorpProp.Services.Base
{
    public interface IAllObjectsService : IBaseObjectCrudService
    {

    }

    
    public interface IGetAllService<T> : IAllObjectsService, ITypeObjectService<T> where T : TypeObject
    {

    }


    /// <summary>
    /// Представляет сервис по работе со всеми объектами, в том числе с удаленными.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GetAllService<T> : TypeObjectService<T>, IGetAllService<T> where T : TypeObject
    {

        private readonly ILogService _logger;
        public GetAllService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        public override IQueryable<T> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return base.GetAll(unitOfWork, true);
        }
               

    }
}

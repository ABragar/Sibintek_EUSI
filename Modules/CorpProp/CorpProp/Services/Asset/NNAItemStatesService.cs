using System;
using System.Linq;
using Base.DAL;
using Base.Security;
using Base.Service;
using CorpProp.Entities.Asset;
using ExcelDataReader;
using System.Data;
using System.Collections.Generic;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using AppContext = Base.Ambient.AppContext;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using CorpProp.Common;
using Base.Entities.Complex;
using CorpProp.Entities.Settings;
using System.Reflection;
using Base.Extensions;
using CorpProp.Entities.NSI;
using CorpProp.Extentions;
using CorpProp.Services.Base;
using Base.Service.Log;

namespace CorpProp.Services.NSI
{
   
    public interface INNAItemStatesService : ITypeObjectService<NonCoreAssetListItemState>
    {
    }

    
    public class NNAItemStatesService : TypeObjectService<NonCoreAssetListItemState>, INNAItemStatesService
    {
        private readonly ILogService _logger;

        public NNAItemStatesService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
        }

        public override IQueryable<NonCoreAssetListItemState> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            return
                (AppContext.SecurityUser.IsFromCauk(unitOfWork)) ?
                base.GetAll(unitOfWork, hidden)
                : base.GetAll(unitOfWork, hidden).Where(f => f.Code != "109");

        }

    }
}

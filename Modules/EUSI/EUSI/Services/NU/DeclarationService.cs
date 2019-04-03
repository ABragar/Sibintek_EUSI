using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.Common;
using CorpProp.Entities.Document;
using CorpProp.Entities.Import;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;
using EUSI.Entities.BSC;
using EUSI.Entities.NU;
using EUSI.Import;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Services.NU
{

    public interface IDeclarationService<T> : ITypeObjectService<T>, IXmlImportEntity where T : Declaration
    {

    }

    public class DeclarationService<T> : TypeObjectService<T>, IDeclarationService<T> where T : Declaration
    {
        private readonly ILogService _logger;
        IImportHolder _holder;
      

        public DeclarationService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }


        public IImportHolder Holder { get { return _holder; } }

        /// <summary>
        /// Импорт из файла XML.
        /// </summary>        
        public void CreateHolder(
            IUnitOfWork uow
            , IUnitOfWork histUow
            , StreamReader reader
            , FileCardOne file
            )
        {
            _holder = new TaxImportHolder(uow, histUow, reader, file);
            
        }





    }
}

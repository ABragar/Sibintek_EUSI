using System;
using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Import;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Import;

namespace EUSI.Services.Import
{
    public class ImportHistoryServiceEx : ImportHistoryService
    {
        private readonly ILogService _logger;
        public ImportHistoryServiceEx(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределнный метод инициализации ОГ в истории импорта
        /// </summary>
        /// <param name="uofw">сессия</param>
        /// <param name="code">в зависимости от импортируемого типа код ЕУП или код БЕ</param>
        /// <param name="sysObjName">название импортируемого типа</param>
        /// <param name="history">история импорта</param>
        public override void InitSociety(IUnitOfWork uofw, string code, bool isRequired, ref ImportHistory history)
        {           
            if (!String.IsNullOrEmpty(code))
            {
                history.Society = uofw
                        .GetRepository<Society>()
                        .Filter(f => !f.Hidden && !f.IsHistory 
                            && f.ConsolidationUnit != null 
                            && f.ConsolidationUnit.Code == code)
                        .FirstOrDefault();
                
                history.Consolidation = uofw.GetRepository<Consolidation>()
                    .Filter(x => x.Code == code && !x.Hidden && !x.IsHistory).FirstOrDefault();

                if (history.Society == null && isRequired)
                    history.ImportErrorLogs.AddError(ErrorTypeName.InvalidFileNameFormat
                    + $"ОГ с кодом БЕ <{code}> не найдено в Системе.");

                if (history.Consolidation == null && isRequired)
                    history.ImportErrorLogs.AddError(ErrorTypeName.InvalidFileNameFormat
                    + $"БЕ с кодом <{code}> не найдено в Системе.");
            }
            // иначе осуществляем стандартный поиск по коду ЕУП
            else
            {
                base.InitSociety(uofw, code, isRequired, ref history);
            }
        }
       
    }
}

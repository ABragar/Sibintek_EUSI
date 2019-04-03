using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Service;
using Base.UI.Service;
using CorpProp.Common;
using CorpProp.Entities.Common;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using ExcelDataReader;

namespace EUSI.Import
{
    public class EusiImportStarter : ImportStarter
    {
        public EusiImportStarter(IAccessService accessService, IExcelImportChecker checker) : base(accessService, checker)
        {
        }

        public override CheckImportResult CheckImport(IUiFasade uiFacade, IExcelDataReader reader, ITransactionUnitOfWork uofw,
            IUnitOfWork histUnitOfWork, StreamReader stream, string fileName, ImportHistory importHistory)
        {
            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration() { ConfigureDataTable = x => new ExcelDataTableConfiguration() { UseHeaderRow = false } });
            var tables = result.Tables;
            DataTable entryTable = tables[0];
            var checkResult = base.CheckImport(uiFacade, reader, uofw, histUnitOfWork, stream, fileName, importHistory);
            string mnemonic = ImportHelper.FindSystemName(entryTable);
            var config = uiFacade.GetViewModelConfig(mnemonic);

            if (config.ServiceType.GetInterfaces().Contains(typeof(IConfirmImportChecker)))
            {
                var checkService = config.GetService<IConfirmImportChecker>();
                checkResult.IsConfirmationRequired = checkService.IsConfirmationRequired(uofw, fileName, reader, histUnitOfWork, entryTable, importHistory);

                if (checkResult.IsConfirmationRequired)
                {
                    checkResult.ConfirmationItemDescription = checkService.GetConfirmationDescription(uofw, fileName, reader, histUnitOfWork, entryTable, importHistory);
                }
            }

            return checkResult;
        }
    }
}
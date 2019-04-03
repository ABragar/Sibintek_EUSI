using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.Import;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using EUSI.Entities.Import;

namespace EUSI.Services.Import
{
    public interface IExternalImportLogService : IBaseObjectService<ExternalImportLog>, IExcelImportEntity
    {

    }
    public class ExternalImportLogService : BaseObjectService<ExternalImportLog>, IExternalImportLogService
    {
        public ExternalImportLogService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<ExternalImportLog> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<ExternalImportLog> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(s => s.Society)
                ;
        }

        public void CancelImport(IUnitOfWork uofw, ref ImportHistory history)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Импорт журнала импорта из файла Excel.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="histUofw"></param>
        /// <param name="table"></param>
        /// <param name="error"></param>
        /// <param name="count"></param>
        /// <param name="history"></param>
        public void Import(
            IUnitOfWork uofw
            , IUnitOfWork histUofw
            , DataTable table
            , Dictionary<string, string> colsNameMapping
            , ref int count
            , ref ImportHistory history)
        {
            try
            {
                string err = "";
                //пропускаем первые 10 строк (историчность) файла не считая строки названия колонок.
                int start = ImportHelper.GetRowStartIndex(table);
                for (int i = start; i < table.Rows.Count; i++)
                {
                    var row = table.Rows[i];
                    ImportObject(uofw, row, colsNameMapping, ref err, ref count, ref history);
                    count++;
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// Имопртирует запись журнала импорта из строки файла.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="row">Строка файла.</param>      
        /// <param name="error">Текст ошибки.</param>
        /// <param name="count">Количество импортированных объектов.</param>
        public void ImportObject(
            IUnitOfWork uofw
            , DataRow row
            , Dictionary<string, string> colsNameMapping
            , ref string error
            , ref int count
            , ref ImportHistory history)
        {
            try
            {
                var idEupObj = ImportHelper.GetValueByName(uofw, typeof(string), row, "Society", colsNameMapping);
                ExternalImportLog obj = new ExternalImportLog();

                if (idEupObj != null && !String.IsNullOrEmpty(idEupObj.ToString()))
                {
                    string idEup = ImportHelper.GetIDEUP(idEupObj);
                    
                    obj.FillObject(uofw, typeof(ExternalImportLog),
                       row, row.Table.Columns, ref error, ref history, colsNameMapping);
                    obj.ImportDate = DateTime.Now;

                    if (!String.IsNullOrEmpty(error))
                        obj = null;
                    else
                    {
                        ExternalImportLog newObj = null;
                        newObj = this.Create(uofw, obj);
                    }
                }
                else
                {
                    error = $"Неверное значение ИДЕУП. {System.Environment.NewLine}";
                    history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", error, ErrorType.System);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }
    }
}

using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.Common;
using CorpProp.Entities.Import;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;
using EUSI.Entities.BSC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Services.BSC
{

    public interface IBSCDataService : ITypeObjectService<BCSData>, IExcelImportEntity
    {

    }

    public class BSCDataService : TypeObjectService<BCSData>, IBSCDataService
    {
        private readonly ILogService _logger;
        public BSCDataService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }


        /// <summary>
        /// Импорт из файла Excel.
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
                //пропускаем первые 9 строк файла не считая строки названия колонок.
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

        public List<BCSData> FindObjects(
            IUnitOfWork uofw
            , object startDate
            , object endDate
            , string be
            , string groupCode
            , string posCode)
        {
            DateTime? start = startDate.ToString().GetDate();
            DateTime? end = endDate.ToString().GetDate();

            List<BCSData> list = new List<BCSData>();
            list = uofw.GetRepository<BCSData>()
            .Filter(x => !x.Hidden &&
                !x.IsHistory &&
                x.StartDate == start &&
                x.EndDate == end &&
                x.Consolidation != null &&
                x.Consolidation.Code == be &&
                x.GroupConsolidation != null &&
                x.GroupConsolidation.Code == groupCode &&
                x.PositionConsolidation != null &&
                x.PositionConsolidation.Code == posCode
             ).ToList<BCSData>();
            return list;
        }




        /// <summary>
        /// Имопртирует из строки файла.
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
                bool isNew = true;
                BCSData obj = null;
                var startDate = ImportHelper.GetValueByName(uofw, typeof(string), row, "StartDate", colsNameMapping);
                var endDate = ImportHelper.GetValueByName(uofw, typeof(string), row, "EndDate", colsNameMapping);
                var be = ImportHelper.GetValueByName(uofw, typeof(string), row, "Consolidation", colsNameMapping);
                var groupCode = ImportHelper.GetValueByName(uofw, typeof(string), row, "GroupConsolidation", colsNameMapping);
                var posCode = ImportHelper.GetValueByName(uofw, typeof(string), row, "PositionConsolidation", colsNameMapping);

                if (startDate != null && endDate != null && be != null && groupCode != null && posCode != null)
                {

                    List<BCSData> list = FindObjects(uofw, startDate, endDate, be.ToString(), groupCode.ToString(), posCode.ToString());
                    if (list == null || list.Count == 0)
                        obj = new BCSData();
                    else if (list.Count > 1)
                    {
                        error += $"Невозможно обновить объект. В Системе найдено более одной записи.{System.Environment.NewLine}";
                        history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", error, ErrorType.System);
                        return;
                    }
                    else if (list.Count == 1)
                    {
                        isNew = false;
                        obj = list[0];
                    }
                    if (obj != null)
                    {
                        obj.FillObject(uofw, typeof(BCSData),
                           row, row.Table.Columns, ref error, ref history, colsNameMapping, true);
                        obj.ImportDate = DateTime.Now;

                        BCSData newObj = null;

                        if (isNew)
                            newObj = this.Create(uofw, obj);
                        else
                            newObj = this.Update(uofw, obj);
                    }
                }
                else
                {
                    error += $"Невозможно идентифицировать запись. {System.Environment.NewLine}";
                    history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, "", error, ErrorType.System);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        public void CancelImport(
            IUnitOfWork uofw
           , ref ImportHistory history
           )
        {
            throw new NotImplementedException();
        }
    }
}

using Base;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security.Service;
using Base.Service;
using Base.Service.Crud;
using CorpProp.Common;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Law;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Import;
using System.Data;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;
using Base.Service.Log;

namespace CorpProp.Services.Subject
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - Банковские реквизиты.
    /// </summary>
    public interface IBankingDetailService : ITypeObjectService<BankingDetail>, IExcelImportEntity
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - Банковские реквизиты.
    /// </summary>
    public class BankingDetailService : TypeObjectService<BankingDetail>, IBankingDetailService
    {

        private readonly ILogService _logger;
        /// <summary>
        /// Инициализирует новый экземпляр класса BankingDetailService.
        /// </summary>
        /// <param name="facade"></param>
        public BankingDetailService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade, logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// Переопределяет метод при событии обновления объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Банковские реквизиты</returns>
        public override BankingDetail Update(IUnitOfWork unitOfWork, BankingDetail obj)
        {
            return base.Update(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>Банковские реквизиты</returns>
        public override BankingDetail Create(IUnitOfWork unitOfWork, BankingDetail obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<BankingDetail> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<BankingDetail> objectSaver)
        {

            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Subject)

                    //.SaveOneToMany(x => x.EstateAppraisals, x=> x.SaveOneObject(s=>s.Appraisal))
                    //.SaveOneToMany(x => x.NonCoreAssetAppraisals, x => x.SaveOneObject(s => s.Appraisal))
                    ;
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


        public List<BankingDetail> FindObjects(IUnitOfWork uofw, string bankAccount, string bik)
        {
            List<BankingDetail> list = new List<BankingDetail>();
            list = uofw.GetRepository<BankingDetail>().Filter(x =>
            !x.IsHistory &&
            x.BankAccount != null && x.BankAccount == bankAccount
            && x.BIK != null && x.BIK == bik
            && !x.Hidden).ToList<BankingDetail>();
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
                BankingDetail obj = null;
                var bankAccount = ImportHelper.GetValueByName(uofw, typeof(string), row, "BankAccount", colsNameMapping);
                var bik = ImportHelper.GetValueByName(uofw, typeof(string), row, "BIK", colsNameMapping);

                if (bik != null && bankAccount != null
                    && !String.IsNullOrEmpty(bik.ToString())
                    && !String.IsNullOrEmpty(bankAccount.ToString()))
                {

                    //TODO: почистить
                    List<BankingDetail> list = FindObjects(uofw, bankAccount.ToString(), bik.ToString());
                    if (list == null || list.Count == 0)
                        obj = new BankingDetail();
                    else if (list.Count > 1)
                        history.ImportErrorLogs.AddError(row.Table.Rows.IndexOf(row), 0, $"Невозможно обновить объект. В Системе найдено более одной записи.", error, ErrorType.System);


                    else if (list.Count == 1)
                    {
                        isNew = false;
                        obj = list[0];
                    }
                    if (obj != null)
                    {
                        obj.FillObject(uofw, typeof(BankingDetail),
                            row, row.Table.Columns, ref error, ref history, colsNameMapping);
                        obj.ImportDate = DateTime.Now;
                    }

                    if (!String.IsNullOrEmpty(error))
                        obj = null;
                    else
                    {
                        if (obj != null)
                        {
                            BankingDetail newObj = null;

                            if (isNew)
                                newObj = this.Create(uofw, obj);
                            else
                                newObj = this.Update(uofw, obj);

                        }
                    }
                }
                else
                {
                    error += $"Неверное значение расчетного счета или БИК. {System.Environment.NewLine}";
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

using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Extensions;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Document;
using CorpProp.Entities.Estate;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.Import;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Settings;
using EUSI.Entities.Estate;
using EUSI.Entities.ManyToMany;
using EUSI.Entities.NSI;
using EUSI.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using EUSI.Validators;
using CorpProp.Entities.Common;
using CorpProp.Extentions;

namespace EUSI.Import
{
    public class ERImportHolder : IImportHolder
    {
        private IUnitOfWork _UofW;
        private IUnitOfWork _UofWHistory;
        private ImportHistory _ImportHistory;
        private DataSet _ds;
        private string _fileName;
        private int? _userID;
        private FileCard _attachFile;
        private int startRow;
        private int endRow;
        private int fieldNameColIndex = 0;
        private int valueColIndex = 0;
        private List<string> simpleTypes;
        private IWorkflowService _workFlow;
        private IEstateRegistrationService _erService;
        private ISibEmailService _emailService;

        /// <summary>
        /// Инициализирует новый экземпляр класса ERImportHolder.
        /// </summary>
        public ERImportHolder(
             FileCard attachFile
            , IUnitOfWork uofw
            , IUnitOfWork uofwhistory
            , DataSet ds
            , string fileName
            , int? userID
            , IWorkflowService workFlow
            , IEstateRegistrationService erService
            , ISibEmailService emailService
            , ERImportWizard wizard
            )
        {
            _UofW = uofw;
            _UofWHistory = uofwhistory;
            _ds = ds;
            AccountingObjects = new List<AccountingObject>();
            _attachFile = attachFile;
            _fileName = fileName;
            _userID = userID;
            simpleTypes = new List<string>() { "string", "int16", "int32", "decimal", "bool", "datetime", "float" };
            Rows = new List<EstateRegistrationRow>();
            _workFlow = workFlow;
            _erService = erService;
            _emailService = emailService;
            Wizard = wizard;
            Tables = _ds?.GetVisbleTables();
        }

        /// <summary>
        /// Получает сессию объектов выписки.
        /// </summary>
        public IUnitOfWork UnitOfWork { get { return _UofW; } }

        /// <summary>
        /// Получает сессию истории импорта.
        /// </summary>
        public IUnitOfWork UofWHistory { get { return _UofWHistory; } }

        /// <summary>
        /// Получает или задает заявку.
        /// </summary>
        public EstateRegistration EstateRegistration { get; set; }

        /// <summary>
        /// Получает или задает поставщика/арендатора.
        /// </summary>
        public Society ContragentOG { get; set; }

        /// <summary>
        /// Получает или задает БЕ поставщика/арендатора.
        /// </summary>
        public Consolidation ConsolidationOG { get; set; }

        /// <summary>
        /// Получает или задает строки заявки.
        /// </summary>
        public IList<EstateRegistrationRow> Rows { get; set; }

        /// <summary>
        /// Получает или задает первичные документы.
        /// </summary>
        public IList<FileCard> Docs { get; set; }

        /// <summary>
        /// Получает или задает мастера импорта.
        /// </summary>
        public ERImportWizard Wizard { get; set; }

        /// <summary>
        /// Получает или задает признак импорта корректирующей заявки вида "Первичные документы".
        /// </summary>
        public bool IsDocCorrective { get; set; } = false;

        /// <summary>
        /// Получает или задает историю импорта.
        /// </summary>
        public ImportHistory ImportHistory { get { return _ImportHistory; } }

        //public DataSet DS { get { return _ds; } }

        private List<DataTable> Tables { get; set; }

        private bool? CheckAppropriate { get; set; }

        /// <summary>
        /// Созданные ОС/НМА.
        /// </summary>
        public List<AccountingObject> AccountingObjects { get; set; }

        public void SetHistory(ImportHistory hist)
        {
            _ImportHistory = hist;
        }

        public EstateRegistration CreateER()
        {
            ERControlDateAttributes erControl = null;

            if (EstateRegistration == null)
            {
                EstateRegistration = this.UnitOfWork.GetRepository<EstateRegistration>()
                   .Create(new EstateRegistration());
                EstateRegistration.Date = DateTime.Now.Date;
            }
            if (EstateRegistration != null)
            {
                if (EstateRegistration.ERControlDateAttributes == null)
                {
                    erControl = this.UnitOfWork.GetRepository<ERControlDateAttributes>().Create(new ERControlDateAttributes());
                    erControl.DateСreation = EstateRegistration.Date;
                    EstateRegistration.ERControlDateAttributes = erControl;
                }
            }

            if (this._ImportHistory.FileCard != null)
            {
                this._ImportHistory.FileCard.FileCardPermission = this.UofWHistory.GetRepository<FileCardPermission>()
                        .Filter(f => !f.Hidden && f.AccessModifier == AccessModifier.Everyone)
                        .FirstOrDefault();

                var fileID = this._ImportHistory.FileCard.ID;
                EstateRegistration.FileCardID = fileID;
            }
            if (EstateRegistration.ID == 0)
                EstateRegistration.State =
                    this.UnitOfWork.GetRepository<EstateRegistrationStateNSI>()
                               .Filter(f =>
                               !f.Hidden
                               && !f.IsHistory
                               && f.Code == "CREATED"
                               ).FirstOrDefault();

            return EstateRegistration;
        }

        /// <summary>
        /// Создает экземпляр истории импорта.
        /// </summary>
        public void CreateImportHistory()
        {
            _ImportHistory = this.UofWHistory.GetRepository<ImportHistory>()
                .Create(new ImportHistory() { FileName = _fileName, Mnemonic = "EstateRegistration" });

            _ImportHistory.Mnemonic = nameof(EstateRegistration);
            _ImportHistory.FileCard = _attachFile;

            if (_userID != null)
            {
                SibUser us = UofWHistory.GetRepository<SibUser>().Filter(x => x.UserID == _userID).FirstOrDefault();
                if (us != null)
                    _ImportHistory.SibUser = us;
            }
        }

        public void Import()
        {
            try
            {
                //этап проверки 3
                if (!CheckNumberER())
                    return;

                if (Wizard != null)
                {
                    ImportByWizard();
                    return;
                }
                CheckData();
                ImportData();
                if (!this.ImportHistory.ImportErrorLogs.Any())
                {
                    this.UnitOfWork.SaveChanges();
                    this.UnitOfWork.GetRepository<ImportObject>()
                        .Create(new ImportObject(this.EstateRegistration, this.ImportHistory.Oid, TypeImportObject.CreateObject));
                    if (this.EstateRegistration.State != null && this.EstateRegistration.State.Code == "COMPLETED")
                    {
                        try
                        {
                            _erService.SendUserNotification(this.UnitOfWork, new int[] { this.EstateRegistration.ID });
                        }
                        catch (Exception ex)
                        {
                            //this._ImportHistory.ImportErrorLogs.AddError(ex);
                        }
                    }

                    _workFlow.ReStartWorkflow(this.UnitOfWork, this.EstateRegistration, _erService);
                }
                else
                {
                    if (CheckAppropriate == false && _ImportHistory.ImportErrorLogs.Any())
                    {
                        if (!string.IsNullOrEmpty(this.ImportHistory.ContactEmail))
                            if (_emailService.SendNotice(this.UnitOfWork
                                , this._ImportHistory, null, this.ImportHistory.ContactEmail, "ImportHistory_Fail"))
                            {
                                this._ImportHistory.IsResultSentByEmail = true;
                                if (this._ImportHistory.SentByEmailDate == null)
                                    this._ImportHistory.SentByEmailDate = DateTime.Now;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                this._ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }

        public void ImportByWizard()
        {
            try
            {
                CheckData();
                ImportData();
                if (this.ImportHistory.ImportErrorLogs.Any())
                {
                    if (CheckAppropriate == false)
                    {
                        if (!string.IsNullOrEmpty(this.ImportHistory.ContactEmail))
                            if (_emailService.SendNotice(this.UnitOfWork
                                , this._ImportHistory, null, this.ImportHistory.ContactEmail, "ImportHistory_Fail"))
                            {
                                this._ImportHistory.IsResultSentByEmail = true;
                                if (this._ImportHistory.SentByEmailDate == null)
                                    this._ImportHistory.SentByEmailDate = DateTime.Now;
                            }
                    }

                    //контрольные процедуры импорта не пройдены, отправляем уведомление
                    _emailService.SendNotice(this.UnitOfWork, this.EstateRegistration, null, "", "ER_BadImportZDS");
                    return;
                }

                if (!this.ImportHistory.ImportErrorLogs.Any())
                {
                    //визуальные контроли пройдены?
                    if (!Wizard.VisualCheck)
                    {
                        this._ImportHistory.ImportErrorLogs.AddError("Визуальные контроли не пройдены.");
                        _emailService.SendNotice(this.UnitOfWork, this.EstateRegistration, null, "", "ER_ImportVisualNotCheckZDS");
                        return;
                    }

                    this.UnitOfWork.SaveChanges();
                    this.UnitOfWork.GetRepository<ImportObject>()
                        .Create(new ImportObject(this.EstateRegistration, this.ImportHistory.Oid, TypeImportObject.CreateObject));
                    if (Wizard.QuickClose)
                        _erService.SendNotification(this.UnitOfWork, new int[] { this.EstateRegistration.ID }, "", "ER_GoodImportZDS");
                    _workFlow.ReStartWorkflow(this.UnitOfWork, this.EstateRegistration, _erService);
                }
            }
            catch (Exception ex)
            {
                this._ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }

        public bool CheckData()
        {
            try
            {
                CheckRequiredFields();
                CheckSystemTypes();
                CheckNavigationFields();
                CheckErTypeAndReceiptReason();
                CheckAppropriate = CheckAppropriateList();
                return !this._ImportHistory.ImportErrorLogs.Any();
            }
            catch (Exception ex)
            {
                this._ImportHistory.ImportErrorLogs.AddError(ex);
                return false;
            }
        }

        /// <summary>
        /// Проверяет наличие данных на соответствующем Excel-листе согласно выбранному виду объекта заявки
        /// </summary>
        private bool CheckAppropriateList()
        {
            //получаем вид объекта заявки
            var table = Tables[0];
            var valueIndex = GetValueColumnIndex(table);
            var erTypeRow = GetRowBySystemFieldName(table, nameof(EstateRegistration.ERType));
            var erType = erTypeRow?.ItemArray[valueIndex].ToString();

            try
            {
                //получаем соответствующий Excel-лист для валидации
                var selectedList = GetSelectedList(erType);

                if (selectedList != null)
                {
                    int start = GetStartDataRow(selectedList);
                    int end = GetEndDataRow(start, selectedList);
                    Dictionary<int, string> fields = GetHorizontalSysFileds(selectedList);
                    var items = selectedList.Rows.Cast<DataRow>()
                        .Where(row => selectedList.Rows.IndexOf(row) >= start && selectedList.Rows.IndexOf(row) <= end).ToList();

                    var isAnyFullCells = false;

                    foreach (var item in items)
                    {
                        foreach (KeyValuePair<int, string> entry in fields)
                        {
                            if (!item.ItemArray[entry.Key].Equals(DBNull.Value) ||
                                (item.ItemArray[entry.Key] != null && !string.IsNullOrWhiteSpace(item.ItemArray[entry.Key].ToString())))
                            {
                                isAnyFullCells = true;
                                break;
                            }
                        }

                        if (isAnyFullCells)
                        {
                            break;
                        }
                    }

                    if (!isAnyFullCells)
                    {
                        _ImportHistory.ImportErrorLogs.AddError($"По заявке - объекты <{erType}> не заполнены.");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _ImportHistory.ImportErrorLogs.AddError(ex);
                return false;
            }
        }

        /// <summary>
        /// Возвращает выбранный Excel-лист для валидации
        /// </summary>
        /// <param name="erType">Вид объекта заявки</param>
        /// <returns></returns>
        private DataTable GetSelectedList(string erType)
        {
            //ищем соответствующий лист по системному наименованию
            DataTable selectedList = null;

            //TODO: переименовать листы шаблона импорта согласно значениям дропбокса "Вид объекта заявки", после чего переработать секцию switch
            switch (erType)
            {
                case "ОС (кроме аренды)":
                    {
                        selectedList = Tables
                            .First(tab => tab.Rows[1].ItemArray[1].ToString() == "OS-01");
                        break;
                    }
                case "НМА":
                    {
                        selectedList = Tables
                            .First(tab => tab.Rows[1].ItemArray[1].ToString() == "NMA");
                        break;
                    }
                case "НКС":
                    {
                        selectedList = Tables
                            .First(tab => tab.Rows[1].ItemArray[1].ToString() == "NKS");
                        break;
                    }
                case "ОС (аренда)":
                    {
                        selectedList = Tables
                            .First(tab => tab.Rows[1].ItemArray[1].ToString() == "ArendaOS");
                        break;
                    }
                case "Внутригрупповые перемещения":
                    {
                        selectedList = Tables
                            .First(tab => tab.Rows[1].ItemArray[1].ToString() == "OSVGP");
                        break;
                    }
                case "Объединение/разукрупнение":
                    {
                        selectedList = Tables
                            .First(tab => tab.Rows[1].ItemArray[1].ToString() == "Union"
                            || tab.Rows[1].ItemArray[1].ToString() == "Division");
                        break;
                    }
                case "Первичные документы":
                    {
                        selectedList = Tables
                            .First(tab => tab.Rows[1].ItemArray[1].ToString() == "Docs");
                        break;
                    }
                default:
                    {
                        this.ImportHistory.ImportErrorLogs.AddError($"Выбранный вид объекта заявки ({erType}) отсуствует в Excel-документе");
                        break;
                    }
            }

            return selectedList;
        }

        private void CheckErTypeAndReceiptReason()
        {
            var table = Tables[0];
            var valueIndex = GetValueColumnIndex(table);
            var erTypeRow = GetRowBySystemFieldName(table, nameof(EstateRegistration.ERType));
            var erType = erTypeRow?.ItemArray[valueIndex].ToString();

            var erReceiptReasonRow = GetRowBySystemFieldName(table, nameof(EstateRegistration.ERReceiptReason));
            var erReceiptReason = erReceiptReasonRow?.ItemArray[valueIndex].ToString();

            var validator = new EstateRegistrationDataValidator();

            if (!validator.ValidateErTypeAndReceiptReason(UnitOfWork, erType, erReceiptReason))
            {
                _ImportHistory.ImportErrorLogs.AddError(
                    table.Rows.IndexOf(erReceiptReasonRow) + 1
                    , valueColIndex + 1
                    , GetFiledName(erReceiptReasonRow)
                    , "Способ поступления не соответствует виду объекта заявки"
                    , CorpProp.Helpers.ErrorType.BreaksBusinessRules
                    , table.TableName);
            }
        }

        private void CheckNavigationFields()
        {
            var table = Tables[0];
            var prColumnIndex = GetSysTypeColumnIndex(table);
            var valueIndex = GetValueColumnIndex(table);
            var list = table.Rows.Cast<DataRow>()
                .Where(row =>
                    table.Rows.IndexOf(row) >= startRow
                    && table.Rows.IndexOf(row) <= endRow
                    && !row.ItemArray[valueIndex].Equals(System.DBNull.Value)
                    && (!row.ItemArray[prColumnIndex].Equals(System.DBNull.Value)
                        && !simpleTypes.Contains(row.ItemArray[prColumnIndex].ToString().ToLower()))
                    );

            foreach (var item in list)
            {
                try
                {
                    if (!CheckSystemObjectExist(valueIndex, prColumnIndex, item))
                    {
                        this._ImportHistory.ImportErrorLogs.AddError(
                        table.Rows.IndexOf(item) + 1
                       , valueIndex + 1
                       , GetFiledName(item)
                       , CorpProp.Helpers.ErrorType.ObjectNotFound
                       , table.TableName);
                    }
                }
                catch (Exception ex)
                {
                    this._ImportHistory.ImportErrorLogs.AddError(ex);
                }
            }
        }

        private bool CheckSystemObjectExist(int valueIndex, int sysTypeIndex, DataRow item)
        {
            Type tt = TypesHelper.GetTypeByName((item.ItemArray[sysTypeIndex].ToString().ToLower()));
            if (tt == null) return false;
            var val = item.ItemArray[valueIndex].ToString();
            if (tt.Equals(typeof(Society)))
            {
                return
                    this.UnitOfWork.GetRepository<Society>()
                    .Filter(f => !f.Hidden && !f.IsHistory
                    && f.INN == val).Any();
            }

            if (tt.Equals(typeof(Subject)))
            {
                return
                    this.UnitOfWork.GetRepository<Subject>()
                    .Filter(f => !f.Hidden && !f.IsHistory
                    && f.INN == val).Any();
            }

            //Задача 11751 - так как инициатор заявки всегда или находится или создаётся заново, всегда возвращаем true.
            if (tt.Equals(typeof(EstateRegistrationOriginator)))
            {
                return true;
            }

            var hist = this.ImportHistory;
            return CorpProp.ImportChecker.CheckSystemObjectExist(this.UnitOfWork
                , val
                , item.ItemArray[sysTypeIndex].ToString()
                , ref hist);
        }

        private void CheckSystemTypes()
        {
            var stColumnIndex = GetSysTypeColumnIndex(Tables[0]);

            var table = Tables[0];
            var valueIndex = GetValueColumnIndex(table);
            var list = table.Rows.Cast<DataRow>()
                .Where(row =>
                    table.Rows.IndexOf(row) >= startRow
                    && table.Rows.IndexOf(row) <= endRow
                    && !row.ItemArray[valueIndex].Equals(System.DBNull.Value)
                    && (!row.ItemArray[stColumnIndex].Equals(System.DBNull.Value)
                        && simpleTypes.Contains(row.ItemArray[stColumnIndex].ToString().ToLower()))
                    );

            foreach (var item in list)
            {
                try
                {
                    Type tt = typeof(string).Assembly.GetTypes()
                        .Where(t => t.Namespace == "System"
                        && simpleTypes.Contains(t.Name.ToLower())
                        && t.Name.ToLower() == item.ItemArray[stColumnIndex].ToString().ToLower()
                        )
                        .FirstOrDefault();
                    var val = item.ItemArray[valueIndex];
                    if (tt == null && item.ItemArray[stColumnIndex].ToString() == "bool")
                    {
                        tt = typeof(Boolean);
                        var bl = new List<string>() { "да", "нет" };
                        if (bl.Contains(item.ItemArray[valueIndex].ToString().ToLower()))
                            val = (item.ItemArray[valueIndex].ToString().ToLower() == "да") ? true : false;
                    }

                    Convert.ChangeType(val, tt);
                }
                catch
                {
                    this._ImportHistory.ImportErrorLogs.AddError(
                     table.Rows.IndexOf(item) + 1
                   , valueIndex + 1
                   , GetFiledName(item)
                   , CorpProp.Helpers.ErrorType.Type
                   , table.TableName);
                }
            }
        }

        private bool CheckHorizontalSysTypes(DataTable table)
        {
            var stRow = table.Rows.Cast<DataRow>()
                .Where(f => f.ItemArray.Where(col => col != null && col.ToString() == "SystemType").Any())
                .FirstOrDefault();
            var stColumnIndex = GetSysTypeColumnIndex(table);
            var startRow = GetStartDataRow(table);
            var fields = GetHorizontalSysFileds(table);
            var list = table.Rows.Cast<DataRow>()
                .Where(row => table.Rows.IndexOf(row) >= startRow);
            var res = true;

            foreach (var item in list)
            {
                foreach (KeyValuePair<int, string> entry in fields)
                {
                    try
                    {
                        if (item.ItemArray[entry.Key].Equals(System.DBNull.Value))
                            continue;
                        Type tt = typeof(string).Assembly.GetTypes()
                        .Where(t => t.Namespace == "System"
                        && simpleTypes.Contains(t.Name.ToLower())
                        && t.Name.ToLower() == stRow.ItemArray[entry.Key].ToString().ToLower()
                        )
                        .FirstOrDefault();
                        if (tt != null)
                            Convert.ChangeType(item.ItemArray[entry.Key], tt);
                        else
                        {
                            var hist = this.ImportHistory;
                            if (!CorpProp.ImportChecker.CheckSystemObjectExist(this.UnitOfWork
                                 , item.ItemArray[entry.Key].ToString()
                                 , stRow.ItemArray[entry.Key].ToString()
                                 , ref hist))
                            {
                                this._ImportHistory.ImportErrorLogs.AddError(
                                 table.Rows.IndexOf(item) + 1
                               , entry.Key + 1
                               , GetHorizontalFiledName(entry.Key, table)
                               , CorpProp.Helpers.ErrorType.ObjectNotFound
                               , table.TableName);
                                res = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        this._ImportHistory.ImportErrorLogs.AddError(
                             table.Rows.IndexOf(item) + 1
                           , entry.Key + 1
                           , GetHorizontalFiledName(entry.Key, table)
                           , CorpProp.Helpers.ErrorType.Type
                           , table.TableName);
                        res = false;
                    }
                }
            }
            return res;
        }

        private int GetSysTypeColumnIndex(DataTable table)
        {
            var row = table.Rows.Cast<DataRow>().Where(f => f.ItemArray.Where(col => col != null && col.ToString() == "SystemType").Any()).FirstOrDefault();

            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                object col = row.ItemArray[i];
                if (col != null && col.ToString() == "SystemType")
                    return i;
            }
            return 0;
        }

        private void CheckHorizontRequiredFields(DataTable table)
        {
            var startRow = GetStartDataRow(table);
            var fields = GetHorizontalSysFileds(table);
            var cols = GetRequiredColumns(table);
            var endRow = GetRealEndDataRow(startRow, table, fields.Keys.ToArray());

            foreach (var col in cols)
            {
                var rows = table.Rows.Cast<DataRow>()
                .Where(row =>
                    table.Rows.IndexOf(row) >= startRow
                    && table.Rows.IndexOf(row) <= endRow
                    && row.ItemArray[col].Equals(System.DBNull.Value))
                    .ToList();

                foreach (var row in rows)
                {
                    this._ImportHistory.ImportErrorLogs.AddError(
                      table.Rows.IndexOf(row) + 1
                    , col + 1
                    , GetHorizontalFiledName(col, table)
                    , CorpProp.Helpers.ErrorType.Required
                    , table.TableName);
                }
            }
        }

        private void CheckRequiredFields(DataTable table = null)
        {
            if (table == null)
            {
                table = Tables[0];
            }

            startRow = GetStartDataRow(table);
            endRow = GetEndDataRow(startRow, table);
            var requiredColIndex = GetRequiredColumnIndex(table);
            var valueIndex = GetValueColumnIndex(table);

            var list = table.Rows.Cast<DataRow>()
                .Where(row =>
                    table.Rows.IndexOf(row) >= startRow
                    && table.Rows.IndexOf(row) <= endRow
                    && (row.ItemArray[requiredColIndex] != null && row.ItemArray[requiredColIndex].ToString().ToLower() == "да")
                    && row.ItemArray[valueIndex].Equals(System.DBNull.Value));

            foreach (var item in list)
            {
                this._ImportHistory.ImportErrorLogs.AddError(
                      ((table.Rows.IndexOf(item) + 1) - startRow)
                    , (valueIndex == 3) ? (valueIndex - 1) : (valueIndex - 2)
                    , GetFiledName(item)
                    , CorpProp.Helpers.ErrorType.Required
                    , table.TableName);
            }
        }

        private string GetFiledName(DataRow row)
        {
            if (fieldNameColIndex == 0)
                fieldNameColIndex = GetFiledColumnIndex(row.Table);
            string tx = "";
            if (row.ItemArray[fieldNameColIndex] != null && !String.IsNullOrEmpty(row.ItemArray[fieldNameColIndex].ToString()))
                tx += row.ItemArray[fieldNameColIndex].ToString();
            if (row.ItemArray[fieldNameColIndex + 1] != null && !String.IsNullOrEmpty(row.ItemArray[fieldNameColIndex + 1].ToString()))
                tx += ((!String.IsNullOrEmpty(tx)) ? "." : "") + row.ItemArray[fieldNameColIndex + 1].ToString();
            return tx;
        }

        private string GetHorizontalFiledName(int colIndex, DataTable table)
        {
            return table.Rows.Cast<DataRow>()
                .Where(r => r.ItemArray.Where(col => col.Equals("FieldName")).Any())
                .FirstOrDefault()?
                .ItemArray[colIndex]
                .ToString();
        }

        private int GetFiledColumnIndex(DataTable table)
        {
            var row = table.Rows.Cast<DataRow>().Where(f => f.ItemArray.Where(col => col != null && col.ToString() == "FieldName").Any()).FirstOrDefault();

            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                object col = row.ItemArray[i];
                if (col != null && col.ToString() == "FieldName")
                    return i;
            }
            return 0;
        }

        private int GetSysFiledColumnIndex(DataTable table)
        {
            var row = table.Rows.Cast<DataRow>().Where(f => f.ItemArray.Where(col => col != null && col.ToString() == "SystemField").Any()).FirstOrDefault();

            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                object col = row.ItemArray[i];
                if (col != null && col.ToString() == "SystemField")
                    return i;
            }
            return 0;
        }

        private int GetValueColumnIndex(DataTable table)
        {
            if (valueColIndex != 0)
                return valueColIndex;

            var row = table.Rows.Cast<DataRow>().Where(f => f.ItemArray.Where(col => col != null && col.ToString() == "Value").Any()).FirstOrDefault();

            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                object col = row.ItemArray[i];
                if (col != null && col.ToString() == "Value")
                    return i;
            }
            return 0;
        }

        private int GetRequiredColumnIndex(DataTable table)
        {
            var row = table.Rows.Cast<DataRow>().Where(f => f.ItemArray.Where(col => col != null && col.ToString() == "Required").Any()).FirstOrDefault();

            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                object col = row.ItemArray[i];
                if (col != null && col.ToString() == "Required")
                    return i;
            }
            return 0;
        }

        private IList<int> GetRequiredColumns(DataTable table)
        {
            IList<int> cols = new List<int>();

            var row = table.Rows.Cast<DataRow>()
                .Where(f => f.ItemArray.Where(col => col != null && col.ToString() == "Required").Any())
                .FirstOrDefault();
            if (row == null)
                return cols;
            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                object col = row.ItemArray[i];
                if (col != null && col.ToString().ToLower() == "да")
                    cols.Add(i);
            }
            return cols;
        }

        private int GetStartDataRow(DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                if (row.ItemArray.Where(col => col != null && col.ToString() == "PreStartData").Any())
                    return i + 1;
            }
            return 0;
        }

        private int GetEndDataRow(int startRow, DataTable table)
        {
            //for (int i = startRow; i < table.Rows.Count; i++)
            //{
            //    DataRow row = table.Rows[i];

            //    if (row.ItemArray[0].Equals(System.DBNull.Value))
            //        return i ;
            //}
            return (table.Rows.Count == 0) ? 0 : (table.Rows.Count - 1);
        }

        private int GetRealEndDataRow(int startRow, DataTable table, int[] columns)
        {
            for (int i = table.Rows.Count - 1; i >= startRow; i--)
            {
                DataRow row = table.Rows[i];
                foreach (int col in columns)
                {
                    if (!row.ItemArray[col].Equals(System.DBNull.Value)
                        || (row.ItemArray[col] != null && !String.IsNullOrEmpty(row.ItemArray[col].ToString())))
                        return i;
                }
            }
            return (table.Rows.Count - 1);
        }

        public void ImportData()
        {
            try
            {
                if (IsDocCorrective)
                {
                    ImportDocs();
                    return;
                }
                ImportMainData();

                if (this._ImportHistory.ImportErrorLogs.Any())
                    return;

                if (this.EstateRegistration.ERType == null)
                {
                    this._ImportHistory.ImportErrorLogs.AddError($"Не удалось определить вид объекта заявки");
                    return;
                }
                //когда заполнять поставщика
                //способ поступления от контрагента
                var rrContragent = new List<string>() { "FreeOfCharge", "Rent", "RentOut", "Purchase", "ExchangeAssets", "Restructuring", "Investment" };
                if (!(EstateRegistration.ERReceiptReason != null && rrContragent.Contains(EstateRegistration.ERReceiptReason.Code)))
                {
                    EstateRegistration.Contragent = null;
                    EstateRegistration.ContragentID = null;
                }

                if (EstateRegistration.ERType != null && EstateRegistration.ERType.Code == "Docs")
                {
                    EstateRegistration.ERReceiptReason = null;
                    EstateRegistration.ERReceiptReasonID = null;
                }

                //проверка контрагента
                CheckContragent();

                switch (this.EstateRegistration.ERType.Code)
                {
                    case "AccountingObject":
                        ImportOS("OS-01", EstateRegistrationRowType.OS);
                        break;

                    case "NMA":
                        ImportOS("NMA", EstateRegistrationRowType.NMA);
                        break;

                    case "NKS":
                        ImportOS("NKS", EstateRegistrationRowType.NKS);
                        break;

                    case "OSArenda":
                        ImportOS("ArendaOS", EstateRegistrationRowType.ArendaOS);
                        break;

                    case "OSVGP":
                        SetVals();
                        ImportVGP();
                        break;

                    case "Docs":
                        ImportDocs(isPrimaryDocs: true);
                        break;

                    case "Union":

                        if (EstateRegistration.ERReceiptReason != null && EstateRegistration.ERReceiptReason.Code == "Union")
                        {
                            ImportOS("Union", EstateRegistrationRowType.Union);
                            foreach (var item in
                                Services.Estate.EstateRegistrationImport.CheckUnion(
                                this.UnitOfWork
                                , this.Rows
                                , this.EstateRegistration
                                ))
                            {
                                _ImportHistory.ImportErrorLogs.Add(item);
                            }                            
                            if (this.EstateRegistration.ClaimObject == null)
                            {
                                var firstObj = Rows.FirstOrDefault();
                                var nameEstateByDoc = firstObj.NameEstateByDoc;
                                if (String.IsNullOrWhiteSpace(nameEstateByDoc))
                                    nameEstateByDoc = Rows.FirstOrDefault(f => !String.IsNullOrEmpty(f.NameEstateByDoc))?.NameEstateByDoc;
                                if (firstObj != null)
                                {
                                    var claimObject = new EstateRegistrationRow();
                                    claimObject.NameEstateByDoc = nameEstateByDoc;
                                    claimObject.RowType = firstObj.RowType;
                                    claimObject.EstateDefinitionType = firstObj.EstateDefinitionType;
                                    this.EstateRegistration.ClaimObject = this.UnitOfWork.GetRepository<EstateRegistrationRow>().Create(claimObject);
                                }
                            }
                        }
                        else
                        {
                            ImportOS("Division", EstateRegistrationRowType.Division);
                            foreach (var item in
                                Services.Estate.EstateRegistrationImport.CheckDivision(
                                this.UnitOfWork
                                , this.Rows
                                , this.EstateRegistration
                                ))
                            {
                                _ImportHistory.ImportErrorLogs.Add(item);
                            };
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this._ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// Проверки контрагента.
        /// </summary>
        private void CheckContragent()
        {
            if (this.EstateRegistration.Contragent == null)
                return;

            var erType = this.EstateRegistration.ERType?.Code;
            var ersTypes = new List<string>() { "AccountingObject", "NMA", "NKS", "OSArenda" };
            var vgpType = "OSVGP";

            var og = this.EstateRegistration.Contragent?.SocietyID;
            var be = UnitOfWork.GetRepository<Society>()
                    .FilterAsNoTracking(f => f.ID == og)
                    .Include(inc => inc.ConsolidationUnit)
                    .FirstOrDefault()?.ConsolidationUnit;

            var isOG = (og != null);
            var connectToEUSI = (be?.ConnectToEUSI) ?? false;

            //если контаргент это ОГ подключенное к ЕУСИ, то заявка должна быть вида ВГП
            if (ersTypes.Contains(erType) && isOG && connectToEUSI)
                this.ImportHistory.ImportErrorLogs.AddError("Контрагент является обществом группы, подключенным к ЕУСИ, но вид объекта заявки не \"Внутригрупповые перемещения\"");

            //если заявка вида ВГП
            if (erType == vgpType)
            {
                //если ВГП аренда
                if (EstateRegistration.ERReceiptReason != null && EstateRegistration.ERReceiptReason.Code == "RentOut")
                {
                    //ошибка если контрагент – НЕ ОГ
                    if (!isOG)
                        this.ImportHistory.ImportErrorLogs
                            .AddError($"Контрагент не является обществом группы, но вид объекта заявки \"Внутригрупповые перемещения\" со способом поступления {EstateRegistration.ERReceiptReason.Name}.");
                }
                //иное = ВГП реализация
                else
                {
                    //контрагент д.б. ОГ, подключенное к ЕУСИ, иначе - ошибка
                    if (!(isOG && connectToEUSI))
                        this.ImportHistory.ImportErrorLogs
                            .AddError($"Контрагент не является обществом группы, или не подключен к ЕУСИ, но вид объекта заявки \"Внутригрупповые перемещения\" со способом поступления {EstateRegistration.ERReceiptReason?.Name}.");
                }
            }
        }

        private void ImportVGP()
        {
            ImportOS("OSVGP", EstateRegistrationRowType.VGP);
            var table = Tables
                .First(tab => tab.Rows[1].ItemArray[1].ToString() == "OSVGP");
            if (table == null)
                return;

            bool isArenda = (EstateRegistration.ERReceiptReason != null
               && EstateRegistration.ERReceiptReason.Code.Contains("Rent"));
            if (!isArenda)
                return;
        }

        public void SetVals()
        {
            var inn = EstateRegistration.Contragent?.INN;
            if (String.IsNullOrEmpty(inn))
                return;

            ContragentOG = UnitOfWork.GetRepository<Society>()
                .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.INN == inn)
                .FirstOrDefault();
            if (ContragentOG != null && ContragentOG.ConsolidationUnitID != null)
                ConsolidationOG = UnitOfWork.GetRepository<Consolidation>()
                    .FilterAsNoTracking(f => f.ID == ContragentOG.ConsolidationUnitID)
                    .FirstOrDefault();
            else
                ConsolidationOG = UnitOfWork.GetRepository<Consolidation>()
                    .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.INN == inn)
                    .FirstOrDefault();
            if (ContragentOG == null || ConsolidationOG == null)
            {
                if (ContragentOG == null)
                {
                    var str = $"Не найдено ОГ с ИНН <{inn}>";
                    if (this._ImportHistory != null)
                        this._ImportHistory.ImportErrorLogs.AddError(str);
                    else
                        throw new Exception(str);
                }

                if (ConsolidationOG == null)
                {
                    var str = $"Не удалось определить БЕ для ОГ с ИНН <{inn}>";
                    if (this._ImportHistory != null)
                        this._ImportHistory.ImportErrorLogs.AddError(str);
                    else
                        throw new Exception(str);
                }
                return;
            }
        }

        public void ExecuteVGP(bool isArenda)
        {
            //Если ВГП Аренда, то beCode принимает Арендатора, а не ОГ
            var beCode = (isArenda) ? this.EstateRegistration.Consolidation.Code
              : ConsolidationOG.Code;

            var eusiNumbs = Rows.Where(f => !String.IsNullOrEmpty(f.EUSINumber))
               .Select(s => s.EUSINumber)
               .Distinct();
            var oss = new System.Collections.Concurrent.ConcurrentBag<AccountingObject>();

            foreach (var numb in eusiNumbs)
            {
                try
                {
                    int num = Int32.Parse(numb);
                    var os = UnitOfWork.GetRepository<AccountingObject>()
                                .Filter(f =>
                                   !f.Hidden
                                   && !f.IsHistory
                                   && f.Estate != null
                                   && f.Estate.Number == num
                                   && f.Consolidation != null
                                   && f.Consolidation.Code == beCode)
                                 .ToList();
                    if (os.Count == 0)
                    {
                        var str = $"Для БЕ <{beCode}> не найдено ОС/НМА с номером ЕУСИ <{numb}>.";
                        if (this._ImportHistory != null)
                            this._ImportHistory.ImportErrorLogs
                                .AddError(str);
                        else
                            throw new Exception(str);
                    }
                    else
                    {
                        foreach (AccountingObject item in os)
                        {
                            var clone = CloneOS(item, isArenda);
                            clone.CreatingFromER = this.EstateRegistration?.Number;
                            clone.CreatingFromERPosition = Rows.Where(f => f.EUSINumber == numb)
                                                            .DefaultIfEmpty()
                                                            .FirstOrDefault()?.Position;
                            oss.Add(clone);
                            _UofW.GetRepository<AccountingObjectAndEstateRegistrationObject>()
                                .Create(new AccountingObjectAndEstateRegistrationObject()
                                {
                                    ObjLeftId = item.ID,
                                    ObjRigth = EstateRegistration
                                });

                            _UofW.GetRepository<AccountingObjectAndEstateRegistrationObject>()
                                .Create(new AccountingObjectAndEstateRegistrationObject()
                                {
                                    ObjLeft = clone,
                                    ObjRigth = EstateRegistration,
                                    IsPrototype = true
                                });

                            foreach (var doc in Docs)
                            {
                                _UofW.GetRepository<FileCardAndAccountingObject>()
                                .Create(new FileCardAndAccountingObject()
                                {
                                    ObjLeft = doc,
                                    ObjRigth = item
                                });
                                _UofW.GetRepository<FileCardAndAccountingObject>()
                                .Create(new FileCardAndAccountingObject()
                                {
                                    ObjLeft = doc,
                                    ObjRigth = clone
                                });
                            }
                        }

                        var est = _UofW.GetRepository<Estate>()
                            .Filter(f => !f.Hidden && !f.IsHistory && f.Number == num)
                            .FirstOrDefault();
                        if (est != null)
                        {
                            // Задача 10185: Реализовать автоматическое установление значания признака "За балансом" (реестр замечаний п.п. 39)
                            est.OutOfBalance = EUSIImportHelper.IsOutOfBalance(EstateRegistration.ERReceiptReason?.Code);

                            _UofW.GetRepository<EstateAndEstateRegistrationObject>()
                               .Create(new EstateAndEstateRegistrationObject()
                               {
                                   ObjLeftId = est.ID,
                                   ObjRigth = EstateRegistration
                               });

                            foreach (var doc in Docs)
                            {
                                _UofW.GetRepository<FileCardAndEstate>()
                                .Create(new FileCardAndEstate()
                                {
                                    ObjLeft = doc,
                                    ObjRigthId = est.ID
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ImportHistory != null)
                        ImportHistory.ImportErrorLogs.AddError(ex);
                    else
                        throw new Exception(ex.Message);
                }
            }
            this.UnitOfWork.GetRepository<AccountingObject>().CreateCollection(oss);
            AccountingObjects = oss.ToList();
            this.EstateRegistration.State =
                            this.UnitOfWork.GetRepository<EstateRegistrationStateNSI>()
                               .Filter(f =>
                               !f.Hidden
                               && !f.IsHistory
                               && f.Code == "COMPLETED"
                               ).FirstOrDefault();
        }

        private AccountingObject CloneOS(AccountingObject obj, bool isArenda)
        {
            AccountingObject os = new AccountingObject();
            os.ActualDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            os.EstateID = obj.EstateID;
            os.ConsolidationID = (isArenda) ? ConsolidationOG?.ID : EstateRegistration.Consolidation?.ID;
            os.OwnerID = (isArenda) ? ContragentOG?.ID : EstateRegistration.Society?.ID;
            os.ReceiptReasonID = (isArenda) ? UnitOfWork.GetRepository<ReceiptReason>()
                .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.Code == "Rent")
                .FirstOrDefault()?.ID : UnitOfWork.GetRepository<ReceiptReason>()
                .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.Code == EstateRegistration.ERReceiptReason.Code)
                .FirstOrDefault()?.ID;
            os.EstateDefinitionTypeID = obj.EstateDefinitionTypeID;
            os.IntangibleAssetTypeID = obj.IntangibleAssetTypeID;
            os.NameByDoc = obj.NameByDoc;
            os.NameEUSI = obj.NameEUSI;
            os.CadastralNumber = obj.CadastralNumber;
            os.DepreciationMethodMSFOID = obj.DepreciationMethodMSFOID;
            os.OKTMOID = obj.OKTMOID;
            os.PositionConsolidationID = obj.PositionConsolidationID;
            os.CadastralValue = obj.CadastralValue;
            os.LandPurposeID = obj.LandPurposeID;

            ///TODO: описать обработку атрибутов "ContragentID" или "LessorSubjectID" опираясь на признак "isArenda"
            ///Временное решение для показа 03.09.2018. При обсуждении на этапе анализа, проговаривали, что необходимо добавить в Заявку атрибут "Контрагент" и в него записывать значение в зависимости от вида Заявки ВГП
            os.ContragentID = (isArenda && EstateRegistration.Consolidation != null && !string.IsNullOrEmpty(EstateRegistration.Consolidation.INN)) ? UnitOfWork.GetRepository<Subject>()
                .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.INN == EstateRegistration.Consolidation.INN)
                .FirstOrDefault()?.ID : null;

            os.LessorSubjectID = (isArenda) ? EstateRegistration.Contragent?.ID : null;
            os.ProprietorSubjectID = (isArenda) ? EstateRegistration.Society?.ID : null;
            os.VehicleClassID = obj.VehicleClassID;
            os.InOtherSystem = obj.InOtherSystem;
            os.VehicleTypeID = obj.VehicleTypeID;
            os.YearOfIssue = obj.YearOfIssue;
            os.VehicleCategoryID = obj.VehicleCategoryID;
            os.DieselEngine = obj.DieselEngine;
            os.VehicleLabelID = obj.VehicleLabelID;
            os.SibMeasureID = obj.SibMeasureID;
            os.Power = obj.Power;
            os.SerialNumber = obj.SerialNumber;
            os.EngineSize = obj.EngineSize;
            os.VehicleModelID = obj.VehicleModelID;
            os.VehicleAverageCost = obj.VehicleAverageCost;
            os.VehicleTaxFactor = obj.VehicleTaxFactor;
            os.VehicleRegNumber = obj.VehicleRegNumber;
            os.VehicleMarketCost = obj.VehicleMarketCost;
            os.CadRegDate = obj.CadRegDate;
            if (!isArenda)
            {
                os.ContragentID = EstateRegistration.Contragent?.ID;
                os.StartDateUse = obj.StartDateUse;
                os.DepreciationMethodRSBUID = obj.DepreciationMethodRSBUID;
                os.DepreciationMethodNUID = obj.DepreciationMethodNUID;
                os.SPPCode = obj.SPPCode;
                os.OKOF2014ID = obj.OKOF2014ID;
                os.OKOF94ID = obj.OKOF94ID;
                os.EstateMovableNSIID = obj.EstateMovableNSIID;
                os.DepreciationGroupID = obj.DepreciationGroupID;
                os.SibCountryID = obj.SibCountryID;
                os.SibFederalDistrictID = obj.SibFederalDistrictID;
                os.RegionID = obj.RegionID;
                os.SibCityNSIID = obj.SibCityNSIID;
                os.Address = obj.Address;
                os.PermittedUseKindID = obj.PermittedUseKindID;
                os.PermittedByDoc = obj.PermittedByDoc;
                os.GroundCategoryID = obj.GroundCategoryID;
                os.AddonAttributeGroundCategoryID = obj.AddonAttributeGroundCategoryID;
                os.LandTypeID = obj.LandTypeID;
                os.Area = obj.Area;
                os.BuildingLength = obj.BuildingLength;
                os.PipelineLength = obj.PipelineLength;
                os.ShareRightNumerator = obj.ShareRightNumerator;
                os.ShareRightDenominator = obj.ShareRightDenominator;
                os.AddonOKOFID = obj.AddonOKOFID;
                os.DepositID = obj.DepositID;
                os.LicenseArea = obj.LicenseArea;
                os.WellCategoryID = obj.WellCategoryID;
                os.Bush = obj.Bush;
                os.Well = obj.Well;
                os.IsEnergy = obj.IsEnergy;
                os.EnergyLabelID = obj.EnergyLabelID;
                os.EnergyDocsExist = obj.EnergyDocsExist;
                os.TaxCadastralIncludeDate = obj.TaxCadastralIncludeDate;
                os.TaxCadastralIncludeDoc = obj.TaxCadastralIncludeDoc;
                os.IsSocial = obj.IsSocial;
                os.IsCultural = obj.IsCultural;
            }

            if (isArenda)
            {
                obj.LessorSubjectID = EstateRegistration.Contragent?.ID;
            }

            //устанавливаем статус = Прототип
            os.StateObjectRSBUID = UnitOfWork.GetRepository<StateObjectRSBU>()
                .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.Code != null && f.Code.ToLower() == "draft")
                .FirstOrDefault()?.ID;

            os.PrimaryDocNumber = EstateRegistration.PrimaryDocNumber;
            os.PrimaryDocDate = EstateRegistration.PrimaryDocDate;
            os.ContractNumber = EstateRegistration.ERContractNumber;
            os.ContractDate = EstateRegistration.ERContractDate;
            return os;
        }

        private void ImportOS(string listName, EstateRegistrationRowType rowType)
        {
            try
            {
                var table = Tables
                .First(tab => tab.Rows[1].ItemArray[1].ToString() == listName);

                if (table == null)
                {
                    this.ImportHistory.ImportErrorLogs.Clear();
                    this.ImportHistory.ImportErrorLogs.AddError("Файл не соответствует шаблону.");
                    return;
                }

                var startRow = GetStartDataRow(table);
                var endRow = GetEndDataRow(startRow, table);
                var erRows = new System.Collections.Concurrent.ConcurrentBag<EstateRegistrationRow>();
                var items = table.Rows.Cast<DataRow>()
                    .Where(row => table.Rows.IndexOf(row) >= startRow && table.Rows.IndexOf(row) <= endRow);
                var fields = GetHorizontalSysFileds(table);
                CheckHorizontRequiredFields(table);
                if (!CheckHorizontalSysTypes(table))
                    return;

                if (this._ImportHistory.ImportErrorLogs.Any())
                    return;
                int? orderIndex = GetOrderSysFileds(table);
                foreach (var item in items)
                {
                    try
                    {
                        var res = false;
                        foreach (KeyValuePair<int, string> entry in fields)
                        {
                            if (!item.ItemArray[entry.Key].Equals(System.DBNull.Value))
                            {
                                res = true;
                                break;
                            }
                        }
                        if (res)
                        {
                            if (!CheckRequiredRow(item, fields, rowType))
                                continue;

                            var rr = new EstateRegistrationRow();
                            rr.RowType = rowType;
                            rr.EstateRegistration = this.EstateRegistration;

                            if (rowType == EstateRegistrationRowType.NMA)
                                rr.EstateDefinitionTypeID = _UofW.GetRepository<EstateDefinitionType>()
                                    .FilterAsNoTracking(f => !f.Hidden && f.Code == "IntangibleAsset")
                                    .FirstOrDefault()?.ID;

                            var err = "";
                            if (orderIndex != null)
                            {
                                var order = ImportHelper.GetValue(this.UnitOfWork, typeof(int?), item.ItemArray[orderIndex.Value], ref err);
                                rr.Position = order as int?;
                            }
                            foreach (KeyValuePair<int, string> entry in fields)
                            {
                                PropertyInfo pr = rr.GetType().GetProperty(entry.Value);
                                if (pr != null && pr.SetMethod != null)
                                {
                                    var val = ImportHelper.GetValue(this.UnitOfWork, pr.PropertyType, item.ItemArray[entry.Key], ref err);
                                    pr.SetValue(rr, val);
                                }
                            }
                            if (String.IsNullOrEmpty(rr.NameEUSI))
                                rr.NameEUSI = rr.NameEstateByDoc;

                            if (rr.SibCountry == null)
                                rr.SibCountryID = _UofW.GetRepository<SibCountry>()
                                .FilterAsNoTracking(f => !f.Hidden && f.Code == "RU")
                                .FirstOrDefault()?.ID;
                            erRows.Add(rr);
                        }
                    }
                    catch (Exception ex)
                    {
                        ImportHistory.ImportErrorLogs.AddError(ex);
                    }
                }
                this.UnitOfWork.GetRepository<EstateRegistrationRow>().CreateCollection(erRows);
                Rows = erRows.ToList<EstateRegistrationRow>();

                bool isArenda = (EstateRegistration.ERReceiptReason != null
                                    && EstateRegistration.ERReceiptReason.Code.Contains("Rent"));
                ImportDocs(isVGPArenda: (rowType == EstateRegistrationRowType.VGP && isArenda));
            }
            catch (Exception ex)
            {
                this._ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// проверка обязательных полей для строки.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="fileds"></param>
        /// <param name="rowType"></param>
        /// <returns></returns>
        private bool CheckRequiredRow(DataRow item, Dictionary<int, string> fileds, EstateRegistrationRowType rowType)
        {
            bool rez = true;
            //набор обязательных полей в зависимости от типа заявки
            IList<string> requiredFileds = new List<string>();
            switch (rowType)
            {
                case EstateRegistrationRowType.OS:
                case EstateRegistrationRowType.ArendaOS:
                    requiredFileds = new List<string>() {
                        nameof(EstateRegistrationRow.EstateDefinitionType),
                        nameof(EstateRegistrationRow.NameEstateByDoc)
                    };
                    break;

                case EstateRegistrationRowType.NMA:
                    requiredFileds = new List<string>() {
                        nameof(EstateRegistrationRow.IntangibleAssetType),
                        nameof(EstateRegistrationRow.NameEstateByDoc)
                    };
                    break;

                case EstateRegistrationRowType.NKS:
                    requiredFileds = new List<string>() {
                        nameof(EstateRegistrationRow.EstateDefinitionType),
                        nameof(EstateRegistrationRow.NameEstateByDoc),
                        nameof(EstateRegistrationRow.StartDateUse)
                    };
                    break;

                case EstateRegistrationRowType.VGP:
                    requiredFileds = new List<string>() {
                          nameof(EstateRegistrationRow.EUSINumber)
                    };
                    break;

                case EstateRegistrationRowType.Union:
                    requiredFileds = new List<string>() {
                          nameof(EstateRegistrationRow.EUSINumber)
                    };
                    break;

                case EstateRegistrationRowType.Division:
                    requiredFileds = new List<string>() {
                          nameof(EstateRegistrationRow.EUSINumber)
                    };
                    break;

                default:
                    break;
            }

            //TODO: обработать отсутствующие обязательные поля.
            //if (requiredFileds.Except(fileds.Values).Any())

            foreach (var rfield in requiredFileds)
            {
                if (fileds.Values.Contains(rfield))
                {
                    var entryKey = fileds.FirstOrDefault(f => f.Value == rfield).Key;
                    if (item.ItemArray[entryKey].Equals(System.DBNull.Value))
                    {
                        rez = false;
                        this._ImportHistory.ImportErrorLogs.AddError(
                                     item.Table.Rows.IndexOf(item) + 1
                                   , entryKey + 1
                                   , GetHorizontalFiledName(entryKey, item.Table)
                                   , CorpProp.Helpers.ErrorType.Required
                                   , item.Table.TableName);
                    }
                }
            }

            return rez;
        }

        private void ImportDocs(bool isPrimaryDocs = false, bool isVGPArenda = false)
        {
            var table = Tables
               .First(tab => tab.Rows[1].ItemArray[1].ToString() == "Docs");

            if (!CheckHorizontalSysTypes(table))
                return;
            var startRow = GetStartDataRow(table);

            DeleteDescriptionRows(table, startRow);

            var files = new System.Collections.Concurrent.ConcurrentBag<FileCardOne>();
            var links = new System.Collections.Concurrent.ConcurrentBag<FileCardAndEstateRegistrationObject>();
            var items = table.Rows.Cast<DataRow>()
                .Where(row => table.Rows.IndexOf(row) >= startRow);
            var fields = GetHorizontalSysFileds(table);
            var categoryID = this.UnitOfWork.GetRepository<CardFolder>()
                .Filter(f => !f.Hidden).Min(m => m.ID);
            int rowNumber = 1;
            foreach (var item in items)
            {
                try
                {
                    var isnull = true;
                    foreach (KeyValuePair<int, string> entry in fields)
                    {
                        if (!item.ItemArray[entry.Key].Equals(System.DBNull.Value))
                        {
                            isnull = false;
                            break;
                        }
                    }
                    if (!isnull)
                    {
                        if (isPrimaryDocs && fields.Any(f => f.Value == "EUSINumber") && !IsDocCorrective)
                        {
                            //Номер ЕУСИ становится обязательным
                            var key = fields.Where(f => f.Value == "EUSINumber").FirstOrDefault().Key;
                            if (item.ItemArray[key].Equals(System.DBNull.Value))
                            {
                                this._ImportHistory.ImportErrorLogs.AddError(
                                     table.Rows.IndexOf(item) + 1
                                   , key + 1
                                   , GetHorizontalFiledName(key, table)
                                   , CorpProp.Helpers.ErrorType.Required
                                   , table.TableName);
                                continue;
                            }
                            //проверка типа
                            var number = 0;
                            if (!int.TryParse(item.ItemArray[key].ToString(), out number))
                            {
                                this._ImportHistory.ImportErrorLogs.AddError(
                                     table.Rows.IndexOf(item) + 1
                                   , key + 1
                                   , GetHorizontalFiledName(key, table)
                                   , CorpProp.Helpers.ErrorType.Type
                                   , table.TableName);
                                continue;
                            }

                            //проверка наличия в системе
                            if (isPrimaryDocs)
                            {
                                var code = this.EstateRegistration.Consolidation.Code;
                                var os = UnitOfWork.GetRepository<AccountingObject>()
                                   .Filter(f =>
                                      !f.Hidden
                                      && !f.IsHistory
                                      && f.Estate != null
                                      && f.Estate.Number == number
                                      && f.Consolidation != null
                                      && f.Consolidation.Code == code)
                                    .ToList();
                                if (os.Count == 0)
                                {
                                    this._ImportHistory.ImportErrorLogs.AddError(
                                        table.Rows.IndexOf(item) + 1
                                      , key + 1
                                      , GetHorizontalFiledName(key, table)
                                      , $"Для БЕ <{code}> не найдено ОС/НМА с номером ЕУСИ <{number}>."
                                      , CorpProp.Helpers.ErrorType.System
                                      , table.TableName);
                                    continue;
                                }
                            }
                        }

                        var file = new FileCardOne();
                        file.CategoryID = categoryID;
                        FileCardPermission fileCardPerm = this.UnitOfWork.GetRepository<FileCardPermission>()
                        .Filter(f => !f.Hidden && f.AccessModifier == AccessModifier.Everyone)
                        .FirstOrDefault();
                        file.FileCardPermission = fileCardPerm ?? null;
                        file.FileCardPermissionID = fileCardPerm?.ID;
                        foreach (KeyValuePair<int, string> entry in fields)
                        {
                            var err = "";
                            PropertyInfo pr = file.GetType().GetProperty(entry.Value);
                            if (pr != null && pr.SetMethod != null)
                            {
                                var val = ImportHelper.GetValue(this.UnitOfWork, pr.PropertyType, item.ItemArray[entry.Key], ref err);
                                pr.SetValue(file, val);
                            }
                        }

                        if (rowNumber == 1 && !IsDocCorrective)
                        {
                            EstateRegistration.PrimaryDocNumber = file.Number;
                            EstateRegistration.PrimaryDocDate = file.FileCardDate;
                        }

                        if (!IsDocCorrective || (IsDocCorrective && rowNumber != 1 && !ExistFileDoc(file)))
                        {
                            files.Add(file);
                            links.Add(new FileCardAndEstateRegistrationObject()
                            {
                                ObjLeft = file,
                                ObjRigth = this.EstateRegistration
                            });
                        }

                        rowNumber++;
                    }
                }
                catch (Exception ex)
                {
                    ImportHistory.ImportErrorLogs.AddError(ex);
                }
            }
            this.UnitOfWork.GetRepository<FileCardOne>().CreateCollection(files);
            this.UnitOfWork.GetRepository<FileCardAndEstateRegistrationObject>().CreateCollection(links);
            Docs = files.ToList<FileCard>();
        }

        /// <summary>
        /// Ищет в документах оригинальной заявки файл по составному ключу: тип, номер, дата.
        /// </summary>
        /// <param name="doc">Импортируемый документ.</param>
        /// <returns></returns>
        private bool ExistFileDoc(FileCard doc)
        {
            var q = UnitOfWork.GetRepository<FileCardAndEstateRegistrationObject>()
                .FilterAsNoTracking(f =>
                    !f.Hidden
                    && f.ObjRigthId == this.EstateRegistration.ID
                    && f.ObjLeft != null
                    && f.ObjLeft.Number == doc.Number
                    && f.ObjLeft.FileCardDate == doc.FileCardDate);

            var ftype = doc.FileCardType?.Code;
            if (!String.IsNullOrWhiteSpace(ftype))
                q.Where(f => f.ObjLeft.FileCardType != null && f.ObjLeft.FileCardType.Code == ftype);
            else
                q.Where(f => f.ObjLeft.FileCardType == null);

            return q.Any();
        }

        /// <summary>
        /// Удаляет строки описания (Основные документы, Дополнительные документы)
        /// </summary>
        /// <param name="table">Лист Excel-документа "Прилагаемые документы"</param>
        /// <param name="startRow">Первая строка после хедера таблицы</param>
        private static void DeleteDescriptionRows(DataTable table, int startRow)
        {
            table.Rows.Remove(table.Rows[startRow]);
            table.Rows.Remove(table.Rows[startRow + 1]);

            table.AcceptChanges();
        }

        private Dictionary<int, string> GetHorizontalSysFileds(DataTable table)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            var row = table.Rows.Cast<DataRow>()
                .Where(f => f.ItemArray.Where(col => col != null && col.ToString() == "SystemField")
                .Any()).FirstOrDefault();
            var filedColIndex = GetSysFiledColumnIndex(table);

            for (int i = filedColIndex + 1; i < row.ItemArray.Length; i++)
            {
                if (!row.ItemArray[i].Equals(System.DBNull.Value) && !row.ItemArray[i].Equals("order"))
                    dict.Add(i, row.ItemArray[i].ToString());
            }

            return dict;
        }

        private int? GetOrderSysFileds(DataTable table)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            var row = table.Rows.Cast<DataRow>()
                .Where(f => f.ItemArray.Where(col => col != null && col.ToString() == "SystemField")
                .Any()).FirstOrDefault();
            var filedColIndex = GetSysFiledColumnIndex(table);

            for (int i = filedColIndex + 1; i < row.ItemArray.Length; i++)
            {
                if (!row.ItemArray[i].Equals(System.DBNull.Value) && row.ItemArray[i].Equals("order"))
                    return i;
            }

            return null;
        }

        private DataRow GetRowBySystemFieldName(DataTable table, string sysFieldName)
        {
            if (string.IsNullOrEmpty(sysFieldName))
                return null;

            var sysFieldIndex = GetSysFiledColumnIndex(table);

            return table.Rows.Cast<DataRow>()
                .FirstOrDefault(r => r.ItemArray[sysFieldIndex].ToString() == sysFieldName);
        }

        public void ImportMainData()
        {
            try
            {
                CreateER();

                DataTable table = Tables[0];
                var filedColIndex = GetSysFiledColumnIndex(table);
                var valueIndex = GetValueColumnIndex(table);
                var litValues = new List<string>() { "да", "yes", "true", "1" };

                for (int i = startRow; i <= endRow; i++)
                {
                    var row = table.Rows[i];

                    if (row.ItemArray[filedColIndex].Equals(DBNull.Value))
                    {
                        continue;
                    }

                    PropertyInfo pr = typeof(EstateRegistration).GetProperty(row.ItemArray[filedColIndex].ToString());

                    if (pr == null)
                    {
                        continue;
                    }

                    object val = row.ItemArray[valueIndex];
                    string err = "";
                    object value = null;

                    if (val == System.DBNull.Value)
                    {
                        value = null;
                    }
                    else if (pr.PropertyType.Equals(typeof(Society)))
                    {
                        string strVal = val.ToString();

                        value = UnitOfWork.GetRepository<Society>()
                            .Filter(f => !f.Hidden && !f.IsHistory && f.INN == strVal)
                            .FirstOrDefault();
                    }
                    else if (pr.PropertyType.Equals(typeof(Subject)))
                    {
                        string strVal = val.ToString();

                        value = UnitOfWork.GetRepository<Subject>()
                            .Filter(f => !f.Hidden && !f.IsHistory && f.INN == strVal)
                            .FirstOrDefault();
                    }
                    else if (pr.PropertyType.Equals(typeof(EstateRegistrationOriginator)))
                    {
                        value = FindOrCreateOriginator(table, filedColIndex, valueIndex);
                    }
                    else if (pr.PropertyType.Equals(typeof(bool)))
                    {
                        value = !string.IsNullOrEmpty(val.ToString()) && litValues.Contains(val.ToString().ToLower());
                    }
                    else
                    {
                        value = ImportHelper.GetValue(UnitOfWork, pr.PropertyType, val, ref err);
                    }

                    pr.SetValue(EstateRegistration, value);
                }

                if (Wizard != null)
                {
                    this.EstateRegistration.NumberCDS = Wizard.NumberCDS;
                    this.EstateRegistration.ERControlDateAttributes.DateCDS = Wizard.DateCDS;
                    this.EstateRegistration.QuickClose = Wizard.QuickClose;
                    this.ImportHistory.NumberCDS = Wizard.NumberCDS;
                }

                this.ImportHistory.ContactEmail = this.EstateRegistration.ContactEmail;
                this.ImportHistory.ContactPhone = this.EstateRegistration.ContactPhone;
                if (String.IsNullOrEmpty(this.EstateRegistration.ContacName))
                    this.EstateRegistration.ContacName = this.EstateRegistration.Originator?.Name;
                this.ImportHistory.ContactName = this.EstateRegistration.ContacName;
                this.ImportHistory.SocietyID = this.EstateRegistration.SocietyID ?? this.EstateRegistration.Society?.ID;
            }
            catch (Exception ex)
            {
                this._ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// Находит или создаёт инициатора заявки.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="filedColIndex"></param>
        /// <param name="valueIndex"></param>
        /// <returns></returns>
        private object FindOrCreateOriginator(DataTable table, int filedColIndex, int valueIndex)
        {
            object originator = null;

            DataRow consolidationRow = table.Select($"Column{filedColIndex} = 'Consolidation'").FirstOrDefault();
            DataRow emailRow = table.Select($"Column{filedColIndex} = 'ContactEmail'").FirstOrDefault();
            DataRow originatorRow = table.Select($"Column{filedColIndex} = 'Originator'").FirstOrDefault();

            //По полному совпадению  ФИО+БЕ+Email инициатора из файла заявки ЕУСИ
            string fio = originatorRow.ItemArray[valueIndex].ToString();
            string consolidationCode = consolidationRow.ItemArray[valueIndex].ToString();
            string email = emailRow.ItemArray[valueIndex].ToString();

            originator = UnitOfWork
                .GetRepository<EstateRegistrationOriginator>()
                .Filter(f =>
                    !f.Hidden &&
                    f.Name == fio &&
                    f.ContactEmail == email &&
                    f.Consolidation != null &&
                    f.Consolidation.Code == consolidationCode)
                .FirstOrDefault();

            if (originator == null)
            {
                var consolidationID = UnitOfWork
                    .GetRepository<Consolidation>()
                    .FilterAsNoTracking(c => !c.Hidden && c.Code == consolidationCode)
                    .FirstOrDefault()?.ID;

                originator = UnitOfWork
                    .GetRepository<EstateRegistrationOriginator>()
                    .Create(new EstateRegistrationOriginator
                    {
                        Name = fio,
                        ConsolidationID = consolidationID,
                        ContactEmail = email
                    });
            }
            return originator;
        }

        public CorpProp.Entities.Common.CheckImportResult CheckConfirmResultERNumber(DataTable table)
        {
            var startRow = GetStartDataRow(table);
            var endRow = GetEndDataRow(startRow, table);
            var filedColIndex = GetSysFiledColumnIndex(table);
            var valueIndex = GetValueColumnIndex(table);

            var value = table.Rows.Cast<DataRow>()
                .FirstOrDefault(row => table.Rows.IndexOf(row) >= startRow && table.Rows.IndexOf(row) <= endRow
                && row.ItemArray[filedColIndex] != null && row.ItemArray[filedColIndex].ToString() == nameof(EstateRegistration.Number))
                ?[valueIndex];

            string err = "";
            var val = (int?)ImportHelper.GetValue(this.UnitOfWork, typeof(int?), value, ref err);

            if (val != null && UnitOfWork.GetRepository<EstateRegistration>()
                    .FilterAsNoTracking(f => !f.Hidden && f.Number == val.Value)
                    .Any())
            {
                CheckImportResult res = new CheckImportResult();
                res.IsConfirmationRequired = true;
                res.ConfirmationItemDescription = $"№ {val}";
                return res;
            }

            return null;
        }

        private bool CheckNumberER()
        {
            var table = Tables[0];
            var valueIndex = GetValueColumnIndex(table);
            var erNumberRow = GetRowBySystemFieldName(table, nameof(EUSI.Entities.Estate.EstateRegistration.Number));
            var erNumber = erNumberRow?.ItemArray[valueIndex]?.ToString();

            //вид объекта заявки
            var erTypeRow = GetRowBySystemFieldName(table, nameof(EUSI.Entities.Estate.EstateRegistration.ERType));
            var erType = erTypeRow?.ItemArray[valueIndex]?.ToString();

            if (!String.IsNullOrEmpty(erNumber))
            {
                var reg = _UofW.GetRepository<EstateRegistration>()
                    .Filter(f =>
                    !f.Hidden
                    && !f.IsHistory
                    && f.Number.ToString() == erNumber)
                    .Include(inc => inc.State)
                    .Include(inc => inc.ERType)
                    .Include(inc => inc.ERReceiptReason)
                    .Include(inc => inc.Consolidation)
                    .Include(inc => inc.Society)
                    .Include(inc => inc.Contragent)
                    .FirstOrDefault();

                if (reg != null)
                {
                    // если значения поля "Быстрое закрытие" == true, отклоняем импорт
                    if (reg.QuickClose)
                    {
                        _ImportHistory.ImportErrorLogs
                            .AddError("Импорт заявки отклонён, " +
                                      "корректировка заявки с признаком \"Быстрое закрытие\" невозможна");
                        return false;
                    }
                    // если значения поля "Вид объекта заявки" == "Объединение/разукрупнение" , отклоняем импорт
                    if (reg.ERType.Code == "Union")
                    {
                        _ImportHistory.ImportErrorLogs
                            .AddError("Импорт заявки отклонён, " +
                                      "корректировка заявки вида \"Объединение/Разукрупнение\" невозможна");
                        return false;
                    }

                    this.EstateRegistration = reg;
                    if (reg.State == null || (reg.State != null && reg.State.Code.ToUpper() == "REDIRECTED"))
                    {
                        EstateRegistrationStateNSI clarifiedState = _UofW.GetRepository<EstateRegistrationStateNSI>()
                                .Filter(nsi => !nsi.Hidden && nsi.Code.ToUpper() == "CLARIFIED")
                                .FirstOrDefault();

                        if (clarifiedState != null)
                        {
                            EstateRegistration.State = clarifiedState;
                            EstateRegistration.StateID = clarifiedState?.ID;
                        }
                        if (!String.IsNullOrWhiteSpace(erType) && erType.Trim().ToLower() == "первичные документы")
                        {
                            IsDocCorrective = true;
                        }
                        else
                            _erService.DeleteRows(_UofW, reg);
                    }
                    else
                        _ImportHistory.ImportErrorLogs.AddError($"Невозможно обновить заявку в статусе <{reg.State.Name}>");
                }
                else
                    _ImportHistory.ImportErrorLogs.AddError($"В Системе не найдена заявка с номером <{erNumber}>");
            }
            return !_ImportHistory.ImportErrorLogs.Any();
        }
    }
}
using Base.DAL;
using Base.Service;
using Base.Service.Log;
using CorpProp.Entities.Import;
using CorpProp.Entities.Law;
using CorpProp.Entities.Security;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Base;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CorpProp.Services.Law
{
    /// <summary>
    /// Предоставляет данные и методы сервиса объекта - ГГР.
    /// </summary>
    public interface IScheduleStateTerminateService : ITypeObjectService<ScheduleStateTerminate>//, IExcelImportEntity
    {

    }

    /// <summary>
    /// Представляет сервис для работы с объектом - ГГР.
    /// </summary>
    public class ScheduleStateTerminateService : TypeObjectService<ScheduleStateTerminate>, IScheduleStateTerminateService
    {
        private readonly ILogService _logger;
        private readonly ScheduleStateTerminateRecordService _recordService;
        /// <summary>
        /// Инициализирует новый экземпляр класса ScheduleStateTerminateService.
        /// </summary>
        /// <param name="facade"></param>
        public ScheduleStateTerminateService(IBaseObjectServiceFacade facade, ScheduleStateTerminateRecordService recordService, ILogService logger) : base(facade, logger)
        {
            _logger = logger;
            _recordService = recordService;

        }

        /// <summary>
        /// Переопределяет метод при событии создания объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="obj">Создаваемый объект.</param>
        /// <returns>ГГР.</returns>
        public override ScheduleStateTerminate Create(IUnitOfWork unitOfWork, ScheduleStateTerminate obj)
        {
            return base.Create(unitOfWork, obj);
        }

        /// <summary>
        /// Переопределяет метод при событии сохранения объекта.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="objectSaver">Метаданные сохраняемого объекта.</param>
        /// <returns></returns>
        protected override IObjectSaver<ScheduleStateTerminate> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<ScheduleStateTerminate> objectSaver)
        {
            if (objectSaver != null && objectSaver.Dest != null)
                objectSaver.Dest.Name =
                     objectSaver.Dest.Year?.ToString() + " "
                    + objectSaver.Dest.Society?.ShortName + " "
                    + " ГГР прекращения права"
                    ;
            return
                base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneObject(x => x.Society)
                    .SaveOneObject(x => x.EmployeeUploadedData)
                    .SaveOneObject(x => x.ScheduleStateYear)
                    .SaveOneObject(x => x.ScheduleStateRegistrationStatus)                  
                    ;
        }

        /// <summary>
        /// Переопределяет метод при событии получения всех элементов.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="hidden">Объекты являются удаленными.</param>
        /// <returns>Коллекция ГГРП.</returns>
        public override IQueryable<ScheduleStateTerminate> GetAll(IUnitOfWork unitOfWork, bool? hidden = false)
        {
            
            return base.GetAll(unitOfWork, hidden);
        }

        /// <summary>
        /// Импорт ГГР из файла Excel.
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="reader"></param>
        /// <param name="error"></param>
        /// <param name="count"></param>
        public void Import(IUnitOfWork uofw, IUnitOfWork histUnitOfWork, IExcelDataReader reader, Type type, ref int count, ref ImportHistory history)
        {
            try
            {
                var tables = reader.GetVisbleTables();
                DataTable entryTable = tables[0];
                ImportStarter.DeleteEmptyRows(ref entryTable);
                if (type != null && Type.Equals(type, typeof(ScheduleStateTerminate)))
                {
                    Dictionary<string, string> colsNameMapping = ImportHelper.ColumnsNameMapping(entryTable);
                                        
                    int start = ImportHelper.GetRowStartIndex(entryTable);
                    for (int i = start; i < entryTable.Rows.Count; i++)
                    {
                        var rowError = "";
                        var row = entryTable.Rows[i];

                        ScheduleStateTerminate item = ImportObject(uofw, row, colsNameMapping, ref rowError, ref count, ref history);

                        if (item != null)
                        {
                            new ImportChecker().StartDataCheck(uofw, histUnitOfWork, tables[1], typeof(ScheduleStateTerminateRecord), ref history);

                            if (history.ImportErrorLogs.Count > 0)
                            {
                                throw new ImportException("Ошибки при проверке.");
                            }
                            _recordService.Import(uofw, reader, ref count, ref history, item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uofw"></param>
        /// <param name="numb"></param>
        /// <param name="idEup"></param>
        /// <returns></returns>
        public List<ScheduleStateTerminate> FindObjects(IUnitOfWork uofw, string idEup, int year)
        {
            List<ScheduleStateTerminate> list = new List<ScheduleStateTerminate>();
            list = uofw.GetRepository<ScheduleStateTerminate>().Filter(x =>
            (x.Society != null && x.Society.IDEUP == idEup) && x.Year == year && !x.Hidden).ToList<ScheduleStateTerminate>();
            return list;
        }

        /// <summary>
        /// Имопртирует ОБУ из строки файла.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="row">Строка файла.</param>      
        /// <param name="error">Текст ошибки.</param>
        /// <param name="count">Количество импортированных объектов.</param>
        public ScheduleStateTerminate ImportObject(IUnitOfWork uofw, DataRow row, Dictionary<string, string> colsNameMapping, ref string error, ref int count, ref ImportHistory history)
        {
            try
            {
                bool isNew = true;
                ScheduleStateTerminate obj = null;

                //читаем инв номер и ИД ЕУП               
                var year = ImportHelper.GetValueByName(uofw, typeof(int), row, "Year", colsNameMapping);
                var IDEUP = ImportHelper.GetValueByName(uofw, typeof(string), row, "Society", colsNameMapping);

                //ищем в Системе ОБУ
                if (IDEUP != null && year != null
                    && !String.IsNullOrEmpty(year.ToString())
                    && !String.IsNullOrEmpty(IDEUP.ToString()))
                {
                    string ideup = ImportHelper.GetIDEUP(IDEUP);
                    //TODO: почистить
                    List<ScheduleStateTerminate> list = FindObjects(uofw, ideup, int.Parse(year.ToString()));
                    if (list == null || list.Count == 0)
                        obj = new ScheduleStateTerminate();
                    else if (list.Count > 1)
                        error += $"Невозможно обновить объект. В Системе найдено более одной записи.{System.Environment.NewLine}";
                    else if (list.Count == 1)
                    {
                        isNew = false;
                        obj = list[0];
                    }
                    if (obj != null)
                    {
                        obj.FillObject(uofw, typeof(ScheduleStateTerminate),
                           row, row.Table.Columns, ref error, ref history, colsNameMapping);
                        obj.ImportDate = DateTime.Now;

                        if (history.SibUser != null)
                            obj.EmployeeUploadedData = GetUploader(uofw, history.SibUser.ID);
                    }

                    if (!String.IsNullOrEmpty(error))
                        obj = null;
                    else
                    {
                        if (obj != null)
                        {
                            count++;
                            if (isNew)
                                return this.Create(uofw, obj);
                            else
                                return this.Update(uofw, obj);
                        }
                    }
                }
                else
                {
                    error += $"Неверное значение года, ИД ЕУП или типа. {System.Environment.NewLine}";
                    obj = null;
                }
                return null;
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
                return null;
            }
        }
        private SibUser GetUploader(IUnitOfWork uofw, int userId)
        {
            return uofw.GetRepository<SibUser>().Find(f => f.ID == userId);
        }
    }
}

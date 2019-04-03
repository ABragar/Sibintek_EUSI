using Base.DAL;
using Base.Service;
using Base.UI.Service;
using CorpProp.Entities.Document;
using CorpProp.Entities.Import;
using ExcelDataReader;
using System.Collections.Generic;
using System.Data;
using System.IO;
using CorpProp.Entities.Common;

namespace CorpProp.Common
{
    //TODO: перенести IExcelImportEntity в п/с импорта/экспорта.
    /// <summary>
    /// Предоставляет методы импорта объектов из файлов Excel с помощью библиотеки ExcelDataReader.
    /// </summary>
    public interface IExcelImportEntity : IService
    {
        /// <summary>
        /// Импорт из файла Excel.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="histUofw">Сессия лога импорта.</param>
        /// <param name="table">Таблица.</param>
        /// <param name="colsNameMapping">Мэппинг имен колонок.</param>
        /// <param name="count">Кол-во обработанных объектов.</param>
        /// <param name="history">Лог импорта.</param>
        void Import(
             IUnitOfWork uofw
            , IUnitOfWork histUofw
            , DataTable table
            , Dictionary<string, string> colsNameMapping
            , ref int count
            , ref ImportHistory history
            );

        /// <summary>
        /// Отмена импорта.
        /// </summary>        
        void CancelImport(
             IUnitOfWork uofw            
            , ref ImportHistory history
            );
    }

    /// <summary>
    /// Предоставляет методы импорта объектов из файлов XML.
    /// </summary>
    public interface IXmlImportEntity : IService
    {
        
        IImportHolder Holder { get; }

        /// <summary>
        /// Импорт из файла XML.
        /// </summary>        
        void CreateHolder(
            IUnitOfWork uow
            , IUnitOfWork histUow
            , StreamReader reader
            , FileCardOne file
            );
                
    }

    //TODO: удалить, сделать натсройку идентификации объектов в шаблонах импорта.
    public interface ISystemImportEntity : IService
    {
        /// <summary>
        /// Возвращает признак идентификации справочников типа DictObject по коду.
        /// </summary>
        bool IdentyDictByCode();        

    }

    public interface IImportStarter : IService
    {
        /// <summary>
        /// Инициирует импорт.
        /// </summary>
        /// <param name="uiFacade">Общий сервис контроллера для получения сервисов объектов Системы.</param>
        /// <param name="unitOfWork">Сессия для импортируемых объектов.</param>
        /// <param name="histUnitOfWork">Отдельная сессия для истории импорта</param>
        /// <param name="reader">Источник.</param>
        /// <param name="error">Ошибки.</param>
        /// <param name="count">Кол-во обработанных объектов.</param>
        /// <param name="history">Экземпляр истории импорта.</param>
        /// <param name="fileName"></param>
        void Import(
              IUiFasade uiFacade
            , IUnitOfWork unitOfWork
            , IUnitOfWork histUnitOfWork
            , IExcelDataReader reader
            , ref string error
            , ref int count
            , ref ImportHistory history
            , string fileName = "");

        CheckImportResult CheckImport(
           IUiFasade uiFacade,
           IExcelDataReader reader,
           ITransactionUnitOfWork uofw,
           IUnitOfWork histUnitOfWork,
           StreamReader stream,
           string fileName,
           ImportHistory importHistory
       );
    }

    public interface IImportHolder
    {

        IUnitOfWork UnitOfWork { get; }

        IUnitOfWork UofWHistory { get; }

        ImportHistory ImportHistory { get; }


        void Import();
        void CreateImportHistory();
    }
}

using Base.DAL;
using Base.Service;
using Base.Utils.Common.Wrappers;
using CorpProp.Common;
using CorpProp.Entities.Document;
using CorpProp.Entities.Import;
using CorpProp.Entities.Law;
using CorpProp.RosReestr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Helpers
{    

    /// <summary>
    /// Основные данные импорта xml-выписки.
    /// </summary>
    public partial class ImportHolder : IImportHolder
    {
        private IUnitOfWork _UofW;
        private IUnitOfWork _UofWHistory;
        private ImportHistory _ImportHistory;
        private FileCard _file;
        private string _fileName;
        private int? _userID;

        /// <summary>
        /// Инициализирует новый экземпляр класса ImportHolder.
        /// </summary>
        public ImportHolder(            
             FileCard attachFile
            , IUnitOfWork uofw
            , IUnitOfWork uofwhistory
            , string fileName
            , int? userID)
        {
            
            _UofW = uofw;
            _UofWHistory = uofwhistory;
            _file = attachFile;
            _userID = userID;
            _fileName = fileName;
            CreateImportHistory();
        }

        /// <summary>
        /// Получает сессию объектов выписки.
        /// </summary>
        public IUnitOfWork UnitOfWork { get { return _UofW; }  }

        /// <summary>
        /// Получает сессию истории импорта.
        /// </summary>
        public IUnitOfWork UofWHistory { get { return _UofWHistory; } }

        /// <summary>
        /// Получает или задает выписку.
        /// </summary>
        public Extract Extract { get; set; } 

        /// <summary>
        /// Получает или задает историю импорта.
        /// </summary>
        public ImportHistory ImportHistory { get { return _ImportHistory; } }

        /// <summary>
        /// Получает или задает отчет импорта.
        /// </summary>
        public string Report { get; set; }

        public void Import()
        {

        }
        
    }
}

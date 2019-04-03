using Base.DAL;
using Base.Utils.Common.Wrappers;
using CorpProp.Entities.Law;
using CorpProp.Entities.Security;
using CorpProp.RosReestr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Migration
{
    /// <summary>
    /// Основные данные миграции xml-выписки.
    /// </summary>
    public partial class MigrateHolder
    {
        private IUnitOfWork _UofW;
        private IUnitOfWork _UofWHistory;
        private MigrateHistory _MigrateHistory;
        private Extract _Extract;

        /// <summary>
        /// Инициализирует новый экземпляр класса MigrateHolder.
        /// </summary>
        public MigrateHolder (  
              IUnitOfWork uofw
            , IUnitOfWork uofwhistory
            , int? extractID
            , int? userID)
        {
            MigrateLogs = new List<MigrateLog>() { };
            _UofW = uofw;
            _UofWHistory = uofwhistory;           
            CreateHistory(extractID, userID);

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
        /// Получает выписку.
        /// </summary>
        public Extract Extract { get { return _Extract; } }

        /// <summary>
        /// Получает историю миграции.
        /// </summary>
        public MigrateHistory MigrateHistory { get { return _MigrateHistory; } }

        /// <summary>
        /// Получает или задает логи миграции.
        /// </summary>
        public List<MigrateLog> MigrateLogs { get; set; }

        

        public void SetExtract(Extract ext)
        {
            _Extract = ext;
            if (_Extract != null && _MigrateHistory != null && _MigrateHistory.Extract == null)
            {
                var extractID = _Extract.ID;
                _MigrateHistory.Extract = UofWHistory.GetRepository<Extract>()
                    .Filter(x => x.ID == extractID).FirstOrDefault();
                
            }
        }


        /// <summary>
        /// Создает экземпляр истории миграции.
        /// </summary>
        private void CreateHistory(
             int? extractID            
            , int? userID)
        {
            _MigrateHistory = this.UofWHistory.GetRepository<MigrateHistory>()
                .Create(new MigrateHistory());

           if (extractID != null)
            {
                _MigrateHistory.Extract = UofWHistory.GetRepository<Extract>().Filter(x => x.ID == extractID).FirstOrDefault();
                _Extract = this.UofWHistory.GetRepository<Extract>().Filter(x => x.ID == extractID).FirstOrDefault();
            }
               
            
            if (userID != null)
            {
                SibUser us = UofWHistory.GetRepository<SibUser>().Filter(x => x.UserID == userID).FirstOrDefault();
                if (us != null)
                    _MigrateHistory.SibUser = us;
            }

        }

        /// <summary>
        /// Добавляет строку журнала миграции.
        /// </summary>
        /// <param name="mnemonic">Мнемоника объекта.</param>
        /// <param name="note">Описание объекта.</param>
        /// <param name="code">Код статуса миграции.</param>
        public void AddLog(string mnemonic, string note, string code)
        {
            var log = UofWHistory.GetRepository<MigrateLog>().Create(new MigrateLog());
            log.Mnemonic = mnemonic;
            log.Description = note;
            log.MigrateHistory = MigrateHistory;
            log.MigrateState = UofWHistory.GetRepository<MigrateState>().Filter(f => !f.Hidden && f.Code == code).FirstOrDefault();
            this.MigrateLogs.Add(log);

        }

        public void AddError(string mnemonic, string note, string text)
        {
            var log = UofWHistory.GetRepository<MigrateLog>().Create(new MigrateLog());
            log.Mnemonic = mnemonic;
            log.Description = note;
            log.MigrateHistory = MigrateHistory;
            log.ErrorText = text;
            log.MigrateState = UofWHistory.GetRepository<MigrateState>().Filter(f => !f.Hidden && f.Code == "103").FirstOrDefault();
            this.MigrateLogs.Add(log);

        }

        public void AddInfo(string mnemonic, string note, string text)
        {
            var log = UofWHistory.GetRepository<MigrateLog>().Create(new MigrateLog());
            log.Mnemonic = mnemonic;
            log.Description = note;
            log.MigrateHistory = MigrateHistory;
            log.ErrorText = text;
            log.MigrateState = UofWHistory.GetRepository<MigrateState>().Filter(f => !f.Hidden && f.Code == "104").FirstOrDefault();
            this.MigrateLogs.Add(log);

        }


    }
}

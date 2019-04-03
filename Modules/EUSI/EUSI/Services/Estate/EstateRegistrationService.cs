using Base;
using Base.Attributes;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Mail.Entities;
using Base.Service;
using Base.Service.Log;
using Base.Settings;
using Base.UI.Service;
using CorpProp.Entities.Common;
using CorpProp.Entities.Import;
using CorpProp.Extentions;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Settings;
using EUSI.Entities.Estate;
using EUSI.Entities.ManyToMany;
using EUSI.Export;
using EUSI.Import;
using EUSI.Services.Accounting;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EUSI.Services.Estate
{
    public class EstateRegistrationService : EstateRegistrationBpService, IEstateRegistrationService
    {
        
        IFileSystemService _fileSystemService;
        ISibEmailService _emailService;
        private readonly ILogService _logger;

        public EstateRegistrationService(IBaseObjectServiceFacade facade
            , IFileSystemService fileSystemService
            , IAccountingObjectExtService accountingObjectService
            , ISibEmailService emailService
            , IWorkflowService workflow
            , ISettingService<MailSetting> setting
            , ILogService logger) : 
            base(facade, accountingObjectService, emailService, workflow, setting, logger)
        {            
            _fileSystemService = fileSystemService;
            _emailService = emailService;
            _logger = logger;
        }

        void InvalidFieldThrow(string propertyName, string messageEnding)
        {
            var propDisplayName =
                (typeof(EstateRegistration).GetProperty(propertyName)?.
                                            GetCustomAttribute(typeof(DetailViewAttribute)) as DetailViewAttribute)?.Name;
            throw new Exception($"{propDisplayName} {messageEnding}");
        }

       

        public override EstateRegistration Create(IUnitOfWork unitOfWork, EstateRegistration obj)
        {
           
            //obj.OriginatorID = AppContext.SecurityUser.FindLinkedSibUser(unitOfWork)?.ID;
            return base.Create(unitOfWork, obj);
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
                ERImportHolder holder = new ERImportHolder(
                    history.FileCard
                    , uofw
                    , histUofw
                    , table.DataSet
                    , ""
                    , Base.Ambient.AppContext.SecurityUser.GetSibUserID(uofw)
                    , WFService
                    , this
                    , _emailService
                    , null);
                holder.SetHistory(history);
                holder.Import();
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

               

        public void CancelImport(
              IUnitOfWork uow
             , ref ImportHistory history
             )
        {
           
        }

        public void CustomImport(
           IUnitOfWork uofw
           , IUnitOfWork histUofw
           , DataTable table
           , Dictionary<string, string> colsNameMapping
           , ERImportWizard wizard
           , ref int count
           , ref ImportHistory history)
        {
            try
            {
                ERImportHolder holder = new ERImportHolder(
                    history.FileCard
                    , uofw
                    , histUofw
                    , table.DataSet
                    , ""
                    , Base.Ambient.AppContext.SecurityUser.GetSibUserID(uofw)
                    , WFService
                    , this
                    , _emailService
                    , wizard
                    );
                holder.SetHistory(history);
                holder.Import();
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        public void StartDataCheck(IUnitOfWork uofw, IUnitOfWork histUow, DataTable table, Type type, 
            ref ImportHistory history, bool dictCode = false)
        {
            return;
        }

        public string FormatConfirmImportMessage(List<string> fileDescriptions)
        {
            var singleName = "заявке";
            var pluralName = "заявкам";
            return EUSI.Common.ConfirmImportMessageFormatter.FormatConfirmImportMessage(singleName, pluralName, fileDescriptions);
        }
        

        public override void Delete(IUnitOfWork unitOfWork, EstateRegistration obj)
        {
            base.Delete(unitOfWork, obj);
            var rows = unitOfWork.GetRepository<EstateRegistrationRow>()
                .Filter(f => !f.Hidden && !f.IsHistory && f.EstateRegistrationID == obj.ID)
                .ToList();
            foreach (var row in rows)
            {
                row.Hidden = true;
            }

            var estLinks = unitOfWork.GetRepository<EstateAndEstateRegistrationObject>()
                .Filter(f => !f.Hidden && f.ObjRigthId == obj.ID)
                .ToList();
            foreach (var link in estLinks)
            {
                link.Hidden = true;
            }

            var osLinks = unitOfWork.GetRepository<AccountingObjectAndEstateRegistrationObject>()
                .Filter(f => !f.Hidden && f.ObjRigthId == obj.ID)
                .ToList();
            foreach (var link in osLinks)
            {
                link.Hidden = true;
            }

            var fileLinks = unitOfWork.GetRepository<FileCardAndEstateRegistrationObject>()
               .Filter(f => !f.Hidden && f.ObjRigthId == obj.ID)
               .ToList();
            foreach (var link in fileLinks)
            {
                link.Hidden = true;
            }

            unitOfWork.SaveChanges();
        }

        public void DeleteRows(IUnitOfWork unitOfWork, EstateRegistration obj)
        {           
            var rows = unitOfWork.GetRepository<EstateRegistrationRow>()
                .Filter(f => !f.Hidden && !f.IsHistory && f.EstateRegistrationID == obj.ID)
                .ToList();
            foreach (var row in rows)
            {
                row.Hidden = true;
            }

            var estLinks = unitOfWork.GetRepository<EstateAndEstateRegistrationObject>()
                .Filter(f => !f.Hidden && f.ObjRigthId == obj.ID)
                .ToList();
            foreach (var link in estLinks)
            {
                link.Hidden = true;
            }

            var osLinks = unitOfWork.GetRepository<AccountingObjectAndEstateRegistrationObject>()
                .Filter(f => !f.Hidden && f.ObjRigthId == obj.ID)
                .ToList();
            foreach (var link in osLinks)
            {
                link.Hidden = true;
            }

            var fileLinks = unitOfWork.GetRepository<FileCardAndEstateRegistrationObject>()
               .Filter(f => !f.Hidden && f.ObjRigthId == obj.ID)
               .ToList();
            foreach (var link in fileLinks)
            {
                link.Hidden = true;
            }

            unitOfWork.SaveChanges();
        }

        /// <summary>
        /// Экспорт в Zip.
        /// </summary>
        /// <param name="uow"></param>
        /// <returns></returns>
        public string ExportToZip(IUnitOfWork uow, int[] ids)
        {
            var ers = uow.GetRepository<EstateRegistration>()
                .Filter(f => ids.Contains(f.ID) && f.State != null && f.State.Code == "COMPLETED")
                .Select(s => s.ID)
                .ToArray();
            if (ers.Any())
            {
                AccObjectExport export = new AccObjectExport(uow, _fileSystemService, false);
                int emptyeusinumbers = 0;
                return export.Export(ers, ref emptyeusinumbers);
            }
            return null;
        }

       
        /// <summary>
        /// Инициирует выполнение этапа бизнес-процесса по заявкам.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="objID">ИД заявки.</param>
        /// <param name="stageSysName">Системное наименование этапа.</param>
        public void InvokeWFStage(IUnitOfWork uow, int objID, string stageSysName)
        {
            var objType = typeof(EstateRegistration).GetTypeName();
            var action = uow.GetRepository<Base.BusinessProcesses.Entities.StageAction>()
                .Filter(f => 
                !f.Hidden 
                && f.SystemName == stageSysName 
                && f.Step!= null && f.Step.WorkflowImplementation != null && f.Step.WorkflowImplementation.Workflow != null
                && f.Step.WorkflowImplementation.Workflow.ObjectType == objType)
                .FirstOrDefault();
            if (action == null)
                throw new Exception($"Не найден этап бизнес-процесса с системным наименованием <{stageSysName}>.");

            var obj = this.Get(uow, objID);
            WFService.InvokeStage(uow, this, obj, action, new Base.BusinessProcesses.Entities.ActionComment() { Message = ""}, null);
        }

        public CheckImportResult CheckConfirmResult(IUnitOfWork uow
           , string fileName
           , IExcelDataReader input          
           , DataTable table
           )
        {
            ERImportHolder holder = new ERImportHolder(
                null
                , uow
                , null
                , table.DataSet
                , ""
                , Base.Ambient.AppContext.SecurityUser.GetSibUserID(uow)
                , WFService
                , this
                , _emailService
                , null
            );
           
            return holder.CheckConfirmResultERNumber(table);
           
        }

        public CheckImportResult CheckVersionImport(IUiFasade uiFacade, IExcelDataReader reader, ITransactionUnitOfWork uofw,
           StreamReader stream, string fileName)
        {
            //реализовано в EUSIImportChecker
            return null;
        }

    }
}

using System;
using System.Linq;
using Base;
using Base.BusinessProcesses.Entities;
using Base.Service;
using Base.Service.Log;
using CorpProp.Services.Accounting;
using EUSI.Entities.Estate;
using EUSI.Entities.NSI;
using EUSI.Services.Accounting;
using Base.Extensions;
using Base.Settings;
using Base.Mail.Entities;
using CorpProp.Services.Settings;
using Base.BusinessProcesses.Services.Abstract;

namespace EUSI.Services.Estate
{
    public class EstateRegistrationBpService : EstateRegistrationSecurityService, IWFObjectService
    {
        private ISettingService<MailSetting> _setting;
        private IWorkflowService _workflow;
        private readonly ILogService _logger;

        public EstateRegistrationBpService(
            IBaseObjectServiceFacade facade
            , IAccountingObjectExtService accountingObjectService
            , ISibEmailService emailService
            , IWorkflowService workflow
            , ISettingService<MailSetting> setting
            , ILogService logger) : base(facade, emailService, logger)
        {
            EstateRegistrationImport = new EstateRegistrationImport(accountingObjectService, emailService);
            _setting = setting;
            _workflow = workflow;
            _logger = logger;
        }

        public IWorkflowService WFService { get { return _workflow; } }

        private EstateRegistrationImport EstateRegistrationImport { get; }

        public void BeforeInvoke(BaseObject obj)
        {
            //TODO
        }



        public void OnActionExecuting(ActionExecuteArgs args)
        {
            var sysname = args?.EvaluatedAction?.SystemName;

            var estate = args?.NewObject as EstateRegistration;
            if (estate?.WorkflowContextID != null)
            {
                var workflowContext = args.UnitOfWork.GetRepository<WorkflowContext>().Find(estate?.WorkflowContextID);
            }

            //направление на проверку
            if (sysname?.ToUpper() == "DIRECTED")
            {
                var validationResult = CheckEstateDuplicates(args);
                if (validationResult) //совпадение по всем полям
                {
                    //заявка ЕУСИ должна переводиться в статус "Отклонена" с последующей отправкой уведомления инициатору заявки.
                    args.Comment = "Не пройдена проверка на однократное присвоение номера ЕУСИ одному ОИ";
                    sysname = "REJECTED";
                    estate.LastComment = args.Comment;
                    SetStateByBpAction(args, sysname);
                    args.UnitOfWork.SaveChanges();
                    if (!String.IsNullOrEmpty(_setting.Get()?.EmailFrom))
                        SendUserNotification(args.UnitOfWork, new int[] { estate.ID });
                    //TODO: перезапуск процесса из-за насильного перехода, не существующего в КА
                    WFService.ReStartWorkflow(args.UnitOfWork, estate, this);
                    return;
                }
            }
            SetStateByBpAction(args, sysname);
            if (sysname?.ToUpper() == "VERIFIED")
            {
                var estateRegistration = args.UnitOfWork
                    .GetRepository<EstateRegistration>()
                    .Filter(f => f.ID == args.NewObject.ID)
                    .Include(pr => pr.Consolidation)
                    .Include(pr => pr.Contragent)
                    .Include(pr => pr.ERReceiptReason)
                    .Include(pr => pr.ERType)
                    .Include(pr => pr.FileCard)
                    .Include(pr => pr.Originator)
                    .Include(pr => pr.Society)
                    .Include(pr => pr.State)
                    .Include(pr => pr.ERControlDateAttributes)
                    .FirstOrDefault()
                    ;
                if (estateRegistration == null) throw new Exception("Действие применимо только к объекту \"Заявка на регистрацию ОИ\"");
                //при смене статуса проверяем заполненность полей в объектах заявки.
                var check = EstateRegistrationImport.CheckRequiredRows(args.UnitOfWork, estateRegistration);
                if (!String.IsNullOrEmpty(check))
                {
                    throw new Exception(check);
                }

                EstateRegistrationImport.CreateObjectsFromEstateRegistration(args.UnitOfWork, estateRegistration);
                //Если объекты созданы (не было ошибок)
                estateRegistration.StateID = EstateStatesHelper.CompletedStateID;               
                estateRegistration.ERControlDateAttributes.DateVerification = DateTime.Now ;
                Update(args.UnitOfWork, estate);
                //отправка письма
                if (!String.IsNullOrEmpty(_setting.Get()?.EmailFrom))
                {
                    //EstateRegistrationImport.SendEmail(args.UnitOfWork, estateRegistration, EstateStatesHelper.CompletedStateID);
                    SendUserNotification(args.UnitOfWork, new int[] { estateRegistration.ID });
                }
            }

            //REJECTED
            if (sysname?.ToUpper() == "REJECTED" || sysname?.ToUpper() == "REDIRECTED")
            {
                estate.LastComment = args?.Comment;
                Update(args.UnitOfWork, estate);
                //args?.UnitOfWork.SaveChanges();
                //отправка письма
                if (!String.IsNullOrEmpty(_setting.Get()?.EmailFrom))
                {
                    //EstateRegistrationImport.SendEmail(args.UnitOfWork, estate, EstateStatesHelper.RejectedStateID, args?.Comment);
                    SendUserNotification(args.UnitOfWork, new int[] { estate.ID });
                }
            }
        }

        private bool CheckEstateDuplicates(ActionExecuteArgs args)
        {
            var isDuplicates = true;
            var isDuplicatesER = false;
            var estateRegistration = args.UnitOfWork.GetRepository<EstateRegistration>()
                .Filter(x => x.ID == args.NewObject.ID)
                .FirstOrDefault();

            if (estateRegistration.ERType != null && estateRegistration.ERType.Code == "Docs")
                return false;

            var estateRegistrations = args.UnitOfWork.GetRepository<EstateRegistration>()
                .FilterAsNoTracking(x => !x.Hidden && x.ID != estateRegistration.ID
                && x.PrimaryDocNumber == estateRegistration.PrimaryDocNumber
                && x.PrimaryDocDate == estateRegistration.PrimaryDocDate).ToList();
            var isPrimaryDocExists = (estateRegistrations.Count > 0);
            isDuplicates = isDuplicates & isPrimaryDocExists;
            if (!isDuplicates)
            {
                return false;
            }

            var estateRegistrationRows = args.UnitOfWork.GetRepository<EstateRegistrationRow>()
                .Filter(f => !f.Hidden && f.EstateRegistrationID == args.NewObject.ID).ToList();

            foreach (var estateRegistrationRow in estateRegistrationRows)
            {
                foreach (var er in estateRegistrations)
                {
                    isDuplicates = true;
                    //if (er.State?.Code?.ToUpper() == "COMPLETED")
                    //{
                    isDuplicates = isDuplicates & args.UnitOfWork.GetRepository<CorpProp.Entities.Estate.Estate>()
                      .FilterAsNoTracking(x => !x.Hidden && !x.IsHistory &&
                                        x.EstateDefinitionTypeID == estateRegistrationRow.EstateDefinitionTypeID &&
                                        x.NameByDoc == estateRegistrationRow.NameEstateByDoc &&
                                        x.PrimaryDocNumber == estateRegistrationRow.EstateRegistration.PrimaryDocNumber &&
                                        x.PrimaryDocDate == estateRegistrationRow.EstateRegistration.PrimaryDocDate
                                        ).Any();
                    if (isDuplicates)
                        return isDuplicates;
                    //    continue;
                    //}
                    //if (er?.State?.Code?.ToUpper() != "COMPLETED")
                    //{
                    //    isDuplicatesER = true;
                    //    isDuplicatesER = isDuplicatesER & args.UnitOfWork.GetRepository<EstateRegistrationRow>()
                    //          .FilterAsNoTracking(f => !f.Hidden && f.EstateRegistrationID == er.ID &&
                    //                              f.EstateDefinitionTypeID == estateRegistrationRow.EstateDefinitionTypeID &&
                    //                              f.NameEstateByDoc == estateRegistrationRow.NameEstateByDoc).Any();
                    //}
                }
            }

            isDuplicates = (isDuplicates == true || isDuplicatesER == true);

            return isDuplicates;
        }

        private void SetStateByBpAction(ActionExecuteArgs args, string sysname)
        {
            var stateNSIID = args?.UnitOfWork.
                                 GetRepository<EstateRegistrationStateNSI>()                                 
                                 .FilterAsNoTracking(nsi => !nsi.Hidden && nsi.Code == sysname)
                                 .FirstOrDefault()?.ID;
            var registration = args?.NewObject as EstateRegistration;
            if (registration != null && stateNSIID != null) registration.StateID = stateNSIID;


            if (!String.IsNullOrEmpty(sysname) && registration != null)
            {
                if (registration.ERControlDateAttributesID == null && registration.ERControlDateAttributes == null)
                    registration.ERControlDateAttributes = new ERControlDateAttributes();
                else if (registration.ERControlDateAttributesID != null && registration.ERControlDateAttributes == null)
                    registration.ERControlDateAttributes = args?.UnitOfWork.
                                 GetRepository<ERControlDateAttributes>()
                                 .Filter(f => !f.Hidden && f.ID == registration.ID)
                                 .FirstOrDefault();
            }
                switch (sysname.ToUpper())
                {
                    case "REJECTED":                        
                        registration.ERControlDateAttributes.DateRejection = DateTime.Now;
                        break;                        
                    case "COMPLETED":
                    case "VERIFIED":                    
                        registration.ERControlDateAttributes.DateVerification = DateTime.Now;
                        break;                    
                    case "DIRECTED":                    
                        registration.ERControlDateAttributes.DateToVerify = DateTime.Now;
                        break;                    
                    case "REDIRECTED":                    
                        registration.ERControlDateAttributes.DateToСlarify = DateTime.Now;
                        break;                    
                    default:
                        break;
                }
            Update(args?.UnitOfWork, registration);
        }
    }
}

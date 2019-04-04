using Base;
using Base.DAL;
using Base.Notification.Service.Abstract;
using Base.Security;
using Base.Security.Service.Abstract;
using Base.Service;
using Base.UI;
using Base.UI.DetailViewSetting;
using Base.UI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EUSI.Helpers;
using WebApi.Attributes;
using WebApi.Controllers;
using WebHttp = System.Web.Http;
using CorpProp.Entities.NSI;
using EUSI.Entities.Accounting;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Security;
using CorpProp.Entities.Estate;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Services.Abstract;
using Base.Utils.Common;
using CorpProp.Extentions;
using EUSI.WebApi.Services.Accounting;
using CorpProp.Services.Settings;
using CorpProp.Entities.Import;
using CorpProp.Entities.Settings;
using Base.Service.Log;

namespace EUSI.WebApi.Controllers
{
    /// <summary>
    /// Контроллер импорта заявок ЕУСИ.
    /// </summary>
    [CheckSecurityUser]
    [WebHttp.RoutePrefix("eusi/estateRegistration")]
    internal class ERController : BaseApiController
    {
        private readonly IEstateRegistrationService _erService;
        private readonly IWorkflowService _workflowService;
        private ISecurityUser _securityUser = Base.Ambient.AppContext.SecurityUser;
        private readonly ISibEmailService _emailService;
        private readonly ILogService _logger;


        public ERController(
            IViewModelConfigService viewModelConfigService
            , IUnitOfWorkFactory unitOfWorkFactory
            , IEstateRegistrationService estateRegistrationService
            , IWorkflowService workflowService
            , ISibEmailService emailService
            , ILogService logger)
            : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
            _erService = estateRegistrationService;
            _workflowService = workflowService;
            _emailService = emailService;
        }

        /// <summary>
        /// Возвращает Группу ОС/НМаА на основании позиции консолидации.
        /// </summary>
        /// <param name="id">ИД Позации консолидации.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getGroupPosition/{id}")]
        public IHttpActionResult GetGroupPosition(int? id)
        {
            if (id == null)
                return Ok();

            PositionConsolidation group = null;

            using (var uow = CreateUnitOfWork())
            {
                var pos = uow.GetRepository<PositionConsolidation>()
                    .Filter(x => x.ID == id).FirstOrDefault();

                if (!string.IsNullOrEmpty(pos?.GroupCode))
                {
                    group = uow.GetRepository<PositionConsolidation>()
                    .Filter(x => !x.Hidden && !x.IsHistory && x.Code == pos.GroupCode).FirstOrDefault();
                }

            }
            return Ok(group);
        }

        /// <summary>
        /// Расчет налога на имущество.
        /// </summary>
        /// <param name="year">Год.</param>
        /// <param name="consolidationId">БЕ.</param>
        /// <param name="positionConsolidationId">Группа консолидации.</param>
        /// <param name="taxRateTypeCode">Код типа налога.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("eusi/calculateAccountingObject/{year}/{consolidationId}/{taxRateTypeCode}/{periodCalculatedNU}")]
        public IHttpActionResult CalculateAccountingObject(int year, int consolidationId, string taxRateTypeCode, int periodCalculatedNU)
        {
            const string _positiveResult = "Расчет произведен успешно";
            const string _negativeResult = "В результате расчета были выявлены ошибки (подробнее см. Журнал ошибок)";
            const string _errorMessage = "Расчёты завершены не успешно (см. журнал ошибок)";
            const string _osNotFoundMessage = "По заданным параметрам расчёта налога ОС в системе отсутствуют.";
            const decimal _modificator = 0.25M;
            int err = 0,
                counter = 0;

            string mess = "Расчеты завершены.";

            // Коды групп консолидации.
            //Отменено 19.09.2018 (Звягина Е.)
            //string[] consolidationGroupAr = { "0001111110", "0001111120", "0001111200", "0001111300", "0001120000" };
            // Коды типов ОИ для расчета имущественного налога.
            string[] inventoryObjectsCodeAr = { "MOVABLEESTATE", "BUILDINGSTRUCTURE", "ROOM", "CARPARKINGSPACE", "INVENTORYOBJECT", "REALESTATE", "VEHICLE", "SHIP", "AIRCRAFT", "UNFINISHEDCONSTRUCTION" };
            // Коды типов ОИ для расчета транспортного налога.
            string[] transportObjectsCodeAr = { "VEHICLE", "SHIP", "AIRCRAFT" };
            // Коды типов ОИ для расчета земельного налога.
            string[] landObjectsCodeAr = { "LAND" };

            string strEstateMovableNSI = "";
            bool isEstateMovableNSI = false;

            string sYear = year.ToString();
            string sMonth = "01";
            string sDay = "01";
            DateTime tActualDate = DateTime.MinValue;
            tActualDate = DateTime.Parse(sYear + "-" + sMonth + "-" + sDay);
            tActualDate = tActualDate.AddMonths(-1);
            //    tActualDate = tActualDate.ToString().GetDate();



            try
            {
                var tInitiator = _securityUser != null ? _securityUser.ID : 0;
                using (ITransactionUnitOfWork unitOfWork = CreateTransactionUnitOfWork())
                {
                    var baseMonitorService = new CalculatingCreateRecordService();
                    baseMonitorService.CreateCalculateRecord(year, consolidationId, taxRateTypeCode, periodCalculatedNU, tInitiator,  unitOfWork);
                }
                #region Старая реализация

                //using (ITransactionUnitOfWork unitOfWork = CreateTransactionUnitOfWork())
                //{
                //    //var position = unitOfWork.GetRepository<PositionConsolidation>()
                //    //    .Filter(f => f.ID == positionConsolidationId).FirstOrDefault();

                //    //выборка ОБУ за год
                //    //выбираем запись по объекту с максимальной датой актуальности в заданному году.
                //    var q = unitOfWork.GetRepository<AccountingObject>()
                //        .FilterAsNoTracking(f => !f.Hidden && f.ConsolidationID == consolidationId
                //        //&& (f.Oid !=null && f.Oid.ToString() == "B10BD3B5-CDF5-42E4-92F1-95754DE1FD4F") 
                //        && (f.ActualDate != null && (f.ActualDate.Value.Year == year || f.ActualDate.Value == tActualDate)));

                //    List<AccountingObject> accountingObjectsListHistory = q.ToList();
                //    var groups = q
                //        .Where(x => !x.Hidden
                //        && x.ConsolidationID == consolidationId
                //        //&& (x.ActualDate == null || (x.ActualDate != null && (x.ActualDate.Value.Year == year /*|| x.ActualDate == tActualDate*/)))
                //        )
                //        .GroupBy(gr => gr.Oid)
                //        .Select(s => new { Oid = s.Key, MaxActualDate = s.Max(m => m.ActualDate) });
                //    var accounQwer = q.Join(groups, e => e.Oid, o => o.Oid, (e, o) => new { e, o })
                //        .Where(w => w.o.MaxActualDate == w.e.ActualDate)
                //        .Select(s => s.e);

                //    List<AccountingObject> accountingObjectsList = accounQwer.ToList();


                //    string[] EstateDefTypeArrLand = unitOfWork.GetRepository<EstateDefinitionType>()
                //        .FilterAsNoTracking(f => !f.Hidden && (landObjectsCodeAr.Contains(f.Code)))
                //        .Select(s => s.ID.ToString()).ToArray();
                //    string[] EstateDefTypeArrInvent = unitOfWork.GetRepository<EstateDefinitionType>()
                //        .FilterAsNoTracking(f => !f.Hidden && (inventoryObjectsCodeAr.Contains(f.Code)))
                //        .Select(s => s.ID.ToString()).ToArray();
                //    string[] EstateDefTypeArrTS = unitOfWork.GetRepository<EstateDefinitionType>()
                //        .FilterAsNoTracking(f => !f.Hidden && (transportObjectsCodeAr.Contains(f.Code)))
                //        .Select(s => s.ID.ToString()).ToArray();



                //    //if (position != null)
                //    //    accountingObjectsList = accountingObjectsList
                //    //        .Where(w => w.PositionConsolidation != null &&
                //    //                    w.PositionConsolidation.GroupCode == position.GroupCode)
                //    //                    .ToList();
                //    //else
                //    //{
                //    //string[] consolidationGroupIdsAr = unitOfWork.GetRepository<PositionConsolidation>()
                //    //    .Filter(f => consolidationGroupAr.Contains(f.GroupCode) && f.GroupCode != null && f.GroupCode != "")
                //    //    .Select(s => s.ID.ToString())
                //    //    //.Distinct()
                //    //    .ToArray();
                //    //if (consolidationGroupIdsAr.Length > 0)
                //    //{
                //    //    accountingObjectsList = accountingObjectsList
                //    //    .Where(w => w.PositionConsolidationID != null
                //    //    && (consolidationGroupIdsAr.Contains(w.PositionConsolidationID.ToString())))
                //    //    .ToList();
                //    //    accountingObjectsList = accountingObjectsList
                //    //    .Join(unitOfWork.GetRepository<PositionConsolidation>().Filter(f => consolidationGroupAr.Contains(f.GroupCode) && f.GroupCode != null && f.GroupCode != ""), e => e.PositionConsolidationID, o => o.ID, (e, o) => new { e, o })
                //    //    .Where(w => w.e.PositionConsolidationID != null && (consolidationGroupIdsAr.Contains(w.o.GroupCode)))
                //    //    .Select(s => s.e)
                //    //    .ToList();
                //    //        accountingObjectsList = accountingObjectsList
                //    //            .Where(w => w.PositionConsolidation != null
                //    //            && (consolidationGroupIdsAr.Contains(w.PositionConsolidation.GroupCode)))
                //    //            .ToList();
                //    //}

                //    //}

                //    //var llist = accountingObjectsList.Select(s => s.TaxBaseID).ToList();
                //    //var llist2 = accountingObjectsList.Select(s => s.ID).ToList();

                //    List<AccountingObject> accountList = new List<AccountingObject>() { };
                //    switch (taxRateTypeCode)
                //    {
                //        case ("102"):
                //            accountList = accountingObjectsList.Where(w => w.EstateDefinitionTypeID != null && (EstateDefTypeArrLand.Contains(w.EstateDefinitionTypeID.ToString()))).ToList();
                //            break;
                //        case ("101"):
                //            //accountList = accountingObjectsList.Where(w => w.EstateDefinitionTypeID != null && (EstateDefTypeArrInvent.Contains(w.EstateDefinitionTypeID.ToString())) && (w.TaxBaseID==867)).ToList();
                //            accountList = accountingObjectsList.Where(w => w.EstateDefinitionTypeID != null && (EstateDefTypeArrInvent.Contains(w.EstateDefinitionTypeID.ToString()))).ToList();
                //            break;
                //        case ("103"):
                //            accountList = accountingObjectsList.Where(w => w.EstateDefinitionTypeID != null && (EstateDefTypeArrTS.Contains(w.EstateDefinitionTypeID.ToString()))).ToList();
                //            break;

                //    }


                //    if (accountingObjectsList.Count > 0 && accountList.Count > 0)
                //    {
                //        Consolidation consolidation = unitOfWork.GetRepository<Consolidation>().Find(f => f.ID == consolidationId);
                //        TaxRateType taxRateType = unitOfWork.GetRepository<TaxRateType>().Find(f => f.Code == taxRateTypeCode);
                //        SibUser initiator = _securityUser != null
                //            ? unitOfWork.GetRepository<SibUser>().Find(f => f.ID == _securityUser.ID)
                //            : null;
                //        //PositionConsolidation positionConsolidation = positionConsolidationId != null
                //        //    ? unitOfWork.GetRepository<PositionConsolidation>()
                //        //        .Find(f => f.ID == positionConsolidationId)
                //        //    : null;




                //        var calcRecordRepo = unitOfWork.GetRepository<CalculatingRecord>();
                //        var accountingCFRepo = unitOfWork.GetRepository<AccountingCalculatedField>();

                //        CalculatingRecord calculatingRecord =
                //            calcRecordRepo.Create(new CalculatingRecord()
                //            {
                //                Year = year,
                //                Consolidation = consolidation,
                //                //PositionConsolidation = positionConsolidation,
                //                CalculatingDate = DateTime.Now,
                //                Result = _positiveResult,
                //                TaxRateType = taxRateType,
                //                Initiator = initiator
                //            });

                //        unitOfWork.SaveChanges();

                //        foreach (AccountingObject accountingObject in accountList)
                //        {
                //            AccountingCalculatedField accountingCalculatedField = null;
                //            string EUSINumber = "";
                //            if (accountingObject.Estate != null)
                //                EUSINumber = accountingObject.Estate.Number?.ToString();
                //            else if (accountingObject.EstateID != null)
                //            {
                //                var estID = accountingObject.EstateID.Value;
                //                EUSINumber = unitOfWork.GetRepository<Estate>()
                //                    .FilterAsNoTracking(f => f.ID == estID)
                //                    .FirstOrDefault()?.Number?.ToString();
                //            }


                //            /*
                //             * Если тип имущества ao.EstateDefinitionType равен "Земельный участок" 
                //             *      ИЛИ признак отнесение к кат. памятников истории ao.IsCultural активен 
                //             *      ИЛИ ОС включен в 1ую или 2ю аморт. группу НУ ao.DepreciationGroup
                //             * тогда расчеты не выполнять
                //             */
                //            AccountingCalculationHelper helper = new AccountingCalculationHelper(unitOfWork);


                //            AccountingObject accountingQuarter1 = helper.GetAccountingObjectSetQuarter(accountingObjectsListHistory, year, 1, accountingObject.Oid);
                //            AccountingObject accountingQuarter2 = helper.GetAccountingObjectSetQuarter(accountingObjectsListHistory, year, 2, accountingObject.Oid);
                //            AccountingObject accountingQuarter3 = helper.GetAccountingObjectSetQuarter(accountingObjectsListHistory, year, 3, accountingObject.Oid);

                //            accountingQuarter1 = accountingObject;
                //            accountingQuarter2 = accountingObject;
                //            accountingQuarter3 = accountingObject;

                //            strEstateMovableNSI = !(string.IsNullOrEmpty(accountingObject?.EstateMovableNSI?.PublishCode)) ? accountingObject?.EstateMovableNSI?.PublishCode : "";
                //            isEstateMovableNSI = (strEstateMovableNSI.ToUpper() != "VEHICLE" && strEstateMovableNSI.ToUpper() != "MOVABLE");


                //            if (accountingQuarter1 == null || accountingQuarter1 == null || accountingQuarter1 == null)
                //            {
                //                string strErrQuarter = "Для выпонения расчета недостаточно данных на:{0}";
                //                string strQuarter = "";
                //                if (accountingQuarter1 == null)
                //                    strQuarter = "1 Квартал";
                //                if (accountingQuarter2 == null)
                //                    strQuarter += (string.IsNullOrEmpty(strQuarter) ? "2 Квартал" : ", " + "2 Квартал");
                //                if (accountingQuarter3 == null)
                //                    strQuarter += (string.IsNullOrEmpty(strQuarter) ? "3 Квартал" : ", " + "3 Квартал");
                //                if (!string.IsNullOrEmpty(strQuarter))
                //                    strErrQuarter = string.Format(strErrQuarter, strQuarter);
                //                //throw new Exception(strErrQuarter);
                //                helper.WriteCalculatingError(accountingObject, calculatingRecord.ID, strErrQuarter);
                //            }



                //            int
                //                owningMonthCount = 0,
                //                owningMonthCountQuarter1 = 0,
                //                owningMonthCountQuarter2 = 0,
                //                owningMonthCountQuarter3 = 0;

                //            int? shareRightNumerator = accountingObject.ShareRightNumerator ?? 1;
                //            int? shareRightDenominator = accountingObject.ShareRightDenominator ?? 1;
                //            decimal shareRight = (decimal)shareRightNumerator / (decimal)shareRightDenominator;

                //            int? shareRightNumeratorQuarter1 = (accountingQuarter1 == null) ? 1 : accountingQuarter1.ShareRightNumerator ?? 1;
                //            int? shareRightDenominatorQuarter1 = (accountingQuarter1 == null) ? 1 : accountingQuarter1.ShareRightDenominator ?? 1;
                //            decimal shareRightQuarter1 = (decimal)shareRightNumeratorQuarter1 / (decimal)shareRightDenominatorQuarter1;

                //            int? shareRightNumeratorQuarter2 = (accountingQuarter2 == null) ? 1 : accountingQuarter2.ShareRightNumerator ?? 1;
                //            int? shareRightDenominatorQuarter2 = (accountingQuarter2 == null) ? 1 : accountingQuarter2.ShareRightDenominator ?? 1;
                //            decimal shareRightQuarter2 = (decimal)shareRightNumeratorQuarter2 / (decimal)shareRightDenominatorQuarter2;

                //            int? shareRightNumeratorQuarter3 = (accountingQuarter3 == null) ? 1 : accountingQuarter3.ShareRightNumerator ?? 1;
                //            int? shareRightDenominatorQuarter3 = (accountingQuarter3 == null) ? 1 : accountingQuarter3.ShareRightDenominator ?? 1;
                //            decimal shareRightQuarter3 = (decimal)shareRightNumeratorQuarter3 / (decimal)shareRightDenominatorQuarter3;


                //            decimal
                //            //Налоговая база
                //            taxBaseValue = 0m,
                //            taxBaseValueQuarter1 = 0m,
                //            taxBaseValueQuarter2 = 0m,
                //            taxBaseValueQuarter3 = 0m,
                //            //Кварталы
                //            avgPriceFirstQuarter = 0m,
                //            avgPriceSecondQuarter = 0m,
                //            avgPriceThirdQuarter = 0m,
                //            factorK1 = 0m,
                //            factorK2 = 0m,
                //            factorK3 = 0m,
                //            factorKv1 = 0m,
                //            factorKv2 = 0m,
                //            factorKv3 = 0m,
                //            factorKl1 = 0m,
                //            factorKl2 = 0m,
                //            factorKl3 = 0m,
                //            prepaymentSumFirstQuarter = 0m,
                //            prepaymentSumSecondQuarter = 0m,
                //            prepaymentSumThirdQuarter = 0m,
                //            avgPriceYear = 0m,
                //            paymentTaxSum = 0m,
                //            //TODO: необходимо получать из истории (кадастровая стоимость на 1 января года, являющегося налоговым периодом)?
                //            cadastralCost = accountingObject.CadastralValue ?? 0m,
                //            cadastralCostQuarter1 = accountingQuarter1?.CadastralValue ?? 0m,
                //            cadastralCostQuarter2 = accountingQuarter2?.CadastralValue ?? 0m,
                //            cadastralCostQuarter3 = accountingQuarter3?.CadastralValue ?? 0m,
                //            //taxYearSum = 0m,
                //            taxSumYear = 0m,
                //            taxSumYearQuarter1 = 0m,
                //            taxSumYearQuarter2 = 0m,
                //            taxSumYearQuarter3 = 0m,
                //            taxRate = 0m,
                //            taxRateQuarter1 = 0m,
                //            taxRateQuarter2 = 0m,
                //            taxRateQuarter3 = 0m,
                //            factorK = 0m,
                //            factorKv = 0m,
                //            factorKl = 0m,
                //            taxLowSum = 0m,
                //            vehicleTaxFactor = 0m,
                //            vehicleTaxFactorQuarter1 = 0m,
                //            vehicleTaxFactorQuarter2 = 0m,
                //            vehicleTaxFactorQuarter3 = 0m,
                //            taxLowSumQuarter1 = 0m,
                //            taxLowSumQuarter2 = 0m,
                //            taxLowSumQuarter3 = 0m,
                //            taxExemptionFreeSum = 0m,
                //            taxExemptionFreeSumQuarter1 = 0m,
                //            taxExemptionFreeSumQuarter2 = 0m,
                //            taxExemptionFreeSumQuarter3 = 0m,
                //            taxExemptionLowSumTS = 0m,
                //            taxExemptionLowSumTSQuarter1 = 0m,
                //            taxExemptionLowSumTSQuarter2 = 0m,
                //            taxExemptionLowSumTSQuarter3 = 0m,
                //            calcSum = 0m,
                //            calcSumQuarter1 = 0m,
                //            calcSumQuarter2 = 0m,
                //            calcSumQuarter3 = 0m,
                //            untaxedAnnualCostAvg = 0m,
                //            untaxedAnnualCostAvgQuarter1 = 0m,
                //            untaxedAnnualCostAvgQuarter2 = 0m,
                //            untaxedAnnualCostAvgQuarter3 = 0m;




                //            TaxBase taxBase = accountingObject.TaxBaseID != null ? unitOfWork.GetRepository<TaxBase>().Find(f => f.ID == accountingObject.TaxBaseID) : null;
                //            //if (taxBase == null)
                //            //{
                //            //    helper.WriteCalculatingError(accountingObject, calculatingRecord.ID, "Не указан выбор базы налогообложения");
                //            //    counter++;
                //            //    continue;
                //            //}

                //            //ОКТМО
                //            OKTMO tOKTMO = unitOfWork.GetRepository<OKTMO>().Find(f => f.ID == accountingObject.OKTMOID);


                //            EstateDefinitionType estateDefinitionType = accountingObject.EstateDefinitionTypeID != null
                //                ? unitOfWork.GetRepository<EstateDefinitionType>()
                //                    .Find(f => f.ID == accountingObject.EstateDefinitionTypeID)
                //                : null;

                //            // Расчет земельного налога
                //            if ((estateDefinitionType != null && estateDefinitionType.Code.ToUpper() == "LAND") &&
                //                accountingObject.InServiceDate != null &&

                //                //TODO: Если объект выбыл в середине года (делаем обсчет за период владения)
                //                //accountingObject.LeavingDate == null &&
                //                taxRateType.Code == "102")
                //            {

                //                if (!helper.CheckLandObject(accountingObject, calculatingRecord.ID))
                //                {
                //                    calculatingRecord.Result = _negativeResult;
                //                    counter++;
                //                    continue;
                //                }

                //                // TODO: брать значение cadastralCost на 31.03
                //                // Налоговая база (налоговый период) (гр. 11)
                //                taxBaseValue = decimal.Round(cadastralCost * shareRight, 2);
                //                // Налоговая база (1 квартал) (гр. 11)
                //                taxBaseValueQuarter1 = decimal.Round(cadastralCostQuarter1 * shareRightQuarter1, 2);
                //                // Налоговая база (2 квартал) (гр. 11)
                //                taxBaseValueQuarter2 = decimal.Round(cadastralCostQuarter2 * shareRightQuarter2, 2);
                //                // Налоговая база (3 квартал) (гр. 11)
                //                taxBaseValueQuarter3 = decimal.Round(cadastralCostQuarter3 * shareRightQuarter3, 2);



                //                // Налоговая ставка (налоговый период) (гр. 12)
                //                taxRate = (accountingObject.TaxRateValueLand ?? 0m) / 100m;

                //                // Налоговая ставка (1 квартал) (гр. 12)
                //                taxRateQuarter1 = (accountingQuarter1?.TaxRateValueLand ?? 0m) / 100m;
                //                // Налоговая ставка (2 квартал) (гр. 12)
                //                taxRateQuarter2 = (accountingQuarter2?.TaxRateValueLand ?? 0m) / 100m;
                //                // Налоговая ставка (3 квартал) (гр. 12)
                //                taxRateQuarter3 = (accountingQuarter3?.TaxRateValueLand ?? 0m) / 100m;

                //                // Категория земли
                //                GroundCategory groundCategory = unitOfWork.GetRepository<GroundCategory>().Find(f => f.ID == accountingObject.GroundCategoryID);

                //                if (groundCategory?.Code == "3005000000")
                //                    continue;


                //                // Количество полных месяцев владения ЗУ (налоговый период) (гр. 13)
                //                owningMonthCount = helper.CalculateOwningMonthCount(accountingObject.InServiceDate, accountingObject.LeavingDate, year, 1, 12);

                //                // Количество полных месяцев владения ЗУ (1 квартал) (гр. 13)
                //                owningMonthCountQuarter1 = helper.CalculateOwningMonthCount(accountingQuarter1?.InServiceDate, accountingQuarter1?.LeavingDate, year, 1, 3);
                //                // Количество полных месяцев владения ЗУ (2 квартал) (гр. 13)
                //                owningMonthCountQuarter2 = helper.CalculateOwningMonthCount(accountingQuarter2?.InServiceDate, accountingQuarter2?.LeavingDate, year, 4, 6);
                //                // Количество полных месяцев владения ЗУ (3 квартал) (гр. 13)
                //                owningMonthCountQuarter3 = helper.CalculateOwningMonthCount(accountingQuarter3?.InServiceDate, accountingQuarter3?.LeavingDate, year, 7, 9);

                //                // Коэффициент Кв (налоговый период) (гр. 14)
                //                factorKv = decimal.Round(owningMonthCount / 12m, 4);

                //                // Коэффициент Кв (1 квартал) (гр. 14)
                //                factorKv1 = decimal.Round(owningMonthCountQuarter1 / 3m, 4);
                //                // Коэффициент Кв (2 квартал) (гр. 14)
                //                factorKv2 = decimal.Round(owningMonthCountQuarter2 / 3m, 4);
                //                // Коэффициент Кв (3 квартал) (гр. 14)
                //                factorKv3 = decimal.Round(owningMonthCountQuarter3 / 3m, 4);


                //                // Сумма исчисленного налога за налоговый период (гр. 15)
                //                taxSumYear = decimal.Round(taxBaseValue * taxRate * factorKv, 2);

                //                // Сумма исчисленного налога за 1 квартал (гр. 15)
                //                taxSumYearQuarter1 = decimal.Round((taxBaseValue * taxRate * factorKv1) * _modificator, 2);
                //                // Сумма исчисленного налога за 2 квартал (гр. 15)
                //                taxSumYearQuarter2 = decimal.Round((taxBaseValue * taxRate * factorKv2) * _modificator, 2);
                //                // Сумма исчисленного налога за 3 квартал (гр. 15)
                //                taxSumYearQuarter3 = decimal.Round((taxBaseValue * taxRate * factorKv3) * _modificator, 2);

                //                // Количество месяцев действия льгот (гр. 18)
                //                int ActionMonthCount = helper.CalculateTaxExemptionActionMonthCount(accountingObject.TaxExemptionStartDateLand, accountingObject.TaxExemptionEndDateLand, year, 1, 12);
                //                int quarter1ActionMonthCount = helper.CalculateTaxExemptionActionMonthCount(accountingObject.TaxExemptionStartDateLand, accountingObject.TaxExemptionEndDateLand, year, 1, 3);
                //                int quarter2ActionMonthCount = helper.CalculateTaxExemptionActionMonthCount(accountingObject.TaxExemptionStartDateLand, accountingObject.TaxExemptionEndDateLand, year, 4, 6);
                //                int quarter3ActionMonthCount = helper.CalculateTaxExemptionActionMonthCount(accountingObject.TaxExemptionStartDateLand, accountingObject.TaxExemptionEndDateLand, year, 7, 9);


                //                // Количество месяцев отсутствия льгот
                //                int NonActionMonthCount = helper.CalculateTaxExemptionNonActionMonthCount(accountingObject.TaxExemptionStartDateLand, accountingObject.TaxExemptionEndDateLand, year, 1, 12);
                //                int quarter1NonActionMonthCount = helper.CalculateTaxExemptionNonActionMonthCount(accountingObject.TaxExemptionStartDateLand, accountingObject.TaxExemptionEndDateLand, year, 1, 3);
                //                int quarter2NonActionMonthCount = helper.CalculateTaxExemptionNonActionMonthCount(accountingObject.TaxExemptionStartDateLand, accountingObject.TaxExemptionEndDateLand, year, 4, 6);
                //                int quarter3NonActionMonthCount = helper.CalculateTaxExemptionNonActionMonthCount(accountingObject.TaxExemptionStartDateLand, accountingObject.TaxExemptionEndDateLand, year, 7, 9);


                //                // Коэффициент Кл (налоговый период) (гр. 19)
                //                factorKl = decimal.Round(NonActionMonthCount / 12m, 4);
                //                // Коэффициент Кл (1 квартал) (гр. 19)
                //                factorKl1 = decimal.Round(quarter1NonActionMonthCount / 3m, 4);
                //                // Коэффициент Кл (2 квартал) (гр. 19)
                //                factorKl2 = decimal.Round(quarter2NonActionMonthCount / 3m, 4);
                //                // Коэффициент Кл (3 квартал) (гр. 19)
                //                factorKl3 = decimal.Round(quarter3NonActionMonthCount / 3m, 4);

                //                string TLLowCode = (accountingObject.TaxLowerLand == null) ? "" : accountingObject.TaxLowerLand.Code;
                //                string TLLowCodeQuarter1 = (accountingQuarter1?.TaxLowerLand == null) ? "" : accountingQuarter1?.TaxLowerLand.Code;
                //                string TLLowCodeQuarter2 = (accountingQuarter2?.TaxLowerLand == null) ? "" : accountingQuarter2?.TaxLowerLand.Code;
                //                string TLLowCodeQuarter3 = (accountingQuarter3?.TaxLowerLand == null) ? "" : accountingQuarter3?.TaxLowerLand.Code;
                //                string TLFreeCode = (accountingObject.TaxFreeLand == null) ? "" : accountingObject.TaxFreeLand.Code;
                //                string TLFreeCodeQuarter1 = (accountingQuarter1?.TaxFreeLand == null) ? "" : accountingQuarter1?.TaxFreeLand.Code;
                //                string TLFreeCodeQuarter2 = (accountingQuarter2?.TaxFreeLand == null) ? "" : accountingQuarter2?.TaxFreeLand.Code;
                //                string TLFreeCodeQuarter3 = (accountingQuarter3?.TaxFreeLand == null) ? "" : accountingQuarter3?.TaxFreeLand.Code;
                //                string TLRateLowerCode = (accountingObject.TaxRateLowerLand == null) ? "" : accountingObject.TaxRateLowerLand.Code;
                //                string TLRateLowerCodeQuarter1 = (accountingQuarter1?.TaxRateLowerLand == null) ? "" : accountingQuarter1?.TaxRateLowerLand.Code;
                //                string TLRateLowerCodeQuarter2 = (accountingQuarter2?.TaxRateLowerLand == null) ? "" : accountingQuarter2?.TaxRateLowerLand.Code;
                //                string TLRateLowerCodeQuarter3 = (accountingQuarter3?.TaxRateLowerLand == null) ? "" : accountingQuarter3?.TaxRateLowerLand.Code;


                //                if (
                //                    (!string.IsNullOrEmpty(TLLowCode) && TLLowCode == "3022200") ||
                //                    (!string.IsNullOrEmpty(TLLowCodeQuarter1) && TLLowCodeQuarter1 == "3022200") ||
                //                    (!string.IsNullOrEmpty(TLLowCodeQuarter2) && TLLowCodeQuarter2 == "3022200") ||
                //                    (!string.IsNullOrEmpty(TLLowCodeQuarter3) && TLLowCodeQuarter3 == "3022200")
                //                   )
                //                {
                //                    // Сумма налоговой льготы в виде уменьшения суммы налога (налоговый период) (гр. 21)
                //                    taxLowSum = decimal.Round(taxSumYear * accountingObject.TaxLowerPercent ?? 0 / 100, 2);

                //                    // Сумма налоговой льготы в виде уменьшения суммы налога (1 квартал) (гр. 21)
                //                    taxLowSumQuarter1 = decimal.Round(taxSumYearQuarter1 * accountingQuarter1?.TaxLowerPercent ?? 0 / 100m, 2);
                //                    // Сумма налоговой льготы в виде уменьшения суммы налога (2 квартал) (гр. 21)
                //                    taxLowSumQuarter2 = decimal.Round(taxSumYearQuarter2 * accountingQuarter2?.TaxLowerPercent ?? 0 / 100m, 2);
                //                    // Сумма налоговой льготы в виде уменьшения суммы налога (3 квартал) (гр. 21)
                //                    taxLowSumQuarter3 = decimal.Round(taxSumYearQuarter3 * accountingQuarter3?.TaxLowerPercent ?? 0 / 100m, 2);


                //                    calcSum = Math.Round(taxSumYear - taxLowSum - taxExemptionFreeSum);
                //                    calcSumQuarter1 = Math.Round(taxSumYearQuarter1 - taxLowSumQuarter1 - taxExemptionFreeSumQuarter1);
                //                    calcSumQuarter2 = Math.Round(taxSumYearQuarter2 - taxLowSumQuarter2 - taxExemptionFreeSumQuarter2);
                //                    calcSumQuarter3 = Math.Round(taxSumYearQuarter3 - taxLowSumQuarter3 - taxExemptionFreeSumQuarter3);
                //                }
                //                else
                //                if (
                //                    (!string.IsNullOrEmpty(TLFreeCode) && TLFreeCode == "3022400") ||
                //                    (!string.IsNullOrEmpty(TLFreeCodeQuarter1) && TLFreeCodeQuarter1 == "3022400") ||
                //                    (!string.IsNullOrEmpty(TLFreeCodeQuarter2) && TLFreeCodeQuarter2 == "3022400") ||
                //                    (!string.IsNullOrEmpty(TLFreeCodeQuarter3) && TLFreeCodeQuarter3 == "3022400")
                //                    )
                //                {
                //                    // Сумма налоговой льготы в виде освобождения от налогообложения (налоговый период) (гр. 20)
                //                    taxExemptionFreeSum = decimal.Round(taxSumYear * (1 - factorKl), 2);

                //                    // Сумма налоговой льготы в виде освобождения от налогообложения (1 квартал) (гр. 20)
                //                    taxExemptionFreeSumQuarter1 = decimal.Round(taxSumYearQuarter1 * (1 - factorKl1), 2);
                //                    // Сумма налоговой льготы в виде освобождения от налогообложения (2 квартал) (гр. 20)
                //                    taxExemptionFreeSumQuarter2 = decimal.Round(taxSumYearQuarter2 * (1 - factorKl2), 2);
                //                    // Сумма налоговой льготы в виде освобождения от налогообложения (3 квартал) (гр. 20)
                //                    taxExemptionFreeSumQuarter3 = decimal.Round(taxSumYearQuarter3 * (1 - factorKl3), 2);


                //                    calcSum = Math.Round(taxSumYear - taxExemptionFreeSum - taxLowSum);
                //                    calcSumQuarter1 = Math.Round(taxSumYearQuarter1 - taxExemptionFreeSumQuarter1 - taxLowSumQuarter1);
                //                    calcSumQuarter2 = Math.Round(taxSumYearQuarter2 - taxExemptionFreeSumQuarter2 - taxLowSumQuarter2);
                //                    calcSumQuarter3 = Math.Round(taxSumYearQuarter3 - taxExemptionFreeSumQuarter3 - taxLowSumQuarter3);
                //                }
                //                else
                //                if (
                //                    (!string.IsNullOrEmpty(TLRateLowerCode) && TLRateLowerCode == "3022500") ||
                //                    (!string.IsNullOrEmpty(TLRateLowerCodeQuarter1) && TLRateLowerCodeQuarter1 == "3022500") ||
                //                    (!string.IsNullOrEmpty(TLRateLowerCodeQuarter2) && TLRateLowerCodeQuarter2 == "3022500") ||
                //                    (!string.IsNullOrEmpty(TLRateLowerCodeQuarter3) && TLRateLowerCodeQuarter3 == "3022500")
                //                    )
                //                {
                //                    calcSum = Math.Round(cadastralCost * taxRate * factorKv);
                //                    calcSumQuarter1 = Math.Round(cadastralCostQuarter1 * taxRateQuarter1 * factorKv1);
                //                    calcSumQuarter2 = Math.Round(cadastralCostQuarter2 * taxRateQuarter2 * factorKv2);
                //                    calcSumQuarter3 = Math.Round(cadastralCostQuarter3 * taxRateQuarter3 * factorKv3);
                //                }
                //                else
                //                {
                //                    calcSum = Math.Round(taxSumYear);
                //                    calcSumQuarter1 = Math.Round(taxSumYearQuarter1);
                //                    calcSumQuarter2 = Math.Round(taxSumYearQuarter2);
                //                    calcSumQuarter3 = Math.Round(taxSumYearQuarter3);
                //                }


                //                // Сумма авансового платежа (1 квартал) (гр. 24)
                //                prepaymentSumFirstQuarter = Math.Round(calcSumQuarter1);
                //                // Сумма авансового платежа (2 квартал) (гр. 25)
                //                prepaymentSumSecondQuarter = Math.Round(calcSumQuarter2);
                //                // Сумма авансового платежа (3 квартал) (гр. 26)
                //                prepaymentSumThirdQuarter = Math.Round(calcSumQuarter3);


                //                // Сумма налога, исчисленная к уплате в бюджет (налоговый период) (гр. 27)
                //                paymentTaxSum = Math.Round(calcSum - (prepaymentSumFirstQuarter + prepaymentSumSecondQuarter + prepaymentSumThirdQuarter));


                //                accountingCalculatedField = new AccountingCalculatedField()
                //                {

                //                    CalculationDatasource = "ЕУСИ",
                //                    AccountingObjectID = accountingObject.ID,
                //                    CalculatingRecordID = calculatingRecord.ID,
                //                    Year = year,
                //                    CalculateDate = DateTime.Now,
                //                    IsEstateMovable = isEstateMovableNSI,
                //                    AccountingObjectName = accountingObject.Name,
                //                    ConsolidationID = accountingObject.ConsolidationID,
                //                    ExternalID = accountingObject?.ExternalID,
                //                    SubNumber = accountingObject?.SubNumber,
                //                    OKOF = accountingObject?.OKOF2014?.Code,

                //                    EUSINumber = EUSINumber,
                //                    IFNS = accountingObject.IFNS,
                //                    OKTMO = tOKTMO?.PublishCode,
                //                    InventoryNumber = accountingObject?.InventoryNumber,
                //                    CadastralNumber = accountingObject.CadastralNumber,
                //                    LandCategory = accountingObject?.GroundCategory?.PublishCode,
                //                    CadastralValue = cadastralCost,
                //                    CadastralValueQuarter1 = cadastralCostQuarter1,
                //                    CadastralValueQuarter2 = cadastralCostQuarter2,
                //                    CadastralValueQuarter3 = cadastralCostQuarter3,
                //                    CadRegDate = accountingObject?.CadRegDate,
                //                    ShareRightNumerator = accountingObject.ShareRightNumerator,
                //                    ShareRightDenominator = accountingObject.ShareRightDenominator,
                //                    TaxBaseValue = taxBaseValue,
                //                    TaxBaseValueQuarter1 = taxBaseValueQuarter1,
                //                    TaxBaseValueQuarter2 = taxBaseValueQuarter2,
                //                    TaxBaseValueQuarter3 = taxBaseValueQuarter3,
                //                    TaxRate = (accountingObject.TaxRateValueLand ?? 0m),
                //                    TaxRateQuarter1 = (accountingQuarter1.TaxRateValueLand ?? 0m),
                //                    TaxRateQuarter2 = (accountingQuarter2.TaxRateValueLand ?? 0m),
                //                    TaxRateQuarter3 = (accountingQuarter3.TaxRateValueLand ?? 0m),
                //                    CountFullMonthsLand = owningMonthCount,
                //                    FactorKv = factorKv,
                //                    FactorKv1 = factorKv1,
                //                    FactorKv2 = factorKv2,
                //                    FactorKv3 = factorKv3,
                //                    TaxSumYear = taxSumYear,
                //                    TaxSumYearQuarter1 = taxSumYearQuarter1,
                //                    TaxSumYearQuarter2 = taxSumYearQuarter2,
                //                    TaxSumYearQuarter3 = taxSumYearQuarter3,
                //                    TaxExemptionStartDateLand = accountingObject.TaxExemptionStartDateLand,
                //                    TaxExemptionEndDateLand = accountingObject.TaxExemptionEndDateLand,
                //                    CountFullMonthsBenefit = ActionMonthCount,
                //                    FactorKl = factorKl,
                //                    FactorKl1 = factorKl1,
                //                    FactorKl2 = factorKl2,
                //                    FactorKl3 = factorKl3,
                //                    TaxExemptionFreeLand = accountingObject?.TaxFreeLand?.PublishCode,
                //                    TaxExemptionFreeSum = taxExemptionFreeSum,
                //                    TaxExemptionFreeSumQuarter1 = taxExemptionFreeSumQuarter1,
                //                    TaxExemptionFreeSumQuarter2 = taxExemptionFreeSumQuarter2,
                //                    TaxExemptionFreeSumQuarter3 = taxExemptionFreeSumQuarter3,
                //                    TaxLow = accountingObject?.TaxLowerLand?.PublishCode,
                //                    TaxLowSum = taxLowSum,
                //                    TaxLowSumQuarter1 = taxLowSumQuarter1,
                //                    TaxLowSumQuarter2 = taxLowSumQuarter2,
                //                    TaxLowSumQuarter3 = taxLowSumQuarter3,
                //                    TaxLowerPercent = accountingObject?.TaxLowerPercent,
                //                    CalcSum = calcSum,
                //                    PrepaymentSumFirstQuarter = prepaymentSumFirstQuarter,
                //                    PrepaymentSumSecondQuarter = prepaymentSumSecondQuarter,
                //                    PrepaymentSumThirdQuarter = prepaymentSumThirdQuarter,
                //                    PaymentTaxSum = paymentTaxSum,

                //                };
                //            }




                //            // Расчет налога на имущество
                //            else if (estateDefinitionType != null &&
                //                     inventoryObjectsCodeAr.Contains(estateDefinitionType.Code.ToUpper()) &&
                //                     taxRateType.Code == "101")
                //            {
                //                if (accountingObject.IsCultural)
                //                    continue;
                //                if (!helper.CheckInventoryObject(accountingObject, calculatingRecord.ID, taxBase?.Code))
                //                {
                //                    calculatingRecord.Result = _negativeResult;
                //                    counter++;
                //                    continue;
                //                }

                //                // Налоговая ставка (налоговый период) (гр. 24)
                //                taxRate = (accountingObject.TaxRateValue ?? 0m) / 100m;

                //                // Налоговая ставка (1 квартал) (гр. 24)
                //                taxRateQuarter1 = (accountingQuarter1?.TaxRateValue ?? 0m) / 100m;
                //                // Налоговая ставка (2 квартал) (гр. 24)
                //                taxRateQuarter2 = (accountingQuarter2?.TaxRateValue ?? 0m) / 100m;
                //                // Налоговая ставка (3 квартал) (гр. 24)
                //                taxRateQuarter3 = (accountingQuarter3?.TaxRateValue ?? 0m) / 100m;



                //                //Выбор налоговой базы: Среднегодовая стоимость 
                //                if (taxBase.Code == "101")
                //                {
                //                    //accountingObjectsListHistory = accountingObjectsListHistory
                //                    //    //.Join(accountingObjectsList, e => e.Oid, o => o.Oid, (e, o) => new { e, o })
                //                    //    .Where(w => w.IsHistory)
                //                    //    //.Select(s => s.e)
                //                    //    .ToList();


                //                    // Среднегодовая стоимость имущества (налоговый период) (гр. 16)
                //                    avgPriceYear = decimal.Round(helper.CalculateAvgPriceYear(accountingObjectsListHistory, year, accountingObject.Oid), 2);

                //                    // Среднегодовая стоимость имущества (1 квартал) (гр. 16)
                //                    avgPriceFirstQuarter = decimal.Round(helper.CalculateAvgPriceFirstQuarter(accountingObjectsListHistory, year, accountingObject.Oid), 2);
                //                    // Среднегодовая стоимость имущества (6 месяцев) (гр. 16)
                //                    avgPriceSecondQuarter = decimal.Round(helper.CalculateAvgPriceSecondQuarter(accountingObjectsListHistory, year, accountingObject.Oid, taxBase.Code), 2);
                //                    // Среднегодовая стоимость имущества (9 месяцев) (гр. 16)
                //                    avgPriceThirdQuarter = decimal.Round(helper.CalculateAvgPriceThirdQuarter(accountingObjectsListHistory, year, accountingObject.Oid, taxBase.Code), 2);


                //                    TaxExemption tTaxExemption = unitOfWork.GetRepository<TaxExemption>().Find(f => f.ID == accountingObject.TaxExemptionID);

                //                    if (tTaxExemption != null && tTaxExemption.Code == "2012000")
                //                    {
                //                        untaxedAnnualCostAvg = decimal.Round(helper.CalculateAvgPriceYear(accountingObjectsListHistory, year, accountingObject.Oid), 2);
                //                        untaxedAnnualCostAvgQuarter1 = decimal.Round(helper.CalculateAvgPriceFirstQuarter(accountingObjectsListHistory, year, accountingObject.Oid), 2);
                //                        untaxedAnnualCostAvgQuarter2 = decimal.Round(helper.CalculateAvgPriceSecondQuarter(accountingObjectsListHistory, year, accountingObject.Oid, taxBase.Code), 2);
                //                        untaxedAnnualCostAvgQuarter3 = decimal.Round(helper.CalculateAvgPriceThirdQuarter(accountingObjectsListHistory, year, accountingObject.Oid, taxBase.Code), 2);

                //                        // Налоговая база (налоговый период) (гр. 22)
                //                        taxBaseValue = avgPriceYear - untaxedAnnualCostAvg;

                //                        // Налоговая база (1 квартал) (гр. 22)
                //                        taxBaseValueQuarter1 = avgPriceFirstQuarter - untaxedAnnualCostAvgQuarter1;
                //                        // Налоговая база (2 квартал) (гр. 22)
                //                        taxBaseValueQuarter2 = avgPriceSecondQuarter - untaxedAnnualCostAvgQuarter2;
                //                        // Налоговая база (3 квартал) (гр. 22)
                //                        taxBaseValueQuarter3 = avgPriceThirdQuarter - untaxedAnnualCostAvgQuarter3;
                //                    }
                //                    else
                //                    {
                //                        // Налоговая база (налоговый период) (гр. 22)
                //                        taxBaseValue = avgPriceYear;

                //                        // Налоговая база (1 квартал) (гр. 22)
                //                        taxBaseValueQuarter1 = avgPriceFirstQuarter;
                //                        // Налоговая база (2 квартал) (гр. 22)
                //                        taxBaseValueQuarter2 = avgPriceSecondQuarter;
                //                        // Налоговая база (3 квартал) (гр. 22)
                //                        taxBaseValueQuarter3 = avgPriceThirdQuarter;
                //                    }


                //                    // Сумма авансового платежа (1 квартал) (гр. 27а)
                //                    prepaymentSumFirstQuarter = Math.Round(taxBaseValueQuarter1 * taxRate / 4m);
                //                    // Сумма авансового платежа (6 месяцев) (гр. 27б)
                //                    prepaymentSumSecondQuarter = Math.Round(taxBaseValueQuarter2 * taxRate / 4m);
                //                    // Сумма авансового платежа (9 месяцев) (гр. 27в)
                //                    prepaymentSumThirdQuarter = Math.Round(taxBaseValueQuarter3 * taxRate / 4m);


                //                    // Сумма налога (налоговый период) (гр. 26)
                //                    taxSumYear = decimal.Round(taxBaseValue * taxRate, 2);


                //                    TaxLower tTaxLower = unitOfWork.GetRepository<TaxLower>().Find(f => f.ID == accountingObject.TaxLowerID);

                //                    if (tTaxLower != null && tTaxLower.Code == "2012500")
                //                    {
                //                        // Сумма налоговой льготы, уменьшающей сумму налога (налоговый период) (гр. 30)
                //                        taxLowSum = decimal.Round(taxSumYear * accountingObject.TaxLowerPercent ?? 0 / 100m, 2);

                //                        // Сумма налоговой льготы, уменьшающей сумму налога (1 квартал) (гр. 30)
                //                        taxLowSumQuarter1 = decimal.Round(prepaymentSumFirstQuarter * accountingQuarter1?.TaxLowerPercent ?? 0 / 100m, 2);
                //                        // Сумма налоговой льготы, уменьшающей сумму налога (2 квартал) (гр. 30)
                //                        taxLowSumQuarter2 = decimal.Round(prepaymentSumSecondQuarter * accountingQuarter2?.TaxLowerPercent ?? 0 / 100m, 2);
                //                        // Сумма налоговой льготы, уменьшающей сумму налога (3 квартал) (гр. 30)
                //                        taxLowSumQuarter3 = decimal.Round(prepaymentSumThirdQuarter * accountingQuarter3?.TaxLowerPercent ?? 0 / 100m, 2);
                //                    }


                //                    // Сумма налога, подлежащая уплате в бюджет (налоговый период)
                //                    paymentTaxSum = Math.Round(taxSumYear - (prepaymentSumFirstQuarter + prepaymentSumSecondQuarter + prepaymentSumThirdQuarter) - taxLowSum);
                //                }

                //                //Выбор налоговой базы: Кадастровая стоимость
                //                if (taxBase.Code == "102")
                //                {

                //                    // Количество полных месяцев владения
                //                    owningMonthCount = helper.CalculateOwningMonthCount(accountingObject.InServiceDate, accountingObject.LeavingDate, year, 1, 12);
                //                    owningMonthCountQuarter1 = helper.CalculateOwningMonthCount(accountingQuarter1?.InServiceDate, accountingQuarter1?.LeavingDate, year, 1, 3);
                //                    owningMonthCountQuarter2 = helper.CalculateOwningMonthCount(accountingQuarter2?.InServiceDate, accountingQuarter2?.LeavingDate, year, 4, 6);
                //                    owningMonthCountQuarter3 = helper.CalculateOwningMonthCount(accountingQuarter3?.InServiceDate, accountingQuarter3?.LeavingDate, year, 7, 9);

                //                    // Коэффициент К (налоговый период) (гр. 25)
                //                    factorK = decimal.Round(owningMonthCount / 12m, 4);

                //                    // Коэффициент К (1 квартал) (гр. 25)
                //                    factorK1 = decimal.Round(owningMonthCountQuarter1 / 3m, 4);
                //                    // Коэффициент К (2 квартал) (гр. 25)
                //                    factorK2 = decimal.Round(owningMonthCountQuarter2 / 3m, 4);
                //                    // Коэффициент К (3 квартал) (гр. 25)
                //                    factorK3 = decimal.Round(owningMonthCountQuarter3 / 3m, 4);


                //                    // Налоговая база (налоговый период) (гр. 22)
                //                    taxBaseValue = decimal.Round(accountingObject.CadastralValue ?? 0 * shareRightQuarter1, 2);

                //                    // Налоговая база (1 квартал) (гр. 22)
                //                    taxBaseValueQuarter1 = decimal.Round(accountingObject.CadastralValue ?? 0 * shareRightQuarter1, 2);
                //                    // Налоговая база (2 квартал) (гр. 22)
                //                    taxBaseValueQuarter2 = decimal.Round(accountingObject.CadastralValue ?? 0 * shareRightQuarter2, 2);
                //                    // Налоговая база (3 квартал) (гр. 22)
                //                    taxBaseValueQuarter3 = decimal.Round(accountingObject.CadastralValue ?? 0 * shareRightQuarter3, 2);


                //                    //количества месяцев владения ОС
                //                    //int owningMonthCount = helper.CalculateOwningMonthCount(accountingObject.InServiceDate, accountingObject.LeavingDate, year, 1, 4);
                //                    //owningMonthCount = helper.CalculateOwningMonthCount(accountingObject.InServiceDate, accountingObject.LeavingDate, year, 4, 9);
                //                    //owningMonthCount = helper.CalculateOwningMonthCount(accountingObject.InServiceDate, accountingObject.LeavingDate, year, 9, 12);


                //                    //Сумма авансового платежа (1 квартал) (гр. 27а)
                //                    prepaymentSumFirstQuarter = Math.Round(taxBaseValue * taxRate * factorK1 / 4m);
                //                    //Сумма авансового платежа (2 квартал) (гр. 27б)
                //                    prepaymentSumSecondQuarter = Math.Round(taxBaseValue * taxRate * factorK2 / 4m);
                //                    //Сумма авансового платежа (3 квартал) (гр. 27в)
                //                    prepaymentSumThirdQuarter = Math.Round(taxBaseValue * taxRate * factorK3 / 4m);


                //                    // Сумма налога (налоговый период) (гр. 26)
                //                    taxSumYear = decimal.Round(taxBaseValue * taxRate * factorK, 2);


                //                    // Сумма налоговой льготы, уменьшающей сумму налога (налоговый период) (гр. 30)
                //                    taxLowSum = decimal.Round(taxSumYear * accountingObject.TaxLowerPercent ?? 0 / 100m, 2);

                //                    // Сумма налоговой льготы, уменьшающей сумму налога (1 квартал) (гр. 30)
                //                    taxLowSumQuarter1 = decimal.Round(prepaymentSumFirstQuarter * accountingQuarter1?.TaxLowerPercent ?? 0 / 100m, 2);
                //                    // Сумма налоговой льготы, уменьшающей сумму налога (2 квартал) (гр. 30)
                //                    taxLowSumQuarter2 = decimal.Round(prepaymentSumSecondQuarter * accountingQuarter2?.TaxLowerPercent ?? 0 / 100m, 2);
                //                    // Сумма налоговой льготы, уменьшающей сумму налога (3 квартал) (гр. 30)
                //                    taxLowSumQuarter3 = decimal.Round(prepaymentSumThirdQuarter * accountingQuarter3?.TaxLowerPercent ?? 0 / 100m, 2);


                //                    // Сумма налога, подлежащая уплате в бюджет (налоговый период)
                //                    paymentTaxSum = Math.Round(taxSumYear - (prepaymentSumFirstQuarter + prepaymentSumSecondQuarter + prepaymentSumThirdQuarter) - taxLowSum);
                //                }

                //                accountingCalculatedField = new AccountingCalculatedField()
                //                {

                //                    CalculationDatasource = "ЕУСИ",
                //                    Year = year,
                //                    CalculateDate = DateTime.Now,
                //                    ConsolidationID = accountingObject.ConsolidationID,
                //                    AccountingObjectID = accountingObject.ID,
                //                    CalculatingRecordID = calculatingRecord.ID,

                //                    BusinessArea = accountingObject?.BusinessArea?.Name,
                //                    ExternalID = accountingObject?.ExternalID,
                //                    SubNumber = accountingObject?.SubNumber,
                //                    IsEstateMovable = isEstateMovableNSI,
                //                    AccountingObjectName = accountingObject.Name,
                //                    InventoryNumber = accountingObject?.InventoryNumber,
                //                    DepreciationGroup = accountingObject?.DepreciationGroup?.PublishCode,
                //                    AccountLedgerLUS = accountingObject?.AccountLedgerLUS,
                //                    //SyntheticAccount =
                //                    OKOF = accountingObject?.OKOF2014?.Code,
                //                    OKTMO = tOKTMO?.PublishCode,
                //                    CadastralNumber = accountingObject.CadastralNumber,
                //                    TaxBaseID = accountingObject?.TaxBaseID,
                //                    //GetByRestruct =
                //                    //GetFromInterdependent =
                //                    //RegDate =
                //                    //ResidualCost_01 =
                //                    //ResidualCost_02 =
                //                    //ResidualCost_03 =
                //                    //ResidualCost_04 =
                //                    //ResidualCost_05 =
                //                    //ResidualCost_06 =
                //                    //ResidualCost_07 =
                //                    //ResidualCost_08 =
                //                    //ResidualCost_09 =
                //                    //ResidualCost_10 =
                //                    //ResidualCost_11 =
                //                    //ResidualCost_12 =
                //                    //ResidualCost_13 =
                //                    //ResidualCost_14 =
                //                    AvgPriceYear = avgPriceYear,
                //                    AvgPriceFirstQuarter = taxBase.Code == "101" ? avgPriceFirstQuarter : 0,
                //                    AvgPriceSecondQuarter = taxBase.Code == "101" ? avgPriceSecondQuarter : 0,
                //                    AvgPriceThirdQuarter = taxBase.Code == "101" ? avgPriceThirdQuarter : 0,
                //                    UntaxedAnnualCostAvg = untaxedAnnualCostAvg,
                //                    UntaxedAnnualCostAvgQuarter1 = untaxedAnnualCostAvgQuarter1,
                //                    UntaxedAnnualCostAvgQuarter2 = untaxedAnnualCostAvgQuarter2,
                //                    UntaxedAnnualCostAvgQuarter3 = untaxedAnnualCostAvgQuarter3,
                //                    CadastralValue = cadastralCost,
                //                    CadastralValueQuarter1 = cadastralCostQuarter1,
                //                    CadastralValueQuarter2 = cadastralCostQuarter2,
                //                    CadastralValueQuarter3 = cadastralCostQuarter3,
                //                    ShareRightNumerator = accountingObject.ShareRightNumerator,
                //                    ShareRightDenominator = accountingObject.ShareRightDenominator,
                //                    TaxExemption = accountingObject?.TaxExemption?.PublishCode,
                //                    TaxBaseValue = taxBaseValue,
                //                    TaxBaseValueQuarter1 = taxBaseValueQuarter1,
                //                    TaxBaseValueQuarter2 = taxBaseValueQuarter2,
                //                    TaxBaseValueQuarter3 = taxBaseValueQuarter3,
                //                    TaxExemptionLow = accountingObject?.TaxRateLower?.PublishCode,
                //                    TaxRate = (accountingObject.TaxRateValue ?? 0m),
                //                    TaxRateQuarter1 = (accountingQuarter1?.TaxRateValue ?? 0m),
                //                    TaxRateQuarter2 = (accountingQuarter2?.TaxRateValue ?? 0m),
                //                    TaxRateQuarter3 = (accountingQuarter3?.TaxRateValue ?? 0m),
                //                    FactorK = factorK,
                //                    FactorK1 = factorK1,
                //                    FactorK2 = factorK2,
                //                    FactorK3 = factorK3,
                //                    TaxSumYear = taxSumYear,
                //                    TaxSumYearQuarter1 = taxSumYearQuarter1,
                //                    TaxSumYearQuarter2 = taxSumYearQuarter2,
                //                    TaxSumYearQuarter3 = taxSumYearQuarter3,
                //                    PrepaymentSumFirstQuarter = prepaymentSumFirstQuarter,
                //                    PrepaymentSumSecondQuarter = prepaymentSumSecondQuarter,
                //                    PrepaymentSumThirdQuarter = prepaymentSumThirdQuarter,
                //                    TaxLow = accountingObject?.TaxLower?.PublishCode,
                //                    TaxLowerPercent = accountingObject?.TaxLowerPercent,
                //                    TaxLowSum = taxLowSum,
                //                    TaxLowSumQuarter1 = taxLowSumQuarter1,
                //                    TaxLowSumQuarter2 = taxLowSumQuarter2,
                //                    TaxLowSumQuarter3 = taxLowSumQuarter3,
                //                    PaymentTaxSum = paymentTaxSum,

                //                    IncludeCadRegDate = accountingObject?.DateInclusion,
                //                    IncludeCadRegDoc = accountingObject?.DocumentNumber,
                //                    IFNS = accountingObject.IFNS,
                //                    EUSINumber = EUSINumber,

                //                    //TaxExemptionFreeSum = taxExemptionFreeSum,
                //                    //TaxExemptionFreeSumQuarter1 = taxExemptionFreeSumQuarter1,
                //                    //TaxExemptionFreeSumQuarter2 = taxExemptionFreeSumQuarter2,
                //                    //TaxExemptionFreeSumQuarter3 = taxExemptionFreeSumQuarter3,

                //                };
                //            }


                //            // Расчет транспортного налога
                //            else if (estateDefinitionType != null &&
                //                     transportObjectsCodeAr.Contains(estateDefinitionType.Code.ToUpper()) &&
                //                     taxRateType.Code == "103")
                //            {
                //                if (!helper.CheckTransportObject(accountingObject, calculatingRecord.ID, year))
                //                {
                //                    calculatingRecord.Result = _negativeResult;
                //                    counter++;
                //                    continue;
                //                }


                //                // Налоговая ставка (налоговый период) (гр. 21)
                //                taxRate = (accountingObject.TaxRateValueTS ?? 0m);

                //                // Налоговая ставка (1 квартал) (гр. 21)
                //                taxRateQuarter1 = (accountingQuarter1?.TaxRateValueTS ?? 0m);
                //                // Налоговая ставка (2 квартал) (гр. 21)
                //                taxRateQuarter2 = (accountingQuarter2?.TaxRateValueTS ?? 0m);
                //                // Налоговая ставка (3 квартал) (гр. 21)
                //                taxRateQuarter3 = (accountingQuarter3?.TaxRateValueTS ?? 0m);


                //                //Дубль
                //                //if (!helper.CheckTransportObject(accountingObject, calculatingRecord.ID))
                //                //{
                //                //    calculatingRecord.Result = _negativeResult;
                //                //    counter++;
                //                //    continue;
                //                //}

                //                //TODO: Налоговая ставка по транспортному налогу из справочника (???) = 15
                //                //TODO: коэффициент повышающий/понижающий из справочника (???) = 1
                //                //TODO: код вида ТС из списка ???_X = 1
                //                //TODO: коэффициент владения (???) = 3

                //                /* Если в ОС указан код вида ТС из списка ??? Тогда
                //                 *      Сумма исчисленного налога (taxSumYear) = ОС.мощность ТС * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???)
                //                 *
                //                 *      Если в ОС НЕ стоит признак учета в системе ПЛАТОН
                //                 *          Авансовый платеж за первый кв. = (ОС.мощность ТС * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???) * коэффициент владения (???)) * 0,25
                //                 *          Авансовый платеж за второй кв. = (ОС.мощность ТС * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???) * коэффициент владения (???)) * 0,25
                //                 *          Авансовый платеж за третий кв. = (ОС.мощность ТС * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???) * коэффициент владения (???)) * 0,25
                //                 * Иначе если в ОС указан код вида ТС из списка ???_2  Тогда
                //                 *      taxSumYear = ОС.Статическая тяга реактивного двигателя * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???)
                //                 *      Авансовый платеж за первый кв. = (ОС.Статическая тяга реактивного двигателя * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???) * коэффициент владения (???)) * 0,25
                //                 *      Авансовый платеж за второй кв. = (ОС.Статическая тяга реактивного двигателя * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???) * коэффициент владения (???)) * 0,25
                //                 *      Авансовый платеж за третий кв. = (ОС.Статическая тяга реактивного двигателя * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???) * коэффициент владения (???)) * 0,25
                //                 * Иначе если в ОС указан код вида ТС из списка ???_3 Тогда
                //                 *      taxSumYear = ОС.Валовая вместимость * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???)
                //                 *      Авансовый платеж за первый кв. = (ОС.Валовая вместимость * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???) * коэффициент владения (???)) * 0,25
                //                 *      Авансовый платеж за второй кв. = (ОС.Валовая вместимость * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???) * коэффициент владения (???)) * 0,25
                //                 *      Авансовый платеж за третий кв. = (ОС.Валовая вместимость * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???) * коэффициент владения (???)) * 0,25
                //                 * Иначе если в ОС указан код вида ТС из списка ???_4 Тогда
                //                 *      taxSumYear = ОС.Единица ТС * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???)
                //                 *      Авансовый платеж за первый кв. = (ОС.Единица ТС * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???) * коэффициент владения (???)) * 0,25
                //                 *      Авансовый платеж за второй кв. = (ОС.Единица ТС * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???) * коэффициент владения (???)) * 0,25
                //                 *      Авансовый платеж за третий кв. = (ОС.Единица ТС * Налоговая ставка по транспортному налогу из справочника (???) * коэффициент повышающий/понижающий из справочника (???) * коэффициент владения (???)) * 0,25
                //                 *
                //                 * Сумма налога к уплате() = Сумма исчисленного налога (taxYearSum) - (Авансовый платеж за первый кв. + Авансовый платеж за второй кв. + Авансовый платеж за третий кв.)
                //                 */


                //                // Количество лет, прошедших с года выпуска ТС (гр. 16)
                //                int countOfYearsIssue = ((year - accountingObject.YearOfIssue) + 1) ?? 0;


                //                // Количество полных месяцев владения ТС (налоговый период) (гр. 18)
                //                owningMonthCount = helper.CalculateOwningMonthCount(accountingObject.VehicleRegDate, accountingObject.VehicleDeRegDate, year, 1, 12);

                //                // Количество полных месяцев владения ТС (1 квартал) (гр. 18)
                //                owningMonthCountQuarter1 = helper.CalculateOwningMonthCount(accountingQuarter1?.VehicleRegDate, accountingQuarter1?.VehicleDeRegDate, year, 1, 3);
                //                // Количество полных месяцев владения ТС (2 квартал) (гр. 18)
                //                owningMonthCountQuarter2 = helper.CalculateOwningMonthCount(accountingQuarter2?.VehicleRegDate, accountingQuarter2?.VehicleDeRegDate, year, 4, 6);
                //                // Количество полных месяцев владения ТС (3 квартал) (гр. 18)
                //                owningMonthCountQuarter3 = helper.CalculateOwningMonthCount(accountingQuarter3?.VehicleRegDate, accountingQuarter3?.VehicleDeRegDate, year, 7, 9);


                //                // Коэффициент Кв (налоговый период) (гр. 20)
                //                factorKv = decimal.Round(owningMonthCount / 12m, 4);

                //                // Коэффициент Кв (1 квартал) (гр. 20)
                //                factorKv1 = decimal.Round(owningMonthCountQuarter1 / 3m, 4);
                //                // Коэффициент Кв (2 квартал) (гр. 20)
                //                factorKv2 = decimal.Round(owningMonthCountQuarter2 / 3m, 4);
                //                // Коэффициент Кв (3 квартал) (гр. 20)
                //                factorKv3 = decimal.Round(owningMonthCountQuarter3 / 3m, 4);


                //                // Повышающий коэффициент Кп (налоговый период) (гр. 22)
                //                vehicleTaxFactor = accountingObject.VehicleTaxFactor ?? 0m;

                //                // Повышающий коэффициент Кп (1 квартал) (гр. 22)
                //                vehicleTaxFactorQuarter1 = accountingQuarter1?.VehicleTaxFactor ?? 0m;
                //                // Повышающий коэффициент Кп (2 квартал) (гр. 22)
                //                vehicleTaxFactorQuarter2 = accountingQuarter2?.VehicleTaxFactor ?? 0m;
                //                // Повышающий коэффициент Кп (3 квартал) (гр. 22)
                //                vehicleTaxFactorQuarter3 = accountingQuarter3?.VehicleTaxFactor ?? 0m;


                //                // Сумма исчисленного налога (налоговый период) (гр. 23)
                //                taxSumYear = decimal.Round((accountingObject.Power ?? 0m) * (accountingObject.TaxRateValueTS ?? 0m) * shareRight * (factorKv) * (vehicleTaxFactor), 2);

                //                // Сумма исчисленного налога (1 квартал) (гр. 23)
                //                taxSumYearQuarter1 = decimal.Round(((accountingObject.Power ?? 0m) * (accountingObject.TaxRateValueTS ?? 0m) * shareRight * factorKv1 * vehicleTaxFactor) * _modificator, 2);
                //                // Сумма исчисленного налога (2 квартал) (гр. 23)
                //                taxSumYearQuarter2 = decimal.Round(((accountingObject.Power ?? 0m) * (accountingObject.TaxRateValueTS ?? 0m) * shareRight * factorKv2 * vehicleTaxFactor) * _modificator, 2);
                //                // Сумма исчисленного налога (3 квартал) (гр. 23)
                //                taxSumYearQuarter3 = decimal.Round(((accountingObject.Power ?? 0m) * (accountingObject.TaxRateValueTS ?? 0m) * shareRight * factorKv3 * vehicleTaxFactor) * _modificator, 2);


                //                // Количество полных месяцев использования льготы (налоговый период) (гр. 26)
                //                int actionMonthCount = helper.CalculateTaxExemptionActionMonthCount(accountingObject.TaxExemptionStartDateTS, accountingObject.TaxExemptionEndDateTS, year, 1, 12);

                //                // Количество полных месяцев использования льготы (1 квартал) (гр. 26)
                //                int actionMonthCountQuarter1 = helper.CalculateTaxExemptionActionMonthCount(accountingQuarter1?.TaxExemptionStartDateTS, accountingQuarter1?.TaxExemptionEndDateTS, year, 1, 3);
                //                // Количество полных месяцев использования льготы (2 квартал) (гр. 26)
                //                int actionMonthCountQuarter2 = helper.CalculateTaxExemptionActionMonthCount(accountingQuarter2?.TaxExemptionStartDateTS, accountingQuarter2?.TaxExemptionEndDateTS, year, 4, 6);
                //                // Количество полных месяцев использования льготы (3 квартал) (гр. 26)
                //                int actionMonthCountQuarter3 = helper.CalculateTaxExemptionActionMonthCount(accountingQuarter3?.TaxExemptionStartDateTS, accountingQuarter3?.TaxExemptionEndDateTS, year, 7, 9);


                //                // Коэффициент Кл (налоговый период) (гр. 27)
                //                factorKl = decimal.Round(((actionMonthCount == 0) ? 12 : actionMonthCount) / 12m, 4);

                //                // Коэффициент Кл (1 квартал) (гр. 27)
                //                factorKl1 = decimal.Round(actionMonthCountQuarter1 / 3m, 4);
                //                // Коэффициент Кл (2 квартал) (гр. 27)
                //                factorKl2 = decimal.Round(actionMonthCountQuarter2 / 3m, 4);
                //                // Коэффициент Кл (3 квартал) (гр. 27)
                //                factorKl3 = decimal.Round(actionMonthCountQuarter3 / 3m, 4);


                //                /* //Сумма налоговой льготы в виде снижения налоговой ставки (налоговый период) (гр. 35)
                //                taxExemptionLowSumTS = taxSumYear - (taxSumYear * accountingObject.TaxRateWithExemptionEstateTS ?? 0m / 100m) * shareRight * factorKv * vehicleTaxFactor;

                //                // Сумма налоговой льготы в виде снижения налоговой ставки (1 квартал) (гр. 35)
                //                taxExemptionLowSumTSQuarter1 = taxSumYearQuarter1 - (taxSumYearQuarter1 * accountingQuarter1?.TaxRateWithExemptionEstateTS ?? 0m / 100m) * shareRightQuarter1 * factorKv1 * vehicleTaxFactorQuarter1;
                //                // Сумма налоговой льготы в виде снижения налоговой ставки (2 квартал) (гр. 35)
                //                taxExemptionLowSumTSQuarter2 = taxSumYearQuarter2 - (taxSumYearQuarter2 * accountingQuarter2?.TaxRateWithExemptionEstateTS ?? 0m / 100m) * shareRightQuarter2 * factorKv2 * vehicleTaxFactorQuarter2;
                //                // Сумма налоговой льготы в виде снижения налоговой ставки (3 квартал) (гр. 35)
                //                taxExemptionLowSumTSQuarter3 = taxSumYearQuarter3 - (taxSumYearQuarter3 * accountingQuarter3?.TaxRateWithExemptionEstateTS ?? 0m / 100m) * shareRightQuarter3 * factorKv3 * vehicleTaxFactorQuarter3;
                //                */

                //                string TLLowCode = (accountingObject.TaxLowerTS == null) ? "" : accountingObject.TaxLowerTS.Code;
                //                string TLLowCodeQuarter1 = (accountingQuarter1?.TaxLowerTS == null) ? "" : accountingQuarter1?.TaxLowerTS.Code;
                //                string TLLowCodeQuarter2 = (accountingQuarter2?.TaxLowerTS == null) ? "" : accountingQuarter2?.TaxLowerTS.Code;
                //                string TLLowCodeQuarter3 = (accountingQuarter3?.TaxLowerTS == null) ? "" : accountingQuarter3?.TaxLowerTS.Code;
                //                string TLFreeCode = (accountingObject.TaxFreeTS == null) ? "" : accountingObject.TaxFreeTS.Code;
                //                string TLFreeCodeQuarter1 = (accountingQuarter1?.TaxFreeTS == null) ? "" : accountingQuarter1?.TaxFreeTS.Code;
                //                string TLFreeCodeQuarter2 = (accountingQuarter2?.TaxFreeTS == null) ? "" : accountingQuarter2?.TaxFreeTS.Code;
                //                string TLFreeCodeQuarter3 = (accountingQuarter3?.TaxFreeTS == null) ? "" : accountingQuarter3?.TaxFreeTS.Code;
                //                string TLRateLowerCode = (accountingObject.TaxRateLowerTS == null) ? "" : accountingObject.TaxRateLowerTS.Code;
                //                string TLRateLowerCodeQuarter1 = (accountingQuarter1?.TaxRateLowerTS == null) ? "" : accountingQuarter1?.TaxRateLowerTS.Code;
                //                string TLRateLowerCodeQuarter2 = (accountingQuarter2?.TaxRateLowerTS == null) ? "" : accountingQuarter2?.TaxRateLowerTS.Code;
                //                string TLRateLowerCodeQuarter3 = (accountingQuarter3?.TaxRateLowerTS == null) ? "" : accountingQuarter3?.TaxRateLowerTS.Code;


                //                if (
                //                    (!string.IsNullOrEmpty(TLLowCode) && TLLowCode == "20220") ||
                //                    (!string.IsNullOrEmpty(TLLowCodeQuarter1) && TLLowCodeQuarter1 == "20220") ||
                //                    (!string.IsNullOrEmpty(TLLowCodeQuarter2) && TLLowCodeQuarter2 == "20220") ||
                //                    (!string.IsNullOrEmpty(TLLowCodeQuarter3) && TLLowCodeQuarter3 == "20220")
                //                    )
                //                {
                //                    // Сумма налоговой льготы в виде уменьшения суммы налога (налоговый период) (гр. 31)
                //                    taxLowSum = decimal.Round((accountingObject.Power ?? 0m) * (accountingObject.TaxRateValueTS ?? 0m) * shareRight * factorKl * vehicleTaxFactor * (accountingObject.TaxLowerPercentTS ?? 0m) / 100m, 2);

                //                    // Сумма налоговой льготы в виде уменьшения суммы налога (1 квартал) (гр. 31)
                //                    taxLowSumQuarter1 = decimal.Round(((accountingObject.Power ?? 0m) * (accountingObject.TaxRateValueTS ?? 0m) * shareRight * factorKl1 * vehicleTaxFactor * ((accountingObject.TaxLowerPercentTS ?? 0m) / 100m)) * _modificator, 2);
                //                    // Сумма налоговой льготы в виде уменьшения суммы налога (2 квартал) (гр. 31)
                //                    taxLowSumQuarter2 = decimal.Round(((accountingObject.Power ?? 0m) * (accountingObject.TaxRateValueTS ?? 0m) * shareRight * (factorKl2) * (vehicleTaxFactor) * ((accountingObject.TaxLowerPercentTS ?? 0m) / 100m)) * _modificator, 2);
                //                    // Сумма налоговой льготы в виде уменьшения суммы налога (3 квартал) (гр. 31)
                //                    taxLowSumQuarter3 = decimal.Round(((accountingObject.Power ?? 0m) * (accountingObject.TaxRateValueTS ?? 0m) * shareRight * (factorKl3) * (vehicleTaxFactor) * ((accountingObject.TaxLowerPercentTS ?? 0m) / 100m)) * _modificator, 2);

                //                    calcSum = Math.Round(taxSumYear - taxLowSum - taxExemptionFreeSum - taxExemptionLowSumTS);
                //                    calcSumQuarter1 = Math.Round(taxSumYearQuarter1 - taxLowSumQuarter1 - taxExemptionFreeSumQuarter1 - taxExemptionLowSumTSQuarter1);
                //                    calcSumQuarter2 = Math.Round(taxSumYearQuarter2 - taxLowSumQuarter2 - taxExemptionFreeSumQuarter2 - taxExemptionLowSumTSQuarter2);
                //                    calcSumQuarter3 = Math.Round(taxSumYearQuarter3 - taxLowSumQuarter3 - taxExemptionFreeSumQuarter3 - taxExemptionLowSumTSQuarter3);
                //                }
                //                else
                //                if (
                //                    (!string.IsNullOrEmpty(TLFreeCode) && TLFreeCode == "20210") ||
                //                    (!string.IsNullOrEmpty(TLFreeCodeQuarter1) && TLFreeCodeQuarter1 == "20210") ||
                //                    (!string.IsNullOrEmpty(TLFreeCodeQuarter2) && TLFreeCodeQuarter2 == "20210") ||
                //                    (!string.IsNullOrEmpty(TLFreeCodeQuarter3) && TLFreeCodeQuarter3 == "20210")
                //                    )
                //                {
                //                    // Сумма налоговой льготы в виде освобождения от налогообложения (налоговый период) (гр. 29)
                //                    taxExemptionFreeSum = decimal.Round((accountingObject.Power ?? 0m) * (accountingObject.TaxRateValueTS ?? 0m) * shareRight * (factorKl) * (vehicleTaxFactor), 2);

                //                    // Сумма налоговой льготы в виде освобождения от налогообложения (1 квартал) (гр. 29)
                //                    taxExemptionFreeSumQuarter1 = decimal.Round(((accountingObject.Power ?? 0m) * (accountingObject.TaxRateValueTS ?? 0m) * shareRight * factorKl1 * vehicleTaxFactor) * _modificator, 2);
                //                    // Сумма налоговой льготы в виде освобождения от налогообложения (2 квартал) (гр. 29)
                //                    taxExemptionFreeSumQuarter2 = decimal.Round(((accountingObject.Power ?? 0m) * (accountingObject.TaxRateValueTS ?? 0m) * shareRight * factorKl2 * vehicleTaxFactor) * _modificator, 2);
                //                    // Сумма налоговой льготы в виде освобождения от налогообложения (3 квартал) (гр. 29)
                //                    taxExemptionFreeSumQuarter3 = decimal.Round(((accountingObject.Power ?? 0m) * (accountingObject.TaxRateValueTS ?? 0m) * shareRight * factorKl3 * vehicleTaxFactor) * _modificator, 2);

                //                    calcSum = Math.Round(taxSumYear - taxExemptionFreeSum - taxLowSum - taxExemptionLowSumTS);
                //                    calcSumQuarter1 = Math.Round(taxSumYearQuarter1 - taxExemptionFreeSumQuarter1 - taxLowSumQuarter1 - taxExemptionLowSumTSQuarter1);
                //                    calcSumQuarter2 = Math.Round(taxSumYearQuarter2 - taxExemptionFreeSumQuarter2 - taxLowSumQuarter2 - taxExemptionLowSumTSQuarter2);
                //                    calcSumQuarter3 = Math.Round(taxSumYearQuarter3 - taxExemptionFreeSumQuarter3 - taxLowSumQuarter3 - taxExemptionLowSumTSQuarter3);
                //                }
                //                else
                //                if (
                //                    (!string.IsNullOrEmpty(TLRateLowerCode) && TLRateLowerCode == "20230") ||
                //                    (!string.IsNullOrEmpty(TLRateLowerCodeQuarter1) && TLRateLowerCodeQuarter1 == "20230") ||
                //                    (!string.IsNullOrEmpty(TLRateLowerCodeQuarter2) && TLRateLowerCodeQuarter2 == "20230") ||
                //                    (!string.IsNullOrEmpty(TLRateLowerCodeQuarter3) && TLRateLowerCodeQuarter3 == "20230")
                //                    )
                //                {
                //                    // Сумма налоговой льготы в виде снижения налоговой ставки (налоговый период) (гр. 35)
                //                    taxExemptionLowSumTS = decimal.Round((accountingObject.Power ?? 0m) * ((taxRate - (accountingObject.TaxRateWithExemptionTS ?? 0m)) / 100m) * shareRight * factorKl * vehicleTaxFactor, 2);

                //                    // Сумма налоговой льготы в виде снижения налоговой ставки (1 квартал) (гр. 35)
                //                    taxExemptionLowSumTSQuarter1 = decimal.Round((accountingObject.Power ?? 0m * ((taxRate - accountingObject.TaxRateWithExemptionTS ?? 0m) / 100m) * shareRight * factorKl1 * vehicleTaxFactor) * _modificator, 2);
                //                    // Сумма налоговой льготы в виде снижения налоговой ставки (2 квартал) (гр. 35)
                //                    taxExemptionLowSumTSQuarter2 = decimal.Round((accountingObject.Power ?? 0m * ((taxRate - accountingObject.TaxRateWithExemptionTS ?? 0m) / 100m) * shareRight * factorKl2 * vehicleTaxFactor) * _modificator, 2);
                //                    // Сумма налоговой льготы в виде снижения налоговой ставки (3 квартал) (гр. 35)
                //                    taxExemptionLowSumTSQuarter3 = decimal.Round((accountingObject.Power ?? 0m * ((taxRate - accountingObject.TaxRateWithExemptionTS ?? 0m) / 100m) * shareRight * factorKl3 * vehicleTaxFactor) * _modificator, 2);

                //                    calcSum = Math.Round(taxSumYear - taxExemptionLowSumTS - taxExemptionFreeSum - taxLowSum);
                //                    calcSumQuarter1 = Math.Round(taxSumYearQuarter1 - taxExemptionLowSumTSQuarter1 - taxExemptionFreeSumQuarter1 - taxLowSumQuarter1);
                //                    calcSumQuarter2 = Math.Round(taxSumYearQuarter2 - taxExemptionLowSumTSQuarter2 - taxExemptionFreeSumQuarter2 - taxLowSumQuarter2);
                //                    calcSumQuarter3 = Math.Round(taxSumYearQuarter3 - taxExemptionLowSumTSQuarter3 - taxExemptionFreeSumQuarter3 - taxLowSumQuarter3);
                //                }
                //                else
                //                {
                //                    calcSum = Math.Round(taxSumYear);
                //                    calcSumQuarter1 = Math.Round(taxSumYearQuarter1);
                //                    calcSumQuarter2 = Math.Round(taxSumYearQuarter2);
                //                    calcSumQuarter3 = Math.Round(taxSumYearQuarter3);
                //                }


                //                // Сумма авансовых платежей (1 квартал) (гр. 40)
                //                prepaymentSumFirstQuarter = Math.Round(calcSumQuarter1);
                //                // Сумма авансовых платежей (2 квартал) (гр. 41)
                //                prepaymentSumSecondQuarter = Math.Round(calcSumQuarter2);
                //                // Сумма авансовых платежей (3 квартал) (гр. 42)
                //                prepaymentSumThirdQuarter = Math.Round(calcSumQuarter3);


                //                // Сумма налога, исчисленная к уплате в бюджет (гр. 43)
                //                paymentTaxSum = Math.Round(calcSum - (prepaymentSumFirstQuarter + prepaymentSumSecondQuarter + prepaymentSumThirdQuarter));


                //                accountingCalculatedField = new AccountingCalculatedField()
                //                {

                //                    CalculationDatasource = "ЕУСИ",
                //                    AccountingObjectID = accountingObject.ID,
                //                    CalculatingRecordID = calculatingRecord.ID,
                //                    AccountingObjectName = accountingObject.Name,
                //                    IsEstateMovable = isEstateMovableNSI,
                //                    Year = year,
                //                    CalculateDate = DateTime.Now,
                //                    ConsolidationID = accountingObject.ConsolidationID,
                //                    ExternalID = accountingObject?.ExternalID,
                //                    SubNumber = accountingObject?.SubNumber,
                //                    OKOF = accountingObject?.OKOF2014?.Code,

                //                    IFNS = accountingObject?.IFNS,
                //                    OKTMO = tOKTMO?.PublishCode,
                //                    VehicleKindCode = accountingObject?.TaxVehicleKindCode?.Name,
                //                    //OSNumber =
                //                    DateOfReceipt = accountingObject?.InServiceDate,
                //                    LeavingDate = accountingObject?.LeavingDate,
                //                    VehicleSerialNumber = accountingObject?.SerialNumber,
                //                    VehicleModel = accountingObject?.Model?.Name,
                //                    VehicleSignNumber = accountingObject?.SignNumber,
                //                    VehicleRegDate = accountingObject?.VehicleRegDate,
                //                    VehicleDeRegDate = accountingObject?.VehicleDeRegDate,
                //                    TaxBaseValueTS = accountingObject?.Power,
                //                    TaxBaseValueQuarter1 = taxBaseValueQuarter1,
                //                    TaxBaseValueQuarter2 = taxBaseValueQuarter2,
                //                    TaxBaseValueQuarter3 = taxBaseValueQuarter3,
                //                    TaxBaseMeasureTS = accountingObject?.PowerUnit?.ToString(),
                //                    EcoKlass = accountingObject?.EcoKlass?.Name,
                //                    CountOfYearsIssue = countOfYearsIssue,
                //                    VehicleYearOfIssue = accountingObject?.YearOfIssue,
                //                    VehicleMonthOwn = owningMonthCount,
                //                    ShareRightNumerator = accountingObject.ShareRightNumerator,
                //                    ShareRightDenominator = accountingObject.ShareRightDenominator,
                //                    FactorKv = factorKv,
                //                    FactorKv1 = factorKv1,
                //                    FactorKv2 = factorKv2,
                //                    FactorKv3 = factorKv3,
                //                    TaxRate = taxRate,
                //                    TaxRateQuarter1 = taxRateQuarter1,
                //                    TaxRateQuarter2 = taxRateQuarter2,
                //                    TaxRateQuarter3 = taxRateQuarter3,
                //                    InitialCost = accountingObject?.InitialCost,
                //                    VehicleTaxFactor = vehicleTaxFactor,
                //                    VehicleTaxFactorQuarter1 = vehicleTaxFactor,
                //                    VehicleTaxFactorQuarter2 = vehicleTaxFactor,
                //                    VehicleTaxFactorQuarter3 = vehicleTaxFactor,
                //                    TaxSumYear = taxSumYear,
                //                    TaxSumYearQuarter1 = taxSumYearQuarter1,
                //                    TaxSumYearQuarter2 = taxSumYearQuarter2,
                //                    TaxSumYearQuarter3 = taxSumYearQuarter3,
                //                    TaxExemptionStartDate = accountingObject?.TaxExemptionStartDateTS,
                //                    TaxExemptionEndDate = accountingObject?.TaxExemptionEndDateTS,
                //                    CountFullMonthsBenefit = actionMonthCount,
                //                    FactorKl = factorKl,
                //                    FactorKl1 = factorKl1,
                //                    FactorKl2 = factorKl2,
                //                    FactorKl3 = factorKl3,
                //                    TaxExemptionFree = accountingObject?.TaxFreeTS?.PublishCode,
                //                    TaxExemptionFreeSum = taxExemptionFreeSum,
                //                    TaxExemptionFreeSumQuarter1 = taxExemptionFreeSumQuarter1,
                //                    TaxExemptionFreeSumQuarter2 = taxExemptionFreeSumQuarter2,
                //                    TaxExemptionFreeSumQuarter3 = taxExemptionFreeSumQuarter3,
                //                    TaxLow = accountingObject?.TaxLowerTS?.PublishCode,
                //                    TaxLowSum = taxLowSum,
                //                    TaxLowSumQuarter1 = taxLowSumQuarter1,
                //                    TaxLowSumQuarter2 = taxLowSumQuarter2,
                //                    TaxLowSumQuarter3 = taxLowSumQuarter3,
                //                    TaxLowerPercent = accountingObject?.TaxLowerPercent,
                //                    TaxExemptionLow = accountingObject?.TaxRateLowerTS?.PublishCode,
                //                    TaxRateWithExemption = accountingObject?.TaxRateWithExemptionTS,
                //                    TaxExemptionLowSum = taxExemptionLowSumTS,
                //                    InOtherSystem = accountingObject.InOtherSystem,
                //                    //TaxDeduction =
                //                    //TaxDeductionSum =
                //                    CalcSum = calcSum,
                //                    PrepaymentSumFirstQuarter = prepaymentSumFirstQuarter,
                //                    PrepaymentSumSecondQuarter = prepaymentSumSecondQuarter,
                //                    PrepaymentSumThirdQuarter = prepaymentSumThirdQuarter,
                //                    PaymentTaxSum = paymentTaxSum,
                //                    EUSINumber = EUSINumber,

                //                };

                //            }

                //            if (accountingCalculatedField != null)
                //            {
                //                helper.ResidualCostByMonth(accountingCalculatedField, year, accountingObject.Oid);

                //                accountingCFRepo.Create(accountingCalculatedField);

                //                unitOfWork.SaveChanges();
                //                counter++;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        //if (consolidationGroupIdsAr.Length > 0)
                //        //{
                //        //    //throw new Exception(AccountingCalculationErrors.AoNotFoundFromGroupPositionConsolidation);
                //        //    //_osNotFoundMessage = AccountingCalculationErrors.AoNotFoundFromGroupPositionConsolidation;
                //        //}

                //        //если не нашли ОС - пишем ошибку в журнал
                //        AccountingCalculationHelper helper = new AccountingCalculationHelper(unitOfWork);
                //        helper.CreateCulatingRecordError(unitOfWork, year, taxRateTypeCode, _osNotFoundMessage, _securityUser);

                //        //возвращаем текст и тип ошибки в попап                        
                //        err = 1;
                //        mess = _errorMessage;
                //    }

                //    if (counter > 0)
                //        unitOfWork.Commit();
                //    else
                //    {
                //        unitOfWork.Rollback();
                //        err = 1;
                //        mess = _errorMessage;
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                err = 1;
                mess = ex.Message;
            }

            var res = new
            {
                error = err,
                message = mess
            };
            return Ok(res);
        }


        /// <summary>
        /// Массовое направление заявок на проверку из ListView.
        /// </summary>
        /// <param name="ids">ИД-ы заявок.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("erOnDirected/{ids}")]
        public IHttpActionResult EROnDirected(string ids)
        {
            var fail = 0;
            var mess = "Заявки направлены на проверку.";
            var arr = ids.Split(',');
            var uow = CreateTransactionUnitOfWork();
            try
            {
                foreach (var item in arr)
                {
                    int id = 0;
                    if (int.TryParse(item, out id))
                        _erService.InvokeWFStage(uow, id, "DIRECTED");
                }
                uow.Commit();
            }
            catch (Exception ex)
            {
                fail = 1;
                mess = ex.ToStringWithInner();
                uow.Rollback();
            }
            var res = new
            {
                err = fail,
                message = mess
            };
            return Ok(res);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids">ИД-ы заявок.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("notifyOfERComplited/{ids}")]
        public IHttpActionResult EROnComplited(string ids)
        {

            var fail = 0;
            var mess = "Информация по заявкам направлена.";
            var arr = ids.Split(',');
            var intAr = new int[arr.Length];
            var uow = CreateTransactionUnitOfWork();
            try
            {

                for (int i = 0; i < arr.Length; i++)
                {
                    intAr[i] = int.Parse(arr[i]);
                }
                _erService.SendUserNotification(uow, intAr);
                uow.Commit();
            }
            catch (Exception ex)
            {
                fail = 1;
                mess = ex.ToStringWithInner();
                uow.Rollback();
            }
            var res = new
            {
                err = fail,
                message = mess
            };
            return Ok(res);
        }


        /// <summary>
        /// Уведомление о получении данных из БУС.
        /// </summary>
        /// <param name="ids">Выбранные ОС/НМА</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("notifyOfNotGettingData/{ids}")]
        public WebHttp.IHttpActionResult NotifyOfNotGettingData(string ids)
        {
            var err = 0;
            var mess = "Уведомления отправлены";
            int[] selectedItemsArr = ids.Split(';').Select(x => int.Parse(x)).ToArray();
            try
            {
                using (var uow = CreateTransactionUnitOfWork())
                {
                    var draftOsConfig = GetConfig("DraftOSPassBus");
                    var serv = draftOsConfig.GetService<EUSI.Services.Accounting.DraftOSPassBusService>();
                    if (serv != null)
                    {
                        var selectedItems = serv.GetAll(uow)
                            .Where(a => selectedItemsArr.Contains(a.ID)
                                        && (!a.NotifyBUS || (!a.NotifyOriginator && a.ERContactEmail != null))
                                  )
                            .ToList();

                        if (!selectedItems.Any())
                        {
                            mess = "Новых уведомлений для отправки нет.";
                        }
                        else
                        {
                            var sent = 0;
                            var noSent = 0;

                            //уведомления для БУС
                            var busGroup = selectedItems.Where(f => !f.NotifyBUS && f.Consolidation != null)
                                .GroupBy(g => g.Consolidation.Code);

                            //уведомления для инициаторов
                            var initGroup = selectedItems.Where(f => !f.NotifyOriginator && f.ERContactEmail != null)
                               .GroupBy(g => g.ERContactEmail);

                            //состав ОС в уведомлении для инициатора и уведмолении отв. БУС не равны и потому не могут быть объединены 
                            //в одном письме, в случае, если отв. БУС==инициатору
                            var busTmp = uow.GetRepository<UserNotificationTemplate>()
                                 .Filter(f => !f.Hidden && f.ByEmail && f.Code == "OS_DraftOSPassBuss_BUS")
                                 .FirstOrDefault();

                            var initTmp = uow.GetRepository<UserNotificationTemplate>()
                                .Filter(f => !f.Hidden && f.ByEmail && f.Code == "OS_DraftOSPassBuss_Originator")
                                .FirstOrDefault();

                            foreach (var busGR in busGroup)
                            {

                                if (busTmp != null)
                                {

                                    //адрес отв. БУС
                                    var busEmail = uow.GetRepository<ImportHistory>()
                                        .FilterAsNoTracking(f =>
                                        ((f.Consolidation != null && f.Consolidation.Code == busGR.Key)
                                        || (f.Consolidation == null && f.Society != null && f.Society.ConsolidationUnit != null && f.Society.ConsolidationUnit.Code == busGR.Key))
                                        && !String.IsNullOrEmpty(f.ContactEmail)
                                        && f.Mnemonic == nameof(AccountingObject))
                                        .OrderByDescending(s => s.ImportDateTime)
                                        .FirstOrDefault()?.ContactEmail;


                                    ICollection<DraftOS> cols = busGR.ToList();
                                var item = new { OSS = cols };                                
                                if (_emailService.SendNotice(uow, item, null, busEmail, "OS_DraftOSPassBuss_BUS", new DraftOS()))
                                {
                                    sent++;
                                    foreach (var os in item.OSS)
                                    {
                                        uow.GetRepository<CorpProp.Entities.Settings.UserNotification>()
                                           .Create(new CorpProp.Entities.Settings.UserNotification()
                                           {
                                               Entity = new Base.Entities.Complex.LinkBaseObject(os),
                                               Date = DateTime.Now,
                                               IsSentByEmail = true,
                                               Template = busTmp,
                                               EmailRecipient = busEmail,
                                               ByEmail = true
                                           });
                                    }                                   
                                }
                                else
                                    noSent++;
                                }
                            }
                            foreach (var initGR in initGroup)
                            {
                                if (initTmp != null)
                                {
                                    //адрес инициатора заявки
                                    var initEmail = initGR?.Key;
                                ICollection<DraftOS> cols = initGR.ToList();
                                var item = new { OSS = cols };
                                if (_emailService.SendNotice(uow, item, null, initEmail, "OS_DraftOSPassBuss_Originator", new DraftOS()))
                                {
                                    sent++;
                                    foreach (var os in item.OSS)
                                    {
                                        uow.GetRepository<CorpProp.Entities.Settings.UserNotification>()
                                           .Create(new CorpProp.Entities.Settings.UserNotification()
                                           {
                                               Entity = new Base.Entities.Complex.LinkBaseObject(os),
                                               Date = DateTime.Now,
                                               IsSentByEmail = true,
                                               Template = initTmp,
                                               EmailRecipient = initEmail,
                                               ByEmail = true
                                           });
                                    }
                                }
                                else
                                    noSent++;
                                }
                            }
                            mess = $"Всего отправлено: {sent} уведомлений.";
                            if (noSent > 0)
                            {
                                err = 1;
                                mess += System.Environment.NewLine;
                                mess += $"Не удалось отправить: {noSent} уведомлений.";
                            }
                        }
                    }
                    uow.SaveChanges();
                    uow.Commit();
                }
            }
            catch (Exception ex)
            {
                err = 1;
                mess = ex.Message;
            }

            var res = new
            {
                err = err,
                message = mess
            };
            return Ok(res);
        }

    }
}

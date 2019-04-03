using Base.DAL;
using Base.Security;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Security;
using EUSI.Entities.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EUSI.Helpers
{
    public class AccountingCalculationHelper
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Инициализирует AccountingCalculationHelper.
        /// </summary>
        /// <param name="unitOfWork"></param>
        public AccountingCalculationHelper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Проверка заполнения требуемых полей ОС для транспортного налога.
        /// </summary>
        /// <param name="accountingObject">ОС</param>
        /// <param name="calculatingRecordId">ИД Записи реестра расчетов.</param>
        /// <returns>true - проверка пройдена.</returns>
        public bool CheckTransportObject(AccountingObject accountingObject, int calculatingRecordId, int year)
        {
            string result = "";

            if (accountingObject.ResidualCost == null)
                result += $"{AccountingCalculationErrors.EmptyResidualCost}. ";

            if (accountingObject.TaxRateValueTS == null)
                result += $"{AccountingCalculationErrors.EmptyTaxRate}. ";

            if (accountingObject.Power == null)
                result += $"{AccountingCalculationErrors.EmptyPower}. ";

            if (accountingObject.VehicleRegDate == null)
                result += $"{AccountingCalculationErrors.EmptyVehicleRegDate}. ";

            if (accountingObject.YearOfIssue == null)
                result += $"{AccountingCalculationErrors.EmptyYearOfIssue}. ";

            if (accountingObject.YearOfIssue > year)
                result += $"{AccountingCalculationErrors.YearOfIssueMoreTaxPeriod}. ";

            if (string.IsNullOrEmpty(result))
                return true;

            this.WriteCalculatingError(accountingObject, calculatingRecordId, result);
            return false;

        }

        /// <summary>
        /// Проверка заполнения требуемых полей ОС для земельных участков.
        /// </summary>
        /// <param name="accountingObject">ОС</param>
        /// <param name="calculatingRecordId">ИД Записи реестра расчетов.</param>
        /// <returns>true - проверка пройдена.</returns>
        public bool CheckLandObject(AccountingObject accountingObject, int calculatingRecordId)
        {
            string result = "";

            if (accountingObject.CadastralValue == null)
                result += $"{AccountingCalculationErrors.EmptyCadastralValue}. ";

            if (accountingObject.TaxRateValueLand == null)
                result += $"{AccountingCalculationErrors.EmptyTaxRate}. ";

            if (accountingObject.ShareRightDenominator.Equals(0.00m))
                result += $"{AccountingCalculationErrors.ShareRightDenominator}. ";

            if (accountingObject.TaxLowerLand != null && (accountingObject.TaxLowerLand.Code == "3022200" && (accountingObject.TaxLowerPercentLand == null || accountingObject.TaxLowerPercentLand == decimal.Zero)))
                result += $"{AccountingCalculationErrors.TaxLowerLand}. ";

            if (accountingObject.InServiceDate == null)
                result += $"{AccountingCalculationErrors.EmptyInServiceDate}. ";

            if ((accountingObject.TaxLowerLand != null | accountingObject.TaxFreeLand != null | accountingObject.TaxRateLowerLand != null) && accountingObject.TaxExemptionStartDateLand == null)
                result += $"{AccountingCalculationErrors.EmptyTaxExemptionStartDateLand}. ";

            if (string.IsNullOrEmpty(result))
                return true;

            this.WriteCalculatingError(accountingObject, calculatingRecordId, result);
            return false;
        }

        /// <summary>
        /// Создает запись в журнал о результате расчета налога на имущество
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="year"></param>
        /// <param name="taxRateTypeCode"></param>
        /// <param name="errMessage">Текст ошибки</param>
        public void CreateCulatingRecordError(ITransactionUnitOfWork unitOfWork, int year, string taxRateTypeCode, string errMessage, ISecurityUser _securityUser)
        {
            var calcRecordRepo = unitOfWork.GetRepository<CalculatingRecord>();
            TaxRateType taxRateType = unitOfWork.GetRepository<TaxRateType>().Find(f => f.Code == taxRateTypeCode);
            SibUser initiator = _securityUser != null
                ? unitOfWork.GetRepository<SibUser>().Find(f => f.ID == _securityUser.ID)
                : null;
            CalculatingRecord calculatingRecord =
                calcRecordRepo.Create(new CalculatingRecord()
                {
                    Year = year,
                    CalculatingDate = DateTime.Now,
                    Result = errMessage,
                    TaxRateType = taxRateType,
                    Initiator = initiator
                });
            unitOfWork.SaveChanges();
            unitOfWork.Commit();
        }

        /// <summary>
        /// Проверка заполнения требуемых полей ОС для ОИ.
        /// </summary>
        /// <param name="accountingObject">ОС</param>
        /// <param name="calculatingRecordId">ИД Записи реестра расчетов.</param>
        /// <returns>true - проверка пройдена.</returns>
        public bool CheckInventoryObject(AccountingObject accountingObject, int calculatingRecordId, string taxBaseCode)
        {
            string result = "";

            if (accountingObject.TaxRateValue == null)
                result += $"{AccountingCalculationErrors.EmptyTaxRate}. ";

            if (accountingObject.TaxLowerID != null && (accountingObject.TaxLowerPercent == null || decimal.Equals(accountingObject.TaxLowerPercent, decimal.Zero)))
                result += $"{AccountingCalculationErrors.EmptyTaxLowerPercent}. ";

            if (accountingObject.TaxBaseID == null)
                result += $"{AccountingCalculationErrors.EmptyTaxBase}. ";
            if (!string.IsNullOrEmpty(taxBaseCode) && taxBaseCode == "102" && (accountingObject.CadastralValue == null || decimal.Equals(accountingObject.CadastralValue, decimal.Zero)))
                result += $"{AccountingCalculationErrors.EmptyCadastralValueForTaxBase}. ";

            if (string.IsNullOrEmpty(result))
                return true;

            this.WriteCalculatingError(accountingObject, calculatingRecordId, result);
            return false;

        }

        /// <summary>
        /// Запись ошибки расчета.
        /// </summary>
        /// <param name="accountingObject">ОС.</param>
        /// <param name="calculatingRecordId">ИД Записи реестра расчетов.</param>
        /// <param name="message">Сообщение.</param>
        public void WriteCalculatingError(AccountingObject accountingObject, int calculatingRecordId, string message)
        {
            _unitOfWork.GetRepository<CalculatingError>().Create(new CalculatingError()
            {
                Message = message,
                CalculatingRecordID = calculatingRecordId,
                AccountingObjectName = (accountingObject != null) ? $"{accountingObject.InventoryNumber ?? ((accountingObject.EUSINumber != null) ? accountingObject.EUSINumber.ToString() : "")} {(accountingObject.Name ?? accountingObject.NameEUSI)}" : ""
            });

            _unitOfWork.SaveChanges();
        }

        /// <summary>
        /// Остаточная стоимость на 1 число каждого месяца.
        /// </summary>
        /// <param name="accountingCalculatedField">Расчет.</param>
        /// <param name="year">Год.</param>
        /// <param name="accountingObjectOid">OID ОС.</param>
        public void ResidualCostByMonth(AccountingCalculatedField accountingCalculatedField, int year, Guid accountingObjectOid)
        {
            var residualCostByMonth = _unitOfWork.GetRepository<AccountingObject>()
                .FilterAsNoTracking(f =>
                    f.Oid == accountingObjectOid &&
                    f.ActualDate.Value.Year == year &&
                    (f.ActualDate != null &&
                     (f.ActualDate.Value.Day == 1 || (f.ActualDate.Value.Day == 31 && f.ActualDate.Value.Month == 12))))
                .Select(s => new
                {
                    s.ActualDate,
                    s.ResidualCost
                })
                .OrderBy(o => o.ResidualCost)
                .ToList();

            foreach (var residualCost in residualCostByMonth)
            {
                if (residualCost.ActualDate?.Day == 1)
                {
                    switch (residualCost.ActualDate?.Month)
                    {
                        case 1:
                            accountingCalculatedField.ResidualCost_01 = residualCost.ResidualCost ?? 0;
                            break;
                        case 2:
                            accountingCalculatedField.ResidualCost_02 = residualCost.ResidualCost ?? 0;
                            break;
                        case 3:
                            accountingCalculatedField.ResidualCost_03 = residualCost.ResidualCost ?? 0;
                            break;
                        case 4:
                            accountingCalculatedField.ResidualCost_04 = residualCost.ResidualCost ?? 0;
                            break;
                        case 5:
                            accountingCalculatedField.ResidualCost_05 = residualCost.ResidualCost ?? 0;
                            break;
                        case 6:
                            accountingCalculatedField.ResidualCost_06 = residualCost.ResidualCost ?? 0;
                            break;
                        case 7:
                            accountingCalculatedField.ResidualCost_07 = residualCost.ResidualCost ?? 0;
                            break;
                        case 8:
                            accountingCalculatedField.ResidualCost_08 = residualCost.ResidualCost ?? 0;
                            break;
                        case 9:
                            accountingCalculatedField.ResidualCost_09 = residualCost.ResidualCost ?? 0;
                            break;
                        case 10:
                            accountingCalculatedField.ResidualCost_10 = residualCost.ResidualCost ?? 0;
                            break;
                        case 11:
                            accountingCalculatedField.ResidualCost_11 = residualCost.ResidualCost ?? 0;
                            break;
                        case 12:
                            accountingCalculatedField.ResidualCost_12 = residualCost.ResidualCost ?? 0;
                            break;
                    }
                }
                else if (residualCost.ActualDate?.Day == 31)
                    accountingCalculatedField.ResidualCost_13 = residualCost.ResidualCost ?? 0;
            }
        }

        /// <summary>
        /// Получить историческую запись по ОС/НМА на конец указанного квартала.
        /// </summary>
        /// <param name="accountingObjectsList">ОС.</param>
        /// <param name="year">Год.</param>
        /// <param name="quarter">Номер квартала.</param>
        /// <param name="accountingObjectOid">Oid.</param>
        /// <returns>Историческая запись по ОС/НМА на конец указанного квартала.</returns>
        public AccountingObject GetAccountingObjectSetQuarter(List<AccountingObject> accountingObjectsList, int year, int quarter, Guid accountingObjectOid)
        {
            decimal result = 0;
            string sYear = year.ToString();
            string sMonth = "";
            string sDay = "31";
            DateTime tActualDate = DateTime.MinValue;

            int monthQuarter = 0;
            switch (quarter)
            {
                case (1):
                    monthQuarter = 3;
                    break;
                case (2):
                    monthQuarter = 6;
                    sDay = "30";
                    break;
                case (3):
                    monthQuarter = 9;
                    sDay = "30";
                    break;
            }


            sMonth = (monthQuarter.ToString().Length < 2) ? ("0" + monthQuarter.ToString()) : monthQuarter.ToString();
            tActualDate = DateTime.Parse(sYear + "-" + sMonth + "-" + sDay);
            AccountingObject item = accountingObjectsList.FirstOrDefault(w => w.IsHistory &&
                                          w.Oid == accountingObjectOid &&
                                          w.ActualDate != null && w.ActualDate.Value.Date <= tActualDate.Date);
            return item;
        }

        /// <summary>
        /// Получить кадастровую стоимость на указанный квартал.
        /// </summary>
        /// <param name="accountingObjectsList">ОС.</param>
        /// <param name="year">Год.</param>
        /// <param name="quarter">Номер квартала.</param>
        /// <param name="accountingObjectOid">Oid.</param>
        /// <returns>Кадастровая стоимость на квартал.</returns>
        public decimal GetCadastralCostSetQuarter(List<AccountingObject> accountingObjectsList, int year, int quarter, Guid accountingObjectOid)
        {
            decimal result = 0;
            string sYear = year.ToString();
            string sMonth = "";
            string sDay = "01";
            DateTime tActualDate = DateTime.MinValue;

            int monthQuarter = 0;
            switch (quarter)
            {
                case (1):
                    monthQuarter = 3;
                    break;
                case (2):
                    monthQuarter = 6;
                    break;
                case (3):
                    monthQuarter = 9;
                    break;
            }


            sMonth = (monthQuarter.ToString().Length < 2) ? ("0" + monthQuarter.ToString()) : monthQuarter.ToString();
            tActualDate = DateTime.Parse(sYear + "-" + sMonth + "-" + sDay);
            AccountingObject item = accountingObjectsList.FirstOrDefault(w => w.IsHistory &&
                                          w.Oid == accountingObjectOid &&
                                          w.ActualDate != null && w.ActualDate.Value.Date == tActualDate.Date);

            if (item != null)
                result = item?.CadastralValue ?? 0;

            return result;
        }



        /// <summary>
        /// Вычисление средней стоимости за I кв.
        /// </summary>
        /// <param name="accountingObjectsList">ОС.</param>
        /// <param name="year">Год.</param>
        /// <param name="accountingObjectOid">Oid.</param>
        /// <returns>Средняя стоимость.</returns>
        public decimal CalculateAvgPriceFirstQuarter(List<AccountingObject> accountingObjectsList, int year, Guid accountingObjectOid)
        {

            /* Если Сумма оценочного обязательства ao.EstimatedAmount не пусто больше 0 
            *      И Срок списания оценочного обязательства не пусто больше 0 
            *      И Дата начала списания оценочного обязательства не пусто
            *      тогда при расчете средней и среднегодовой стоимости из остаточной стоимости вычитать сумму обязательств (EstimatedAmount)
            *      
            *      ((ao.EstimatedAmount != null && ao.EstimatedAmount > 0) && ao.EstimatedAmountWriteOffStart != null && ao.EstimatedAmountWriteOffTerm != null)
            */
            decimal result = 0;
            decimal tResidualCost = 0;
            string sYear = year.ToString();
            string sMonth = "";
            string sDay = "01";
            DateTime tActualDate = DateTime.MinValue;
            for (int m = 1; m <= 4; m++)
            {
                sMonth = (m.ToString().Length < 2) ? ("0" + m.ToString()) : m.ToString();
                tActualDate = DateTime.Parse(sYear + "-" + sMonth + "-" + sDay);
                tActualDate = tActualDate.AddMonths(-1);
                AccountingObject item = accountingObjectsList.FirstOrDefault(w => w.IsHistory == true &&
                              w.Oid == accountingObjectOid &&
                              w.ActualDate != null && w.ActualDate.Value.Date == tActualDate.Date);

                if (item != null && ((item.EstimatedAmount != null && item.EstimatedAmount > 0) && item.EstimatedAmountWriteOffStart != null && item.EstimatedAmountWriteOffTerm != null))
                    tResidualCost = item?.ResidualCost - item.EstimatedAmount ?? 0;
                else
                    tResidualCost = item?.ResidualCost ?? 0;

                result += tResidualCost;
                tResidualCost = 0;
            }

            return result / 4;
        }

        /// <summary>
        /// Вычисление средней стоимости за II кв.
        /// </summary>
        /// <param name="accountingObjectsList">ОС.</param>
        /// <param name="year">Год.</param>
        /// <param name="accountingObjectOid">Oid.</param>
        /// <param name="taxBaseCode">Код налоговой базы.</param>
        /// <returns>Средняя стоимость.</returns>
        public decimal CalculateAvgPriceSecondQuarter(List<AccountingObject> accountingObjectsList, int year, Guid accountingObjectOid, string taxBaseCode)
        {
            /* Если Сумма оценочного обязательства ao.EstimatedAmount не пусто больше 0 
            *      И Срок списания оценочного обязательства не пусто больше 0 
            *      И Дата начала списания оценочного обязательства не пусто
            *      тогда при расчете средней и среднегодовой стоимости из остаточной стоимости вычитать сумму обязательств (EstimatedAmount)
            *      
            *      ((ao.EstimatedAmount != null && ao.EstimatedAmount > 0) && ao.EstimatedAmountWriteOffStart != null && ao.EstimatedAmountWriteOffTerm != null)
            */
            decimal result = 0;
            decimal tResidualCost = 0;
            string sYear = year.ToString();
            string sMonth = "";
            string sDay = "01";
            DateTime tActualDate = DateTime.MinValue;
            for (int m = 1; m <= 7; m++)
            {
                sMonth = (m.ToString().Length < 2) ? ("0" + m.ToString()) : m.ToString();
                tActualDate = DateTime.Parse(sYear + "-" + sMonth + "-" + sDay);
                tActualDate = tActualDate.AddMonths(-1);
                AccountingObject item = accountingObjectsList.FirstOrDefault(w => w.IsHistory &&
                                              w.Oid == accountingObjectOid &&
                                              w.ActualDate != null && w.ActualDate.Value.Date == tActualDate.Date);

                if (item != null && ((item.EstimatedAmount != null && item.EstimatedAmount > 0) && item.EstimatedAmountWriteOffStart != null && item.EstimatedAmountWriteOffTerm != null))
                    tResidualCost = item?.ResidualCost - item.EstimatedAmount ?? 0;
                else
                    tResidualCost = item?.ResidualCost ?? 0;

                result += tResidualCost;
                tResidualCost = 0;
            }

            return result / 7;
        }

        /// <summary>
        /// Вычисление средней стоимости за III кв.
        /// </summary>
        /// <param name="accountingObjectsList">ОС.</param>
        /// <param name="year">Год.</param>
        /// <param name="accountingObjectOid">Oid.</param>
        /// <param name="taxBaseCode">Код налоговой базы.</param>
        /// <returns>Средняя стоимость.</returns>
        public decimal CalculateAvgPriceThirdQuarter(List<AccountingObject> accountingObjectsList, int year, Guid accountingObjectOid, string taxBaseCode)
        {
            /* Если Сумма оценочного обязательства ao.EstimatedAmount не пусто больше 0 
            *      И Срок списания оценочного обязательства не пусто больше 0 
            *      И Дата начала списания оценочного обязательства не пусто
            *      тогда при расчете средней и среднегодовой стоимости из остаточной стоимости вычитать сумму обязательств (EstimatedAmount)
            *      
            *      ((ao.EstimatedAmount != null && ao.EstimatedAmount > 0) && ao.EstimatedAmountWriteOffStart != null && ao.EstimatedAmountWriteOffTerm != null)
            */
            decimal result = 0;
            decimal tResidualCost = 0;
            string sYear = year.ToString();
            string sMonth = "";
            string sDay = "01";
            DateTime tActualDate = DateTime.MinValue;
            for (int m = 1; m <= 10; m++)
            {
                sMonth = (m.ToString().Length < 2) ? ("0" + m.ToString()) : m.ToString();
                tActualDate = DateTime.Parse(sYear + "-" + sMonth + "-" + sDay);
                tActualDate = tActualDate.AddMonths(-1);
                AccountingObject item = accountingObjectsList.FirstOrDefault(w =>
                    w.IsHistory && w.Oid == accountingObjectOid &&
                    w.ActualDate != null && w.ActualDate.Value.Date == tActualDate.Date);

                if (item != null && ((item.EstimatedAmount != null && item.EstimatedAmount > 0) && item.EstimatedAmountWriteOffStart != null && item.EstimatedAmountWriteOffTerm != null))
                    tResidualCost = item?.ResidualCost - item.EstimatedAmount ?? 0;
                else
                    tResidualCost = item?.ResidualCost ?? 0;

                result += tResidualCost;
                tResidualCost = 0;
            }

            return result / 10;
        }

        /// <summary>
        /// Вычисление средней стоимости за год.
        /// </summary>
        /// <param name="accountingObjectsList">ОС.</param>
        /// <param name="year">Год.</param>
        /// <param name="accountingObjectOid">Oid.</param>
        /// <returns>Средняя стоимость.</returns>
        public decimal CalculateAvgPriceYear(List<AccountingObject> accountingObjectsList, int year, Guid accountingObjectOid)
        {
            /* Если Сумма оценочного обязательства ao.EstimatedAmount не пусто больше 0 
            *      И Срок списания оценочного обязательства не пусто больше 0 
            *      И Дата начала списания оценочного обязательства не пусто
            *      тогда при расчете средней и среднегодовой стоимости из остаточной стоимости вычитать сумму обязательств (EstimatedAmount)
            *      
            *      ((ao.EstimatedAmount != null && ao.EstimatedAmount > 0) && ao.EstimatedAmountWriteOffStart != null && ao.EstimatedAmountWriteOffTerm != null)
            */
            decimal result = 0;
            decimal tResidualCost = 0;
            string sYear = year.ToString();
            string sMonth = "";
            string sDay = "01";
            DateTime tActualDate = DateTime.MinValue;
            for (int m = 1; m <= 12; m++)
            {
                sMonth = (m.ToString().Length < 2) ? ("0" + m.ToString()) : m.ToString();
                tActualDate = DateTime.Parse(sYear + "-" + sMonth + "-" + sDay);
                tActualDate = tActualDate.AddMonths(-1);
                AccountingObject item = accountingObjectsList.FirstOrDefault(w =>
                    w.IsHistory && w.Oid == accountingObjectOid &&
                    w.ActualDate != null && w.ActualDate.Value.Date == tActualDate.Date);

                if (item != null && ((item.EstimatedAmount != null && item.EstimatedAmount > 0) && item.EstimatedAmountWriteOffStart != null && item.EstimatedAmountWriteOffTerm != null))
                    tResidualCost = item?.ResidualCost - item.EstimatedAmount ?? 0;
                else
                    tResidualCost = item?.ResidualCost ?? 0;

                result += tResidualCost;
                tResidualCost = 0;
            }

            DateTime tDate = new DateTime(year, 12, 31);

            AccountingObject item13 = accountingObjectsList.FirstOrDefault(w => w.IsHistory && w.Oid == accountingObjectOid &&
                                                       w.ActualDate != null && ((w.ActualDate.Value.Year == tDate.Year) && (w.ActualDate.Value.Month == tDate.Month)));
            if (item13 != null && ((item13.EstimatedAmount != null && item13.EstimatedAmount > 0) && item13.EstimatedAmountWriteOffStart != null && item13.EstimatedAmountWriteOffTerm != null))
                tResidualCost = item13?.ResidualCost - item13.EstimatedAmount ?? 0;
            else
                tResidualCost = item13?.ResidualCost ?? 0;

            result += tResidualCost;
            tResidualCost = 0;

            return result / 13;
        }

        /// <summary>
        /// Расчет количества месяцев действия льготных условий налогообложения.
        /// </summary>
        /// <param name="inSeviceDate">ОС.</param>
        /// <param name="leavingDate"></param>
        /// <param name="calcYear">Год.</param>
        /// <param name="startMonth">Месяц начала расчета.</param>
        /// <param name="endMonth">Месяц окончания расчета.</param>
        /// <returns>Кол-во месяцев.</returns>
        public int CalculateTaxExemptionActionMonthCount(DateTime? TaxExemptionStartDateLand, DateTime? TaxExemptionEndDateLand, int calcYear, int startMonth, int endMonth)
        {
            int monthCounter = 0;
            int? year = calcYear;//inSeviceDate?.Year;

            if (year == null | TaxExemptionStartDateLand == null)
                return monthCounter;

            while (year <= calcYear)
            {
                for (int i = startMonth; i <= endMonth; i++)
                {
                    if (TaxExemptionStartDateLand.Value.Year <= calcYear && TaxExemptionStartDateLand.Value.Month <= i &&
                        (TaxExemptionEndDateLand == null || TaxExemptionStartDateLand.Value.Year >= calcYear || (TaxExemptionStartDateLand.Value.Year == calcYear && TaxExemptionEndDateLand.Value.Month >= i)))
                        monthCounter++;
                }

                year++;
            }

            return monthCounter;
        }
        /// <summary>
        /// Расчет количества месяцев отсутствия льготных условий налогообложения.
        /// </summary>
        /// <param name="inSeviceDate">ОС.</param>
        /// <param name="leavingDate"></param>
        /// <param name="calcYear">Год.</param>
        /// <param name="startMonth">Месяц начала расчета.</param>
        /// <param name="endMonth">Месяц окончания расчета.</param>
        /// <returns>Кол-во месяцев.</returns>
        public int CalculateTaxExemptionNonActionMonthCount(DateTime? TaxExemptionStartDateLand, DateTime? TaxExemptionEndDateLand, int calcYear, int startMonth, int endMonth)
        {
            int monthCounter = 0;

            int mc = CalculateTaxExemptionActionMonthCount(TaxExemptionStartDateLand, TaxExemptionEndDateLand, calcYear, startMonth, endMonth);
            monthCounter = (endMonth + 1 - startMonth) - mc;

            return monthCounter;
        }

        /// <summary>
        /// Расчет количества месяцев владения ОСом.
        /// </summary>
        /// <param name="inSeviceDate">ОС.</param>
        /// <param name="leavingDate"></param>
        /// <param name="calcYear">Год.</param>
        /// <param name="startMonth">Месяц начала расчета.</param>
        /// <param name="endMonth">Месяц окончания расчета.</param>
        /// <returns>Кол-во месяцев.</returns>
        public int CalculateOwningMonthCount(DateTime? inSeviceDate, DateTime? leavingDate, int calcYear, int startMonth, int endMonth)
        {
            int monthCounter = 0;
            int? year = calcYear;//inSeviceDate?.Year;

            if (year == null)
                return monthCounter;

            while (year <= calcYear)
            {
                for (int i = startMonth; i <= endMonth; i++)
                {
                    if (inSeviceDate != null && inSeviceDate.Value.Date <= new DateTime((int)year, i, 15).Date &&
                        (leavingDate == null || (leavingDate.Value.Date > new DateTime((int)year, i, 15).Date)))
                        monthCounter++;
                }

                year++;
            }

            return monthCounter;
        }
    }

    /// <summary>
    /// Тексты ошибок.
    /// </summary>
    public static class AccountingCalculationErrors
    {
        public static string EmptyResidualCost = "Не указана остаточная стоимость";
        public static string EmptyCadastralValue = "Не указана кадастровая стоимость";
        public static string EmptyTaxRate = "Не указана налоговая ставка";
        public static string EmptyTaxExemption = "Не указан код налоговой льготы";
        public static string EmptyTaxBase = "Не указана база налогообложения";
        public static string EmptyCadastralValueForTaxBase = "Не указана кадастровая стоимость при указании соответствующей базы налогообложения";
        public static string EmptyVehicleRegDate = "Не указана дата регистрации ТС";
        public static string EmptyYearOfIssue = "Не указан год выпуска ТС";
        public static string YearOfIssueMoreTaxPeriod = "Год выпуска ТС больше налогового периода";
        public static string AoNotFound = "Не найдены ОС соответствующие критериям расчета (указанному БЕ и с данными за указанный год).";
        public static string AoNotFoundFromGroupPositionConsolidation = "Не найдены ОС соответствующие критериям расчета (указанная Позиция консолидации должна входить в одну из групп позиции консолидации с кодомами: \"0001111110\", \"0001111120\", \"0001111200\", \"0001111300\", \"0001120000\").";
        public static string EmptyPower = "Не указана мощность ТС";
        public static string ShareRightDenominator = "Доля в праве (знаменатель доли) указано как \"0\"";
        public static string TaxLowerLand = "Указан Код налоговой льготы в виде уменьшения суммы налога, но не указан процент уменьшающий сумму налога";
        public static string EmptyTaxLowerPercent = "Указан Код налоговой льготы в виде уменьшения суммы налога, но не указан процент уменьшающий сумму налога";
        public static string EmptyInServiceDate = "Не указана дата ввода в эксплуатацию";
        public static string EmptyLeavingDate = "Не указана дата списания";
        public static string EmptyTaxExemptionStartDateLand = "Не указана дата начала действия льготных условий налогообложения. Земельный налог";
    }
}

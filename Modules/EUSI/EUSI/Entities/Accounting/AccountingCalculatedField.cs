using System;
using System.ComponentModel;
using Base;
using Base.Attributes;
using CorpProp.Entities.NSI;
using EUSI.Entities.NU;

namespace EUSI.Entities.Accounting
{
    /// <summary>
    /// Представляет расчеты по налогам на ОИ, ТС, и ЗУ для ИР ЕУСИ и ИС НА.
    /// </summary>
    public class AccountingCalculatedField : BaseObject
    {
        public AccountingCalculatedField() : base()
        {
            IsCadastralCost = false;
            CalculateDate = DateTime.Now;
        }



        /// <summary>
        /// ОС.
        /// </summary>
        [DetailView(Name = "ОС")]
        [ListView(Name = "ОС")]
        public string AccountingObjectName { get; set; }

        /// <summary>
        /// ОКТМО.
        /// </summary>
        [DetailView(Name = "ОКТМО")]
        [ListView(Name = "ОКТМО")]
        public string OKTMO { get; set; }

        /// <summary>
        /// Кадастровый номер.
        /// </summary>
        [DetailView(Name = "Кадастровый номер")]
        [ListView(Name = "Кадастровый номер")]
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Кадастровая стоимость.
        /// </summary>
        [DetailView(Name = "Кадастровая стоимость")]
        [ListView(Name = "Кадастровая стоимость")]
        public decimal? CadastralValue { get; set; }


        /// <summary>
        /// Кадастровая стоимость.
        /// </summary>
        [DetailView(Name = "Кадастровая стоимость (1 квартал)")]
        [ListView(Name = "Кадастровая стоимость (1 квартал)")]
        public decimal? CadastralValueQuarter1 { get; set; }

        /// <summary>
        /// Кадастровая стоимость.
        /// </summary>
        [DetailView(Name = "Кадастровая стоимость (2 квартал)")]
        [ListView(Name = "Кадастровая стоимость (2 квартал)")]
        public decimal? CadastralValueQuarter2 { get; set; }


        /// <summary>
        /// Кадастровая стоимость.
        /// </summary>
        [DetailView(Name = "Кадастровая стоимость (3 квартал)")]
        [ListView(Name = "Кадастровая стоимость (3 квартал)")]
        public decimal? CadastralValueQuarter3 { get; set; }


        /// <summary>
        /// Получает или задает ИД выбора налоговой базы.
        /// </summary>
        [SystemProperty]
        public int? TaxBaseID { get; set; }

        /// <summary>
        /// Выбор налоговой базы.
        /// </summary>
        [DetailView(Name = "Выбор налоговой базы")]
        [ListView(Name = "Выбор налоговой базы")]
        public TaxBase TaxBase { get; set; }

        /// <summary>
        /// Налоговая база.
        /// </summary>
        [DetailView(Name = "Налоговая база")]
        [ListView(Name = "Налоговая база")]
        [DefaultValue(0)]
        public decimal? TaxBaseValue { get; set; }


        /// <summary>
        /// Налоговая база.
        /// </summary>
        [DetailView(Name = "Налоговая база (1 квартал)")]
        [ListView(Name = "Налоговая база (1 квартал)")]
        [DefaultValue(0)]
        public decimal? TaxBaseValueQuarter1 { get; set; }

        /// <summary>
        /// Налоговая база.
        /// </summary>
        [DetailView(Name = "Налоговая база (2 квартал)")]
        [ListView(Name = "Налоговая база (2 квартал)")]
        [DefaultValue(0)]
        public decimal? TaxBaseValueQuarter2 { get; set; }

        /// <summary>
        /// Налоговая база.
        /// </summary>
        [DetailView(Name = "Налоговая база (3 квартал)")]
        [ListView(Name = "Налоговая база (3 квартал)")]
        [DefaultValue(0)]
        public decimal? TaxBaseValueQuarter3 { get; set; }


        /// <summary>
        /// Налоговая ставка.
        /// </summary>
        [DetailView(Name = "Налоговая ставка")]
        [ListView(Name = "Налоговая ставка")]
        public decimal? TaxRate { get; set; }


        /// <summary>
        /// Налоговая ставка.
        /// </summary>
        [DetailView(Name = "Налоговая ставка (1 квартал)")]
        [ListView(Name = "Налоговая ставка (1 квартал)")]
        public decimal? TaxRateQuarter1 { get; set; }

        /// <summary>
        /// Налоговая ставка.
        /// </summary>
        [DetailView(Name = "Налоговая ставка (2 квартал)")]
        [ListView(Name = "Налоговая ставка (2 квартал)")]
        public decimal? TaxRateQuarter2 { get; set; }

        /// <summary>
        /// Налоговая ставка.
        /// </summary>
        [DetailView(Name = "Налоговая ставка (3 квартал)")]
        [ListView(Name = "Налоговая ставка (3 квартал)")]
        public decimal? TaxRateQuarter3 { get; set; }


        #region Остаточная стоимость
        /// <summary>
        /// Остаточная стоимость на 01.01;
        /// </summary>
        [DetailView(Name = "Остаточная стоимость на 01.01")]
        [ListView(Name = "Остаточная стоимость на 01.01")]
        public decimal? ResidualCost_01 { get; set; }

        /// <summary>
        /// Остаточная стоимость на 01.02;
        /// </summary>
        [DetailView(Name = "Остаточная стоимость на 01.02")]
        [ListView(Name = "Остаточная стоимость на 01.02")]
        public decimal? ResidualCost_02 { get; set; }

        /// <summary>
        /// Остаточная стоимость на 01.03;
        /// </summary>
        [DetailView(Name = "Остаточная стоимость на 01.03")]
        [ListView(Name = "Остаточная стоимость на 01.03")]
        public decimal? ResidualCost_03 { get; set; }

        /// <summary>
        /// Остаточная стоимость на 01.04;
        /// </summary>
        [DetailView(Name = "Остаточная стоимость на 01.04")]
        [ListView(Name = "Остаточная стоимость на 01.04")]
        public decimal? ResidualCost_04 { get; set; }

        /// <summary>
        /// Остаточная стоимость на 01.05;
        /// </summary>
        [DetailView(Name = "Остаточная стоимость на 01.05")]
        [ListView(Name = "Остаточная стоимость на 01.05")]
        public decimal? ResidualCost_05 { get; set; }

        /// <summary>
        /// Остаточная стоимость на 01.06;
        /// </summary>
        [DetailView(Name = "Остаточная стоимость на 01.06")]
        [ListView(Name = "Остаточная стоимость на 01.06")]
        public decimal? ResidualCost_06 { get; set; }

        /// <summary>
        /// Остаточная стоимость на 01.07;
        /// </summary>
        [DetailView(Name = "Остаточная стоимость на 01.07")]
        [ListView(Name = "Остаточная стоимость на 01.07")]
        public decimal? ResidualCost_07 { get; set; }

        /// <summary>
        /// Остаточная стоимость на 01.08;
        /// </summary>
        [DetailView(Name = "Остаточная стоимость на 01.08")]
        [ListView(Name = "Остаточная стоимость на 01.08")]
        public decimal? ResidualCost_08 { get; set; }

        /// <summary>
        /// Остаточная стоимость на 01.09;
        /// </summary>
        [DetailView(Name = "Остаточная стоимость на 01.09")]
        [ListView(Name = "Остаточная стоимость на 01.09")]
        public decimal? ResidualCost_09 { get; set; }

        /// <summary>
        /// Остаточная стоимость на 01.10;
        /// </summary>
        [DetailView(Name = "Остаточная стоимость на 01.10")]
        [ListView(Name = "Остаточная стоимость на 01.10")]
        public decimal? ResidualCost_10 { get; set; }

        /// <summary>
        /// Остаточная стоимость на 01.11;
        /// </summary>
        [DetailView(Name = "Остаточная стоимость на 01.11")]
        [ListView(Name = "Остаточная стоимость на 01.11")]
        public decimal? ResidualCost_11 { get; set; }

        /// <summary>
        /// Остаточная стоимость на 01.12;
        /// </summary>
        [DetailView(Name = "Остаточная стоимость на 01.12")]
        [ListView(Name = "Остаточная стоимость на 01.12")]
        public decimal? ResidualCost_12 { get; set; }

        /// <summary>
        /// Остаточная стоимость на 31.12;
        /// </summary>
        [DetailView(Name = "Остаточная стоимость на 31.12")]
        [ListView(Name = "Остаточная стоимость на 31.12")]
        public decimal? ResidualCost_13 { get; set; }

        /// <summary>
        /// Остаточная стоимость на 31.12;
        /// </summary>
        [DetailView(Name = "Остаточная стоимость НИ на 31.12",
            Description = "Остаточная стоимость недвижимого имущества на конец налогового периода (31 декабря)")]
        [ListView(Name = "Остаточная стоимость НИ на 31.12")]
        public decimal? ResidualCost_14 { get; set; }

        #endregion

        /// <summary>
        /// Год.
        /// </summary>
        [DetailView(Name = "Год")]
        [ListView(Name = "Год")]
        public int? Year { get; set; }

        /// <summary>
        /// Дата вычисления.
        /// </summary>
        [DetailView(Name = "Дата вычисления")]
        [ListView(Name = "Дата вычисления")]
        public DateTime CalculateDate { get; set; }

        /// <summary>
        /// Сумма начисленного налога за налоговый период.
        /// </summary>
        [DetailView(Name = "Сумма начисленного налога за налоговый период.")]
        [ListView(Name = "Сумма начисленного налога за налоговый период")]
        [PropertyDataType("Sib_DecimalN0")]
        public decimal TaxSumYear { get; set; }

        /// <summary>
        /// Сумма начисленного налога за налоговый период.
        /// </summary>
        [DetailView(Name = "Сумма начисленного налога за налоговый период. (1 квартал)")]
        [ListView(Name = "Сумма начисленного налога за налоговый период (1 квартал)")]
        [PropertyDataType("Sib_DecimalN0")]
        public decimal TaxSumYearQuarter1 { get; set; }

        /// <summary>
        /// Сумма начисленного налога за налоговый период.
        /// </summary>
        [DetailView(Name = "Сумма начисленного налога за налоговый период. (2 квартал)")]
        [ListView(Name = "Сумма начисленного налога за налоговый период (2 квартал)")]
        [PropertyDataType("Sib_DecimalN0")]
        public decimal TaxSumYearQuarter2 { get; set; }

        /// <summary>
        /// Сумма начисленного налога за налоговый период.
        /// </summary>
        [DetailView(Name = "Сумма начисленного налога за налоговый период. (3 квартал)")]
        [ListView(Name = "Сумма начисленного налога за налоговый период (3 квартал)")]
        [PropertyDataType("Sib_DecimalN0")]
        public decimal TaxSumYearQuarter3 { get; set; }


        /// <summary>
        /// Сумма налога с учетом льгот.
        /// </summary>
        [DetailView(Name = "Сумма налога с учетом льгот.")]
        [ListView(Name = "Сумма налога с учетом льгот")]
        [PropertyDataType("Sib_DecimalN0")]
        public decimal TaxSumWithPrivilege { get; set; }

        /// <summary>
        /// Средняя стоимость ОС за I кв.
        /// </summary>
        [DetailView(Name = "Средняя стоимость ОС за I кв.")]
        [ListView(Name = "Средняя стоимость ОС за I кв")]
        public decimal AvgPriceFirstQuarter { get; set; }

        /// <summary>
        /// Средняя стоимость ОС за 6 мес.
        /// </summary>
        [DetailView(Name = "Средняя стоимость ОС за 6 мес.")]
        [ListView(Name = "Средняя стоимость ОС за 6 мес")]
        public decimal AvgPriceSecondQuarter { get; set; }

        /// <summary>
        /// Средняя стоимость ОС за 9 мес.
        /// </summary>
        [DetailView(Name = "Средняя стоимость ОС за 9 мес")]
        [ListView(Name = "Средняя стоимость ОС за 9 мес")]
        public decimal AvgPriceThirdQuarter { get; set; }

        /// <summary>
        /// Средняя стоимость ОС за год.
        /// </summary>
        [DetailView(Name = "Средняя стоимость ОС за год")]
        [ListView(Name = "Средняя стоимость ОС за год")]
        public decimal AvgPriceYear { get; set; }

        /// <summary>
        /// Сумма авансового платежа за I кв.
        /// </summary>
        [DetailView(Name = "Сумма авансового платежа за I кв.")]
        [ListView(Name = "Сумма авансового платежа за I кв")]
        [PropertyDataType("Sib_DecimalN0")]
        public decimal PrepaymentSumFirstQuarter { get; set; }

        /// <summary>
        /// Сумма авансового платежа за II кв.
        /// </summary>
        [DetailView(Name = "Сумма авансового платежа за II кв.")]
        [ListView(Name = "Сумма авансового платежа за II кв")]
        [PropertyDataType("Sib_DecimalN0")]
        public decimal PrepaymentSumSecondQuarter { get; set; }

        /// <summary>
        /// Сумма авансового платежа за III кв.
        /// </summary>
        [DetailView(Name = "Сумма авансового платежа за III кв.")]
        [ListView(Name = "Сумма авансового платежа за III кв")]
        [PropertyDataType("Sib_DecimalN0")]
        public decimal PrepaymentSumThirdQuarter { get; set; }

        /// <summary>
        /// Сумма налога подлежащая уплате в бюджет.
        /// </summary>
        [DetailView(Name = "Сумма налога подлежащая уплате в бюджет")]
        [ListView(Name = "Сумма налога подлежащая уплате в бюджет")]
        [PropertyDataType("Sib_DecimalN0")]
        public decimal PaymentTaxSum { get; set; }

        /// <summary>
        /// Источник данных расчета.
        /// </summary>
        [DetailView(Name = "Источник данных расчета", Visible = false)]
        [ListView(Name = "Источник данных расчета", Visible = false)]
        public string CalculationDatasource { get; set; }


        /// <summary>
        /// Налог с кадастровой стоимости.
        /// </summary>
        [DetailView(Name = "Налог с кадастровой стоимости", Visible = false)]
        [ListView(Name = "Налог с кадастровой стоимости", Visible = false)]
        [DefaultValue(false)]
        public bool IsCadastralCost { get; set; }



        /// <summary>
        /// ОС.
        /// </summary>
        //public AccountingObject AccountingObject { get; set; }
        public int? AccountingObjectID { get; set; }

        public int? CalculatingRecordID { get; set; }

        /// <summary>
        /// ИД декларации
        /// </summary>
        [ListView("ИД Декларации", Visible = false)]
        [DetailView("ИД Декларации", Visible = false)]
        public int? DeclarationID { get; set; }

        /// <summary>
        /// Декларация
        /// </summary>
        [ListView("Декларация", Visible = false)]
        [DetailView("Декларация", Visible = false)]
        public Declaration Declaration { get; set; }

        /// <summary>
        /// Код вида имущества (НА).
        /// </summary>
        [DetailView(Name = "Код вида имущества (НА)", Visible = false)]
        [ListView(Name = "Код вида имущества (НА)", Visible = false)]
        public string EstateKindCode { get; set; }


        /// <summary>
        /// Код налоговой льготы.
        /// </summary>
        [DetailView(Name = "Код налоговой льготы", Visible = false)]
        [ListView(Name = "Код налоговой льготы", Visible = false)]
        public string TaxExemption { get; set; }

        /// <summary>
        /// Код налоговой льготы (понижение ставки).
        /// </summary>
        [DetailView("Код налоговой льготы (понижение ставки)", Description = "Код налоговой льготы (установленной в виде понижения налоговой ставки)", Visible = false)]
        [ListView("Код налоговой льготы (понижение ставки)", Visible = false)]
        public string TaxExemptionLow { get; set; }

        [DetailView("Сумма налоговой льготы (понижение ставки)", Description = "Сумма налоговой льготы (установленной в виде понижения налоговой ставки)", Visible = false)]
        [ListView("Сумма налоговой льготы (понижение ставки)", Visible = false)]
        public decimal? TaxExemptionLowSum { get; set; }

        [DetailView("Код налоговой льготы (уменьшение суммы)", Description = "Код налоговой льготы в виде уменьшения суммы налога", Visible = false)]
        [ListView("Код налоговой льготы (уменьшение суммы)", Visible = false)]
        public string TaxLow { get; set; }

        [DetailView("Сумма налоговой льготы (уменьшение суммы)", Description = "Сумма налоговой льготы в виде уменьшения суммы налога", Visible = false)]
        [ListView("Сумма налоговой льготы (уменьшение суммы)", Visible = false)]
        public decimal? TaxLowSum { get; set; }

        [DetailView("Сумма налоговой льготы (уменьшение суммы) (1 квартал)", Description = "Сумма налоговой льготы в виде уменьшения суммы налога", Visible = false)]
        [ListView("Сумма налоговой льготы (уменьшение суммы) (1 квартал)", Visible = false)]
        public decimal? TaxLowSumQuarter1 { get; set; }

        [DetailView("Сумма налоговой льготы (уменьшение суммы) (2 квартал)", Description = "Сумма налоговой льготы в виде уменьшения суммы налога", Visible = false)]
        [ListView("Сумма налоговой льготы (уменьшение суммы) (2 квартал)", Visible = false)]
        public decimal? TaxLowSumQuarter2 { get; set; }

        [DetailView("Сумма налоговой льготы (уменьшение суммы) (3 квартал)", Description = "Сумма налоговой льготы в виде уменьшения суммы налога", Visible = false)]
        [ListView("Сумма налоговой льготы (уменьшение суммы) (3 квартал)", Visible = false)]
        public decimal? TaxLowSumQuarter3 { get; set; }


        /// <summary>
        /// Код налоговой льготы (освобождение).
        /// </summary>
        [DetailView("Код налоговой льготы (освобождение)", Description = "Код налоговой льготы (установленной в виде освобождения от налогооблажения)", Visible = false)]
        [ListView("Код налоговой льготы (освобождение)", Visible = false)]
        public string TaxExemptionFree { get; set; }

        [DetailView("Сумма налоговой льготы (освобождение)", Description = "Сумма налоговой льготы (установленной в виде освобождения от налогооблажения)", Visible = false)]
        [ListView("Сумма налоговой льготы (освобождение)", Visible = false)]
        public decimal? TaxExemptionFreeSum { get; set; }


        [DetailView("Сумма налоговой льготы (освобождение) (1 квартал)", Description = "Сумма налоговой льготы (установленной в виде освобождения от налогооблажения)", Visible = false)]
        [ListView("Сумма налоговой льготы (освобождение) (1 квартал)", Visible = false)]
        public decimal? TaxExemptionFreeSumQuarter1 { get; set; }


        [DetailView("Сумма налоговой льготы (освобождение) (2 квартал)", Description = "Сумма налоговой льготы (установленной в виде освобождения от налогооблажения)", Visible = false)]
        [ListView("Сумма налоговой льготы (освобождение) (2 квартал)", Visible = false)]
        public decimal? TaxExemptionFreeSumQuarter2 { get; set; }


        [DetailView("Сумма налоговой льготы (освобождение) (3 квартал)", Description = "Сумма налоговой льготы (установленной в виде освобождения от налогооблажения)", Visible = false)]
        [ListView("Сумма налоговой льготы (освобождение) (3 квартал)", Visible = false)]
        public decimal? TaxExemptionFreeSumQuarter3 { get; set; }


        /// <summary>
        /// Код Налоговой льготы в виде освобождения от налогообложения (пункт 2 статьи 387 Налогового кодекса Российской Федерации).
        /// </summary>
        [DetailView("Код налоговой льготы (освобождение) ЗУ", Description = "Налоговая льгота в виде освобождения от налогообложения (пункт 2 статьи 387 Налогового кодекса Российской Федерации)", Visible = false)]
        [ListView("Код налоговой льготы (освобождение) ЗУ", Visible = false)]
        public string TaxExemptionFreeLand { get; set; }

        /// <summary>
        /// Сумма Налоговой льготы в виде освобождения от налогообложения (пункт 2 статьи 387 Налогового кодекса Российской Федерации).
        /// </summary>
        [DetailView("Сумма налоговой льготы (освобождение) ЗУ", Description = "Налоговая льгота в виде освобождения от налогообложения (пункт 2 статьи 387 Налогового кодекса Российской Федерации)", Visible = false)]
        [ListView("Сумма налоговой льготы (освобождение) ЗУ", Visible = false)]
        public decimal? TaxExemptionFreeSumLand { get; set; }


        /// <summary>
        /// Сумма авансовых платежей, исчисленная за отчетные периоды.
        /// </summary>
        [DetailView(Name = "Сумма авансовых платежей", Description = "Сумма авансовых платежей, исчисленная за отчетные периоды", Visible = false)]
        [ListView(Name = "Сумма авансовых платежей", Visible = false)]
        [PropertyDataType("Sib_DecimalN0")]
        public decimal? PrepaymentSumYear { get; set; }



        [SystemProperty]
        public int? TaxReportPeriodID { get; set; }

        /// <summary>
        /// Отчетный период
        /// </summary>
        [ListView("Отчетный период", Visible = false)]
        [DetailView("Отчетный период", Visible = false)]
        public TaxReportPeriod TaxReportPeriod { get; set; }

        [SystemProperty]
        public int? ConsolidationID { get; set; }

        /// <summary>
        /// Балансовая единица.
        /// </summary>
        [ListView("Балансовая единица", Visible = false)]
        [DetailView("Балансовая единица", Visible = false)]
        public Consolidation Consolidation { get; set; }


        /// <summary>
        /// Получает или задает идентификационный № ТС.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [ListView("Идентификационный номер ТС", Visible = false)]
        [DetailView("", Visible = false)]
        public String VehicleSerialNumber { get; set; }

        /// <summary>
        /// Получает или задает Номерной знак транспортного средства
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView("Номерной знак транспортного средства", Visible = false), ListView(Visible = false)]
        public String VehicleSignNumber { get; set; }

        /// <summary>
        /// Экологический класс.
        /// </summary>
        [DetailView(Name = "Экологический класс", Visible = false)]
        [ListView(Name = "Экологический класс", Visible = false)]
        public string EcoKlass { get; set; }

        /// <summary>
        /// Получает или задает год выпуска ТС. 
        /// </summary>
        [DetailView("Год выпуска ТС", Visible = false)]
        public int? VehicleYearOfIssue { get; set; }


        /// <summary>
        /// Получает или задает Кол-во мес владения ТС. 
        /// </summary>
        [ListView("Кол-во мес владения ТС", Visible = false)]
        [DetailView("Кол-во мес владения ТС", Visible = false)]
        public int? VehicleMonthOwn { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [ListView("Доля владения в праве", Visible = false)]
        [DetailView("Доля владения в праве", Visible = false)]
        public String Share { get; set; }

        [DetailView("Повышающий коэффициент расчета ТС", Visible = false)]
        [ListView("Повышающий коэффициент расчета ТС", Visible = false)]
        public decimal? VehicleTaxFactor { get; set; }

        [DetailView("Повышающий коэффициент расчета ТС (1 квартал)", Visible = false)]
        [ListView("Повышающий коэффициент расчета ТС (1 квартал)", Visible = false)]
        public decimal? VehicleTaxFactorQuarter1 { get; set; }

        [DetailView("Повышающий коэффициент расчета ТС (2 квартал)", Visible = false)]
        [ListView("Повышающий коэффициент расчета ТС (2 квартал)", Visible = false)]
        public decimal? VehicleTaxFactorQuarter2 { get; set; }

        [DetailView("Повышающий коэффициент расчета ТС (3 квартал)", Visible = false)]
        [ListView("Повышающий коэффициент расчета ТС (3 квартал)", Visible = false)]
        public decimal? VehicleTaxFactorQuarter3 { get; set; }

        /// <summary>Получает или задает Бизнес-сфера.</summary>
        [DetailView("Бизнес-сфера", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string BusinessArea { get; set; }

        /// <summary>Получает или задает Номер карточки ОС (Системный номер).</summary>
        [DetailView("Номер карточки ОС (Системный номер)", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExternalID { get; set; }

        /// <summary>Получает или задает Субномер.</summary>
        [DetailView("Субномер", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SubNumber { get; set; }

        /// <summary>Получает или задает Признак недвижимого имущества.</summary>
        [DetailView("Признак недвижимого имущества", Visible = false), ListView(Visible = false)]
        [DefaultValue(false)]
        public bool IsEstateMovable { get; set; }

        /// <summary>Получает или задает Инвентарный номер.</summary>
        [DetailView("Инвентарный номер", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string InventoryNumber { get; set; }

        /// <summary>Получает или задает Амортизационная группа.</summary>
        [DetailView("Амортизационная группа", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string DepreciationGroup { get; set; }

        /// <summary>Получает или задает Счет Главной книги в ЛУС.</summary>
        [DetailView("Счет Главной книги в ЛУС", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AccountLedgerLUS { get; set; }

        /// <summary>Получает или задает Синтетический счет учета РСБУ (01) или (08 ).</summary>
        [DetailView("Синтетический счет учета РСБУ (01) или (08 )", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SyntheticAccount { get; set; }

        /// <summary>Получает или задает ОКОФ.</summary>
        [DetailView("ОКОФ", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string OKOF { get; set; }

        /// <summary>Получает или задает Получено в результате реорганизации/ликвидации.</summary>
        [DetailView("Получено в результате реорганизации/ликвидации", Visible = false), ListView(Visible = false)]
        [DefaultValue(false)]
        public bool GetByRestruct { get; set; }

        /// <summary>Получает или задает Приобретено у взаимозависимых лиц.</summary>
        [DetailView("Приобретено у взаимозависимых лиц", Visible = false), ListView(Visible = false)]
        [DefaultValue(false)]
        public bool GetFromInterdependent { get; set; }

        /// <summary>Получает или задает Дата постановки на учет.</summary>
        [DetailView("Дата постановки на учет", Visible = false), ListView(Visible = false)]
        public DateTime? RegDate { get; set; }

        /// <summary>Получает или задает Доля в праве общей собственности, числитель.</summary>
        [DetailView("Доля в праве общей собственности, числитель", Visible = false), ListView(Visible = false)]
        public int? ShareRightNumerator { get; set; }

        /// <summary>Получает или задает Доля в праве общей собственности, знаменатель.</summary>
        [DetailView("Доля в праве общей собственности, знаменатель", Visible = false), ListView(Visible = false)]
        public int? ShareRightDenominator { get; set; }

        /// <summary>Получает или задает Коэффициент К.</summary>
        [DetailView("Коэффициент К", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        [DecimalPrecision(18, 4)]
        public decimal? FactorK { get; set; }

        /// <summary>Получает или задает Коэффициент К  (1 квартал).</summary>
        [DetailView("Коэффициент К (1 квартал)", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        [DecimalPrecision(18, 4)]
        public decimal? FactorK1 { get; set; }

        /// <summary>Получает или задает Коэффициент К  (2 квартал).</summary>
        [DetailView("Коэффициент К (2 квартал)", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        [DecimalPrecision(18, 4)]
        public decimal? FactorK2 { get; set; }

        /// <summary>Получает или задает Коэффициент К  (3 квартал).</summary>
        [DetailView("Коэффициент К (3 квартал)", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        [DecimalPrecision(18, 4)]
        public decimal? FactorK3 { get; set; }


        /// <summary>Получает или задает Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот).</summary>
        [DetailView("Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот)", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxLowerPercent { get; set; }

        /// <summary>Получает или задает Дата включения в кадастровый реестр.</summary>
        [DetailView("Дата включения в кадастровый реестр", Visible = false), ListView(Visible = false)]
        public DateTime? IncludeCadRegDate { get; set; }

        /// <summary>Получает или задает Номер документа включения в кадастровый реестр.</summary>
        [DetailView("Номер документа включения в кадастровый реестр", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string IncludeCadRegDoc { get; set; }

        /// <summary>Получает или задает Код ИФНС.</summary>
        [DetailView("Код ИФНС", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string IFNS { get; set; }

        /// <summary>Получает или задает Код ЕУСИ.</summary>
        [DetailView("Код ЕУСИ", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string EUSINumber { get; set; }

        /// <summary>Получает или задает Среднегодовая стоимость необлагаемого налогом имущества за налоговый период.</summary>
        [DetailView("Среднегодовая стоимость необлагаемого налогом имущества за налоговый период", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        public decimal? UntaxedAnnualCostAvg { get; set; }

        /// <summary>Получает или задает Среднегодовая стоимость необлагаемого налогом имущества за 1 квартал.</summary>
        [DetailView("Среднегодовая стоимость необлагаемого налогом имущества за 1 квартал", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        public decimal? UntaxedAnnualCostAvgQuarter1 { get; set; }

        /// <summary>Получает или задает Среднегодовая стоимость необлагаемого налогом имущества за 2 квартал.</summary>
        [DetailView("Среднегодовая стоимость необлагаемого налогом имущества за 2 квартал", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        public decimal? UntaxedAnnualCostAvgQuarter2 { get; set; }

        /// <summary>Получает или задает Среднегодовая стоимость необлагаемого налогом имущества за 3 квартал.</summary>
        [DetailView("Среднегодовая стоимость необлагаемого налогом имущества за 3 квартал", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        public decimal? UntaxedAnnualCostAvgQuarter3 { get; set; }

        /// <summary>Получает или задает Код категории ЗУ.</summary>
        [DetailView("Код категории ЗУ", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string LandCategory { get; set; }

        /// <summary>Получает или задает Дата постановки на государственный кадастровый учет.</summary>
        [DetailView("Дата постановки на государственный кадастровый учет", Visible = false), ListView(Visible = false)]
        public DateTime? CadRegDate { get; set; }

        /// <summary>Получает или задает Доля налогоплательщика в праве на ЗУ (числитель доли)  .</summary>
        [DetailView("Доля налогоплательщика в праве на ЗУ (числитель доли)  ", Visible = false), ListView(Visible = false)]
        public int? ShareTaxPayerNumerator { get; set; }

        /// <summary>Получает или задает Доля налогоплательщика в праве на ЗУ (знаменатель доли).</summary>
        [DetailView("Доля налогоплательщика в праве на ЗУ (знаменатель доли)", Visible = false), ListView(Visible = false)]
        public int? ShareTaxPayerDenominator { get; set; }

        /// <summary>Получает или задает Количество полных месяцев владения ЗУ  в течение налогового периода.</summary>
        [DetailView("Количество полных месяцев владения ЗУ  в течение налогового периода", Visible = false), ListView(Visible = false)]
        public int? CountFullMonthsLand { get; set; }

        /// <summary>Получает или задает Коэффициент Кв.</summary>
        [DetailView("Коэффициент Кв", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        [DecimalPrecision(18, 4)]
        public decimal? FactorKv { get; set; }

        /// <summary>Получает или задает Коэффициент Кв.  (1 квартал)</summary>
        [DetailView("Коэффициент Кв (1 квартал)", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        [DecimalPrecision(18, 4)]
        public decimal? FactorKv1 { get; set; }

        /// <summary>Получает или задает Коэффициент Кв.  (2 квартал)</summary>
        [DetailView("Коэффициент Кв (2 квартал)", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        [DecimalPrecision(18, 4)]
        public decimal? FactorKv2 { get; set; }

        /// <summary>Получает или задает Коэффициент Кв.  (3 квартал)</summary>
        [DetailView("Коэффициент Кв (3 квартал)", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        [DecimalPrecision(18, 4)]
        public decimal? FactorKv3 { get; set; }

        /// <summary>Получает или задает Дата начала действия льготных условий налогообложения. Земельный налог.</summary>
        [DetailView("Дата начала действия льготных условий налогообложения. Земельный налог", Visible = false), ListView(Visible = false)]
        public DateTime? TaxExemptionStartDateLand { get; set; }

        /// <summary>Получает или задает Дата окончания действия льготных условий налогообложения. Земельный налог.</summary>
        [DetailView("Дата окончания действия льготных условий налогообложения. Земельный налог", Visible = false), ListView(Visible = false)]
        public DateTime? TaxExemptionEndDateLand { get; set; }

        /// <summary>Получает или задает Количество полных месяцев использования льготы.</summary>
        [DetailView("Количество полных месяцев использования льготы", Visible = false), ListView(Visible = false)]
        public int? CountFullMonthsBenefit { get; set; }

        /// <summary>Получает или задает Коэффициент Кл.</summary>
        [DetailView("Коэффициент Кл", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        [DecimalPrecision(18, 4)]
        public decimal? FactorKl { get; set; }

        /// <summary>Получает или задает Коэффициент Кл. 1 квартал</summary>
        [DetailView("Коэффициент Кл (1 квартал)", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        [DecimalPrecision(18, 4)]
        public decimal? FactorKl1 { get; set; }

        /// <summary>Получает или задает Коэффициент Кл. 2 квартал</summary>
        [DetailView("Коэффициент Кл (2 квартал)", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        [DecimalPrecision(18, 4)]
        public decimal? FactorKl2 { get; set; }

        /// <summary>Получает или задает Коэффициент Кл.  3 квартал</summary>
        [DetailView("Коэффициент Кл (3 квартал)", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        [DecimalPrecision(18, 4)]
        public decimal? FactorKl3 { get; set; }
        /// <summary>Получает или задает Исчисленная сумма налога, подлежащая уплате в бюджет  за налоговый период (за минусом суммы льготы).</summary>
        [DetailView("Исчисленная сумма налога, подлежащая уплате в бюджет  за налоговый период (за минусом суммы льготы)", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        public decimal? CalcSum { get; set; }

        /// <summary>Получает или задает Код вида ТС.</summary>
        [DetailView("Код вида ТС", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string VehicleKindCode { get; set; }

        /// <summary>Получает или задает Дата регистрации ТС.</summary>
        [DetailView("Дата регистрации ТС", Visible = false), ListView(Visible = false)]
        public DateTime? VehicleRegDate { get; set; }

        /// <summary>Получает или задает Дата прекращения регистрации ТС (снятие с учета).</summary>
        [DetailView("Дата прекращения регистрации ТС (снятие с учета)", Visible = false), ListView(Visible = false)]
        public DateTime? VehicleDeRegDate { get; set; }

        /// <summary>Получает или задает Код налогового вычета.</summary>
        [DetailView("Код налогового вычета", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaxDeduction { get; set; }

        /// <summary>Получает или задает Сумма налогового вычета.</summary>
        [DetailView("Сумма налогового вычета", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxDeductionSum { get; set; }

        /// <summary>Получает или задает Учет в системе взимания платы «ПЛАТОН».</summary>
        [DetailView("Учет в системе взимания платы «ПЛАТОН»", Visible = false), ListView(Visible = false)]
        [DefaultValue(false)]
        public bool InOtherSystem { get; set; }

        /// <summary>Получает или задает Налоговая ставка с учетом применяемых льгот.</summary>
        [DetailView("Налоговая ставка с учетом применяемых льгот", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxRateWithExemption { get; set; }

        /// <summary>Получает или задает Дата начала действия льготных условий налогообложения.</summary>
        [DetailView("Дата начала действия льготных условий налогообложения", Visible = false), ListView(Visible = false)]
        public DateTime? TaxExemptionStartDate { get; set; }

        /// <summary>Получает или задает Дата окончания действия льготных условий налогообложения.</summary>
        [DetailView("Дата окончания действия льготных условий налогообложения", Visible = false), ListView(Visible = false)]
        public DateTime? TaxExemptionEndDate { get; set; }

        /// <summary>Получает или задает Первоначальная стоимость объекта по БУ, руб..</summary>
        [DetailView("Первоначальная стоимость объекта по БУ, руб.", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        public decimal? InitialCost { get; set; }

        /// <summary>Получает или задает Количество лет, прошедших с  года выпуска ТС.</summary>
        [DetailView("Количество лет, прошедших с  года выпуска ТС", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public int? CountOfYearsIssue { get; set; }

        /// <summary>Получает или задает Дата прихода.</summary>
        [DetailView("Дата прихода", Visible = false), ListView(Visible = false)]
        public DateTime? DateOfReceipt { get; set; }

        /// <summary>Получает или задает Дата выбытия.</summary>
        [DetailView("Дата выбытия", Visible = false), ListView(Visible = false)]
        public DateTime? LeavingDate { get; set; }

        /// <summary>Получает или задает Налоговая база (ТС).</summary>
        [DetailView("Налоговая база (ТС)", Visible = false), ListView(Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxBaseValueTS { get; set; }

        /// <summary>Получает или задает Единица измерения налоговой  базы (ТС).</summary>
        [DetailView("Единица измерения налоговой  базы (ТС)", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaxBaseMeasureTS { get; set; }

        /// <summary>Получает или задает Марка ТС.</summary>
        [DetailView("Марка ТС", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string VehicleModel { get; set; }

        /// <summary>Получает или задает Номер основного средства.</summary>
        [DetailView("Номер основного средства", Visible = false), ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string OSNumber { get; set; }

    }
}

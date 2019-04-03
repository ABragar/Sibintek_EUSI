using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.NSI;

namespace CorpProp.Entities.Estate
{
    public class EstateTaxes : BaseObject
    {
        public EstateTaxes()
        {
        }

        public int TaxesOfID { get; set; }
        public virtual InventoryObject TaxesOf { get; set; }

        [DetailView("Льготируемый объект", Visible = false)]
        [ListView(Visible = false)]
        [DefaultValue(false)]
        public bool? Benefit { get; set; }

        [DetailView("Применение льготы для энергоэффективного оборудования", Visible = false)]
        [ListView(Visible = false)]
        [DefaultValue(false)]
        public bool? BenefitApplyForEnergy { get; set; }

        [DetailView("Наличие документов, подтверждающих применение льготы", Visible = false)]
        [ListView(Visible = false)]
        [DefaultValue(false)]
        public bool? BenefitDocsExist { get; set; }

        [ListView("Реквизиты решения органа субъектов/муниципальных образований", Visible = false)]
        [DetailView("Реквизиты решения органа субъектов/муниципальных образований", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [ForeignKey("DecisionsDetailsID")]
        public DecisionsDetails DecisionsDetails { get; set; }

        [SystemProperty]
        public int? DecisionsDetailsID { get; set; }

        [ListView("Реквизиты решения органа субъектов/муниципальных образований  по земельному налогу", Visible = false)]
        [DetailView("Реквизиты решения органа субъектов/муниципальных образований  по земельному налогу", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [ForeignKey("DecisionsDetailsLandID")]
        public DecisionsDetailsLand DecisionsDetailsLand { get; set; }

        [SystemProperty]
        public int? DecisionsDetailsLandID { get; set; }

        [ListView("Реквизиты решения органа субъектов/муниципальных образований  по транспортному налогу", Visible = false)]
        [DetailView("Реквизиты решения органа субъектов/муниципальных образований  по транспортному налогу", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [ForeignKey("DecisionsDetailsTSID")]
        public DecisionsDetailsTS DecisionsDetailsTS { get; set; }

        [SystemProperty]
        public int? DecisionsDetailsTSID { get; set; }

        [DetailView("Наличие документов, подтверждающих энергоэффективность оборудования", Visible = false)]
        [ListView(Visible = false)]
        public bool? EnergyDocsExist { get; set; }

        [DetailView("Класс энергетической эффективности", Visible = false)]
        [ListView(Visible = false)]
        [DefaultValue(false)]
        public EnergyLabel EnergyLabel { get; set; }

        [SystemProperty]
        public int? EnergyLabelID { get; set; }

        [DetailView("Энергоэффективное оборудование", Visible = false)]
        [ListView(Visible = false)]
        [DefaultValue(false)]
        public bool? IsEnergy { get; set; }

        [DetailView("Имущество, созданное по инвестиционной программе", Visible = false,
            Description = "Имущество, созданное по инвестиционной программе (в соответствии с программой развития регионов)")]
        [ListView("Имущество, созданное по инвестиционной программе", Visible = false)]
        [DefaultValue(false)]
        public bool? IsInvestmentProgramm { get; set; }

        [ListView("Выбор базы налогообложения", Visible = false)]
        [DetailView("Выбор базы налогообложения", Visible = false)]
        public TaxBase TaxBase { get; set; }

        [SystemProperty]
        public int? TaxBaseID { get; set; }

        [ListView("Дата включения в перечень объектов, учитываемых по кадастровой стоимости", Visible = false)]
        [DetailView(Name = "Дата включения в перечень объектов, учитываемых по кадастровой стоимости", Visible = false, Description = "Дата включения в перечень объектов, учитываемых по кадастровой стоимости для целей налогообложения")]
        public DateTime? TaxCadastralIncludeDate { get; set; }

        [ListView("Номер документа включения в перечень объектов, учитываемых по кадастровой стоимости", Visible = false)]
        [DetailView(Name = "Номер документа включения в перечень объектов, учитываемых по кадастровой стоимости", Visible = false, Description = "Номер документа включения в перечень объектов, учитываемых по кадастровой стоимости для целей налогообложения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaxCadastralIncludeDoc { get; set; }

        [ListView(Visible = false)]
        [DetailView("Код налоговой льготы", Visible = false)]
        public TaxExemption TaxExemption { get; set; }

        [ListView("Дата окончания действия льготных условий налогообложения", Visible = false)]
        [DetailView("Дата окончания действия льготных условий налогообложения", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionEndDate { get; set; }

        [ListView("Дата окончания действия льготных условий налогообложения. Земельный налог", Visible = false)]
        [DetailView("Дата окончания действия льготных условий налогообложения. Земельный налог", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionEndDateLand { get; set; }

        [ListView("Дата окончания действия льготных условий налогообложения. Транспортный налог", Visible = false)]
        [DetailView("Дата окончания действия льготных условий налогообложения. Транспортный налог", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionEndDateTS { get; set; }

        [SystemProperty]
        public int? TaxExemptionID { get; set; }

        [DetailView("Причина налоговой льготы", Visible = false)]
        [ListView("Причина налоговой льготы", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaxExemptionReason { get; set; }

        [DetailView("Причина налоговой льготы. Земельный налог", Visible = false)]
        [ListView("Причина налоговой льготы. Земельный налог", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaxExemptionReasonLand { get; set; }

        [DetailView("Причина налоговой льготы. Транспортный налог", Visible = false)]
        [ListView("Причина налоговой льготы. Транспортный налог", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string TaxExemptionReasonTS { get; set; }

        [ListView("Дата начала действия льготных условий налогообложения", Visible = false)]
        [DetailView("Дата начала действия льготных условий налогообложения", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionStartDate { get; set; }

        [ListView("Дата начала действия льготных условий налогообложения. Земельный налог", Visible = false)]
        [DetailView("Дата начала действия льготных условий налогообложения. Земельный налог", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionStartDateLand { get; set; }

        [ListView("Дата начала действия льготных условий налогообложения. Транспортный налог", Visible = false)]
        [DetailView("Дата начала действия льготных условий налогообложения. Транспортный налог", Visible = false)]
        [DefaultValue(0)]
        public DateTime? TaxExemptionStartDateTS { get; set; }

        [ListView("Наименование налога", Visible = false)]
        [DetailView("Наименование налога", Visible = false)]
        public TaxRateType TaxRateType { get; set; }

        [SystemProperty]
        public int? TaxRateTypeID { get; set; }

        [ListView("Налоговая ставка с учетом применяемых льгот, %", Visible = false)]
        [DetailView("Налоговая ставка с учетом применяемых льгот, %", Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxRateWithExemption { get; set; }

        [ListView("Налоговая ставка с учетом применяемых льгот, %. Земельный налог", Visible = false)]
        [DetailView("Налоговая ставка с учетом применяемых льгот, %. Земельный налог", Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxRateWithExemptionLand { get; set; }

        [ListView("Налоговая ставка с учетом применяемых льгот, %. Налог на ТС", Visible = false)]
        [DetailView("Налоговая ставка с учетом применяемых льгот, %. Налог на ТС", Visible = false)]
        [DefaultValue(0)]
        public decimal? TaxRateWithExemptionTS { get; set; }
    }
}
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.NU
{
    /// <summary>
    /// Строка декларации.
    /// </summary>
    [EnableFullTextSearch]
    public class DeclarationRow : TypeObject
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса DeclarationRow.
        /// </summary>
        public DeclarationRow(): base()
        {

        }

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
        /// Код по ОКТМО
        /// </summary>
        [ListView("Код по ОКТМО")]
        [DetailView("Код по ОКТМО")]
        [PropertyDataType(PropertyDataType.Text)]
        public string OKTMO { get; set; }

        /// <summary>
        /// Код бюджетной классификации
        /// </summary>
        [ListView("Код бюджетной классификации")]
        [DetailView("Код бюджетной классификации")]
        [PropertyDataType(PropertyDataType.Text)]
        public string KBK { get; set; }

        /// <summary>
        /// Сумма налога, подлежащая уплате в бюджет (Сумма налога, исчисленная к уменьшению)
        /// </summary>
        [ListView("Сумма налога, подлежащая уплате в бюджет")]
        [DetailView("Сумма налога, подлежащая уплате в бюджет", Description = "Сумма налога, подлежащая уплате в бюджет (Сумма налога, исчисленная к уменьшению)")]
        public decimal? Sum { get; set; }
             

        /// <summary>
        /// Исчисленная сумма налога, подлежащая уплате в бюджет за налоговый период, в т.ч. сумма авансовых платежей
        /// </summary>
        [ListView("Исчисленная сумма налога, подлежащая уплате в бюджет за налоговый период")]
        [DetailView("Исчисленная сумма налога, подлежащая уплате в бюджет за налоговый период", Description = "Исчисленная сумма налога, подлежащая уплате в бюджет за налоговый период, в т.ч. сумма авансовых платежей")]
        public decimal? CalcSum { get; set; }

        /// <summary>
        /// Сумма авансового платежа за I кв.
        /// </summary>
        [DetailView(Name = "Сумма авансового платежа за I кв.")]
        [ListView(Name = "Сумма авансового платежа за I кв")]
        public decimal? PrepaymentSumFirstQuarter { get; set; }

        
        /// <summary>
        /// Сумма авансового платежа за II кв.
        /// </summary>
        [DetailView(Name = "Сумма авансового платежа за II кв.")]
        [ListView(Name = "Сумма авансового платежа за II кв")]
        public decimal? PrepaymentSumSecondQuarter { get; set; }
               

        /// <summary>
        /// Сумма авансового платежа за III кв.
        /// </summary>
        [DetailView(Name = "Сумма авансового платежа за III кв.")]
        [ListView(Name = "Сумма авансового платежа за III кв")]
        public decimal? PrepaymentSumThirdQuarter { get; set; }


        /// <summary>
        /// Кадастровый номер.
        /// </summary>
        [ListView("Кадастровый номер")]
        [DetailView("Кадастровый номер")]
        [PropertyDataType(PropertyDataType.Text)]
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Условный номер.
        /// </summary>
        [ListView("Условный номер")]
        [DetailView("Условный номер")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ConditionalNumber { get; set; }

        /// <summary>
        /// Инвентарный номер.
        /// </summary>
        [ListView("Инвентарный номер")]
        [DetailView("Инвентарный номер")]
        [PropertyDataType(PropertyDataType.Text)]
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Код ОКОФ.
        /// </summary>
        [ListView("Код ОКОФ")]
        [DetailView("Код ОКОФ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string OKOF { get; set; }

        /// <summary>
        /// Остаточная стоимость на 31.12.
        /// </summary>
        [ListView("Остаточная стоимость на 31.12")]
        [DetailView("Остаточная стоимость на 31.12")]
        [DefaultValue(0)]
        public decimal? ResidualCost_3112 { get; set; }

        /// <summary>
        /// Остаточная стоимость на конец периода.
        /// </summary>
        [ListView("Остаточная стоимость на конец периода")]
        [DetailView("Остаточная стоимость конец периода")]
        [DefaultValue(0)]
        public decimal? ResidualCost_End { get; set; }

        /// <summary>
        /// Тип строки.
        /// </summary>
        /// <remarks>
        /// По разделам.
        /// </remarks>
        [SystemProperty]
        public string TypeRow { get; set; }
    }
}

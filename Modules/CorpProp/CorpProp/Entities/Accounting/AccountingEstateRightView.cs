using Base;
using Base.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Accounting
{
    public class AccountingEstateRightView : BaseObject
    {
        public AccountingEstateRightView(): base()
        {

        }

        [ListView]
        [DetailView("Инвентарный номер ОБУ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ObuInventoryNumber { get; set; }

        [ListView]
        [DetailView("Наименование ОБУ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ObuName { get; set; }

        [ListView]
        [DetailView("ОБУ Первоначальная стоимость, руб.")]        
        public decimal? ObuInitialCost { get; set; }

        [ListView]
        [DetailView("ОБУ Остаточная стоимость, руб.")]      
        public decimal? ObuResidualCost { get; set; }

        [ListView]
        [DetailView("Инвентарный номер ОИ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string EstateInventoryNumber { get; set; }

        [ListView]
        [DetailView("Наименование ОИ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string EstateName { get; set; }

        [ListView]
        [DetailView("ОИ Первоначальная стоимость, руб.")]       
        public decimal? EstateInitialCost { get; set; }

        [ListView]
        [DetailView("ОИ Остаточная стоимость, руб.")]      
        public decimal? EstateResidualCost { get; set; }

        [ListView]
        [DetailView("Наименование объекта права")]
        [PropertyDataType(PropertyDataType.Text)]
        public string LawObjectName { get; set; }

        [ListView]
        [DetailView("Кадастровый номер")]
        [PropertyDataType(PropertyDataType.Text)]
        public string LawCadastralNumber { get; set; }

        [ListView]
        [DetailView("Регистрационный номер")]
        [PropertyDataType(PropertyDataType.Text)]
        public string LawRegNumber { get; set; }

    }
}

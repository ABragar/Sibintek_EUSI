using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Entities.Complex;
using Base.Entities.Complex.KLADR;
using Base.Enums;
using Base.Security;
using Base.UI.Wizard;

namespace Base.Contact.Entities
{
    public class EmployeeUserWizard : DecoratedWizardObject<EmployeeUser>
    {

        [DetailView("Источник")]
        public SourceContact Source { get; set; }

        [DetailView("Пользователь", Required = true)]
        public User User { get; set; }

        
        //место работы
        [DetailView]
        [SystemProperty]
        public int CategoryID { get; set; }

        [DetailView("Отдел")]
        public Department Category { get; set; }

        [DetailView("Должность")]
        public EmployeePost Post { get; set; }

        //Примечание
        [PropertyDataType(PropertyDataType.SimpleHtml)]
        [DetailView(HideLabel = true)]
        public string Description { get; set; }

        public override EmployeeUser GetObject()
        {
            return new EmployeeUser()
            {
                CategoryID = this.CategoryID,                
            };
        }
    }
}

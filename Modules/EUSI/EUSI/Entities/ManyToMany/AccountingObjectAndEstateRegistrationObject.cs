using Base;
using Base.Attributes;
using CorpProp.Entities.Accounting;
using EUSI.Entities.Estate;
using System.ComponentModel;

namespace EUSI.Entities.ManyToMany
{
    /// <summary>
    /// Представляет связь ОС/НМА с заявкой на регистрацию.
    /// </summary>
    public class AccountingObjectAndEstateRegistrationObject: ManyToManyAssociation<AccountingObject, EstateRegistration>
    {
        public AccountingObjectAndEstateRegistrationObject() : base()
        {

        }

        /// <summary>
        /// Получает или задает признак, что связь заявки установлена с прототипом (новым) ОС/НМА. 
        /// </summary>
        [ListView("Прототип", Visible = false)]
        [DetailView("Прототип", Visible = false)]
        [SystemProperty]
        [DefaultValue(false)]
        public bool IsPrototype { get; set; }
    }
}

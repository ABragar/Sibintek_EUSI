using Base;
using Base.Attributes;
using CorpProp.Entities.Accounting;
using EUSI.Entities.Estate;
using System.ComponentModel;

namespace EUSI.Entities.ManyToMany
{
    /// <summary>
    /// Представляет связь ОИ с заявкой на регистрацию.
    /// </summary>
    public class EstateAndEstateRegistrationObject: ManyToManyAssociation<CorpProp.Entities.Estate.Estate, EstateRegistration>
    {
        public EstateAndEstateRegistrationObject() : base()
        {

        }

        /// <summary>
        /// Получает или задает признак, что связь заявки установлена с прототипом (новым) ОИ. 
        /// </summary>
        [ListView("Прототип", Visible = false)]
        [DetailView("Прототип", Visible = false)]
        [SystemProperty]
        [DefaultValue(false)]
        public bool IsPrototype { get; set; }
    }
}

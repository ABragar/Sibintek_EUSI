using System.ComponentModel;
using Base;
using Base.Attributes;
using CorpProp.Entities.Estate;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;

namespace CorpProp.Model.Partial.Entities.SeparLand
{
    public class SeparLand : BaseObject
    {
        public int? EstateID { get; set; }
        [DetailView("ОИ", TabName = EstateTabs.GeneralInfo, Order = 1)]
        public Estate Estate { get; set; }
        public bool IsFake { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Получает или задает номер ЕУСИ.
        /// </summary>
        [DetailView("Номер ЕУСИ", Visible = false, ReadOnly = true)]
        [SystemProperty]
        public int Number { get; set; }

        /// <summary>
        /// Получает или задает статус ОИ по заявке ЗР.
        /// </summary>
        [DetailView("Статус", Visible = false)]
        [ListView("Статус", Visible = false)]
        [SystemProperty]
        public EstateStatus EstateStatus { get; set; }

        /// <summary>
        /// Получает или задает родительский инв. объект.
        /// </summary>        
        [DetailView(Name = "Вышестоящий объект имущества")]
        public InventoryObject Parent { get; set; }

        #region EUSI
        /// <summary>
        /// Получает или задает признак что это имущество как ИК.
        /// </summary>
        [SystemProperty]
        [DetailView("ОИ как имущественный комплекс", TabName = EstateTabs.GeneralInfo)]
        [DefaultValue(false)]
        public bool IsPropertyComplex { get; set; }
        #endregion
    }
}

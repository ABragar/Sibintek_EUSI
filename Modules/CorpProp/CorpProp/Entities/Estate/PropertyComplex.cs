using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using Newtonsoft.Json;
using CorpProp.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Base.DAL;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.Security;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет имущественный комплекс.
    /// </summary>
    [Obsolete("Use PropertyComplexIO")]
    public class PropertyComplex : HCategory, ITreeObject, ITypeObject//, IAdditionalEstateCharacteristics, ITypeObject
    {

        /// <summary>
        /// Получает или задает уникальный ИД объекта.
        /// </summary>
        [SystemProperty]
        public System.Guid Oid { get; set; }


        ///// <summary>
        ///// Получает или задает дату создания объекта.
        ///// </summary>
        [SystemProperty]
        public DateTime? CreateDate { get; set; }


        #region History 
        //TODO: натянуть историю

        public virtual void InitHistory(IUnitOfWork uow) { }


        [SystemProperty]
        [DefaultValue(false)]
        public bool IsHistory { get; set; }

        ///// <summary>
        ///// Получает или задает дату начала действия историчности записи.
        ///// </summary>
        [SystemProperty]
        public DateTime? ActualDate { get; set; }

        /// <summary>
        /// Получает или задает дату окончания действия историчности записи.
        /// </summary>
        [SystemProperty]
        public DateTime? NonActualDate { get; set; }

        #endregion

        /// <summary>
        /// Получает или задает ИД класса ИК.
        /// </summary>
        public int? PropertyComplexKindID { get; set; }

        /// <summary>
        /// Получает или задает класс ИК.
        /// </summary>
        
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Класс", Required = true)]
        public PropertyComplexKind PropertyComplexKind { get; set; }


        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Примечание")]
        public string Description { get; set; }



        /// <summary>
        /// Получает или задает кол-во объектов.
        /// </summary>
        /// <remarks>
        /// Сумма всех связанных инвентарных объектов (рекурсивно по всем вложенным ИК).
        /// </remarks>
        // TODO : добавить логику дя суммы объектов ИК.
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Количество объектов", ReadOnly = false)]
        public int? InventoryObjectsCount { get; set; }



        ///// <summary>
        ///// Получает или задает стоимость объектов.
        ///// </summary>
        ///// <remarks>
        ///// Сумма остаточных стоимостей связанных инвентарных объектов (рекурсивно по всем вложенным ИК).
        ///// </remarks>
        //// TODO : добавить логику дя стоимости объектов ИК.
        //[FullTextSearchProperty]
        //[ListView]
        //[DetailView(Name = "Стоимость объектов", ReadOnly = true)]
        //public decimal? ResidualCost { get; set; }


        /// <summary>
        /// Получает или задает наименование вышестоящего ИК.
        /// </summary>       
        // TODO : добавить логику дя наименования вышестоящего ИК.
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Наименование вышестоящего ИК", ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ParentName { get; set; }



        /// <summary>
        /// Получает или задает родительский элемент.
        /// </summary>
        //[JsonIgnore]
        [ForeignKey("ParentID")]
        
        [ListView(Hidden = true)]
        [DetailView(Name = "Вышестоящий ИК", Visible = false)]
        public virtual PropertyComplex Parent_ { get; set; }

        //[JsonIgnore]    
        
        //[DetailView(Name = "Нижестоящие ИК", TabName = "[7]Нижестоящие ИК", HideLabel = true)]
        public virtual ICollection<PropertyComplex> Children_ { get; set; }

        [NotMapped]
        //
        //[ListView(Hidden = true)]
        //[DetailView(Name = "Вышестоящий ИК")]
        public override HCategory Parent => this.Parent_;




        [NotMapped]
        //
        //[DetailView(Name = "Нижестоящие ИК", TabName = "[7]Нижестоящие ИК", HideLabel = true)]
        public override IEnumerable<HCategory> Children => Children_ ?? Enumerable.Empty<PropertyComplex>();



        /// <summary>
        /// Получает или задает полное наименование объекта в дереве ИК.
        /// </summary>
        /// <remarks>
        /// Возвращает в формате: rootName\childName1\childName11 и т.д.
        /// </remarks>
        [ListView(Hidden = true)]
        [FullTextSearchProperty]
        [DetailView(Name = "Полное наименование", Order = 1, Visible = false, ReadOnly = true)]
        public string FullName { get; set; }

        #region EUSI
        [SystemProperty]
        public int? SibCountryID { get; set; }

        [ListView(Visible = false)]
        [FullTextSearchProperty]
        [DetailView(Name = "Страна", Visible = false)]
        public SibCountry SibCountry { get; set; }

        [SystemProperty]
        public int? SibRegionID { get; set; }

        [ListView(Visible = false)]
        [FullTextSearchProperty]
        [DetailView(Name = "Субъект РФ", Visible = false)]
        public SibRegion SibRegion { get; set; }


        [ListView(Visible = false)]
        [FullTextSearchProperty]
        [DetailView(Name = "Город/ Населенный пункт", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string City { get; set; }

        [ListView(Visible = false)]
        [FullTextSearchProperty]
        [DetailView(Name = "Адрес", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Address { get; set; }


        [SystemProperty]
        public int? SibUserID { get; set; }

        [ListView(Visible = false)]
        [FullTextSearchProperty]
        [DetailView(Name = "Пользователь", Visible = false)]
        public SibUser SibUser { get; set; }
        


        [ListView(Visible = false)]
        [DetailView(Name = "Первоначальная стоимость ИК по данным БУ", ReadOnly = true, Visible = false)]
        [DefaultValue(0)]
        public decimal? InitialCostOBU { get; set; }


        [ListView(Visible = false)]
        [DetailView(Name = "Остаточная стоимость ИК по данным БУ", ReadOnly = true, Visible = false)]
        [DefaultValue(0)]
        public decimal? ResidualCostOBU { get; set; }

        [ListView(Visible = false)]
        [DetailView(Name = "Первоначальная стоимость ИК по данным НУ", ReadOnly = true, Visible = false)]
        [DefaultValue(0)]
        public decimal? InitialCostNU { get; set; }


        [ListView(Visible = false)]
        [DetailView(Name = "Остаточная стоимость ИК по данным НУ", ReadOnly = true, Visible = false)]
        [DefaultValue(0)]
        public decimal? ResidualCostNU { get; set; }
        #endregion
        /// <summary>
        /// Инициализирует новый экземпляр класса PropertyComplex.
        /// </summary>
        public PropertyComplex() : base()
        {
            Oid = System.Guid.NewGuid();
            CreateDate = DateTime.Now;
            ActualDate = DateTime.Now.Date;
            IsHistory = false;
        }


        //#region Дополнительные характеристики
        //[DetailView(Name = "TxtField1", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 200)]
        //public string TxtField1 { get; set; }
        //[DetailView(Name = "TxtField2", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 201)]
        //public string TxtField2 { get; set; }
        //[DetailView(Name = "TxtField3", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 202)]
        //public string TxtField3 { get; set; }
        //[DetailView(Name = "TxtField4", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 203)]
        //public string TxtField4 { get; set; }
        //[DetailView(Name = "TxtField5", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 204)]
        //public string TxtField5 { get; set; }
        //[DetailView(Name = "TxtField6", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 205)]
        //public string TxtField6 { get; set; }
        //[DetailView(Name = "TxtField7", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 206)]
        //public string TxtField7 { get; set; }
        //[DetailView(Name = "TxtField8", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 207)]
        //public string TxtField8 { get; set; }
        //[DetailView(Name = "TxtField9", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 208)]
        //public string TxtField9 { get; set; }
        //[DetailView(Name = "TxtField10", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 209)]
        //public string TxtField10 { get; set; }
        //[DetailView(Name = "FloatIntField1", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 210)]
        //public int? FloatIntField1 { get; set; }
        //[DetailView(Name = "FloatIntField2", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 211)]
        //public int? FloatIntField2 { get; set; }
        //[DetailView(Name = "FloatIntField3", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 212)]
        //public int? FloatIntField3 { get; set; }
        //[DetailView(Name = "FloatIntField4", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 213)]
        //public int? FloatIntField4 { get; set; }
        //[DetailView(Name = "FloatIntField5", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 214)]
        //public int? FloatIntField5 { get; set; }
        //[DetailView(Name = "FloatIntField6", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 215)]
        //public int? FloatIntField6 { get; set; }
        //[DetailView(Name = "FloatIntField7", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 216)]
        //public int? FloatIntField7 { get; set; }
        //[DetailView(Name = "FloatIntField8", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 217)]
        //public int? FloatIntField8 { get; set; }
        //[DetailView(Name = "FloatIntField9", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 218)]
        //public int? FloatIntField9 { get; set; }
        //[DetailView(Name = "FloatIntField10", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 219)]
        //public int? FloatIntField10 { get; set; }
        //[DetailView(Name = "FloatDecimalField1", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 220)]
        //public decimal? FloatDecimalField1 { get; set; }
        //[DetailView(Name = "FloatDecimalField2", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 221)]
        //public decimal? FloatDecimalField2 { get; set; }
        //[DetailView(Name = "FloatDecimalField3", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 222)]
        //public decimal? FloatDecimalField3 { get; set; }
        //[DetailView(Name = "FloatDecimalField4", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 223)]
        //public decimal? FloatDecimalField4 { get; set; }
        //[DetailView(Name = "FloatDecimalField5", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 224)]
        //public decimal? FloatDecimalField5 { get; set; }
        //[DetailView(Name = "FloatDecimalField6", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 225)]
        //public decimal? FloatDecimalField6 { get; set; }
        //[DetailView(Name = "FloatDecimalField7", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 226)]
        //public decimal? FloatDecimalField7 { get; set; }
        //[DetailView(Name = "FloatDecimalField8", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 227)]
        //public decimal? FloatDecimalField8 { get; set; }
        //[DetailView(Name = "FloatDecimalField9", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 228)]
        //public decimal? FloatDecimalField9 { get; set; }
        //[DetailView(Name = "FloatDecimalField10", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 229)]
        //public decimal? FloatDecimalField10 { get; set; }
        //[DetailView(Name = "BoolField1", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 230)]
        //public bool? BoolField1 { get; set; }
        //[DetailView(Name = "BoolField2", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 231)]
        //public bool? BoolField2 { get; set; }
        //[DetailView(Name = "BoolField3", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 232)]
        //public bool? BoolField3 { get; set; }
        //[DetailView(Name = "BoolField4", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 233)]
        //public bool? BoolField4 { get; set; }
        //[DetailView(Name = "BoolField5", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 234)]
        //public bool? BoolField5 { get; set; }
        //[DetailView(Name = "BoolField6", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 235)]
        //public bool? BoolField6 { get; set; }
        //[DetailView(Name = "BoolField7", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 236)]
        //public bool? BoolField7 { get; set; }
        //[DetailView(Name = "BoolField8", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 237)]
        //public bool? BoolField8 { get; set; }
        //[DetailView(Name = "BoolField9", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 238)]
        //public bool? BoolField9 { get; set; }
        //[DetailView(Name = "BoolField10", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 239)]
        //public bool? BoolField10 { get; set; }
        //[DetailView(Name = "DataField1", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 240)]
        //public DateTime? DataField1 { get; set; }
        //[DetailView(Name = "DataField2", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 241)]
        //public DateTime? DataField2 { get; set; }
        //[DetailView(Name = "DataField3", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 242)]
        //public DateTime? DataField3 { get; set; }
        //[DetailView(Name = "DataField4", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 243)]
        //public DateTime? DataField4 { get; set; }
        //[DetailView(Name = "DataField5", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 244)]
        //public DateTime? DataField5 { get; set; }
        //[DetailView(Name = "DataField6", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 245)]
        //public DateTime? DataField6 { get; set; }
        //[DetailView(Name = "DataField7", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 246)]
        //public DateTime? DataField7 { get; set; }
        //[DetailView(Name = "DataField8", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 247)]
        //public DateTime? DataField8 { get; set; }
        //[DetailView(Name = "DataField9", Visible = false, TabName = "[8]Дополнительные характеристики"), ListView(Visible = false, Order = 248)]
        //public DateTime? DataField9 { get; set; }
        //[DetailView(Name = "DataField10", Visible = false, TabName = "[8]Дополнительные характеристики", Order = 201), ListView(Visible = false, Order = 249)]
        //public DateTime? DataField10 { get; set; }

        //#endregion

    }
}


using Base.Attributes;
using CorpProp.Attributes;
using CorpProp.Entities.NSI;
using System;
using System.ComponentModel;
using Base.DAL;
using CorpProp.Entities.Accounting;
using Base.Utils.Common.Attributes;
using Base.EntityFrameworkTypes.Complex;

namespace CorpProp.Entities.Estate
{

    /// <summary>
    /// Представляет кадастровый объект.
    /// </summary>
    /// <remarks>
    /// Объект в том составе (границах), в котором он поставлен на кадастровый учёт и/или в отношении него зарегистированы права.
    /// Идентифицируется уникальным кадастровым(условным) номером в ЕГРН. 
    /// Один кадастровый объект может с т.зр. физических границ включать в себя несколько инвентарных либо наоборот.
    /// Класс 'Кадастровый объект' наследует все атрибуты класса 'Инвентарный объект', 
    /// однако экземпляры данных классов могут представлять взаимно пересекающиеся физические объекты.
    /// </remarks>
    [EnableFullTextSearch]
    public class Cadastral : RealEstate
    {


        /// <summary>
        /// Инициализирует новый экземпляр класса Cadastral.
        /// </summary>
        public Cadastral() : base()
        {
          
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса Cadastral из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public Cadastral(IUnitOfWork uofw, AccountingObject obj) : base (uofw, obj)
        {

            this.CadastralNumber = obj.CadastralNumber;
        }

        #region EUSI
        

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView("Условный номер", Visible = false)]
        [ListView("Условный номер", Visible = false)]
        public String ConditionalNumber { get; set; }


        [SystemProperty]
        public int? WellCategoryID { get; set; }

        [DetailView("Категория скважины", Visible = false)]
        [ListView(Visible = false)]
        public WellCategory WellCategory { get; set; }



        #endregion//END EUSI REGION
        //-----------------------------------------------------------


        /// <summary>
        /// Получает или задает признак фэйкового объекта.
        /// </summary>
        [SystemProperty]
        [DefaultValue(false)]
        public bool IsFake { get; set; }
       

        /// <summary>
        /// Получает или задает наименование по данным Росреестра. 
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Наименование по данным ЕГРН", Visible = false, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string NameByRight { get; set; }


        [ListView(Hidden = true)]
        [DetailView(Name = "Налог с кад.стоимости", Visible =false)]
        [DefaultValue(false)]
        public bool IsTaxCadastral { get; set; }


        /// <summary>
        /// Получает или задает Объем в кубических метрах с округлением до 1 кубического метра
        /// </summary>
        [DefaultValue(0)]
        [DetailView(Visible = false)]
        public decimal? Volume { get; set; }

        /// <summary>
        /// Получает или задает Глубина в метрах с округлением до 0,1 метра
        /// </summary>
        [PropertyDataType("Sib_Decimal2")]
        [DetailView(Visible = false)]
        public decimal? Depth { get; set; }

        /// <summary>
        /// Получает или задает Глубина залегания в метрах с округлением до 0,1 метра
        /// </summary>
        [DetailView(Visible = false)]
        public decimal? DepthOf { get; set; }

        /// <summary>
        /// Получает или задает Высота в метрах с округлением до 0,1 метра
        /// </summary>
        [DetailView(Visible = false)]
        public decimal? Height { get; set; }

        /// <summary>
        /// Получает или задает Протяженность в метрах с округлением до 1 метра
        /// </summary>
        [DetailView(Visible = false)]
        public decimal? Extension { get; set; }
       
        

        /// <summary>
        /// Куст
        /// </summary>
        /// <remarks>Соответствующие поля типа объекта имущества	ОИ</remarks>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView("Куст",Visible = false)]
        public string Bush { get; set; }

        /// <summary>
        /// Скважина
        /// </summary>
        /// <remarks>Соответствующие поля типа объекта имущества	ОИ</remarks>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView("Скважина",Visible = false)]
        public string Well { get; set; }

        /// <summary>
        /// Получает или задает адрес
        /// </summary>     
        //[ListView(Order = 7)]
        //[DetailView(Name = "Адрес (местоположение)", TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        //[PropertyDataType(PropertyDataType.Text)]

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string AddressID { get; set; }


        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string District { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string City { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string Locality { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string Street { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string House { get; set; }

        /// <summary>
        /// Получает или задает особые отметки.
        /// </summary>
        
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string SpecialMarks { get; set; }

        /// <summary>
        /// Получает или задает дату постановки на учет/ регистрации.
        /// </summary>
        [DetailView(Visible = false)]
        public DateTime? RegDate { get; set; }


        /// <summary>
        /// Получает или задает дату снятия с учета/регистрации.
        /// </summary>
        [DetailView(Visible = false)]
        public DateTime? DeRegDate { get; set; }



        /// <summary>
        /// Получает или задает кадастровый номер.
        /// </summary>
        //[FullTextSearchProperty]
        //[ListView]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Получает или задает кадастровый номер ЗУ.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        [FullTextSearchProperty]
        public string CadastralNumberLand { get; set; }


        /// <summary>
        /// Получает или задает ранее присвоенный гос. учетный номер.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string OldRegNumbers { get; set; }

        /// <summary>
        /// Получает или задает сведения об изъятии.
        /// </summary>
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        
        public string Confiscation { get; set; }

        /// <summary>
        /// Получает или задает номер кадастрового квартала.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string BlocksNumber { get; set; }

        /// <summary>
        /// Получает или задает Кадастровые номера иных объектов недвижимости.
        /// </summary>
        //[FullTextSearchProperty]
        //[ListView]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string OtherCadastralNumber { get; set; }

        /// <summary>
        /// Получает или задает текущую кадастровая стоимость.
        /// </summary> 
        // TODO: добавить логику для вычисления текущей кадастровой стоимости.
        //[FullTextSearchProperty]
        ////[ListView]
        //[DetailView(Name = "Текущая кадастровая стоимость", TabName = TabName6)]
        [DefaultValue(0)]
        [DetailView(Visible = false)]
        public decimal? CadastralValue { get; set; } 
        
      
        /// <summary>
        /// Плучает или задает виды разрешенного использования.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String UsesKind { get; set; }

        

        /// <summary>
        /// Получает или задает площадь застройки.
        /// </summary> 
        
        //[FullTextSearchProperty]
        //[ListView]
        [DetailView(Visible = false)]
        [PropertyDataType("Sib_Decimal2")]
        public decimal? BuildingArea { get; set; }

        [PropertyDataType(PropertyDataType.Text)]

        public string AreaText { get; set; }

        /// <summary>
        /// Получает или задает площадь.
        /// </summary> 
        
        //[FullTextSearchProperty]
        //[ListView]
        [DetailView(Visible = false)]
        [PropertyDataType("Sib_Decimal2")]
        [DefaultValue(0)]
        public decimal? Area { get; set; }


        ///// <summary>
        ///// Получает или задает единицу измерения площади.
        ///// </summary>
        //
        //[FullTextSearchProperty]
        //[ListView]
        [DetailView(Visible = false)]
        public  SibMeasure AreaUnit { get; set; }


        
        /// <summary>
        /// Получает или задает код вида права.
        /// </summary>           
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string RightKindCode { get; set; }

        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RightRegNumber { get; set; }

        [DetailView(Visible = false)]
        public DateTime? RightRegDate { get; set; }

        [DetailView(Visible = false)]
        public DateTime? RightRegEndDate { get; set; }

        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String RighKindAndShare { get; set; }

        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String RighHolder { get; set; }

        /// <summary>
        /// Получает или задает долю в праве
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string ShareText { get; set; }


        [DetailView("Местоположение")]
        [ListView(Hidden = true)]
        [PropertyDataType(PropertyDataType.LocationPolygon)]
        public Location Location { get; set; } = new Location();
    }
}

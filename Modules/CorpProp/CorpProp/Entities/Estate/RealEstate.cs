using Base;
using Base.Attributes;
using Base.DAL;
using Base.Utils.Common.Attributes;
using BaseCatalog.Entities;
using CorpProp.Attributes;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет объект недвижимого имущества.
    /// </summary>
    /// <remarks>
    /// Инвентарный объект, являющийся недвижимым имуществом в силу закона, независимо от регистрации прав на него.
    /// В случае, если к объекту привязан 1 кадастровый объект - заполняется по соотв. атрибутам кадастрового объекта. 
    /// В случае, если привязано несколько объектов - суммируется. 
    /// В случае, если объекты не привязаны - заполняется пользователем.
    /// Описание конкретных атрибутов см.в описании соответствующих атрибутов кадастрового объекта.
    /// </remarks>
    [EnableFullTextSearch]
    public class RealEstate : InventoryObject
    {

        #region EUSI

        [SystemProperty]
        public int? PermittedUseKindID { get; set; }

        /// <summary>
        /// Получает или задает вид разрешенного использования.
        /// </summary>
        [DetailView("Вид разрешенного использования", Visible = false)]
        [ListView("Вид разрешенного использования", Visible = false)]
        //[PropertyDataType(PropertyDataType.Text)]
        public PermittedUseKind PermittedUseKind { get; set; }

                


        #endregion//END EUSI REGION
        //--------------------------------------------------------------------------------------------------

        /// <summary>
        /// Получает или задает ИД вида ОНИ.
        /// </summary>
        public int? RealEstateKindID { get; set; }

        /// <summary>
        /// Получает или задает вид объекта недвижимого имущества.
        /// </summary>
        
        //[ListView]
        [DetailView(Visible = false)]
        public  RealEstateKind RealEstateKind { get; set; }

        /// <summary>
        /// Получает или задает вид объекта недвижимого имущества.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String RealEstateKinds { get; set; }

              

        /// <summary>
        /// Получает или задает ИД типа основной характеристики.
        /// </summary>
        public int? FeatureTypesID { get; set; }


        /// <summary>
        /// Получает или задает тип основной характеристики.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public  FeatureType FeatureTypes { get; set; }

        /// <summary>
        /// Получает или задает значение основной характеристики.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public decimal? FeatureValues { get; set; }


        /// <summary>
        /// Получает или задает ИД ед. измерения основной характеристики.
        /// </summary>
        public int? FeatureUnitsID { get; set; }


        /// <summary>
        /// Получает или задает ед. измерения основной характеристики.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public  SibMeasure FeatureUnits { get; set; }


       
        
        /// <summary>
        /// Получает или задает назначение.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String Appointments { get; set; }


        /// <summary>
        /// Получает или задает проектируемое назначение.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String AppointmentOnPlans { get; set; }

        /// <summary>
        /// Получает или задает кол-во этажей, в том числе подземных.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public decimal? FloorsCount { get; set; }

        public int? RealEstatePurposeID { get; set; }
        [DetailView(Visible = false)]
        public  RealEstatePurpose RealEstatePurpose { get; set; }


        /// <summary>
        /// Получает или задает номер, тип этажа, на котором расположено помещение, машино-место.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String Floors { get; set; }

        /// <summary>
        /// Получает или задает вид жилого помещения.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String BuildingKinds { get; set; }

        /// <summary>
        /// Получает или задает материал наружных стен.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String WallMaterials { get; set; }


        //commissioning

        /// <summary>
        /// Получает или задает год ввода в эксплуатацию по завершении строительства.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public int? YearCommissionings { get; set; }
 

        /// <summary>
        /// Инициализирует новы йэкземпляр класса RealEstate.
        /// </summary>
        public RealEstate() : base()
        {
            
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса RealEstate из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public RealEstate(IUnitOfWork uofw, AccountingObject obj) : base (uofw, obj)
        {
            
        }
    }
}

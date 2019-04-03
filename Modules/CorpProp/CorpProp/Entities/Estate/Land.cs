using Base;

using Base.Attributes;
using BaseCatalog.Entities;
using CorpProp.Attributes;
using CorpProp.Entities.Law;
using CorpProp.Entities.NSI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Asset;
using CorpProp.Helpers;
using CorpProp.Entities.Accounting;
using Base.DAL;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет земельный участок.
    /// </summary>    
    [EnableFullTextSearch]
    public class Land : Cadastral
    {

        #region EUSI

        [SystemProperty]
        public int? AddonAttributeGroundCategoryID { get; set; }

        /// <summary>
        /// Получает или задает Доп. признак категории земель.
        /// </summary>
        [DetailView("Доп. признак категории земель", Visible = false)]
        [ListView("Доп. признак категории земель", Visible = false)]       
        public AddonAttributeGroundCategory AddonAttributeGroundCategory { get; set; }

        //LandPurpose
        [SystemProperty]
        public int? LandPurposeID { get; set; }

        /// <summary>
        /// Получает или задает Доп. признак категории земель.
        /// </summary>
        [DetailView("Назначение ЗУ", Visible = false)]
        [ListView("Назначение ЗУ", Visible = false)]        
        public LandPurpose LandPurpose { get; set; }


       
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView("Номер квартала/выдела", Visible = false)]
        [ListView("Номер квартала/выдела", Visible = false)]
        public String PlotsBlock { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView("Номер выдела", Visible = false)]
        [ListView("Номер выдела", Visible = false)]
        public String PlotsNumber { get; set; }
               

        #endregion//END EUSI REGION
        //--------------------------------------------------------------------

        /// <summary>
        /// Инициализирует новый экземпляр класса Land.
        /// </summary>
        public Land() : base() { }

        /// <summary>
        /// Инициализирует новый экземпляр класса Land из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public Land(IUnitOfWork uofw, AccountingObject obj) : base (uofw, obj)
        {
            Area = obj.GroundArea;
            GroundCadastralNumber = obj.GroundCadastralNumber;
            Wood = obj.Wood;
            GroundFullArea = obj.GroundFullArea;
            GroundCategory = obj.GroundCategory;
            UsesKind = obj.UsesKind;
        }


        /// <summary>
        /// Плучает или задает категории земель.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        //[PropertyDataType(PropertyDataType.Text)]
        public GroundCategory GroundCategory { get; set; }

        /// <summary>
        /// Получает или задает ИД категории земель.
        /// </summary>
        [SystemProperty]
        public int? GroundCategoryID { get; set; }

        /// <summary>
        /// вид разрешенного использования По документу
        /// </summary>              
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string PermittedByDoc { get; set; }

        /// <summary>
        /// Вид разрешенного использования земельного участка в соответствии с ранее использовавшимся классификатором
        /// </summary>           
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string PermittedLandUse { get; set; }
        
        /// <summary>
        /// Вид разрешенного использования земельного участка в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540
        /// </summary>   
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string PermittedLandUseMer { get; set; }

        /// <summary>
        /// Реестровый номер границы  градостроительному регламенту
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string PermittesGradRegNumbBorder { get; set; }

        /// <summary> 
        /// Вид разрешенного использования в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string PermittesGradLandUse { get; set; }

        /// <summary>
        /// Получает или задает эксплуатируемую площадь ЗУ.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType("Sib_Decimal2")]
        public decimal? UseArea { get; set; }

        

        public int? LandTypeID { get; set; }

        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public LandType LandType { get; set; }

        /// <summary>
        /// Получает или задает сведения об иных природных объектах, расположенных в пределах земельного участка.
        /// </summary>
        
        [PropertyDataType(PropertyDataType.Text)]
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public String OtherObjectsData { get; set; }

        /// <summary>
        /// Получает или задает сведения о том, что земельный участок полностью или частично расположен в границах зоны с особыми условиями использования территории или территории объекта культурного наследия.
        /// </summary>
        
        [PropertyDataType(PropertyDataType.Text)]
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public String SpecialZone { get; set; }

        /// <summary>
        /// Получает или задает сведения о том, что земельный участок расположен в границах особой экономической зоны, территории опережающего социально-экономического развития, зоны территориального развития в Российской Федерации, игорной зоны.
        /// </summary>
        
        [PropertyDataType(PropertyDataType.Text)]
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public String EconomicZone { get; set; }

        /// <summary>
        /// Получает или задает сведения о том, что земельный участок расположен в границах особо охраняемой природной территории, охотничьих угодий, лесничеств, лесопарков.
        /// </summary>
        
        [PropertyDataType(PropertyDataType.Text)]
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public String EcoZone { get; set; }

        /// <summary>
        /// Получает или задает сведения о результатах проведения государственного земельного надзора.
        /// </summary>
        
        [PropertyDataType(PropertyDataType.Text)]
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public String MonitorData { get; set; }

        /// <summary>
        /// Получает или задает сведения о расположении земельного участка в границах территории, в отношении которой утвержден проект межевания территории.
        /// </summary>
        
        [PropertyDataType(PropertyDataType.Text)]
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public String DemarcationZone { get; set; }

        /// <summary>
        /// Получает или задает условный номер земельного участка.
        /// </summary>
        
        [PropertyDataType(PropertyDataType.Text)]
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public String LandNumber { get; set; }

        /// <summary>
        /// Дата постановки на кадастровый учет по документу
        /// </summary>
        public DateTime? RegDateByDoc { get; set; }

        //distinction

        /// <summary>
        /// Получает или задает сведения о том, что земельный участок образован из земель или земельного участка, государственная собственность на которые не разграничена.
        /// </summary>
        
        [PropertyDataType(PropertyDataType.Text)]
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public String DistinctionData { get; set; }

        /// <summary>
        /// Получает или задает сведения о наличии земельного спора о местоположении границ земельных участков.
        /// </summary>
        
        [PropertyDataType(PropertyDataType.Text)]
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public String DisputeData { get; set; }

       

        /// <summary>
        /// Получает или задает сведения о лесничестве.
        /// </summary>
        
        [PropertyDataType(PropertyDataType.Text)]
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        public String Forestry { get; set; }

        

        /// <summary>
        /// Получает или задает сведения о выделах.
        /// </summary>
        
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public String Plots { get; set; }


        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string GroundCadastralNumber { get; set; }

        [DetailView(Visible = false)]
        public bool Wood { get; set; } = false;

        [DetailView(Visible = false)]
        public decimal? GroundFullArea { get; set; }

        

    }
}

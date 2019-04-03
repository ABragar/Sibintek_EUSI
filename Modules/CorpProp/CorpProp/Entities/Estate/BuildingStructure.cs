
using Base.Attributes;
using Base.DAL;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Accounting;
using System;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет Здание /сооружение.
    /// </summary>  
    [EnableFullTextSearch]
    public class BuildingStructure : Cadastral
    {
        
        /// <summary>
        /// Инициализирует новый экземпляр класса BuildingsStructures.
        /// </summary>
        public BuildingStructure() : base()
        {

        }

        /// <summary>
        /// Инициализирует новый экземпляр класса BuildingStructure из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public BuildingStructure(IUnitOfWork uofw, AccountingObject obj) : base (uofw, obj)
        {          
            this.YearComplete = obj.Year?.ToString();
            BuildingArea = obj.BuildingArea;
            Area = obj.BuildingFullArea;
            Floor = (obj.BuildingFloor != null) ?  obj.BuildingFloor.ToString(): "";
            UndergroundFloorCount = obj.BuildingUnderground;
            //?? = obj.BuildingLength;
            LayingType = obj.LayingType;
            Volume = obj.ContainmentVolume;
            //Well = obj.DepthWell;
            Height = obj.Height;
            WallMaterial = obj.Material;
            //Status = obj.State;


        }


        /// <summary>
        /// Получает или задает год завершения строительства.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string YearComplete { get; set; }

        /// <summary>
        /// Получает или задает год ввода в эксплуатацию.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string YearStart { get; set; }



        /// <summary>
        /// Получает или задает кол-во подземных этажей.
        /// </summary>    
        [DetailView(Visible = false)]
        public decimal? UndergroundFloorCount { get; set; }


        /// <summary>
        /// Получает или задает номер, тип этажа, на котором расположено помещение, машино-место.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String Floor { get; set; }

        /// <summary>
        /// Получает или задает вид жилого помещения.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String BuildingKind { get; set; }

        /// <summary>
        /// Получает или задает материал наружных стен.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String WallMaterial { get; set; }

        /// <summary>
        /// Получает или задает сведения об отнесении жилого помещения к определенному виду жилых помещений специализированного жилищного фонда, к жилым помещениям наемного дома социального использования или наемного дома коммерческого использования.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String LifeKindData { get; set; }
        
        /// <summary>
        /// Получает или задает сведения о принятии акта и (или) заключении договора, предусматривающих предоставление в соответствии 
        /// с земельным законодательством исполнительным органом государственной власти или органом местного самоуправления 
        /// находящегося в государственной или муниципальной собственности земельного участка для строительства наемного дома 
        /// социального использования или наемного дома коммерческого использования.
        /// </summary>
        
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String ActData { get; set; }

        /// <summary>
        /// Кадастровые номера машиномест
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string CarPlaceCadastralNumber { get; set; }

        /// <summary>
        /// кадастровые номера помещений
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string RoomsCadastralNumber { get; set; }

        
    }
}

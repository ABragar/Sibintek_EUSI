using Base;
using Base.Attributes;
using Base.ComplexKeyObjects.Superb;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Сведения об объекте недвижимого имущества.
    /// </summary>
    [EnableFullTextSearch]
    public class ObjectRecord : BaseObject, ISuperObject<ObjectRecord>
    {
        protected const string TabName1 = "[001]Реквизиты выписки";
        protected const string TabName2 = "[002]Характеристики недвижимости";
        protected const string TabName22 = "[002]Земельный участок";
        protected const string TabName3 = "[003]Адрес (местоположение)";
        protected const string TabName4 = "[004]Права";
        protected const string TabName5 = "[005]Сведения о праве (бесхозяйное имущество)";
        protected const string TabName6 = "[006]Обременения/ограничения";
        protected const string TabName7 = "[007]Сделки, совершенные без необходимого в силу закона согласия третьего лица, органа";
        public ObjectRecord()
        {
            //Room_records = new List<RoomLocationInBuildPlans>();
            //Car_parking_space_records = new List<CarParkingSpaceLocationInBuildPlans>();
            //Land_cad_numbers = new List<CadNumber>();
            //Room_cad_numbers = new List<CadNumber>();
            //Car_parking_space_cad_numbers = new List<CadNumber>();
            //Old_numbers = new List<OldNumber>();
            //Permitted_uses = new List<PermittedUse>();
            //Object_parts = new List<ObjectPartNumberRestrictions>();
            //Contours = new List<ContourOKSOut>();

            //RightRecords = new List<RightRecord>();
            //RestrictRecords = new List<RestrictRecord>();
        }


        #region Общие сведения об ОНИ TObject
        

        /// <summary>
        /// Уникальный идентификатор объекта
        /// </summary>       
        public string ID_Object { get; set; }

        public string ObjectNumber { get; set; }
        /// <summary>
        /// Дата модификации
        /// </summary>       
        public string MdfDate { get; set; }

        /// <summary>
        /// Имеются содольщики ФЛ.
        /// </summary>
        [DefaultValue(false)]
        [DetailView("Имеются содольщики ФЛ.")]
        public bool PersonHolder { get; set; }

        [FullTextSearchProperty]
        [DetailView(ReadOnly = true, Name = "Кадастровый номер", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CadastralNumber { get; set; }

        public bool IsConditional { get; set; }

        
        /// <summary>
        /// Целевое назначение (категория) земель
        /// </summary>        
        public string GroundCategory { get; set; }

        /// <summary>
        /// текстовое описание целевоего назначения(категории) земель
        /// </summary>     
        public string GroundCategoryText { get; set; }


        /// <summary>
        /// Значение площади
        /// </summary>       
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Площадь, в кв. метрах", TabName = TabName2)]
        [DefaultValue(0)]
        public decimal? Area { get; set; }

        /// <summary>
        /// Значение площади текстом
        /// </summary>       
        public string AreaText { get; set; }

        /// <summary>
        /// Единица измерений
        /// </summary>       
        public string UnitCode { get; set; }

        public string Unit { get; set; }

        /// <summary>
        /// Инвентарный номер, литер
        /// </summary>      
        public string Inv_No { get; set; }

        /// <summary>
        /// Этажность (этаж)
        /// </summary>        
        public string Floor { get; set; }

        /// <summary>
        /// Номера на поэтажном плане
        /// </summary>       
        public string FloorPlan_No { get; set; }

        public string ID_Address { get; set; }
        /// <summary>
        /// Адрес
        /// </summary>       
        public string Address { get; set; }
        
      

        /// <summary>
        /// Дата ликвидации объекта
        /// </summary>       
        public string ReEndDate { get; set; }
        #endregion



        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Выписки", Visible = false)]       
        public int? ExtractID { get; set; }

        
        [DetailView(ReadOnly = true, Name="Выписка", Visible = false)]       
        public virtual Extract Extract { get; set; }

        [PropertyDataType(PropertyDataType.ExtraId)]
        public string ExtraID { get; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Кадастрового объекта", Visible = false)]       
        public int? CadastralID { get; set; }

       
        [DetailView(ReadOnly = true, Name="Кадастровый объект АИС КС", Visible = false)]
        public virtual Cadastral Cadastral { get; set; }


      

        #region Сведения об объекте недвижимости - здании BuildRecordBaseParams

        /// <summary>
        /// Дата постановки на учет
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Дата постановки на учет", TabName = TabName2)]
        public System.DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// Дата снятия с учета
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Дата снятия с учета", TabName = TabName2)]
        public System.DateTime? CancelDate { get; set; }




        /// <summary>
        /// Номер кадастрового квартала
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Номер кадастрового квартала", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Quarter_cad_number { get; set; }

        /// <summary>
        /// Код вида объекта недвижимости
        /// </summary> 
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Код вида объекта недвижимости", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string TypeCode { get; set; }

        /// <summary>
        /// Наименование вида недвижимости
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Наименование вида недвижимости", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string TypeValue { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Вид недвижимости", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string TypeStr { get; set; }

        ///// <summary>
        ///// Кадастровые номера иных объектов недвижимости (земельных участков), в пределах которых расположен объект недвижимости
        ///// </summary>       
        //public virtual ICollection<CadNumber> Land_cad_numbers { get; set; }

        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Кадастровые номера ЗУ", TabName = TabName2
           , Description = "Кадастровые номера иных объектов недвижимости (земельных участков), в пределах которых расположен объект недвижимости")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Land_cad_numbersStr { get; set; }

        ///// <summary>
        ///// Кадастровые номера помещений, расположенных в объекте недвижимости
        ///// </summary>       
        //public virtual ICollection<CadNumber> Room_cad_numbers { get; set; }

        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Кадастровые номера помещений", TabName = TabName2, Description = "Кадастровые номера помещений, расположенных в объекте недвижимости")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Room_cad_numbersStr { get; set; }

        ///// <summary>
        ///// Кадастровые номера машино-мест, расположенных в объекте недвижимости
        ///// </summary>       
        //public virtual ICollection<CadNumber> Car_parking_space_cad_numbers { get; set; }

        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Кадастровые номера машино-мест", TabName = TabName2, Description = "Кадастровые номера машино-мест, расположенных в объекте недвижимости")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Car_parking_space_cad_numbersStr { get; set; }

        ///// <summary>
        ///// Ранее присвоенные номера
        ///// </summary>
        //public virtual ICollection<OldNumber> Old_numbers { get; set; }

        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Ранее присвоенные номера", TabName = TabName2, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Old_numbersStr { get; set; }


        #region Характеристики здания ParamsBuildWithoutMaterials       

        /// <summary>
        /// Погрешность площади
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Погрешность площади", TabName = TabName2, Visible = false)]
        [DefaultValue(0)]
        public decimal? Inaccuracy { get; set; }

        /// <summary>
        /// Количество этажей (в том числе подземных)
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Количество этажей (в том числе подземных)", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Floors { get; set; }
        /// <summary>
        /// Количество подземных этажей
        /// </summary> 
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Количество подземных этажей", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Underground_floors { get; set; }

        /// <summary>
        /// Код назначения здания
        /// </summary>  
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Код назначения здания", TabName = TabName2, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PurposeCode { get; set; }

        /// <summary>
        /// Наименование назначения здания
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Наименование назначения здания", TabName = TabName2, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PurposeName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Назначение здания", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PurposeStr { get; set; }

        /// <summary>
        /// Наименование здания
        /// </summary>   
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Наименование", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }

        /// <summary>
        /// Год завершения строительства
        /// </summary>  
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Год завершения строительства", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Year_built { get; set; }

        /// <summary>
        /// Год ввода в эксплуатацию по завершении строительства
        /// </summary> 
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Год ввода в эксплуатацию по завершении строительства", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Year_commisioning { get; set; }

        ///// <summary>
        ///// Вид(ы) разрешенного использования
        ///// </summary>       
        //public virtual ICollection<PermittedUse> Permitted_uses { get; set; }

        /// <summary>
        /// Вид(ы) разрешенного использования строкой.
        /// </summary>  
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Вид(ы) разрешенного использования", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Permitted_usesStr { get; set; }

        #endregion


        #region Адрес (местоположение) AddressLocationBuild
        /// <summary>
        /// Тип адреса
        /// </summary>  
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Код типа адреса", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Address_typeCode { get; set; }
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Наименование типа адреса", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Address_typeName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Тип адреса", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Address_typeStr { get; set; }

        #region AddressMain
        #region Адрес (по справочнику ФИАС)    
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Адрес (по справочнику ФИАС)", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressFias { get; set; }
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Адрес - ОКАТО", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressOkato { get; set; }
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Адрес - КЛАДР", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressKladr { get; set; }
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Адрес -  ОКТМО", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressOktmo { get; set; }
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Почтовый индекс", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressPostal_code { get; set; }
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Код региона", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegionCode { get; set; }
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Регион", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegionName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Регион", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegionStr { get; set; }


        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Тип района", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressDistrictType { get; set; }
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Район", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressDistrictName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Район", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressDistrictStr { get; set; }

        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Тип муниципального образования", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressCityType { get; set; }
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Муниципальное образование", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressCityName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Муниципальное образование", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressCityStr { get; set; }

        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Тип городского района", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressUrban_districtType { get; set; }
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Городской район", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressUrban_districtName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Городской район", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressUrban_districtStr { get; set; }

        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Тип сельсовета", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressSoviet_villageType { get; set; }
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Сельсовет", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressSoviet_villageName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Сельсовет", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressSoviet_villageStr { get; set; }

        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Тип населенного пункта", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressLocalityType { get; set; }
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Населенный пункт", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressLocalityName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Населенный пункт", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressLocalityStr { get; set; }

        /// <summary>
        /// Тип улицы
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Тип улицы", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressStreetType { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Улица", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressStreetName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Улица", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressStreetStr { get; set; }

        /// <summary>
        /// тип дома
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Тип дома", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressLevel1Type { get; set; }
        /// <summary>
        /// Дом
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Дом", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressLevel1Name { get; set; }


        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Дом", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressLevel1Str { get; set; }

        /// <summary>
        /// тип корпуса
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Тип корпуса", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressLevel2Type { get; set; }

        /// <summary>
        /// Корпус
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Корпус", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressLevel2Name { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Корпус", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressLevel2Str { get; set; }

        /// <summary>
        /// Тип строения
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Тип строения", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressLevel3Type { get; set; }

        /// <summary>
        /// Строение
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Строение", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressLevel3Name { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Строение", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressLevel3Str { get; set; }

        /// <summary>
        /// Тип квартиры
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Тип квартиры", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressApartmentType { get; set; }

        /// <summary>
        /// Квартира
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Квартира", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressApartmentName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Квартира", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressApartmentStr { get; set; }

        /// <summary>
        /// Иное описание местоположения
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Иное описание местоположения", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string AddressOther { get; set; }

        #endregion

        /// <summary>
        /// Неформализованное описание
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Неформализованное описание", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Note { get; set; }

        /// <summary>
        /// Адрес в соответствии с ФИАС (Текст)
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Адрес в соответствии с ФИАС (Текст)", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Readable_address { get; set; }
        #endregion


        #region Местоположение ОКС LocationOks
        /// <summary>
        /// OKATO
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "ОКС - ОКАТО", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string LocationOksOkato { get; set; }
        /// <summary>
        /// OKTMO
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "ОКС - ОКТМО", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string LocationOksOktmo { get; set; }
        /// <summary>
        /// Код региона
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "ОКС - код региона", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string LocationOksRegionCode { get; set; }
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "ОКС - регион", TabName = TabName3, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string LocationOksRegionName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "ОКС - регион", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string LocationOksRegionStr { get; set; }

        /// <summary>
        /// Описание местоположения
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Описание местоположения", TabName = TabName3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string LocationOksPosition_description { get; set; }

        #endregion



        #endregion

        /// <summary>
        /// Кадастровая стоимость
        /// </summary>      
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Кадастровая стоимость", TabName = TabName2)]
        public decimal? Cost { get; set; }

        ///// <summary>
        ///// Сведения о частях здания
        ///// </summary>       
        ////public virtual ICollection<ObjectPartNumberRestrictions> Object_parts { get; set; }

        ///// <summary>
        ///// Описание местоположения контура здания
        ///// </summary>        
        ////public virtual ICollection<ContourOKSOut> Contours { get; set; }

        /// <summary>
        /// Особые отметки
        /// </summary>       
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name = "Особые отметки", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Special_notes { get; set; }

        #endregion

        #region Сведения о ЗУ LandRecordBaseParams 

        /// <summary>
        /// Вид земельного участка
        /// </summary>     
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Код вида ЗУ", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SubtypeCode { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Вид ЗУ", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SubtypeName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Вид ЗУ", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Subtype { get; set; }

        /// <summary>
        /// Сведения о наличии решения об изъятии объекта недвижимости для государственных и муниципальных нужд
        /// </summary>      
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Сведения об изъятии", TabName = TabName22, Visible = false
            , Description = "Сведения о наличии решения об изъятии объекта недвижимости для государственных и муниципальных нужд")]
        public DocumentRecord Date_removed_cad_account { get; set; }

        /// <summary>
        /// Дата постановки по документу
        /// </summary>       
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Дата постановки по документу", TabName = TabName22, Visible = false)]
        public System.DateTime? Reg_date_by_doc { get; set; }



        /// <summary>
        /// Вид категории
        /// </summary>       
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Код категории ЗУ", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CategoryCode { get; set; }
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Вид ЗУ", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CategoryName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Вид ЗУ", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Category { get; set; }

        /// <summary>
        /// вид разрешенного использования По документу
        /// </summary>       
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Вид разрешенного использования по документу", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PermittedBy_document { get; set; }
        /// <summary>
        /// Вид разрешенного использования земельного участка в соответствии с ранее использовавшимся классификатором
        /// </summary>       
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Код вида разрешенного использования (старый)", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PermittedLand_useCode { get; set; }
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Вид разрешенного использования (старый)", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PermittedLand_useName { get; set; }
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Вид разрешенного использования (старый)", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PermittedLand_use { get; set; }
        /// <summary>
        /// Вид разрешенного использования земельного участка в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540
        /// </summary>       
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Код вида разрешенного использования (приказ 540 от 01.09.2014)", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PermittedLand_use_merCode { get; set; }
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Вид разрешенного использования (приказ 540 от 01.09.2014)", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PermittedLand_use_merName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Вид разрешенного использования (приказ 540 от 01.09.2014)", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PermittedLand_use_mer { get; set; }

        /// <summary>
        /// Реестровый номер границы  градостроительному регламенту
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Реестровый номер границы", TabName = TabName22, Description = "Вид разрешенного использования по градостроительному регламенту", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Permittes_Grad_Reg_numb_border { get; set; }
        /// <summary>
        /// Вид разрешенного использования в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Код вида использования по градостроительному регламенту", TabName = TabName22, Visible = false
            , Description = "Вид разрешенного использования в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Permittes_Grad_Land_useCode { get; set; }
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Вид использования по градостроительному регламенту", TabName = TabName22, Visible = false
           , Description = "Вид разрешенного использования в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Permittes_Grad_Land_useName { get; set; }
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Вид использования по градостроительному регламенту", TabName = TabName22, Visible = false
          , Description = "Вид разрешенного использования в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Permittes_Grad_Land_use { get; set; }
        /// <summary>
        /// Разрешенное использование (текстовое описание)
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Разрешенное использование", TabName = TabName22, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Permittes_Grad_use_text { get; set; }




        #region Местоположение относительно ориентира LocationByARefPoint
        /// <summary>
        /// Ориентир расположен в границах участка
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = " Ориентир расположен в границах участка", TabName = TabName3, Visible = false)]
        public bool In_boundaries_mark { get; set; } = false;
        /// <summary>
        /// Наименование ориентира
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Наименование ориентира", TabName = TabName3, Visible = false)]
        public string Ref_point_name { get; set; }
        /// <summary>
        /// Расположение относительно ориентира
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Расположение относительно ориентира", TabName = TabName3, Visible = false)]
        public string Location_description { get; set; }
        #endregion

        #endregion

        #region  Сведения о праве (бесхозяйное имущество) OwnerlessRightRecordOut

        /// <summary>
        /// Дата регистрации
        /// </summary>       
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name= "Дата регистрации", TabName = TabName5)]
        public System.DateTime? OwnerlessRightRecordRegDate { get; set; }

        /// <summary>
        /// Номер регистрации
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name= "Номер регистрации", TabName = TabName5)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Ownerless_right_number { get; set; }

        /// <summary>
        /// Наименование органа местного самоуправления (органа государственной власти - для городов федерального значения Москвы, Санкт-Петербурга, Севастополя), представившего заявление о постановке на учет данного объекта недвижимости в качестве бесхозяйного
        /// </summary>
        [ListView(Hidden=true)]
        [DetailView(ReadOnly = true, Name= "Наименование органа", TabName = TabName5,
        Description = "Наименование органа местного самоуправления (органа государственной власти - для городов федерального значения Москвы, Санкт-Петербурга, Севастополя), представившего заявление о постановке на учет данного объекта недвижимости в качестве бесхозяйного")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Authority_name { get; set; }

        #endregion

       

        ///// <summary>
        ///// Местоположение помещений в объекте недвижимости (план(ы) расположения помещения)
        ///// </summary>
        //public virtual ICollection<RoomLocationInBuildPlans> Room_records { get; set; }

        ///// <summary>
        ///// Местоположение машино-мест в объекте недвижимости (план(ы) расположения машино-места)
        ///// </summary>
        //public virtual ICollection<CarParkingSpaceLocationInBuildPlans> Car_parking_space_records { get; set; }

        

        ///// <summary>
        ///// Сведения о правах
        ///// </summary>
        //public virtual ICollection<RightRecord> RightRecords { get; set; }

        ///// <summary>
        ///// Сведения об ограничениях
        ///// </summary>
        //public virtual ICollection<RestrictRecord> RestrictRecords { get; set; }

    }
}

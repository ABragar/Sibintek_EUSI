using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Выписка из ЕГРН об объекте недвижимого имущества.
    /// </summary>
    [EnableFullTextSearch]
    public class ExtractObject : Extract
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ExtractObject.
        /// </summary>
        public ExtractObject() : base ()
        {
            //Land_cad_numbers = new List<CadNumber>();
            //Room_cad_numbers = new List<CadNumber>();
            //Car_parking_space_cad_numbers = new List<CadNumber>();
            //Old_numbers = new List<OldNumber>();
            //Permitted_uses = new List<PermittedUse>();
            //Object_parts = new List<ObjectPartNumberRestrictions>();
            //Contours = new List<ContourOKSOut>();
            //Room_records = new List<RoomLocationInBuildPlans>();
            //Car_parking_space_records = new List<CarParkingSpaceLocationInBuildPlans>();
            //Deal_records = new List<DealRecord>();
        }

        /// <summary>
        /// Получает или задает статус обновления записи в АИС КС.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Статус обновления в АИС КС", Visible = false)]
        public StatusCorpProp UpdateCPStatus { get; set; }

        /// <summary>
        /// Получает или задает статус обновления записи в АИС КС.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView("Дата обновления в АИС КС", Visible = false)]
        public DateTime? UpdateCPDateTime { get; set; }

        ///// <summary>
        ///// Местоположение помещений в объекте недвижимости (план(ы) расположения помещения)
        ///// </summary>
        //public virtual ICollection<RoomLocationInBuildPlans> Room_records { get; set; }

        ///// <summary>
        ///// Местоположение машино-мест в объекте недвижимости (план(ы) расположения машино-места)
        ///// </summary>
        //public virtual ICollection<CarParkingSpaceLocationInBuildPlans> Car_parking_space_records { get; set; }


        #region  Сведения о праве (бесхозяйное имущество) OwnerlessRightRecordOut

        /// <summary>
        /// Дата регистрации
        /// </summary>       
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Дата регистрации", TabName = TabName5)]
        public System.DateTime? OwnerlessRightRecordRegDate { get; set; }

        /// <summary>
        /// Номер регистрации
        /// </summary>
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Номер регистрации", TabName = TabName5)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Ownerless_right_number { get; set; }

        /// <summary>
        /// Наименование органа местного самоуправления (органа государственной власти - для городов федерального значения Москвы, Санкт-Петербурга, Севастополя), представившего заявление о постановке на учет данного объекта недвижимости в качестве бесхозяйного
        /// </summary>
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Наименование органа", TabName = TabName5,
        //Description = "Наименование органа местного самоуправления (органа государственной власти - для городов федерального значения Москвы, Санкт-Петербурга, Севастополя), представившего заявление о постановке на учет данного объекта недвижимости в качестве бесхозяйного")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Authority_name { get; set; }

        #endregion

        ///// <summary>
        ///// Сведения о сделках, совершенных без необходимого в силу закона согласия третьего лица, органа
        ///// </summary>
        //public virtual ICollection<DealRecord> Deal_records { get; set; }



        #region Сведения об объекте недвижимости - здании BuildRecordBaseParams

        /// <summary>
        /// Дата постановки на учет
        /// </summary>
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Дата постановки на учет", TabName = TabName2)]
        public System.DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// Дата снятия с учета
        /// </summary>
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Дата снятия с учета", TabName = TabName2)]
        public System.DateTime? CancelDate { get; set; }


        /// <summary>
        /// Кадастровый номер
        /// </summary>
        //[ListView]
        [FullTextSearchProperty]
        //[DetailView(ReadOnly = true, Name = "Кадастровый номер", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string CadNumber { get; set; }

        /// <summary>
        /// Номер кадастрового квартала
        /// </summary>
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Номер кадастрового квартала", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Quarter_cad_number { get; set; }

        /// <summary>
        /// Код вида объекта недвижимости
        /// </summary> 
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Код вида объекта недвижимости", TabName = TabName2, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string TypeCode { get; set; }

        /// <summary>
        /// Наименование вида недвижимости
        /// </summary>
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Наименование вида недвижимости", TabName = TabName2, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string TypeValue { get; set; }

        //[ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Вид недвижимости", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string TypeStr { get; set; }

        ///// <summary>
        ///// Кадастровые номера иных объектов недвижимости (земельных участков), в пределах которых расположен объект недвижимости
        ///// </summary>       
        //public virtual ICollection<CadNumber> Land_cad_numbers { get; set; }

        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Кадастровые номера ЗУ", TabName = TabName2
        //   , Description = "Кадастровые номера иных объектов недвижимости (земельных участков), в пределах которых расположен объект недвижимости")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Land_cad_numbersStr { get; set; }

        ///// <summary>
        ///// Кадастровые номера помещений, расположенных в объекте недвижимости
        ///// </summary>       
        //public virtual ICollection<CadNumber> Room_cad_numbers { get; set; }

        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Кадастровые номера помещений", TabName = TabName2, Description = "Кадастровые номера помещений, расположенных в объекте недвижимости")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Room_cad_numbersStr { get; set; }

        ///// <summary>
        ///// Кадастровые номера машино-мест, расположенных в объекте недвижимости
        ///// </summary>       
        //public virtual ICollection<CadNumber> Car_parking_space_cad_numbers { get; set; }

        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Кадастровые номера машино-мест", TabName = TabName2, Description = "Кадастровые номера машино-мест, расположенных в объекте недвижимости")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Car_parking_space_cad_numbersStr { get; set; }

        ///// <summary>
        ///// Ранее присвоенные номера
        ///// </summary>
        //public virtual ICollection<OldNumber> Old_numbers { get; set; }

        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Ранее присвоенные номера", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Old_numbersStr { get; set; }

        public string Address { get; set; }

        #region Характеристики здания ParamsBuildWithoutMaterials
        /// <summary>
        /// Площадь, в кв. метрах
        /// </summary>      
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Площадь, в кв. метрах", TabName = TabName2)]
        public decimal? Area { get; set; }

        /// <summary>
        /// Погрешность площади
        /// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Погрешность площади", TabName = TabName2, Visible = false)]
        public decimal? Inaccuracy { get; set; }

        /// <summary>
        /// Количество этажей (в том числе подземных)
        /// </summary>
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Количество этажей (в том числе подземных)", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Floors { get; set; }
        /// <summary>
        /// Количество подземных этажей
        /// </summary> 
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Количество подземных этажей", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Underground_floors { get; set; }

        /// <summary>
        /// Код назначения здания
        /// </summary>  
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Код назначения здания", TabName = TabName2, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PurposeCode { get; set; }

        /// <summary>
        /// Наименование назначения здания
        /// </summary>
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Наименование назначения здания", TabName = TabName2, Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PurposeName { get; set; }

        //[ListView(Hidden = true)]
        //[DetailView(ReadOnly = true, Name = "Назначение здания", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string PurposeStr { get; set; }

        /// <summary>
        /// Наименование здания
        /// </summary>   
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Наименование ОНи", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ObjectName { get; set; }

        /// <summary>
        /// Год завершения строительства
        /// </summary>  
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Год завершения строительства", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Year_built { get; set; }

        /// <summary>
        /// Год ввода в эксплуатацию по завершении строительства
        /// </summary> 
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Год ввода в эксплуатацию по завершении строительства", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Year_commisioning { get; set; }

        ///// <summary>
        ///// Вид(ы) разрешенного использования
        ///// </summary>       
        //public virtual ICollection<PermittedUse> Permitted_uses { get; set; }

        /// <summary>
        /// Вид(ы) разрешенного использования строкой.
        /// </summary>  
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Вид(ы) разрешенного использования", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Permitted_usesStr { get; set; }

        #endregion     

        /// <summary>
        /// Кадастровая стоимость
        /// </summary>      
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Кадастровая стоимость", TabName = TabName2)]
        [DefaultValue(0)]
        public decimal? Cost { get; set; }

        ///// <summary>
        ///// Сведения о частях здания
        ///// </summary>       
        //public virtual ICollection<ObjectPartNumberRestrictions> Object_parts { get; set; }

        ///// <summary>
        ///// Описание местоположения контура здания
        ///// </summary>        
        //public virtual ICollection<ContourOKSOut> Contours { get; set; }

        /// <summary>
        /// Особые отметки
        /// </summary>       
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Особые отметки", TabName = TabName2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Special_notes { get; set; }

        #endregion


        /// <summary>
        /// Статус записи об объекте недвижимости
        /// </summary>  
        //[ListView]
        //[DetailView(ReadOnly = true, Name = "Статус записи об объекте недвижимости", TabName = TabName1)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Status { get; set; }
    }
}

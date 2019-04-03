using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Law
{
    /// <summary>
    /// Представляет "непрекращенное" право на кадастровый объект.
    /// </summary>
    /// <remarks>
    /// Используется в отчете - контроль задвоения прав.
    /// Права на кадастровые объекты, периоды действия которых пересекаются, т.е. 
    /// - имеется более одного права на кадастровый объект с открытой датой окончания(долевые права не рассматриваются, где доля  <> 1);
    /// - дата регистрации права на объект входит в диапазон дат иных прав на тот же объект (долевые права не рассматриваются, где доля  <> 1);
    /// </remarks>

    [EnableFullTextSearch]
    public class DuplicateRightView : TypeObject
    {
        protected const string TabName1 = "[0]Общая информация";
        protected const string TabName2 = "[1]Право";
        protected const string TabName3 = "[2]Ограничения/обременения";

        /// <summary>
        /// Инициализирует новый экземпляр класса DuplicateRight.
        /// </summary>
        public DuplicateRightView()
        {

        }

        public int? SocietyID { get; set; }

        /// <summary>
        /// Получает или задает общество группы.
        /// </summary>       
        [ListView(Order = 1)]
        [DetailView(Name = "Правообладатель", TabName = TabName1, Order = 1)]
        public virtual Society Society { get; set; }


        /// <summary>
        /// Получает или задает кадастровый и/или условный номер объекта.
        /// </summary>
        [ListView(Order = 2)]
        [FullTextSearchProperty]
        [DetailView(Name = "Кадастровый (условный) номер", TabName = TabName1, ReadOnly = true, Order = 2)]
        [PropertyDataType(PropertyDataType.Text)]
        public String CadastralNumber { get; set; }

        /// <summary>
        /// Получает или задает Код вида ОН.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Код вида ОН", TabName = TabName1, ReadOnly = true, Order = 3)]
        [PropertyDataType(PropertyDataType.Text)]
        public String RealEstateKindCode { get; set; }

        /// <summary>
        /// Получает или задает ИД вида ОНИ.
        /// </summary>
        public int? RealEstateKindID { get; set; }

        /// <summary>
        /// Получает или задает вид объекта недвижимого имущества.
        /// </summary>        
        [ListView(Order = 7)]
        [DetailView(Name = "Вид ОН", ReadOnly = true, Order = 4,
        TabName = TabName1)]
        public virtual RealEstateKind RealEstateKind { get; set; }

        /// <summary>
        /// Получает или задает код назначения.
        /// </summary>

        [ListView(Hidden = true)]
        [DetailView(Name = "Назначение (код)", TabName = TabName1, ReadOnly = true, Order = 5)]
        [PropertyDataType(PropertyDataType.Text)]
        public String AppointmentsCode { get; set; }

        /// <summary>
        /// Получает или задает назначение.
        /// </summary>

        [ListView(Order = 8)]
        [DetailView(Name = "Назначение", TabName = TabName1, ReadOnly = true, Order = 6)]
        [PropertyDataType(PropertyDataType.Text)]
        public String Appointments { get; set; }



        /// <summary>
        /// Получает или задает наименование объекта.
        /// </summary>
        [ListView(Order = 9)]
        [DetailView(Name = "Наименование", TabName = TabName1, ReadOnly = true, Order = 7)]
        [PropertyDataType(PropertyDataType.Text)]
        public String ObjectName { get; set; }


        /// <summary>
        /// Плучает или задает код вида разрешенного использования.
        /// </summary>        
        [ListView(Hidden = true)]
        [DetailView(Name = "Код видов разрешенного использования", TabName = TabName1, ReadOnly = true, Order = 8)]
        [PropertyDataType(PropertyDataType.Text)]
        public String UsesKindCode { get; set; }

        /// <summary>
        /// Плучает или задает виды разрешенного использования.
        /// </summary>        
        [ListView(Order = 10)]
        [DetailView(Name = "Виды разрешенного использования", TabName = TabName1, ReadOnly = true, Order = 9)]
        [PropertyDataType(PropertyDataType.Text)]
        public String UsesKind { get; set; }


        [ListView(Hidden = true)]
        [DetailView(Name = "Текстовое описание площади", TabName = TabName1, ReadOnly = true, Order = 10)]
        [PropertyDataType(PropertyDataType.Text)]
        public String AreaText { get; set; }

        /// <summary>
        /// Получает или задает площадь.
        /// </summary>        
        [ListView(Order = 11)]
        [DetailView(Name = "Площадь", TabName = TabName1, ReadOnly = true, Order = 11)]
        public decimal? Area { get; set; }


        /// <summary>
        /// Получает или задает единицу измерения площади.
        /// </summary>       
        [ListView(Order = 12)]
        [DetailView(Name = "Единица измерения площади", TabName = TabName1, ReadOnly = true, Order = 12)]
        public string AreaUnit { get; set; }

        //[ListView(Hidden = true)]
        //[DetailView(Name = "Налог с кад.стоимости", TabName = TabName1, ReadOnly = true, Order = 13)]
        //[DefaultValue(false)]
        //public bool IsTaxCadastral { get; set; }

        [ListView(Order = 13)]
        [DetailView(Name = "Адрес (описание)", TabName = TabName1, ReadOnly = true, Order = 14)]
        [PropertyDataType(PropertyDataType.Text)]
        public String Address { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Идентификатор адреса", TabName = TabName1, ReadOnly = true, Order = 15)]
        [PropertyDataType(PropertyDataType.Text)]
        public String AddressID { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Код региона", TabName = TabName1, ReadOnly = true, Order = 16)]
        [PropertyDataType(PropertyDataType.Text)]
        public String RegionCode { get; set; }

        public int? RegionID { get; set; }

        [ListView(Order = 14)]
        [DetailView(Name = "Субъект РФ", TabName = TabName1, ReadOnly = true, Order = 17)]
        public SibRegion Region { get; set; }


        [ListView(Hidden = true)]
        [DetailView(Name = "Код региона по ОКАТО", TabName = TabName1, ReadOnly = true, Order = 18)]
        [PropertyDataType(PropertyDataType.Text)]
        public String RegionCodeOKATO { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Код региона по КЛАДР", TabName = TabName1, ReadOnly = true, Order = 19)]
        [PropertyDataType(PropertyDataType.Text)]
        public String RegionCodeKLADR { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Район", TabName = TabName1, ReadOnly = true, Order = 20)]
        [PropertyDataType(PropertyDataType.Text)]
        public String District { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Город", TabName = TabName1, ReadOnly = true, Order = 21)]
        [PropertyDataType(PropertyDataType.Text)]
        public String City { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Поселок", TabName = TabName1, ReadOnly = true, Order = 22)]
        [PropertyDataType(PropertyDataType.Text)]
        public String Place { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Улица", TabName = TabName1, ReadOnly = true, Order = 23)]
        [PropertyDataType(PropertyDataType.Text)]
        public String Street { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Дом", TabName = TabName1, ReadOnly = true, Order = 24)]
        [PropertyDataType(PropertyDataType.Text)]
        public String House { get; set; }

        /// <summary>
        /// Получает или задает ИД вида права.
        /// </summary>
        public int? RightKindID { get; set; }

        /// <summary>
        /// Получает или задает вид права.
        /// </summary>      
        [ListView(Hidden = true)]
        [DetailView(Name = "Код вида права", TabName = TabName2, Required = true, Order = 25)]
        public virtual RightKind RightKind { get; set; }


        [ListView(Order = 5)]
        [DetailView(Name = "Вид права, доля в праве", TabName = TabName2, Visible = false, Order = 26)]
        [PropertyDataType(PropertyDataType.Text)]
        public String KindAndShare { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Доля в праве", TabName = TabName2, Visible = false, Order = 27)]
        [PropertyDataType(PropertyDataType.Text)]
        public string ShareText { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Числитель/Знаменатель доли", TabName = TabName2, Visible = true, Order = 28)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Share { get; set; }

        /// <summary>
        /// Получает или задает числитель доли.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Числитель", TabName = TabName2, Order = 29)]
        [DefaultValue(1)]
        public int ShareRightNumerator { get; set; }

        /// <summary>
        /// Получает или задает знаменатель доли.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Знаменатель", TabName = TabName2, Order = 30)]
        [DefaultValue(1)]
        public int ShareRightDenominator { get; set; }

        /// <summary>
        /// Получает или задает дату гос.регистрации.
        /// </summary>
        [ListView(Order = 4)]
        [DetailView(Name = "Дата гос.регистрации", TabName = TabName2, Order = 31)]
        public DateTime? RegDate { get; set; }

        /// <summary>
        /// Получает или задает номер.
        /// </summary>
        [ListView(Order = 3)]
        [FullTextSearchProperty]
        [DetailView(Name = "Номер гос. регистрации", TabName = TabName2, Order = 32)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegNumber { get; set; }

        /// <summary>
        /// Получает или задает ИД типа права.
        /// </summary>
        public int? RightTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип записи о регистрации.
        /// </summary>       
        [DetailView(Name = "Тип записи о регистрации", TabName = TabName2, Order = 33)]
        public RightType RightType { get; set; }

        [ListView(Order = 6)]
        [DetailView(Name = "Дата гос.регистрации прекращения права", TabName = TabName2, Order = 34)]
        public DateTime? RegDateEnd { get; set; }


        [DetailView(Name = "Сведения об изъятии", TabName = TabName2, Order = 35
            , Description = "Сведения о наличии решения об изъятии объекта недвижимости для государственных и муниципальных нужд")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Confiscation { get; set; }


        [ListView(Hidden = true)]
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Name = "Форма собственности", TabName = TabName2, Order = 36)]
        public string OwnershipType { get; set; }


        [ListView(Hidden = true)]
        [DetailView(Name = "Вид основания для правообладания", TabName = TabName2, Order = 37)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RightHolderKind { get; set; }


        [ListView(Hidden = true)]
        //[DetailView(Name = "Наличие ограничения/обременения", TabName = TabName3, ReadOnly = true, Order = 36)]

        public bool EncumbrancesExist { get; set; }


        
        /// <summary>
        /// Получает или задает Вид, номер, дата.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Вид, номер, дата", TabName = TabName1, Visible = false, Order = 38)]
        [PropertyDataType(PropertyDataType.Text)]
        public String Title { get; set; }



        /// <summary>
        /// Получает или задает ИД объекта имущества.
        /// </summary>        
        public int? EstateID { get; set; }
               
        /// <summary>
        /// Получает или задает мнемонику объекта имущества
        /// </summary>            
        [DetailView(Name = "Мнемоника ОИ", Order = 10, TabName = CaptionHelper.DefaultTabName, Visible = false, ReadOnly = true)]
        public string EstateMnemonic { get; set; }

    }
}

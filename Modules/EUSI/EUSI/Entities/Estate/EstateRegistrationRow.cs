using Base.Attributes;
using Base.Translations;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Estate;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EUSI.Entities.Estate
{
    /// <summary>
    /// Представляет строку заявки на регистрацию
    /// </summary>
    public class EstateRegistrationRow : TypeObject
    {
        private static readonly CompiledExpression<EstateRegistrationRow, string> _EstateTypeStr =
         DefaultTranslationOf<EstateRegistrationRow>
            .Property(x => x.EstateTypeStr)
            .Is(x => (x.EstateDefinitionType != null) ? x.EstateDefinitionType.Name : "");

        /// <summary>
        /// Инициализирует новый экземпляр класса EstateRegistrationRow.
        /// </summary>
        public EstateRegistrationRow() : base()
        {
        }

        /// <summary>
        /// Получает наименование типа ОЗ.
        /// </summary>
        [SystemProperty]
        public string EstateTypeStr => _EstateTypeStr.Evaluate(this);

        /// <summary>
        /// Получает или задает номер позиции ОЗ.
        /// </summary>
        [DetailView("Номер позиции", Visible = false), ListView(Visible = false)]
        public int? Position { get; set; }

        /// <summary>
        /// Получает или задает дату получения объекта аренды (дата акта приемки-передачи).
        /// </summary>
        [ListView("Дата получения объекта аренды (дата акта приемки-передачи)", Visible = false)]
        [DetailView("Дата получения объекта аренды (дата акта приемки-передачи)", Visible = false)]
        public DateTime? ActRentDate { get; set; }

        /// <summary>
        /// Получает или задает адрес (строка).
        /// </summary>
        [DetailView("Адрес", Visible = false)]
        [ListView("Адрес", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string Address { get; set; }

        /// <summary>
        /// Получает или задает кадастровый номер.
        /// </summary>
        [DetailView("Кадастровый номер", Visible = false)]
        [ListView("Кадастровый номер", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Получает или задает ИД типа ОИ.
        /// </summary>
        [SystemProperty]
        public int? EstateDefinitionTypeID { get; set; }

        /// <summary>
        /// Получате или задает тип ОИ.
        /// </summary>
        [DetailView("Тип Объекта имущества", Visible = false)]
        [ListView("Тип Объекта имущества", Visible = false)]
        public virtual EstateDefinitionType EstateDefinitionType { get; set; }

        /// <summary>
        /// Получает или задает ИД заявки на регистрацию.
        /// </summary>
        [SystemProperty]        
        public int? EstateRegistrationID { get; set; }

        /// <summary>
        /// Получает или задает заявку на регистрацию.
        /// </summary>
        [ListView("Заявка на регистрацию", Visible = false)]
        [DetailView("Заявка на регистрацию", Visible = false)]        
        public EstateRegistration EstateRegistration { get; set; }

        /// <summary>
        /// Получает или задает Ид класса КС.
        /// </summary>
        [SystemProperty]
        public int? EstateTypeID { get; set; }

        /// <summary>
        /// Получает или задает класс КС.
        /// </summary>
        [DetailView("Класс КС", Visible = false)]
        [ListView("Класс КС", Visible = false)]
        public EstateType EstateType { get; set; }

        /// <summary>
        /// Получает или задает номер ЕУСИ.
        /// </summary>
        [DetailView("Номер ЕУСИ", Visible = false)]
        [ListView("Номер ЕУСИ", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string EUSINumber { get; set; }

        /// <summary>
        /// Получает или задает ИД вида НМА.
        /// </summary>
        [SystemProperty]
        public int? IntangibleAssetTypeID { get; set; }

        /// <summary>
        /// Получает или задает вид НМА.
        /// </summary>
        [DetailView("Вид НМА", Visible = false)]
        [ListView("Вид НМА", Visible = false)]
        public IntangibleAssetType IntangibleAssetType { get; set; }

        /// <summary>
        /// Получает или задает инвентарный номер арендодателя.
        /// </summary>
        [DetailView("Инвентарный номер арендодателя", Visible = false)]
        [ListView("Инвентарный номер арендодателя", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Получает или задает наименование объекта (в соответствии с документами).
        /// </summary>
        [DetailView("Наименование объекта (в соответствии с документами)", Visible = false)]
        [ListView("Наименование объекта (в соответствии с документами)", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string NameEstateByDoc { get; set; }

        /// <summary>
        /// Получает или задает наименование ЕУСИ.
        /// </summary>
        [DetailView("Наименование ЕУСИ", Visible = false)]
        [ListView("Наименование ЕУСИ", Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string NameEUSI { get; set; }

        /// <summary>
        /// Получает или задает тип строки заявки.
        /// </summary>
        [SystemProperty]
        public EstateRegistrationRowType RowType { get; set; }

        /// <summary>
        /// Получает или задает ИД города.
        /// </summary>
        [SystemProperty]
        public int? SibCityNSIID { get; set; }

        /// <summary>
        /// Получает или задает город.
        /// </summary>
        [DetailView("Населенный пункт", Visible = false)]
        [ListView("Населенный пункт", Visible = false)]
        public SibCityNSI SibCityNSI { get; set; }

        /// <summary>
        /// Получает или задает ИД страны.
        /// </summary>
        [SystemProperty]
        public int? SibCountryID { get; set; }

        /// <summary>
        /// Получает или задает страну.
        /// </summary>
        [DetailView("Страна", Visible = false)]
        [ListView("Страна", Visible = false)]
        public SibCountry SibCountry { get; set; }

        /// <summary>
        /// Получает или задает ИД федерального округа.
        /// </summary>
        [SystemProperty]
        public int? SibFederalDistrictID { get; set; }

        /// <summary>
        /// Получает или задает федеральный округ.
        /// </summary>
        [DetailView("Федеральный округ", Visible = false)]
        [ListView("Федеральный округ", Visible = false)]
        public SibFederalDistrict SibFederalDistrict { get; set; }

        /// <summary>
        /// Получает или задает ИД региона.
        /// </summary>
        [SystemProperty]
        public int? SibRegionID { get; set; }

        /// <summary>
        /// Получает или задает регион.
        /// </summary>
        [DetailView("Субъект РФ/Регион", Visible = false)]
        [ListView("Субъект РФ/Регион", Visible = false)]
        public SibRegion SibRegion { get; set; }

        /// <summary>
        /// Получает или задает дату начала использования (НКС).
        /// </summary>
        [ListView("Дата начала использования (НКС)", Visible = false)]
        [DetailView("Дата начала использования (НКС)", Visible = false)]
        public DateTime? StartDateUse { get; set; }

        /// <summary>
        /// Получает или задает ИД категории ТС.
        /// </summary>
        [SystemProperty]
        public int? VehicleCategoryID { get; set; }

        /// <summary>
        /// Получает или задает категорию ТС.
        /// </summary>
        [ListView("Категория ТС", Visible = false)]
        [DetailView("Категория ТС", Visible = false)]
        public VehicleCategory VehicleCategory { get; set; }

        /// <summary>
        /// Получает или задает наличие дизельного двигателя ТС.
        /// </summary>
        [ListView("Дизельный двигатель", Visible = false)]
        [DetailView("Дизельный двигатель", Visible = false)]
        [DefaultValue(false)]
        public bool VehicleDieselEngine { get; set; }

        /// <summary>
        /// Получает или задает объем двигателя ТС.
        /// </summary>
        [ListView("Объем двигателя", Visible = false)]
        [DetailView("Объем двигателя", Visible = false)]
        [DefaultValue(0)]
        public decimal? VehicleEngineSize { get; set; }

        /// <summary>
        /// Получает или задает ИД марки ТС.
        /// </summary>
        [SystemProperty]
        public int? VehicleModelID { get; set; }

        /// <summary>
        /// Получает или задает марку ТС.
        /// </summary>
        [ListView("Марка ТС", Visible = false)]
        [DetailView("Марка ТС", Visible = false)]
        public VehicleModel VehicleModel { get; set; }

        /// <summary>
        /// Получает или задает мощность ТС.
        /// </summary>
        [ListView("Мощность ТС", Visible = false)]
        [DetailView("Мощность ТС", Visible = false)]
        [DefaultValue(0)]
        public decimal? VehiclePower { get; set; }

        /// <summary>
        /// Получает или задает ИД единицы измерения мощности ТС.
        /// </summary>
        [SystemProperty]
        public int? VehiclePowerMeasureID { get; set; }

        /// <summary>
        /// Получает или задает единицы измерения мощности ТС.
        /// </summary>
        [ListView("Единицы измерения мощности ТС", Visible = false)]
        [DetailView("Единицы измерения мощности ТС", Visible = false)]
        [DefaultValue(false)]
        public SibMeasure VehiclePowerMeasure { get; set; }

        /// <summary>
        /// Получает или задает дату регистрации ТС.
        /// </summary>
        [ListView("Дата регистрации ТС в гос.органах", Visible = false)]
        [DetailView("Дата регистрации ТС в гос.органах", Visible = false)]
        public DateTime? VehicleRegDate { get; set; }

        /// <summary>
        /// Получает или задает номер госрегистрации ТС.
        /// </summary>
        [ListView("Номер госрегистрации ТС", Visible = false)]
        [DetailView("Номер госрегистрации ТС", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String VehicleRegNumber { get; set; }

        /// <summary>
        /// Получает или задает cерийный номер/Заводской номер/ВИН.
        /// </summary>
        [ListView("Серийный номер/Заводской номер/ВИН", Visible = false)]
        [DetailView("Серийный номер/Заводской номер/ВИН", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public String VehicleSerialNumber { get; set; }

        /// <summary>
        /// Получает или задает год выпуска ТС.
        /// </summary>
        [ListView("Год выпуска ТС", Visible = false)]
        [DetailView("Год выпуска ТС", Visible = false)]
        public int? VehicleYearOfIssue { get; set; }


        [InverseProperty("ClaimObject")]
        public ICollection<EstateRegistration> ClaimEstateRegistrations { get; set; }

        /// <summary>
        /// Получает или задает коментарий
        /// </summary>
        [DetailView(Name = "Комментарий", Visible = false)]
        [ListView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Comment { get; set; }
    }

    /// <summary>
    /// Тип строки заявки на регистрацию.
    /// </summary>
    [UiEnum]
    public enum EstateRegistrationRowType
    {
        [UiEnumValue("Основные средства(кроме аренды)")]
        OS = 10,

        [UiEnumValue("Нематериальные активы")]
        NMA = 20,

        [UiEnumValue("НКС")]
        NKS = 30,

        [UiEnumValue("Аренда ОС")]
        ArendaOS = 40,

        [UiEnumValue("Внутригрупповое перемещение")]
        VGP = 50,

        [UiEnumValue("Объединение")]
        Union = 60,

        [UiEnumValue("Разукрупнение")]
        Division = 70
    }
}
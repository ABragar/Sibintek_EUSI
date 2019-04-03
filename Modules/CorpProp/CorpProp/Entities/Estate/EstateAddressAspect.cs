using Base;
using Base.Attributes;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// from InventoryObject
    /// </summary>
    public partial class EstateAddressAspect : BaseObject
    {
        public EstateAddressAspect()
        {
        }

        public int AddressOfID { get; set; }
        public Estate AddressOf { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView("Адрес", Visible = false)]
        public string Address { get; set; }

        /// <summary>
        /// Получает наименование имущественного комплекса.
        /// </summary>
        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegionCode { get; set; }

        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegionName { get; set; }

        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegionStr { get; set; }
    }

    /// <summary>
    /// From Cadastral
    /// </summary>
    public partial class EstateAddressAspect
    {
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
    }
}
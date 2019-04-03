using Base;
using Base.Attributes;
using Base.EntityFrameworkTypes.Complex;

namespace CorpProp.Model.Partial.Entities.SeparLand
{
    public class SeparLandAddress9 : BaseObject, IChildrenModel
    {
        [SystemProperty]
        public int? ParentID { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string Address { get; set; }

        /// <summary>
        /// Получает или задает адрес
        /// </summary>     
        //[ListView(Order = 7)]
        //[DetailView(Name = "Адрес (местоположение)", TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        //[PropertyDataType(PropertyDataType.Text)]

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string AddressID { get; set; }

        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegionCode { get; set; }

        //[ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegionName { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string OKATORegionCode { get; set; }

        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string KLADRRegionCode { get; set; }

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

        [DetailView("Местоположение")]
        [ListView(Hidden = true)]
        [PropertyDataType(PropertyDataType.LocationPolygon)]
        public Location Location { get; set; } = new Location();
    }
}

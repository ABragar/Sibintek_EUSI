using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Выписка об основных характеристиках и зарегистрированных правах на объект недвижимости (сооружение)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("extract_base_params_construction", Namespace = "", IsNullable = false)]
    public class ExtractBaseParamsConstruction
    {
        public ExtractBaseParamsConstruction()
        {
            Room_records = new List<RoomLocationInBuildPlans>();
            Car_parking_space_records = new List<CarParkingSpaceLocationInBuildPlans>();
            Right_records = new List<RightRecordsBaseParamsRight_record>();
            Restrict_records = new List<RestrictRecordType>();
            Deal_records = new List<DealRecordType>();

        }

        /// <summary>
        /// Реквизиты выписки
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("details_statement")]
        public DetailsStatementRealty Details_statement { get; set; }
        /// <summary>
        /// Реквизиты поступившего запроса
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("details_request")]
        public DetailsRequest Details_request { get; set; }
        /// <summary>
        /// Сведения об объекте недвижимости - сооружении
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("construction_record")]
        public ConstructionRecordBaseParams Construction_record { get; set; }
        /// <summary>
        /// Местоположение помещений в объекте недвижимости (план(ы) расположения помещения)
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("room_record", IsNullable = false, ElementName = "room_records")]
        [System.Xml.Serialization.XmlArray("room_records")]
        [System.Xml.Serialization.XmlArrayItem("room_record", Type = typeof(RoomLocationInBuildPlans), IsNullable = false)]
        public List<RoomLocationInBuildPlans> Room_records { get; set; }
        /// <summary>
        /// Местоположение машино-мест в объекте недвижимости (план(ы) расположения машино-места)
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("car_parking_space_record", IsNullable = false, ElementName = "car_parking_space_records")]
        [System.Xml.Serialization.XmlArray("car_parking_space_records")]
        [System.Xml.Serialization.XmlArrayItem("car_parking_space_record", Type = typeof(CarParkingSpaceLocationInBuildPlans), IsNullable = false)]
        public List<CarParkingSpaceLocationInBuildPlans> Car_parking_space_records { get; set; }
        /// <summary>
        /// Сведения о правах и правообладателях
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("right_record", IsNullable = false, ElementName = "right_records")]
        [System.Xml.Serialization.XmlArray("right_records")]
        [System.Xml.Serialization.XmlArrayItem("right_record", Type = typeof(RightRecordsBaseParamsRight_record), IsNullable = false)]
        public List<RightRecordsBaseParamsRight_record> Right_records { get; set; }
        /// <summary>
        /// Ограничения прав и обременения объекта недвижимости
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("restrict_record", IsNullable = false, ElementName = "restrict_records")]
        [System.Xml.Serialization.XmlArray("restrict_records")]
        [System.Xml.Serialization.XmlArrayItem("restrict_record", Type = typeof(RestrictRecordType), IsNullable = false)]
        public List<RestrictRecordType> Restrict_records { get; set; }
        /// <summary>
        /// Сведения о праве (бесхозяйное имущество)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ownerless_right_record")]
        public OwnerlessRightRecordOut Ownerless_right_record { get; set; }
        /// <summary>
        /// Сведения о сделках, совершенных без необходимого в силу закона согласия третьего лица, органа
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("deal_record", IsNullable = false, ElementName = "deal_records")]
        [System.Xml.Serialization.XmlArray("deal_records")]
        [System.Xml.Serialization.XmlArrayItem("deal_record", Type = typeof(DealRecordType), IsNullable = false)]
        public List<DealRecordType> Deal_records { get; set; }
        /// <summary>
        /// Получатель выписки
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("recipient_statement")]
        public string Recipient_statement { get; set; }
        /// <summary>
        /// Статус записи об объекте недвижимости
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("status")]
        public string Status { get; set; }
        /// <summary>
        /// Глобальный уникальный идентификатор документа
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "guid")]
        public string Guid { get; set; }
    }
}

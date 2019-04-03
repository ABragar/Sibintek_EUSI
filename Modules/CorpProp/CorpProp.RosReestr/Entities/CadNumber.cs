using Base;
using Base.Attributes;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Кадастровый номер.
    /// </summary>
    public class CadNumber : BaseObject
    {
        public CadNumber() { }

        /// <summary>
        /// Кадастровый номер
        /// </summary>       
        [ListView]
        [FullTextSearchProperty]
        [DetailView(ReadOnly = true, Name = "Кадастровый номер")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Cad_number { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Visible = false)]
        public int? ObjectRecordLandID { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Visible = false)]        
        public ObjectRecord ObjectRecordLand { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Visible = false)]
        public int? ObjectRecordRoomID { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Visible = false)]       
        public ObjectRecord ObjectRecordRoom { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Visible = false)]
        public int? ObjectRecordCarParkingID { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Visible = false)]      
        public ObjectRecord ObjectRecordCarParking { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Visible = false)]
        public int? ExtractLandID { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Visible = false)]
        //[InverseProperty("Land_cad_numbers")]
        public Extract ExtractLand { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Visible = false)]
        public int? ExtractRoomID { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Visible = false)]
        //[InverseProperty("Room_cad_numbers")]
        public Extract ExtractRoom { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Visible = false)]
        public int? ExtractCarParkingID { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Visible = false)]
        //[InverseProperty("Car_parking_space_cad_numbers")]
        public Extract ExtractCarParking { get; set; }


    }
}

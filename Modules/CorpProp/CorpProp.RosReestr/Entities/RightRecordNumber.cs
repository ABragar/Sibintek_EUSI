using Base;
using Base.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Номер реестровой записи о вещном праве
    /// </summary>   
    public class RightRecordNumber : BaseObject
    {
        public RightRecordNumber() { }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД обременения", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public int? RestrictRecordID { get; set; }


       
        [DetailView(ReadOnly = true, Name="Обременение", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public RestrictRecord RestrictRecord { get; set; }


        /// <summary>
        /// Номер реестровой записи
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Номер реестровой записи")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Number { get; set; }

        /// <summary>
        /// Номер регистрации вещного права
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Номер регистрации вещного права")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Right_number { get; set; }
    }
}

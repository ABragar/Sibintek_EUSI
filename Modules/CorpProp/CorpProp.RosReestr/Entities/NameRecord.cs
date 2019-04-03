using Base;
using Base.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
   
    public class NameRecord : BaseObject
    {
        public NameRecord()
        {

        }

        /// <summary>
        /// Фамилия
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Фамилия")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Имя")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Отчество")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Patronymic { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="Полное имя")]
        [PropertyDataType(PropertyDataType.Text)]
        public string FullName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Сделки", Visible = false)]
        public int? DealRecordID { get; set; }
       

        [DetailView(ReadOnly = true, Name="Сделка", Visible = false)]
        public DealRecord DealRecord { get; set; }
    }
}

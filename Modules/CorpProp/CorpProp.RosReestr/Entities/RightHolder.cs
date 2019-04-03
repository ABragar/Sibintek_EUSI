using Base;
using Base.Attributes;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Правообладатель.
    /// </summary>
    public class RightHolder : BaseObject
    {
        public RightHolder() { }

        /// <summary>
        /// ИНН
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name= "ИНН")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Inn { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name= "ОГРН")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Ogrn { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name= "Наименование")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }
        /// <summary>
        /// Краткое наименование
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name= "Краткое наименование")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Short_name { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Права")]
        public int? RightRecordID { get; set; }


        /// <summary>
        /// Право
        /// </summary>     
        [DetailView(ReadOnly = true, Name="Право")]
        public RightRecord RightRecord { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Выписки")]
        public int? ExtractID { get; set; }


        /// <summary>
        /// Выписка
        /// </summary>     
        [DetailView(ReadOnly = true, Name="Выписка")]
        public Extract Extract { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Субъекта")]
        public int? SubjectRecordID { get; set; }


        /// <summary>
        /// Право
        /// </summary>     
        [DetailView(ReadOnly = true, Name="Субъект")]
        public SubjectRecord SubjectRecord { get; set; }
    }
}

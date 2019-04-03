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
    /// Вид разрешенного использования
    /// </summary>   
    public class PermittedUse : BaseObject
    {
        public PermittedUse()
        {

        }
        /// <summary>
        /// Наименование вида использования
        /// </summary>       
        [ListView]
        [DetailView(ReadOnly = true, Name="Наименование")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД ОНИ", Visible = false)]
        public int? ObjectRecordID { get; set; }


        [DetailView(ReadOnly = true, Name="ОНИ", Visible = false)]
        public ObjectRecord ObjectRecord { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Выписки", Visible = false)]
        public int? ExtractID { get; set; }


        [DetailView(ReadOnly = true, Name="Выписка", Visible = false)]
        public Extract Extract { get; set; }

    }
}

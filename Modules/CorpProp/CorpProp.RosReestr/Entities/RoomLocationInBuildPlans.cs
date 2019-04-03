using Base;
using Base.Attributes;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Местоположение помещения, расположенного в объекте недвижимости (планы расположения помещения)
    /// </summary>    
    public class RoomLocationInBuildPlans : BaseObject
    {
        public RoomLocationInBuildPlans() { }

        /// <summary>
        /// Общие сведения (кадастровый номер помещения)
        /// </summary>
        [ListView]
        [FullTextSearchProperty]
        [DetailView(ReadOnly = true, Name="Кадастровый номер помещения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ObjectCadNumber { get; set; }

        /// <summary>
        /// Номер этажа
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Номер этажа")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Floor { get; set; }
        /// <summary>
        /// Тип этажа
        /// </summary>       
        [ListView]
        [DetailView(ReadOnly = true, Name="Код типа этажа")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Floor_typeCode { get; set; }

        [ListView]
        [DetailView(ReadOnly = true, Name="Тип этажа")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Floor_typeName { get; set; }

        /// <summary>
        /// Номер на поэтажном плане
        /// </summary>       
        [ListView]
        [DetailView(ReadOnly = true, Name="Номер на поэтажном плане")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Plan_number { get; set; }
        /// <summary>
        /// Описание расположения
        /// </summary>        
        [ListView]
        [DetailView(ReadOnly = true, Name="Описание расположения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Description { get; set; }

        /// <summary>
        /// Планы
        /// </summary>       
        [ListView]
        [DetailView(ReadOnly = true, Name="План")]
        [PropertyDataType(PropertyDataType.Text)]
        public string File_link { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Выписки", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public int? ExtractID { get; set; }

      
        [DetailView(ReadOnly = true, Name="Выписка", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public Extract Extract { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД ОНИ", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public int? ObjectRecordID { get; set; }


     
        [DetailView(ReadOnly = true, Name="ОНИ", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public ObjectRecord ObjectRecord { get; set; }

    }
}

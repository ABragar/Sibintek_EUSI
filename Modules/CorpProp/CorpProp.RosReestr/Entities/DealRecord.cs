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
    /// Сведения о сделке, совершенной без необходимого в силу закона согласия третьего лица, органа
    /// </summary>
    public class DealRecord : BaseObject
    {
        public DealRecord()
        {
            //Dissenting_entities = new List<NameRecord>();
        }

        #region Общие сведения о сделке (вид сделки) DealDataType
        /// <summary>
        /// Вид сделки
        /// </summary>    
        [ListView]
        [DetailView(ReadOnly = true, Name="Код вида сделки")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Deal_typeCode { get; set; }

        [ListView]
        [DetailView(ReadOnly = true, Name="Вид сделки")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Deal_typeName { get; set; }

        #endregion


        #region Law
        /// <summary>
        /// Часть
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Закон - Часть")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Section { get; set; }

        /// <summary>
        /// Пункт
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Закон - Пункт")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Paragraph { get; set; }

        /// <summary>
        /// Статья
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Закон - Статья")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Article { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Закон - Дата")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Law_date { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Закон - Номер")]
        [PropertyDataType(PropertyDataType.Text)]
        public string LawNumber { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Закон - Наименование")]
        [PropertyDataType(PropertyDataType.Text)]
        public string LawName { get; set; }
        #endregion


        ///// <summary>
        ///// Не представлено согласие на совершение сделки
        ///// </summary>
        //public virtual ICollection<NameRecord> Dissenting_entities { get; set; }

        [ListView]
        [DetailView(ReadOnly = true, Name="Не представлено согласие на совершение сделки", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Dissenting_entitiesStr { get; set; }

        /// <summary>
        /// Содержание отметки при возникновении
        /// </summary>        
        [ListView]
        [DetailView(ReadOnly = true, Name="Содержание отметки при возникновении")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Origin_content { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Выписки", Visible = false)]
        public int? ExtractID { get; set; }

        
        [DetailView(ReadOnly = true, Name="Выписка", Visible = false)]
        public Extract Extract { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД ОНИ", Visible = false)]
        public int? ObjectRecordID { get; set; }


        [DetailView(ReadOnly = true, Name="ОНИ", Visible = false)]
        public ObjectRecord ObjectRecord { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Ограничения", Visible = false)]
        public int? RestrictRecordID { get; set; }


        [DetailView(ReadOnly = true, Name="Ограничение", Visible = false)]
        public RestrictRecord RestrictRecord { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Права", Visible = false)]
        public int? RightRecordID { get; set; }


        [DetailView(ReadOnly = true, Name="Право", Visible = false)]
        public RightRecord RightRecord { get; set; }
    }
}

using Base;
using Base.Attributes;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Документ-основание (все реквизиты)
    /// </summary>
    public class DocumentRecord : BaseObject
    {
        public DocumentRecord() { }

        public string ID_Document { get; set; }

        [SystemProperty]
        public string Content { get; set; }

        /// <summary>
        /// Код документа
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Код вида документа")]
        [PropertyDataType(PropertyDataType.Text)]
        public string TypeCode { get; set; }
        [ListView]
        [DetailView(ReadOnly = true, Name="Вид документа", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string TypeName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "Вид документа")]
        [PropertyDataType(PropertyDataType.Text)]
        public string DocumentType { get; set; }


        /// <summary>
        /// Наименование
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Наименование")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }

        /// <summary>
        /// Серия документа
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Серия")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Series { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Номер")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Number { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Дата")]       
        public System.DateTime? DocDate { get; set; }

        /// <summary>
        /// Орган власти, организация, выдавшие документ
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Орган власти, организация, выдавшие документ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Issuer { get; set; }



        #region Нотариальное удостоверение документа DocNotarized
        /// <summary>
        /// Дата нотариального удостоверения
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Дата нотариального удостоверения")]      
        public System.DateTime? Notarize_date { get; set; }

        /// <summary>
        /// Фамилия и инициалы нотариуса
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Фамилия и инициалы нотариуса")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Notary_name { get; set; }

        /// <summary>
        /// Номер в реестре регистрации нотариальных действий
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Номер в реестре регистрации нотариальных действий")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Notary_action_num { get; set; }

        #endregion


        /// <summary>
        /// Полное наименование должности должностного лица
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name="Полное наименование должности должностного лица")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Fullname_posts_person { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name="ИД Обременения")]     
        public int? RestrictRecordID { get; set; }


        /// <summary>
        /// Обременение/ограничение
        /// </summary>     
        [DetailView(ReadOnly = true, Name="Обременение")]      
        public RestrictRecord RestrictRecord { get; set; }

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


       


    }
}

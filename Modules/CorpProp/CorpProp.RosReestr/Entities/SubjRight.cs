using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Представление прав на ОНИ выписок ЮЛ.
    /// </summary>
    [EnableFullTextSearch]
    public class SubjRight : BaseObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SubjRight.
        /// </summary>
        public SubjRight() : base() { }

        #region Ссылки
        [SystemProperty]
        public int? ExtractID { get; set; }

        [DetailView(Name = "Выписка")]
        [ForeignKey("ExtractID")]
        public virtual Extract Extract { get; set; }

        [SystemProperty]
        public int? ObjectRecordID { get; set; }


        [DetailView(Name = "ОНИ", Visible = false)]
        public virtual ObjectRecord ObjectRecord { get; set; }
                

        [SystemProperty]
        public int? RightRecordID { get; set; }

        [DetailView(Name = "Право")]        
        public virtual RightRecord RightRecord { get; set; }

        [SystemProperty]
        public int? RestrictRecordID { get; set; }

        [DetailView(Name = "Обременение")]        
        public virtual RestrictRecord RestrictRecord { get; set; }

        [SystemProperty]
        public int? DocumentRecordID { get; set; }

        [DetailView(Name = "Документ")]        
        public virtual DocumentRecord DocumentRecord { get; set; }
        #endregion

        #region ОНИ
           
        [ListView(Hidden = true)]
        [DetailView("ИД ОНИ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string IDObject { get; set; }


        [ListView]
        [DetailView("Номер ОНИ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ObjectNumber { get; set; }

            
        [ListView]
        [DetailView("ОНИ - Дата изменения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ObjectModifyDate { get; set; }
        

        [ListView]        
        [FullTextSearchProperty]
        [DetailView(ReadOnly = true, Name = "Кадастровый номер")]
        [PropertyDataType(PropertyDataType.Text)]
        public string CadastralNumber { get; set; }


        [ListView]
        [DetailView("Условный номер")]
        public bool IsConditional { get; set; }

        [ListView]
        [DetailView("ОНИ - Наименование")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ObjectName { get; set; }

        [ListView]
        [DetailView("Назначение ОНИ (код)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string AssignationCode { get; set; }

        [ListView]
        [DetailView("Назначение ОНИ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string AssignationName { get; set; }


        [ListView]
        [DetailView("Целевое назначение (категория) земель")]
        [PropertyDataType(PropertyDataType.Text)]
        public string GroundCategory { get; set; }

        [ListView]
        [DetailView("Текстовое описание целевоего назначения(категории) земель")]
        [PropertyDataType(PropertyDataType.Text)]
        public string GroundCategoryText { get; set; }

              
        [ListView]
        [DetailView(ReadOnly = true, Name = "Площадь")]
        [DefaultValue(0)]
        public decimal? Area { get; set; }

        [ListView]
        [DetailView("Значение площади текстом")]
        [PropertyDataType(PropertyDataType.Text)]
        public string AreaText { get; set; }

        [ListView]
        [DetailView("Ед.измерений (код)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string UnitCode { get; set; }

        [ListView]
        [DetailView("Ед.измерений")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Unit { get; set; }

        [ListView]
        [DetailView("Инвентарный номер, литер")]
        [PropertyDataType(PropertyDataType.Text)]
        public string InvNo { get; set; }

        [ListView]
        [DetailView("Этажность (этаж)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Floor { get; set; }

        [ListView]
        [DetailView("Номера на поэтажном плане")]
        [PropertyDataType(PropertyDataType.Text)]
        public string FloorPlanNo { get; set; }

        [ListView(Hidden = true)]
        [DetailView("ИД Адреса")]
        [PropertyDataType(PropertyDataType.Text)]
        public string IDAddress { get; set; }

        [ListView]
        [DetailView("Адрес")]
        public string Address { get; set; }

        [ListView]
        [DetailView("Дата ликвидации объекта")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ObjectEndDate { get; set; }

        #endregion

        #region Право


        [ListView(Hidden = true)]
        [DetailView("ИД записи о праве")]
        [PropertyDataType(PropertyDataType.Text)]
        public string IDRecord { get; set; }


        [ListView]
        [DetailView("Право - дата изменения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RightModifyDate { get; set; }


        [ListView]
        [DetailView("Номер государствеенной регистрации")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegNumber { get; set; }

        [ListView]
        [DetailView("Код права")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RightTypeCode { get; set; }

        [ListView]
        [DetailView("Вид права")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RightTypeName { get; set; }

        [ListView(Hidden = true)]
        [DetailView("Дата государственной регистрации (текст)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RegDateStr { get; set; }

        [ListView]
        [DetailView("Дата государственной регистрации")]
        public DateTime? RegDate { get; set; }

        [ListView]
        [DetailView("Информация о восстановлении права по решению суда")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RestorCourt { get; set; }

        [ListView]
        [DetailView("Номер прекращения (перехода) права")]
        [PropertyDataType(PropertyDataType.Text)]
        public string EndNumber { get; set; }

        [ListView(Hidden = true)]
        [DetailView("Дата прекращения права (текст)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string EndDateStr { get; set; }

        [ListView]
        [DetailView("Дата прекращения права")]
        public DateTime? EndDate { get; set; }

        [ListView]
        [DetailView("Числитель доли")]
        public int? Numerator { get; set; }

        [ListView]
        [DetailView("Знаменатель доли")]
        public int? Denominator { get; set; }

        [ListView]
        [DetailView("Доля текстом")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ShareText { get; set; }


        [ListView]
        [DetailView("Правообладатели")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RightHoldersStr { get; set; }

        [ListView]       
        [DefaultValue(false)]
        [DetailView("Имеются содольщики ФЛ.")]
        public bool PersonHolder { get; set; }

        [SystemProperty]
        public int? SubjectRecordID { get; set; }

        [ListView(Hidden = true)]
        [DetailView("Субъект")]
        public virtual SubjectRecord SubjectRecord { get; set; }

        #endregion

        #region Ограничение

        [ListView(Hidden = true)]
        [DetailView("ИД Обременения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string IDEncumbrance { get; set; }

        [ListView]
        [DetailView("Дата изменения Обременения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string EncumbranceModifyDate { get; set; }

        [ListView]       
        [DetailView(Name = "Обременение - Номер регистрации")]
        [PropertyDataType(PropertyDataType.Text)]
        public string EncumbranceRegNumber { get; set; }

        [ListView]
        [DetailView("Предмет ограничения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string EncumbranceShareText { get; set; }

        [ListView(Hidden = true)]
        [DetailView("Обременение - Дата государственной регистрации (текст)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string EncumbranceRegDateStr { get; set; }

        [ListView]
        [DetailView("Обременение - Дата государственной регистрации")]
        public DateTime? EncumbranceRegDate { get; set; }

        [ListView]        
        [DetailView("Код вида ограничения/обременения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string EncumbranceTypeCode { get; set; }

        [ListView]        
        [DetailView("Вид ограничения/обременения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string EncumbranceTypeName { get; set; }
        

        [ListView]        
        [DetailView("Обременение - Дата начала действия")]
        public System.DateTime? EncumbranceStartDate { get; set; }

        [ListView]        
        [DetailView("Обременение - Дата прекращения действия")]
        public System.DateTime? EmcumbranceEndDate { get; set; }

        [ListView(Hidden = true)]
        [DetailView("Обременение - Дата начала действия (текст)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Started { get; set; }

        [ListView(Hidden = true)]
        [DetailView("Обременение - Дата прекращения действия (текст)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Stopped { get; set; }

        [ListView]        
        [DetailView("Срок действия обременения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Term { get; set; }

        [ListView]        
        [DetailView("Сведения о лицах, в пользу которых установлены ограничения")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Owner { get; set; }

        [ListView]       
        [DetailView("Участники долевого строительства")]
        [PropertyDataType(PropertyDataType.Text)]
        public string AllShareOwner { get; set; }
        #endregion

        #region Документ

        [ListView(Hidden = true)]
        [DetailView("ИД документа")]
        [PropertyDataType(PropertyDataType.Text)]
        public string IDDocument { get; set; }

        [SystemProperty]
        [ListView]
        [DetailView(ReadOnly = true, Name = "Содержание документа")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Content { get; set; }

       
        [ListView]
        [DetailView(ReadOnly = true, Name = "Код вида документа")]
        [PropertyDataType(PropertyDataType.Text)]
        public string DocTypeCode { get; set; }

        [ListView]
        [DetailView(ReadOnly = true, Name = "Вид документа", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        public string DocTypeName { get; set; }

                
        [ListView]
        [DetailView(ReadOnly = true, Name = "Наименование документа")]
        [PropertyDataType(PropertyDataType.Text)]
        public string DocName { get; set; }

        /// <summary>
        /// Серия документа
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name = "Серия документа")]
        [PropertyDataType(PropertyDataType.Text)]
        public string DocSeries { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name = "Номер документа")]
        [PropertyDataType(PropertyDataType.Text)]
        public string DocNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name = "Дата документа")]
        public System.DateTime? DocDate { get; set; }

        /// <summary>
        /// Орган власти, организация, выдавшие документ
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true, Name = "Орган власти, организация, выдавшие документ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string DocIssuer { get; set; }

        #endregion
    }
}

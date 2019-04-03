using Base;
using Base.Attributes;
using Base.Audit.Entities;
using Base.Entities.Complex;
using Base.Security;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.Audit
{

    /// <summary>
    /// Кастомизированное представление по аудиту изменений.
    /// </summary>
    /// <remarks>
    /// Является проекций двух сущностей DiffItem и AuditItem.
    /// </remarks>
    public class CustomDiffItem : BaseObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса CustomDiffItem.
        /// </summary>
        public CustomDiffItem(): base()
        {

        }

        [DetailView(Name = "Дата")]
        [ListView]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime? Date { get; set; }

        [DetailView(Name = "Тип события")]
        [ListView]
        public TypeAuditItem Type { get; set; }

        [DetailView(Name = "Ссылка на объект")]
        public LinkBaseObject Entity { get; set; }

        [ListView("ИД объекта")]
        [PropertyDataType(PropertyDataType.ObjectType)]
        public int? EntityID { get; set; }

        [ListView("Тип объекта")]
        [PropertyDataType(PropertyDataType.ObjectType)]
        public string EntityType { get; set; }

        public int? UserID { get; set; }
        [DetailView("Пользователь")]
        [FullTextSearchProperty]
        [ListView]
        public virtual User User { get; set; }

        public int? SibUserID { get; set; }
        [DetailView("Пользователь")]
        [FullTextSearchProperty]
        [ListView]
        public virtual SibUser SibUser { get; set; }

        [DetailView(Name = "Описание")]
        [FullTextSearchProperty]
        public string Description { get; set; }

        public int? ParentID { get; set; }
        public AuditItem Parent { get; set; }

        [DetailView(Name = "Свойство")]        
        [ListView]
        public string Property { get; set; }

        [DetailView(Name = "Прежнее значение")]
        [ListView]
        public string OldValue { get; set; }

        [DetailView(Name = "Новое значение")]
        [ListView]
        public string NewValue { get; set; }


        /// <summary>
        /// Наименование свойства объекта (свойство в классе)
        /// </summary>
        /// <remarks>
        /// Добавлено, т.к. в Property записывается локализованное наименование.
        /// </remarks>
        [DetailView("Системное наименование свойства", Visible = false)]
        [ListView(Visible = false)]
        [SystemProperty]
        public string Member { get; set; }
        

    }
}

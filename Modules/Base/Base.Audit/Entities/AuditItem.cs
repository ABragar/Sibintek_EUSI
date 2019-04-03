using Base.Attributes;
using Base.Entities.Complex;
using Base.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Translations;
using Base.Utils.Common.Attributes;

namespace Base.Audit.Entities
{
    [EnableFullTextSearch]
    public class AuditItem: BaseObject
    {
        private static readonly CompiledExpression<AuditItem, string> _type =
            DefaultTranslationOf<AuditItem>.Property(x => x.EntityType).Is(x => x.Entity.TypeName);

        public AuditItem()
        {
            this.Entity = new LinkBaseObject();
        }

        [DetailView(Name = "Дата")]
        [ListView]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime Date { get; set; }

        [DetailView(Name = "Тип события")]
        [ListView]
        public TypeAuditItem Type { get; set; }

        [DetailView(Name = "Ссылка на объект")]
        public LinkBaseObject Entity { get; set; }

        [ListView("Тип объекта")]
        [PropertyDataType(PropertyDataType.ObjectType)]
        public string EntityType => _type.Evaluate(this);

        public int? UserID { get; set; }
        [DetailView("Пользователь")]
        [FullTextSearchProperty]
        [ListView]
        public virtual User User { get; set; }

        [DetailView(Name = "Логин")]
        [FullTextSearchProperty]
        [ListView]
        public string UserLogin { get; set; }

        [DetailView(Name = "Описание")]
        [FullTextSearchProperty]
        public string Description { get; set; }

        [DetailView(TabName = "Изменения")]
        public virtual ICollection<DiffItem> Diff { get; set; }

        public Guid? SessionId { get; set; }
    }

    public class DiffItem: BaseObject
    {
        public int ParentID { get; set; }
        public AuditItem Parent { get; set; }

        [DetailView(Name = "Свойство")]
        [MaxLength(255)]
        [ListView]
        public string Property { get; set; }

        [DetailView(Name = "Old")]
        [ListView]
        public string OldValue { get; set; }

        [DetailView(Name = "New")]
        [ListView]
        public string NewValue { get; set; }

        //sib
        /// <summary>
        /// Наименование свойства объекта (свойство в классе)
        /// </summary>
        /// <remarks>
        /// Добавлено, т.к. в Property записывается локализованное наименование.
        /// </remarks>
        [SystemProperty]
        public string Member { get; set; }
        //end sib
    }

    [UiEnum]
    public enum TypeAuditItem
    {
        [UiEnumValue("Отказ входа в систему")]
        LogOnError = 0,
        [UiEnumValue("Успешный вход в систему")]
        LogOn = 1,
        [UiEnumValue("Выход из системы")]
        LogOf = 2,
        [UiEnumValue("Создание объекта")]
        CreateObject = 3,
        [UiEnumValue("Редактирование объекта")]
        UpdateObject = 4,
        [UiEnumValue("Удаление объекта")]
        DeleteObject = 5,
        [UiEnumValue("Новый пользователь зарегистирован.")]
        AccountRegistered = 6,
        [UiEnumValue("Ошибка регистрации нового пользователя.")]
        AccountRegistrationFails = 7
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;
using Base.BusinessProcesses.Entities;
using Base.ComplexKeyObjects.Superb;
using Base.Security;

namespace Base.Support.Entities
{    
    public class BaseSupport : BaseObject, IBPObject, ICreateObject, ISuperObject<BaseSupport>
    {
        public int CreatorID { get; set; }
        [ListView("Пользователь", Order = 100)]
        public User Creator { get; set; }

        [ListView]
        [MaxLength(255)]
        [DetailView(Name = "Тема", Required = true, Order = 300)]
        public string Title { get; set; }

        [ListView]
        [DetailView(Name = "Описание", Required = true, Order = 400)]
        public string Description { get; set; }

        [DetailView(TabName = "[1]Файлы")]
        [PropertyDataType(PropertyDataType.Files)]
        public virtual ICollection<SupportFile> AttachFiles { get; set; }

        [ListView("Дата создания",Order = 500)]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime CreateDate { get; set; }

        [ListView(Order = -100)]
        [DetailView(Name = "Статус", Order = 600, ReadOnly = true)]
        public SupportStatus Status { get; set; }

        #region IBPObject
        public int? WorkflowContextID { get; set; }
        public WorkflowContext WorkflowContext { get; set; }
        #endregion

        [ListView("Тип")]
        [PropertyDataType(PropertyDataType.ExtraId)]
        public string ExtraID { get; }
    }

    public class SupportFile : FileCollectionItem
    {
    }

    [UiEnum]
    public enum SupportStatus
    {
        [UiEnumValue("Новое", Color = "#76e0ff", Icon = "glyphicon glyphicon-circle-plus")]
        Created = 0,
        [UiEnumValue("Отвечено", Color = "#5dd95d", Icon = "glyphicon glyphicon-circle-ok")]
        Answered = 1,
        [UiEnumValue("Проигнорировано", Color = "#d9534f", Icon = "glyphicon glyphicon-circle-remove")]
        Ignored = 2,
        [UiEnumValue("На уточнении", Color = "#F2AD4E", Icon = "glyphicon glyphicon-circle-exclamation-mark")]
        Clarified = 3,
        [UiEnumValue("Завершено", Color = "#319431", Icon = "glyphicon glyphicon-circle-ok")]
        Ended = 4
    }
}

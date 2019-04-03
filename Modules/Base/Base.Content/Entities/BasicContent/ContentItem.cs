using Base.Attributes;
using Base.BusinessProcesses.Entities;
using Base.Security;
using Base.Security.ObjectAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Utils.Common.Attributes;

namespace Base.Content.Entities
{
    [EnableFullTextSearch]
    public class ContentItem : BaseObject, ICategorizedItem, IBPObject
    {

        public ContentItem()
        {
            this.Content = new Content();
        }

        #region COMMON
        public int? ImagePreviewID { get; set; }

        [ListView]
        [DetailView(Name = "Изображение", Order = 0)]
        [Image()]
        public virtual FileData ImagePreview { get; set; }

        [ListView]
        [MaxLength(255)]
        [FullTextSearchProperty]
        [DetailView(Name = "Наименование", Required = true, Order = 1)]
        public string Title { get; set; }

        [ListView]
        [MaxLength(255)]
        [FullTextSearchProperty]
        [DetailView(Name = "Значение", Order = 1)]
        public string Value { get; set; }

        [ListView]
        [FullTextSearchProperty]
        [DetailView(Name = "Топ", Order = 1)]
        public bool Top { get; set; }

        [ListView(Hidden = true)]
        [DetailView(Name = "Описание", Order = 2)]
        [FullTextSearchProperty]
        public string Description { get; set; }

        [ListView(Order = 8)]
        [DetailView(Name = "Дата", Order = 4, Required = true)]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime? Date { get; set; }

        [ListView]
        [DetailView(Name = "Ссылка", Order = 5, Required = true)]
        //[PropertyDataType(PropertyDataType.Url)]
        public string Src { get; set; }

        // STATUS
        // PERFORMER

        [DetailView(TabName = "[1]Контент")]
        public Content Content { get; set; }

        [ListView]
        [DetailView(Name = "Статус", ReadOnly = true, Order = 6)]
        public ContentItemStatus ContentItemStatus { get; set; }

        [ListView]
        [DetailView(Name = "Дата создания", ReadOnly = true, Order = 7)]
        [PropertyDataType(PropertyDataType.DateTime)]
        public DateTime Created { get; set; }

        [DetailView(Name = "Тэги", TabName = "[3]Дополнительно", Order = 2)]
        [FullTextSearchProperty]
        public virtual ICollection<Tag> Tags { get; set; }
        #endregion

        #region IBPObject
        public int? WorkflowContextID { get; set; }
        public  WorkflowContext WorkflowContext { get; set;}
        #endregion

        #region ICategorizedItem
        public int CategoryID { get; set; }

        [JsonIgnore]
        [ForeignKey("CategoryID")]
        public virtual ContentCategory ContentCategory { get; set; }

        HCategory ICategorizedItem.Category => this.ContentCategory;

        #endregion
    }

    [UiEnum]
    public enum ContentItemStatus
    {
        [UiEnumValue("Запись создана")]
        New = 0,

        [UiEnumValue("На модерации")]
        Moderating = 10,

        [UiEnumValue("Доработка")]
        Rework = 20,

        [UiEnumValue("Запись опубликована")]
        Published = 30,

        [UiEnumValue("Архив")]
        Archive = 40,
    }
}

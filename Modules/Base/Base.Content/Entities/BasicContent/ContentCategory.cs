using Base.Attributes;
using Base.UI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Base.Content.Entities
{
    public class ContentCategory : HCategory, ITreeNodeImage
    {
        [ForeignKey("ParentID")]
        public virtual ContentCategory Parent_ { get; set; }
        public virtual ICollection<ContentCategory> Children_ { get; set; }

        [DetailView(Name = "Группа", Order = 2)]
        public ContentCategoryType ContentCategoryType { get; set; }

        [SystemProperty]
        public string Action { get; set; }

        [SystemProperty]
        public string Controller { get; set; }

        [SystemProperty]
        public string Params { get; set; }

        #region HCategory
        public override HCategory Parent => this.Parent_;
        public override IEnumerable<HCategory> Children => this.Children_ ?? new List<ContentCategory>();
        #endregion

        #region ITreeNodeImage
        public int? ImageID { get; set; }

        [ListView(Width = 24, Height = 24)]
        [DetailView(Name = "Изображение", Order = 1)]
        [Image()]
        public virtual FileData Image { get; set; }
        #endregion


        [ListView]
        [DetailView(Name = "Наименование на сайте", Order = 2)]
        public string PublicTitle { get; set; }

        [DetailView(Name = "Показывать в меню", Order = 4)]
        public bool ShowInMenu { get; set; }

        [DetailView("Разворачивать контент", Order = 4)]
        public bool Expanded { get; set; }

        [DetailView("Показывать ссылки на социальные сети", Order = 4)]
        public bool IsShowSocialLinks { get; set; }

        [DetailView(Name = "Доступен для подписки", Order = 6)]
        public bool SubscribeAvailable { get; set; }

        [DetailView(TabName = "[1]Виджет", Name = "Controller")]
        [MaxLength(255)]
        public string WidgetController { get; set; }

        [DetailView(TabName = "[1]Виджет", Name = "Action")]
        [MaxLength(255)]
        public string WidgetAction { get; set; }

        [InverseProperty("ContentCategory")]
        public ICollection<ContentSubscriber> ContentSubscribers { get; set; }
    }

    [UiEnum]
    public enum ContentCategoryType
    {
        [UiEnumValue("Раздел")]
        ContentFolder = 0,

        [UiEnumValue("Контент")]
        ContentRegular = 10,

        [UiEnumValue("Расширенное")]
        ContentExtended = 20,

        [UiEnumValue("Баннер")]
        Banner = 30,
    }
}

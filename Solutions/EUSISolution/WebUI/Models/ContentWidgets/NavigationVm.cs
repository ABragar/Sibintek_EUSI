using Base;
using Base.Attributes;
using System.Collections.Generic;

namespace WebUI.Models.ContentWidgets
{
    public class NavigationVm : WidgetVm
    {
        [DetailView("Якоря")]
        public ICollection<NavigationItem> Items { get; set; } = new List<NavigationItem>()
        {
            new NavigationItem()
            {
                AnchorTitle = "Test",
                Anchor = "test"
            }
        };
    }

    public class NavigationItem : BaseObject
    {
        [DetailView("Текст якоря")]
        [ListView]
        public string AnchorTitle { get; set; }

        [DetailView("Якорь")]
        [ListView]
        public string Anchor { get; set; }
    }
}
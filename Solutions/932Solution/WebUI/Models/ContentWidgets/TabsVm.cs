using Base;
using Base.Attributes;
using System.Collections.Generic;

namespace WebUI.Models.ContentWidgets
{
    public class TabsVm : WidgetVm
    {
        [DetailView("Якоря")]
        public ICollection<TabsItem> Items { get; set; } = new List<TabsItem>()
        {
            new TabsItem()
            {
                AnchorTitle = "Test",
                Anchor = "testtab"
            }
        };
    }

    public class TabsItem : BaseObject
    {
        [DetailView("Текст якоря")]
        [ListView]
        public string AnchorTitle { get; set; }

        [DetailView("Якорь")]
        [ListView]
        public string Anchor { get; set; }
    }
}
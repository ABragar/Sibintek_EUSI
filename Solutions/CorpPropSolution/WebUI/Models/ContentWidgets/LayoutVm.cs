using Base.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models.ContentWidgets
{
    public class LayoutVm : WidgetVm
    {
        [DetailView("Минимальный размер")]
        [MaxLength(255)]
        public int MinHeight { get; set; }

        public LayoutVm()
        {
            this.MinHeight = 300;
        }
    }
}
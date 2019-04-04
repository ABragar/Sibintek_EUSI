using Base;
using Base.Attributes;
using Base.FileStorage;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models.ContentWidgets
{
    public class VideoVm : WidgetVm
    {
    }

    public class ExternalVideoVm : WidgetVm
    {
        [DetailView("Наименование")]
        public string Title { get; set; }

        [DetailView("Файл", Order = 1)]
        [PropertyDataType(PropertyDataType.File, Params = "Upload=false")]
        public FileData File { get; set; }

        [DetailView("Ссылка")]
        [MaxLength(int.MaxValue)]
        public string Url { get; set; }
    }

    public class InternalVideoVm : InternalFileVm
    {   
    }
}
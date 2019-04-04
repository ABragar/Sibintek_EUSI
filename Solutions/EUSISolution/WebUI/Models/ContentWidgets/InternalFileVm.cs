using Base;
using Base.Attributes;
using Base.FileStorage;

namespace WebUI.Models.ContentWidgets
{
    public class InternalFileVm
    {
        [DetailView("Файл")]
        [PropertyDataType(PropertyDataType.File, Params = "Upload=false")]
        public FileData File { get; set; }

        [DetailView("Наименование", Order = 0)]
        public string Title { get; set; }
    }
}
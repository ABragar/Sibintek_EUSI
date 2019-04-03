using Base.Attributes;
using System.ComponentModel.DataAnnotations;
using Base;

namespace WebUI.Models.ContentWidgets
{
    public class InternalImageVm
    {
        [DetailView("Изображение")]
        [Image(Upload = false, Crop = false)]
        public FileData File { get; set; }

        [DetailView("Оригинал (приоритетный параметр)", TabName = "[1]Дополнительно")]
        public bool Original { get; set; }
        [DetailView("Ширина", TabName = "[1]Дополнительно")]
        public int Width { get; set; }

        [DetailView("Высота", TabName = "[1]Дополнительно")]
        public int Height { get; set; }

        [DetailView("Заголовок изображения", Order = 1)]
        public string Title { get; set; }

        [DetailView("Альтернативный текст", Order = 2)]
        [MaxLength(255)]
        public string Alt { get; set; }

        [DetailView("Подпись изображения", Order = 2)]
        [MaxLength(255)]
        public string ImageSignature { get; set; }

        public InternalImageVm()
        {
            this.Original = true;
            this.Height = 300;
            this.Width = 300;
        }
    }
}
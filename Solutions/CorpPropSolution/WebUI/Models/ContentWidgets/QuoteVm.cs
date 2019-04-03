using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Base;
using Base.Attributes;
using Base.Enums;

namespace WebUI.Models.ContentWidgets
{
    public class QuoteVm : WidgetVm
    {
        [DetailView("ФИО")]
        [MaxLength(255)]
        public string Title { get; set; }

        [DetailView("Описание автора")]
        public string Description { get; set; }

        [DetailView(TabName = "[1]Текст цитаты")]
        [PropertyDataType(PropertyDataType.Html)]
        public string QuoteText { get; set; }

        [DetailView("Файл")]
        [Image(Circle = true, DefaultImage = DefaultImage.NoPhoto, Upload = false)]
        public FileData File { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Entities.Complex;

namespace Base.Links.Entities
{
    /// <summary>
    /// Настройка отображений для связей
    /// </summary>
    public class LinkGroupConfig : BaseObject
    {
        [ListView]
        [DetailView("Мнемоника")]
        [PropertyDataType(PropertyDataType.ObjectType)]
        public string Mnemonic { get; set; }
        
        [DetailView("Иконка")]
        [PropertyDataType("LinkGroupIcon")]
        public LinkGroupIcon Icon { get; set; } = new LinkGroupIcon();

        [DetailView("Размер иконки")]
        public int Size { get; set; } = 4;

        [DetailView(TabName = "Шрифт", Name = "Размер")]        
        public int FontSize { get; set; } = 8;

        [DetailView(TabName = "Шрифт", Name = "Цвет")]
        [PropertyDataType(PropertyDataType.Color)]
        public string FontColor { get; set; } = "#343434";

        [DetailView(TabName = "Шрифт", Name = "Шрифт")]        
        public string FontFace { get; set; } = "Arial";

        [DetailView(TabName = "Шрифт", Name = "Расположение")]
        public string FontAlign { get; set; } = "center";
    }

    public class LinkGroupIcon : Icon
    {
        public string Code { get; set; }
    }
}

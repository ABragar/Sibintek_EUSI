using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Entities.Complex;
using Newtonsoft.Json;

namespace Base.UI.Presets
{
    [Serializable]
    [JsonObject]
    public class QuickAccessBarPreset : Preset
    {
        [DetailView("Кнопки панели быстрого доступа")]
        public List<QuickAccessBarButton> BarButtons { get; set; }

        public QuickAccessBarPreset()
        {
            BarButtons = new List<QuickAccessBarButton>();
        }
    }


    [Serializable]
    [JsonObject]
    public class QuickAccessBarButton : BaseObject
    {
        public QuickAccessBarButton()
        {
            Icon = new Icon();
            ButtonType = QABarButtonType.Link;
        }

        [DetailView(Name = "Иконка")]
        public Icon Icon { get; set; }

        [ListView]
        [DetailView(Name = "Наименование", Required = true)]
        public string Title { get; set; }

        [MaxLength(100)]
        [ListView]
        [DetailView(Name = "Мнемоника", Required = true)]
        [PropertyDataType(PropertyDataType.Mnemonic)]
        public string Mnemonic { get; set; }

        [DetailView(Name = "Тип", Required = true)]
        public QABarButtonType ButtonType { get; set; }
    }

    [UiEnum]
    public enum QABarButtonType
    {
        [UiEnumValue]
        Link,
        [UiEnumValue]
        Window
    }
}

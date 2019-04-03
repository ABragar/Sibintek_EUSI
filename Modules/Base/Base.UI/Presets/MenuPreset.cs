using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.Entities.Complex;
using Base.UI.ViewModal;
using Newtonsoft.Json;

namespace Base.UI.Presets
{
    [Serializable]
    [JsonObject]
    public class MenuPreset : Preset
    {
        [DetailView("Кнопки меню")]
        [PropertyDataType("MenuElements")]
        public List<MenuElement> MenuElements { get; set; } = new List<MenuElement>();
    }


    [ViewModelConfig(Title = "Элемент меню", Icon = "halfling halfling-globe")]
    [Serializable]
    [JsonObject]
    public sealed class MenuElement : BaseObject
    {
        private static readonly Random Rnd = new Random();


        public MenuElement()
        {
            ID = Rnd.Next(0, 999999);
        }

        public MenuElement(string name, string mnemonic, string icon = null) : this()
        {
            Name = name;
            Mnemonic = mnemonic;
            Icon.Value = icon;
        }

        [MaxLength(255)]
        [DetailView("Наименование", Required = true)]
        public string Name { get; set; }

        [DetailView("Иконка")]
        public Icon Icon { get; set; } = new Icon();

        [MaxLength(100)]
        [DetailView("Мнемоника")]
        [PropertyDataType(PropertyDataType.Mnemonic)]
        public string Mnemonic { get; set; }

        [DetailView("URL")]
        [MaxLength(255)]
        public string URL { get; set; }

        public ICollection<MenuElement> Children { get; set; } = new List<MenuElement>();

    }
}
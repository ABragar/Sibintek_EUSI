using System;
using Base;
using Base.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.Common
{
    public interface IHasAdditionalFeature
    {
        int? AdditionalFeaturesID { get; set; }

        AdditionalFeatures AdditionalFeature { get; set; }
    }

    public class AdditionalFeatures: BaseObject, IAdditionalEstateCharacteristics
    {
        private const string TabName = "[20]Дополнительные характеристики";

        #region Дополнительные характеристики
        [DetailView(Name = "Доп. характеристика 1 (строка)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 200)]
        public string TxtField1 { get; set; }
        [DetailView(Name = "Доп. характеристика 2 (строка)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 201)]
        public string TxtField2 { get; set; }
        [DetailView(Name = "Доп. характеристика 3 (строка)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 202)]
        public string TxtField3 { get; set; }
        [DetailView(Name = "Доп. характеристика 4 (строка)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 203)]
        public string TxtField4 { get; set; }
        [DetailView(Name = "Доп. характеристика 5 (строка)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 204)]
        public string TxtField5 { get; set; }
        [DetailView(Name = "Доп. характеристика 6 (строка)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 205)]
        public string TxtField6 { get; set; }
        [DetailView(Name = "Доп. характеристика 7 (строка)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 206)]
        public string TxtField7 { get; set; }
        [DetailView(Name = "Доп. характеристика 8 (строка)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 207)]
        public string TxtField8 { get; set; }
        [DetailView(Name = "Доп. характеристика 9 (строка)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 208)]
        public string TxtField9 { get; set; }
        [DetailView(Name = "Доп. характеристика 10 (строка)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 209)]
        public string TxtField10 { get; set; }
        [DetailView(Name = "Доп. характеристика 1 (целое число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 210)]
        public int? FloatIntField1 { get; set; }
        [DetailView(Name = "Доп. характеристика 2 (целое число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 211)]
        public int? FloatIntField2 { get; set; }
        [DetailView(Name = "Доп. характеристика 3 (целое число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 212)]
        public int? FloatIntField3 { get; set; }
        [DetailView(Name = "Доп. характеристика 4 (целое число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 213)]
        public int? FloatIntField4 { get; set; }
        [DetailView(Name = "Доп. характеристика 5 (целое число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 214)]
        public int? FloatIntField5 { get; set; }
        [DetailView(Name = "Доп. характеристика 6 (целое число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 215)]
        public int? FloatIntField6 { get; set; }
        [DetailView(Name = "Доп. характеристика 7 (целое число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 216)]
        public int? FloatIntField7 { get; set; }
        [DetailView(Name = "Доп. характеристика 8 (целое число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 217)]
        public int? FloatIntField8 { get; set; }
        [DetailView(Name = "Доп. характеристика 9 (целое число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 218)]
        public int? FloatIntField9 { get; set; }
        [DetailView(Name = "Доп. характеристика 10 (целое число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 219)]
        public int? FloatIntField10 { get; set; }
        [DetailView(Name = "Доп. характеристика 1 (дробное число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 220)]
        public decimal? FloatDecimalField1 { get; set; }
        [DetailView(Name = "Доп. характеристика 2 (дробное число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 221)]
        public decimal? FloatDecimalField2 { get; set; }
        [DetailView(Name = "Доп. характеристика 3 (дробное число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 222)]
        public decimal? FloatDecimalField3 { get; set; }
        [DetailView(Name = "Доп. характеристика 4 (дробное число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 223)]
        public decimal? FloatDecimalField4 { get; set; }
        [DetailView(Name = "Доп. характеристика 5 (дробное число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 224)]
        public decimal? FloatDecimalField5 { get; set; }
        [DetailView(Name = "Доп. характеристика 6 (дробное число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 225)]
        public decimal? FloatDecimalField6 { get; set; }
        [DetailView(Name = "Доп. характеристика 7 (дробное число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 226)]
        public decimal? FloatDecimalField7 { get; set; }
        [DetailView(Name = "Доп. характеристика 8 (дробное число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 227)]
        public decimal? FloatDecimalField8 { get; set; }
        [DetailView(Name = "Доп. характеристика 9 (дробное число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 228)]
        public decimal? FloatDecimalField9 { get; set; }
        [DetailView(Name = "Доп. характеристика 10 (дробное число)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 229)]
        public decimal? FloatDecimalField10 { get; set; }
        [DetailView(Name = "Доп. характеристика 1 (логическая)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 230)]
        public bool? BoolField1 { get; set; }
        [DetailView(Name = "Доп. характеристика 2 (логическая)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 231)]
        public bool? BoolField2 { get; set; }
        [DetailView(Name = "Доп. характеристика 3 (логическая)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 232)]
        public bool? BoolField3 { get; set; }
        [DetailView(Name = "Доп. характеристика 4 (логическая)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 233)]
        public bool? BoolField4 { get; set; }
        [DetailView(Name = "Доп. характеристика 5 (логическая)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 234)]
        public bool? BoolField5 { get; set; }
        [DetailView(Name = "Доп. характеристика 6 (логическая)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 235)]
        public bool? BoolField6 { get; set; }
        [DetailView(Name = "Доп. характеристика 7 (логическая)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 236)]
        public bool? BoolField7 { get; set; }
        [DetailView(Name = "Доп. характеристика 8 (логическая)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 237)]
        public bool? BoolField8 { get; set; }
        [DetailView(Name = "Доп. характеристика 9 (логическая)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 238)]
        public bool? BoolField9 { get; set; }
        [DetailView(Name = "Доп. характеристика 10 (логическая)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 239)]
        public bool? BoolField10 { get; set; }
        [DetailView(Name = "Доп. характеристика 1 (дата)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 240)]
        public DateTime? DateField1 { get; set; }
        [DetailView(Name = "Доп. характеристика 2 (дата)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 241)]
        public DateTime? DateField2 { get; set; }
        [DetailView(Name = "Доп. характеристика 3 (дата)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 242)]
        public DateTime? DateField3 { get; set; }
        [DetailView(Name = "Доп. характеристика 4 (дата)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 243)]
        public DateTime? DateField4 { get; set; }
        [DetailView(Name = "Доп. характеристика 5 (дата)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 244)]
        public DateTime? DateField5 { get; set; }
        [DetailView(Name = "Доп. характеристика 6 (дата)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 245)]
        public DateTime? DateField6 { get; set; }
        [DetailView(Name = "Доп. характеристика 7 (дата)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 246)]
        public DateTime? DateField7 { get; set; }
        [DetailView(Name = "Доп. характеристика 8 (дата)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 247)]
        public DateTime? DateField8 { get; set; }
        [DetailView(Name = "Доп. характеристика 9 (дата)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 248)]
        public DateTime? DateField9 { get; set; }
        [DetailView(Name = "Доп. характеристика 10 (дата)", Visible = false, TabName = TabName), ListView(Visible = false, Order = 249)]
        public DateTime? DateField10 { get; set; }

        #endregion
    }
}

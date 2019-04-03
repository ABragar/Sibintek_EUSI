using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Base
{
    public interface IAdditionalEstateCharacteristics
    {
        #region Текстовые поля
        string TxtField1 { get; set; }
        string TxtField2 { get; set; }
        string TxtField3 { get; set; }
        string TxtField4 { get; set; }
        string TxtField5 { get; set; }
        string TxtField6 { get; set; }
        string TxtField7 { get; set; }
        string TxtField8 { get; set; }
        string TxtField9 { get; set; }
        string TxtField10 { get; set; }
        #endregion

        #region Числовые поля (целочисленные)
        int? FloatIntField1 { get; set; }
        int? FloatIntField2 { get; set; }
        int? FloatIntField3 { get; set; }
        int? FloatIntField4 { get; set; }
        int? FloatIntField5 { get; set; }
        int? FloatIntField6 { get; set; }
        int? FloatIntField7 { get; set; }
        int? FloatIntField8 { get; set; }
        int? FloatIntField9 { get; set; }
        int? FloatIntField10 { get; set; }
        #endregion

        #region Числовые поля (с плавающей запятой)
        decimal? FloatDecimalField1 { get; set; }
        decimal? FloatDecimalField2 { get; set; }
        decimal? FloatDecimalField3 { get; set; }
        decimal? FloatDecimalField4 { get; set; }
        decimal? FloatDecimalField5 { get; set; }
        decimal? FloatDecimalField6 { get; set; }
        decimal? FloatDecimalField7 { get; set; }
        decimal? FloatDecimalField8 { get; set; }
        decimal? FloatDecimalField9 { get; set; }
        decimal? FloatDecimalField10 { get; set; }
        #endregion

        #region Логические поля
        bool? BoolField1 { get; set; }
        bool? BoolField2 { get; set; }
        bool? BoolField3 { get; set; }
        bool? BoolField4 { get; set; }
        bool? BoolField5 { get; set; }
        bool? BoolField6 { get; set; }
        bool? BoolField7 { get; set; }
        bool? BoolField8 { get; set; }
        bool? BoolField9 { get; set; }
        bool? BoolField10 { get; set; }
        #endregion

        #region Поля формата Дата
        DateTime? DateField1 { get; set; }
        DateTime? DateField2 { get; set; }
        DateTime? DateField3 { get; set; }
        DateTime? DateField4 { get; set; }
        DateTime? DateField5 { get; set; }
        DateTime? DateField6 { get; set; }
        DateTime? DateField7 { get; set; }
        DateTime? DateField8 { get; set; }
        DateTime? DateField9 { get; set; }
        DateTime? DateField10 { get; set; }
        #endregion

    }
}

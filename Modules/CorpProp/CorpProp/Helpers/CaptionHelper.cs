using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Helpers
{
    /// <summary>
    /// Представляет методы и свойства для работы с заголовками и наименованиями объектов Системы.
    /// </summary>
    public static class CaptionHelper
    {
        #region Общее
            /// <summary>
            /// Наименование вкладки на карточке детального просмотра по умолчанию.
            /// </summary>
            public const string DefaultTabName = "[0]Основные данные";
        #endregion
        #region Субъект
            /// <summary>
            /// Наименование вкладки для объекта Оценщик.
            /// </summary>
            public const string AppraiserTabName = "[1]Доп. данные";
        #endregion
        #region Сделки
            /// <summary>
            /// Наименование вкладки для объекта общество группы.
            /// </summary>
            public const string DealTabName = "[1]Информация о сделке";
        #endregion
    }
}

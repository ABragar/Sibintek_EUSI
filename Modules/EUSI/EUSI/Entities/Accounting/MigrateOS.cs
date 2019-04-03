using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using EUSI.Entities.Estate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.Accounting
{
    /// <summary>
    /// Представляет класс-заглушку по инициации миграции ОС/НМА в ЕУСИ.
    /// </summary>
    /// <remarks>
    /// Не хранится в БД.
    /// Используется для сервиса инициации миграции <see cref="nameof(EUSI.Services.Accounting.MigrateOSService)"/> .
    /// </remarks>
    public class MigrateOS : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса MigrateOS.
        /// </summary>
        public MigrateOS() : base()
        {
        }
       
    }
}

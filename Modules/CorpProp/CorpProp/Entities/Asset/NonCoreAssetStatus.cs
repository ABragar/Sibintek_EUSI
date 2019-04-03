using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Asset
{
    /// <summary>
    /// Представляет справочник статусов для ННА.
    /// </summary>
    /// <remarks>
    /// Справочник: Черновик ОГ, Одобрен ОГ, Согласован с Куратором, Проверка ДС, Согласовано ДС, На доработке в ОГ, Утверждено органом управления, Дано поручение об оценке, Оценка проведена и согласована ДС, Принято корп. решение
    /// Статусы реализация ННА:
    /// Проведен первый этап тендера, Первый этап тендера НЕ состоялся, Проведен второй этап тендера, Второй этап тендера НЕ состоялся, Проведен третий этап тендера, Третий этап тендера НЕ состоялся, Тендер состоялся, Заключен договор, Подписан акт приема-передачи, Произошел переход права.
    /// </remarks>
    [EnableFullTextSearch]
    public class NonCoreAssetStatus : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса NonCoreAssetStatus.
        /// </summary>
        public NonCoreAssetStatus()
        {

        }
    }
}

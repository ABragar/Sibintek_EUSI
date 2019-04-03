using System.Collections.Generic;
using System.Data.Entity;
using Base;
using Base.DAL;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;

namespace CorpProp.DefaultData
{
    public interface IDefaultDataHelper
    {
        void InitDefaulData<T>(IUnitOfWork unitOfWork, IFillDataStrategy<T> dataStrategy) where T : class;
        void IsGetRepository<T>(IUnitOfWork unitOfWork, IFillDataStrategy<T> dataStrategy) where T : class; 
        void CreateDictItem<T>(IUnitOfWork uow, List<T> items) where T : DictObject;
        void CreateDefaultItem<T>(IUnitOfWork uow, List<T> items) where T : BaseObject;
        void CreateNSI(IUnitOfWork uow, List<NSI> items);

        /// <summary>
        /// Инициирует создание дефолтных значений в БД.
        /// </summary>
        /// <param name="uow">Сесия.</param>
        /// <remarks>
        /// В отличие от Generate может вызываться многократно и везде с использованием сессии UnitOfWork.
        /// </remarks>
        void CreateDefaulData<T>(IUnitOfWork uow, IFillDataStrategy<T> dataStrategy) where T: class;

        /// <summary>
        /// Добавляет элементы.
        /// </summary>
        /// <typeparam name="T">Тип добавляемых элементов.</typeparam>
        /// <param name="context">Контекст</param>
        /// <param name="items">Элементы.</param>
        /// <remarks>
        /// Использовать с осторожностью,т.к. при каждом запуске приложения, будет добавлять в БД элементы без разбора!
        /// </remarks>
        void AddItems<T>(DbContext context, List<T> items) where T: BaseObject;

        /// <summary>
        /// Добавляет или обновляет элементы справочников, наследуемых от DictObject, по ключу = полю "Code".
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="items"></param>
        void AddDictObjects<T>(DbContext context, List<T> items) where T : DictObject;
    }
}

using Base.DAL;
using CorpProp.Entities.Base;
using CorpProp.Entities.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Helpers
{

    /// <summary>
    /// Методы для работы с историчностью.
    /// </summary>
    public static class HistoryHelper
    {
        /// <summary>
        /// Ищет и возвращает историчную запись объекта за сегодня.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="uow">Сессия.</param>
        /// <param name="instance">Экземпляр текущего объекта.</param>
        /// <returns>Историчная запись.</returns>
        public static T GetCurrentHistory<T>(IUnitOfWork uow, T instance) where T : TypeObject
        {
            if (uow == null || instance == null || instance.ID == 0) return null;
            DateTime dt = DateTime.Now.Date;
            return uow.GetRepository<T>()
                .Filter(x=> x!= null && !x.Hidden && x.Oid == instance.Oid 
                && x.IsHistory && x.NonActualDate != null && x.NonActualDate == dt)
                .FirstOrDefault();
           
        }

        /// <summary>
        /// Ищет и возвращает историчную запись объекта за сегодня.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="instance">Экземпляр текущего объекта.</param>
        /// <returns>Историчная запись.</returns>
        public static object GetCurrHistory(IUnitOfWork uow, Type type, object instance)
        {
            object obj = null;
            try
            {
                if (uow == null) return null;
                
                MethodInfo method = typeof(HistoryHelper).GetMethod("GetCurrentHistory");
                MethodInfo generic = method.MakeGenericMethod(type);                
                obj = generic.Invoke(null, new object[] { uow, instance });
                return obj;

            }
            catch (Exception ex)
            {
                
            }
            return obj;

        }

        /// <summary>
        /// Возвращает оригинал объекта без отслеживания в контексте.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uow"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetOriginal<T>(IUnitOfWork uow, int id) where T : TypeObject
        {
            if (uow == null || id == 0) return null;            
            return 
                uow.GetRepository<T>().GetOriginal(id);
               

        }

        public static object GetOriginalObject(IUnitOfWork uow, Type type, int id)
        {
            object obj = null;
            try
            {
                if (uow == null || id ==0) return null;

                MethodInfo method = typeof(HistoryHelper).GetMethod("GetOriginal");
                MethodInfo generic = method.MakeGenericMethod(type);
                obj = generic.Invoke(null, new object[] { uow, id });
                return obj;

            }
            catch (Exception ex)
            {

            }
            return obj;

        }

    }
}

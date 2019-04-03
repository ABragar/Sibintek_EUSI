using Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Helpers
{
    /// <summary>
    /// Предоставляет методы для работы с объектами Системы.
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// Возвращает текущий контекст объекта.
        /// </summary>
        /// <param name="entity">Экземпляр объекта.</param>
        /// <returns>Текущий контекст, в котором обозначен данный экземпляр объекта.</returns>
        /// <remarks>
        /// Конекст может быть пустым, если экземпляр сущности не был довален в него или контекст был уже сохранен.
        /// </remarks>
        public static Base.DAL.EF.EFContext GetContextFromEntity(object entity)
        {
            var object_context = GetObjectContextFromEntity(entity);

            if (object_context == null || object_context.TransactionHandler == null)
                return null;

            return object_context.TransactionHandler.DbContext as Base.DAL.EF.EFContext;
        }

        /// <summary>
        /// Возвращает ObjectContext экземпляра сущности.
        /// </summary>
        /// <param name="entity">Экземпляр объекта.</param>
        /// <returns>ObjectContext</returns>
        private static ObjectContext GetObjectContextFromEntity(object entity)
        {
            var field = entity.GetType().GetField("_entityWrapper");

            if (field == null)
                return null;

            var wrapper = field.GetValue(entity);
            if (wrapper == null) return null;
            var property = wrapper.GetType().GetProperty("Context");
            if (property == null) return null;
            var context = (ObjectContext)property.GetValue(wrapper, null);

            return context;
        }


        /// <summary>
        /// Возвращает значение свойства propName в объекте obj в виде строки.
        /// </summary>
        /// <param name="obj">Экземпляр объекта.</param>
        /// <param name="propName">Наименование свойства.</param>
        /// <returns></returns>
        public static string GetStrValue(object obj, string propName)
        {
            string str = "";

            PropertyInfo prop = obj.GetType().GetProperty(propName);
            if (prop != null)
            {
                var value = prop.GetValue(obj);
                //TODO: доработать с навигационными свойствами и доступу по Reflection.
                if (value != null)
                    str = value.ToString();
            }
            return str;
        }

        public static IEnumerable<object> GetListValue(object obj, string propName)
        {
           
            PropertyInfo prop = obj.GetType().GetProperty(propName);
            if (prop != null && prop.PropertyType.IsGenericType
                && (typeof(ICollection<>).IsAssignableFrom(prop.PropertyType.GetGenericTypeDefinition())))
            {
                var value = prop.GetValue(obj);               
                if (value != null)
                    return value as IEnumerable<object>;
            }
            return null;
        }
    }
}

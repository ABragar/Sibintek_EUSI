using System;
using System.Linq;
using System.Reflection;
using Base.Attributes;
using Base.Utils.Common;

namespace Base
{
    public static class BaseObjectExtensions
    {
        public static object ToObject(this IBaseObject obj, Type type)
        {
            return ObjectHelper.CreateAndCopyObject(obj, type, new Type[] { typeof(IBaseObject) });
        }
        public static object ToObject(this IBaseObject obj, Type type, string[] exceptProperties = null)
        {
            return ObjectHelper.CreateAndCopyObject(obj, type, new Type[] { typeof(IBaseObject) }, exceptProperties);
        }
        public static T ToObject<T>(this IBaseObject obj) where T : BaseObject
        {
            return ObjectHelper.CreateAndCopyObject<T>(obj);
        }

        public static void ToObject<T>(this IBaseObject obj, T obj_dest, string[] exceptProperties = null) where T : IBaseObject
        {
            ObjectHelper.CopyObject(obj, obj_dest, new Type[] { typeof(IBaseObject) }, exceptProperties);
        }

        public static void ToObject<T>(this IBaseObject obj, T obj_dest, bool systemProperties) where T : IBaseObject
        {
            string[] exceptProperties = null;

            if (!systemProperties)
            {
                exceptProperties = obj.GetSystemProperties().Select(p => p.Name).ToArray();
            }

            ObjectHelper.CopyObject(obj, obj_dest, new Type[] { typeof(IBaseObject) }, exceptProperties);
        }

        public static PropertyInfo[] GetSystemProperties(this IBaseObject obj)
        {
            return obj.GetType().GetProperties().Where(m => m.GetCustomAttribute<SystemPropertyAttribute>() != null).ToArray();
        }
    }
}
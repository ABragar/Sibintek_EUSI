using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Base.Utils.Common
{
    public static class ObjectHelper
    {
        public static void CopyObject(object objSrc, object objDest, Type[] exceptTypes = null, string[] exceptProperties = null)
        {
            PropertyInfo[] srcProps = objSrc.GetType().GetProperties().Where(p =>
            {
                ReadOnlyAttribute readOnlyAttr = p.GetCustomAttribute<ReadOnlyAttribute>();

                return readOnlyAttr == null || !readOnlyAttr.IsReadOnly;
            }).ToArray();

            if (exceptTypes != null)
            {
                foreach (Type type in exceptTypes)
                {
                    //TODO: переписать!!!
                    srcProps = srcProps.Where(p => !type.IsAssignableFrom(p.PropertyType)).ToArray();

                    List<PropertyInfo> list = srcProps.ToList();

                    foreach (PropertyInfo pi in srcProps)
                    {
                        Type genericType;

                        if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        {
                            genericType = pi.PropertyType;
                        }
                        else
                        {
                            genericType = pi.PropertyType.GetInterface("IEnumerable`1");
                        }

                        if (genericType != null && type.IsAssignableFrom(genericType.GenericTypeArguments[0]))
                        {
                            list.Remove(pi);
                        }
                    }

                    srcProps = list.ToArray();
                }
            }

            if (exceptProperties != null)
            {
                srcProps = srcProps.Where(p => !exceptProperties.Contains(p.Name)).ToArray();
            }

            CopyPropertyValues(objSrc, objDest, srcProps);
        }

        public static void CopyPropertyValues(object objectSrouce, object dest, PropertyInfo[] props)
        {
            PropertyInfo[] destProps = dest.GetType().GetProperties();

            foreach (PropertyInfo prSrc in props)
            {
                if (prSrc.CanRead)
                {
                    PropertyInfo prDest = destProps.FirstOrDefault(p => p.Name == prSrc.Name);

                    if (prDest != null && prDest.CanWrite)
                    {
                        if (prDest.PropertyType.Name == prSrc.PropertyType.Name)
                        {
                            prDest.SetValue(dest, prSrc.GetValue(objectSrouce));
                        }
                    }
                }
            }
        }

        public static object CreateAndCopyObject(object objSrc, Type type, Type[] exceptTypes = null, string[] exceptProperties = null)
        {
            object objDest = Activator.CreateInstance(type);

            CopyObject(objSrc, objDest, exceptTypes, exceptProperties);

            return objDest;
        }


        public static T CreateAndCopyObject<T>(object objSrc, Type[] exceptTypes = null, string[] exceptProperties = null)
        {
            return (T)CreateAndCopyObject(objSrc, typeof(T), exceptTypes, exceptProperties);
        }

        public static T CreateAndCopyObject<T>(object objSrc, PropertyInfo[] exceptProperties)
        {
            var props = exceptProperties.Select(x => x.Name).ToArray();
            return (T)CreateAndCopyObject(objSrc, typeof(T), null, props);
        }
    }
}

using Base.Utils.Common.Attributes;
using Base.Utils.Common.Maybe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Base
{
    public static class ReflectionHelper
    {
        public static bool IsEnum(this Type type)
        {
            return type.IsEnum || (type.IsGenericType && Nullable.GetUnderlyingType(type) != null && type.GetGenericArguments()[0].IsEnum);
        }

        public static bool IsBaseObject(this Type type)
        {
            return typeof(BaseObject).IsAssignableFrom(type);
        }

        public static Type GetBaseObjectType(this Type type)
        {
            if (!type.IsBaseObject()) return null;
            return type.IsDynamicProxy() ? type.BaseType : type;
        }

        public static bool IsDynamicProxy(this Type type)
        {
            return type.Namespace == "System.Data.Entity.DynamicProxies";
        }

        public static bool IsTypeCollection(this Type collection, Type type)
        {
            return collection.IsGenericType && type.IsAssignableFrom(collection.GetGenericArguments()[0]);
        }

        public static bool IsBaseCollection(this Type type)
        {
            return type.IsGenericType && IsBaseObject(type.GetGenericArguments()[0]);
        }

        public static Type GetGenericType(this Type collection)
        {
            Type genericType = null;

            if (collection.IsGenericType && collection.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                genericType = collection;
            }
            else
            {
                genericType = collection.GetInterface("IEnumerable`1");
            }

            return genericType;
        }

        public static bool IsAssignableFromBase(this Type type)
        {
            return type.IsBaseObject() || type.IsBaseCollection();
        }


        private static List<PropertyInfo> GetAllProperties(this Type container, Type type, BindingFlags flags, List<PropertyInfo> props = null, List<Type> analyzed = null)
        {
            if (props == null)
            {
                props = new List<PropertyInfo>();
            }

            if (analyzed == null)
            {
                analyzed = new List<Type>();
            }

            foreach (PropertyInfo prop in container.GetProperties(flags))
            {
                if (!analyzed.Contains(prop.PropertyType))
                {
                    analyzed.Add(prop.PropertyType);

                    if (type.IsAssignableFrom(prop.PropertyType))
                    {
                        props.Add(prop);
                    }
                    else
                    {
                        props = prop.PropertyType.GetAllProperties(type, flags, props, analyzed);
                    }
                }
            }

            return props;
        }

        public static bool HasTypeProperty(this Type container, Type type)
        {
            return container.GetAllProperties(type, BindingFlags.Public | BindingFlags.Instance).Count != 0;
        }

        public static bool IsFullTextSearchEnabled(this Type type)
        {
            var attr = type.GetCustomAttribute<EnableFullTextSearchAttribute>();

            if (attr != null)
            {
                return attr.Enabled;
            }

            return false;
        }

        public static Type GetEntryOfUnboundedTypeOfCollection(this Type target, Type type)
        {
            return target
                .GetInterfaces()
                .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .With(x => x.GetGenericArguments())
                .With(x => x[0])
                .With(x => TypeHierarchy(x).FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == type))
                .With(x => x.GetGenericArguments())
                .With(x => x[0]);
        }

        public static Type GetEntryOfUnboundedTypeOf(this Type target, Type type, Type check)
        {
            return target
                .GetInterfaces()
                .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == check)
                .With(x => x.GetGenericArguments())
                .With(x => x[0])
                .With(x => TypeHierarchy(x).FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == type))
                .With(x => x.GetGenericArguments())
                .With(x => x[0]);
        }

        public static IEnumerable<Type> TypeHierarchy(this Type type)
        {
            yield return type;

            if (type.BaseType != null)
                foreach (var t in TypeHierarchy(type.BaseType))
                    yield return t;
        }

        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            if (givenType == null || genericType == null)
            {
                return false;
            }

            return givenType == genericType
              || givenType.MapsToGenericTypeDefinition(genericType)
              || givenType.HasInterfaceThatMapsToGenericTypeDefinition(genericType)
              || givenType.BaseType.IsAssignableToGenericType(genericType);
        }

        public static bool HasInterfaceThatMapsToGenericTypeDefinition(this Type givenType, Type genericType)
        {
            return givenType
              .GetInterfaces()
              .Where(it => it.IsGenericType)
              .Any(it => it.GetGenericTypeDefinition() == genericType);
        }

        private static bool MapsToGenericTypeDefinition(this Type givenType, Type genericType)
        {
            return genericType.IsGenericTypeDefinition
              && givenType.IsGenericType
              && givenType.GetGenericTypeDefinition() == genericType;
        }

        public static string GetTypeName(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var btype = type.IsDynamicProxy() ? type.BaseType ?? type : type;

            return $"{btype.FullName}, {btype.Assembly.FullName.Split(',')[0]}";
        }

        public static Type GetEnumType(this Type type)
        {
            return type.IsNullable() ? type.GetGenericArguments()[0] : type;
        }

        public static bool DoesTypeSupportGenericInterface(this Type type, Type genericInterfaceType)
        {
            if (type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterfaceType))
                return true;

            return false;
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
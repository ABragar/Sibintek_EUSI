using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Base.UI.Helpers
{
    public static class UIHelper
    {
        private static int _increment;

        public static string CreateSystemName(string prefix)
        {
            return $"{prefix}_sys_{unchecked((uint)Interlocked.Increment(ref _increment))}";
        }
    }

    /// <summary>
    ///     Конструктор динамического класса по набору свойств
    /// </summary>
    public static class LinqRuntimeTypeBuilder
    {
        private static readonly AssemblyName AssemblyName = new AssemblyName { Name = "DynamicLinqTypes" };
        private static readonly ModuleBuilder ModuleBuilder;
        private static readonly Dictionary<string, Type> builtTypes = new Dictionary<string, Type>();

        static LinqRuntimeTypeBuilder()
        {
            ModuleBuilder =
                Thread.GetDomain()
                    .DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run)
                    .DefineDynamicModule(AssemblyName.Name);
        }

        private static string GetTypeKey(Dictionary<string, Type> fields)
        {
            return fields.Aggregate(string.Empty, (current, field) => current + field.Key + ";" + field.Value.Name + ";");
        }

        private static Type GetDynamicType(Dictionary<string, Type> fields)
        {
            if (null == fields)
                throw new ArgumentNullException(nameof(fields));
            if (0 == fields.Count)
                throw new ArgumentOutOfRangeException(nameof(fields), "fields must have at least 1 field definition");

            try
            {
                Monitor.Enter(builtTypes);
                var className = GetTypeKey(fields);

                // Ограничение на имя - 1024 символа. Лишнее отрежем
                if (className.Length > 1024)
                    className = className.Substring(0, 1022);

                if (builtTypes.ContainsKey(className))
                    return builtTypes[className];

                var typeBuilder = ModuleBuilder.DefineType(className,
                    TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);

                foreach (var field in fields)
                    typeBuilder.DefineField(field.Key, field.Value, FieldAttributes.Public);

                builtTypes[className] = typeBuilder.CreateType();

                return builtTypes[className];
            }
            finally
            {
                Monitor.Exit(builtTypes);
            }
        }

        public static Type GetDynamicType(IEnumerable<PropertyInfo> fields)
        {
            return GetDynamicType(fields.ToDictionary(f => f.Name, f => f.PropertyType));
        }
    }
}
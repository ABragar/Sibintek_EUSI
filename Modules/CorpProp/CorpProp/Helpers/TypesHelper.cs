using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Helpers
{
    /// <summary>
    /// Помощник по работе с типами и сборками.
    /// </summary>
    public static class TypesHelper
    {
        /// <summary>
        /// Находит и возвращает тип по его полному наименованию в текущей сборке. 
        /// </summary>
        /// <param name="typeFullName">Полное наименование типа.</param>
        /// <returns>Тип.</returns>
        public static Type GetTypeByFullName(string typeFullName)
        {
            try
            {
                Assembly currentAssem = Assembly.GetExecutingAssembly();
                Type tt = currentAssem.GetTypes().Where(n => n.FullName.ToLower() == typeFullName.ToLower()).FirstOrDefault();
                if (tt != null)
                    return tt;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
            return typeof(object);
        }

        /// <summary>
        /// Находит и возвращает тип по его наименованию в указанной сборке. 
        /// </summary>
        /// <param name="typeName">Наименование типа.</param>
        /// <param name="assembly">Сборка</param>
        /// <returns>Тип.</returns>
        public static Type GetTypeByName(string typeName, Assembly[] assemblys)
        {
            try
            {
                Type tt = assemblys
                    .SelectMany(s => s.GetTypes())
                    .FirstOrDefault(n => 
                       !n.IsAbstract
                    && !n.IsInterface 
                    && !n.IsPrimitive
                    && n.IsSubclassOf(typeof(Base.BaseObject))
                    && n.Name.ToLower() == typeName.ToLower());
                if (tt != null)
                    return tt;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// Находит и возвращает тип по его наименованию в текущей сборке. 
        /// </summary>
        /// <param name="typeName">Наименование типа.</param>
        /// <returns>Тип.</returns>
        public static Type GetTypeByName(string typeName)
        {
            //Assembly currentAssem = Assembly.GetExecutingAssembly();
            var ass = AppDomain.CurrentDomain.GetAssemblies();
            return GetTypeByName(typeName, ass);
        }

        /// <summary>
        /// Возвращает полные наименования родительских типов для текущего типа, наследуемых от TypeObject.
        /// </summary>
        /// <param name="tt">Тип, наименования роидельских классов которого нужно найти. Наследуемый от TypeObject.</param>
        /// <param name="str">Строка, передающая наименование дочернего типа.</param>
        /// <returns>Строка, представляющая перечиления полных наименований родительских типов, в нижнем регистре, разделенные точкой с запятой.</returns>
        public static string GetAllBaseTypeNames(Type tt, string str = null)
        {
            if (String.IsNullOrEmpty(str))
                str = "";
            try
            {                
                if (typeof(TypeObject).IsAssignableFrom(tt))                
                    str += (String.IsNullOrEmpty(str))? tt.FullName.ToLower() : (";" + tt.FullName.ToLower());

                if (typeof(TypeObject).IsAssignableFrom(tt.BaseType))
                    str = GetAllBaseTypeNames(tt.BaseType, str);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
            return str;
        }

        public static string GetAllTypeNames(Type tt, string str = null)
        {
            if (String.IsNullOrEmpty(str))
                str = "";
            try
            {
                if (typeof(TypeObject).IsAssignableFrom(tt))
                    str += (String.IsNullOrEmpty(str)) ? tt.Name.ToLower() : (";" + tt.Name.ToLower());

                if (typeof(TypeObject).IsAssignableFrom(tt.BaseType))
                    str = GetAllTypeNames(tt.BaseType, str);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
            return str;
        }
    }
}

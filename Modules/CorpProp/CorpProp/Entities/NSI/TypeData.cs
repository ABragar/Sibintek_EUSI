using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник типов данных.
    /// </summary>
    [EnableFullTextSearch]
    public class TypeData : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса TypeData.
        /// </summary>
        public TypeData()
        {

        }

        ///// <summary>
        ///// Получает или задает полное наименование типа.
        ///// </summary>
        //[DetailView]
        //[MaxLength(1000)]
        //public string DataTypeName { get; set; }

        ///// <summary>
        ///// Получает или задает наименование сборки.
        ///// </summary>
        //[DetailView]
        //[MaxLength(1000)]
        //public string DataAssemblyName { get; set; }

        ///// <summary>
        ///// Получает полное наименование типа.
        ///// </summary>      
        //public string DataTypeFullName => DataGetTypeFullName();

        ///// <summary>
        ///// Возвращает полное наименование типа
        ///// </summary>
        ///// <returns>Полное наименование типа в нижнем регистре.</returns>
        //public virtual string DataGetTypeFullName()
        //{
        //    string str = "";
        //    if (!String.IsNullOrEmpty(DataTypeName))
        //        return DataTypeName.ToLower();
        //    return str;
        //}

        //public Type DataObjectType => DataGetObjectType();

        ///// <summary>
        ///// Возвращает тип.
        ///// </summary>
        ///// <returns>Тип.</returns>
        //protected Type DataGetObjectType()
        //{
        //    //TODO этот-же механизм в BaseObjectType надо привести к общему знаменателю.
        //    var dataAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName == DataAssemblyName) ?? Assembly.GetExecutingAssembly();
        //    var fullname = DataTypeFullName;
        //    var tt = dataAssembly.GetTypes().FirstOrDefault(n => n.FullName?.ToLower() == fullname);
        //    return tt;
        //    //TODO перенести в тест
        //    //var xc = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.FullName.Contains("mscorlib"));
        //    //var x = xc.GetType("System.String");
        //}
    }
}

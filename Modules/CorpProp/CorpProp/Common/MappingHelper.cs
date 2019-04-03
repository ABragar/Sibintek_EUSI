using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Base;

namespace CorpProp.Common
{
    public static class MappingHelper
    {
        public static void MapCopy<TIn, TOut>(TIn objIn, TOut objOut)
        where TIn : IBaseObject
        where TOut : IBaseObject
        {
            Type inputType = typeof(TIn);
            Type outputType = typeof(TOut);
            PropertyInfo[] properties = inputType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            FieldInfo[] fields = inputType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            foreach (PropertyInfo property in properties)
            {
                try
                {
                    var propertyInfo = outputType.GetProperty(property.Name);
                    if (propertyInfo != null)
                        try
                        {
                            propertyInfo.SetValue(objOut, property.GetValue(objIn, null), null);
                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                    else
                        throw new NotSupportedException($"Не найдено одноименное свойство {property.Name} объекта {inputType.Name} в объекте {outputType.Name}");
                }
                catch (ArgumentException) { } // For Get-only-properties
            }
            foreach (FieldInfo field in fields)
            {
                try
                {
                    field.SetValue(objOut, field.GetValue(objIn));
                }
                catch (Exception ex)
                {
                    //TODO: обработать
                    
                }
            }
        }

    }

    //public class MapBuilder<TMap>
    //{
    //    private List<object> Objects { get; set; }

    //    public MapBuilder<TMap> AddObject<Tin>(Tin input, string objectProperyName)
    //    {
    //        Objects.Add(input);
    //        return this;
    //    }

    //    public IQueryable<TMap> Mapp(IQueryable query)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}

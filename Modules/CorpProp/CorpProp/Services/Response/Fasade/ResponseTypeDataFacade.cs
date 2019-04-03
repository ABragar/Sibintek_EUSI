using System;
using System.Collections.Generic;
using System.Linq;
using CorpProp.Entities.NSI;

namespace CorpProp.Services.Response.Fasade
{
    public static class ResponseTypeDataFacade
    {
        
        //Коды стандартных типов данных
        public const string BooleanCode = "Boolean";
        public const string IntCode = "Integer";
        public const string DecimalCode = "Decimal";
        public const string StringCode = "String";
        public const string DateTimeCode = "Date";



        public static Type GetSimpleTypeOrNull(string code)
        {
            switch (code)
            {
                case BooleanCode:
                    return typeof(bool);
                case IntCode:
                    return typeof(int);
                case DecimalCode:
                    return typeof(decimal);
                case StringCode:
                    return typeof(string);
                case DateTimeCode:
                    return typeof(DateTime);
                default:
                    return null;
            }
        }

        public static Type GetColumnType(TypeData typeData)
        {
            return GetSimpleTypeOrNull(typeData.Code) ?? typeof(int);
        }

        /// <summary>
        /// Получение коллекции по типу данных
        /// </summary>
        /// <remarks>Для каждого стандартного типа данных своя коллекция, а для реестров, справочников системы предусмотрена одна коллекция </remarks>
        /// <returns></returns>
        public static IQueryable GetQueryByCode(ICellsData cellData, TypeData typeData)
        {
            switch (typeData.Code)
            {
                case BooleanCode:
                    return cellData.Booleans;
                case IntCode:
                    return cellData.Ints;
                case DecimalCode:
                    return cellData.Decimals;
                case StringCode:
                    return cellData.Strings;
                case DateTimeCode:
                    return cellData.DateTimes;
                default:
                    return cellData.Dicts;
            }
        }

        public static readonly Dictionary<Type, bool> IsNumericDict = new Dictionary<Type, bool>()
                                                               {
                                                                   { typeof(decimal), true },
                                                                   { typeof(double), true },
                                                                   { typeof(float), true },
                                                                   { typeof(int), true },
                                                               };
    }
}

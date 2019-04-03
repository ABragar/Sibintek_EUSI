using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Base.Utils.Common.Attributes;
using Base.Utils.Common.Caching;

namespace Base.Utils.Common
{
    public static class FullTextSearchExtensions
    {
        private const char SearchDelimiter = ';';

        public static string RemoveSpecialCharacters(this string str)
        {
            return str.Replace("'", @"\'").Replace("\"", @"\'");

            // падает если "'''..,l;'\\./.;.;.'/.; 
            //return Regex.Replace(str, "[^a-zA-Z0-9a-яА-Я]+", " ", RegexOptions.Compiled);
        }

        public static Dictionary<string, PropertyInfo> WrapQuery(Dictionary<string, PropertyInfo> props)
        {
            Dictionary<string, PropertyInfo> strings = new Dictionary<string, PropertyInfo>();

            Regex regex = new Regex(Regex.Escape("[]"));

            foreach (KeyValuePair<string, PropertyInfo> prop in props)
            {
                string query = null;

                if (prop.Key.Contains("[]"))
                {
                    string propPath = prop.Key.Replace("[].", "[]");

                    string propName = propPath;

                    string[] arr = propName.Split("[]".ToCharArray());

                    if (arr.Length > 0)
                    {
                        propName = arr[arr.Length - 1];
                    }

                    var isnullble = "";
                    if (prop.Value.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        isnullble += ".Value";
                    }
                    query = propPath + " != null && " + propName + isnullble + ((prop.Value.PropertyType != typeof(string)) ? ".ToString()" : "") + ".ToUpper().Contains({0}.ToUpper())";

                    while (query.Contains("[]"))
                    {
                        query = regex.Replace(query, ".Where(", 1);
                        query += ").Any()";
                    }
                }
                else
                {
                    var isnullble = "";
                    if (prop.Value.PropertyType.IsGenericType && prop.Value.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        isnullble += ".Value";
                    }
                    query = prop.Key + " != null && " + prop.Key + isnullble + ((prop.Value.PropertyType != typeof(string)) ? ".ToString()" : "") + ".ToUpper().Contains({0}.ToUpper())";
                }

                strings.Add(query, prop.Value);
            }

            return strings;
        }

        public static Dictionary<string, PropertyInfo> GetAttributedProperties(Type type, Dictionary<string, PropertyInfo> props = null, PropertyInfo currentProp = null, bool isCollection = false, int depth = 1, string path = "")
        {
            if (props == null)
            {
                props = new Dictionary<string, PropertyInfo>();
            }

            if (depth >= 0)
            {
                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    FullTextSearchPropertyAttribute attr = prop.GetCustomAttribute<FullTextSearchPropertyAttribute>();

                    string newPath = path;

                    if (attr != null)
                    {
                        Type collectionType = prop.PropertyType.GetInterface("IEnumerable`1");

                        if (currentProp != null)
                        {
                            if (collectionType != null && prop.PropertyType != typeof(String))
                            {
                                newPath = newPath + "." + prop.Name + "[]";
                            }
                            else
                            {
                                newPath = newPath + "." + prop.Name;
                            }
                        }
                        else
                        {
                            if (collectionType != null && prop.PropertyType != typeof(String))
                            {
                                newPath = prop.Name + "[]";
                            }
                            else
                            {
                                newPath = prop.Name;
                            }
                        }

                        if (!props.ContainsKey(newPath))
                        {
                            props.Add(newPath, prop);

                            if (prop.PropertyType != typeof(String) && collectionType != null)
                            {
                                Type collectionEntryType = collectionType.GetGenericArguments()[0];

                                GetAttributedProperties(collectionEntryType, props, prop, true, depth: attr.Depth - 1, path: newPath);
                            }
                            else
                            {
                                GetAttributedProperties(prop.PropertyType, props, prop, depth: attr.Depth - 1, path: newPath);
                            }
                        }
                    }
                }
            }

            return props;
        }
        
        public static IQueryable<T> FullTextSearch<T>(this IQueryable<T> query, string searchStr, ISimpleCacheWrapper cache, string[] columns = null, IDictionary<string, string> lookupProps = null)
        {
            return (IQueryable<T>)FullTextSearch((IQueryable)query, searchStr, cache, columns, lookupProps);
        }


        public static CacheAccessor<Dictionary<string, PropertyInfo>> FullTextSearchAccessor = new CacheAccessor<Dictionary<string, PropertyInfo>>();

        public static IQueryable FullTextSearch(this IQueryable query
            , string searchStr
            , ISimpleCacheWrapper cache
            , string[] columns = null
            , IDictionary<string, string> lookupProps = null)
        {
            if (String.IsNullOrEmpty(searchStr) || String.IsNullOrWhiteSpace(searchStr))
            {
                return query;
            }

            string searchStringNew = null;
            var type = query.GetType().GetGenericArguments()[0];
            var searchProps = (columns != null) ? GetColumnProperties(type, columns, lookupProps: lookupProps) : GetAttributedProperties(type);
            var props = cache.GetOrAdd(FullTextSearchAccessor, type.ToString(), () => WrapQuery(searchProps));
            string where = null;

            if (!searchStr.Contains(SearchDelimiter))
            {
                var arrSearch = MorphologyHelper.SearchString(searchStr.RemoveSpecialCharacters()).Take(3);
                var enumerable = arrSearch as string[] ?? arrSearch.ToArray();
                if (!enumerable.Any())
                    return query;
                searchStringNew = string.Join(";", enumerable);
            }

            searchStringNew = searchStringNew ?? searchStr;
            where = GetWhereString(searchStringNew, props);

            return where == null ? query : query.Where(where, GetWhereParams(searchStringNew));

        }


        public static Dictionary<string, PropertyInfo> GetColumnProperties(
            Type type
            , string[] columns
            , IDictionary<string, string> lookupProps = null
            , Dictionary<string, PropertyInfo> props = null
            , PropertyInfo currentProp = null
            , bool isCollection = false
            , int depth = 1
            , string path = ""
            )
        {
            if (props == null)
            {
                props = new Dictionary<string, PropertyInfo>();
            }

            if (depth >= 0)
            {
                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => columns.Contains(p.Name)))
                {
                    string newPath = path;

                    Type collectionType = prop.PropertyType.GetInterface("IEnumerable`1");

                    if (currentProp != null)
                    {
                        if (collectionType != null && prop.PropertyType != typeof(String))
                        {
                            newPath = newPath + "." + prop.Name + "[]";
                        }
                        else
                        {
                            newPath = newPath + "." + prop.Name;
                        }
                    }
                    else
                    {
                        if (collectionType != null && prop.PropertyType != typeof(String))
                        {
                            newPath = prop.Name + "[]";
                        }
                        else
                        {
                            if (lookupProps != null && lookupProps.Keys.Contains(prop.PropertyType.Name))
                                newPath = prop.Name + "." + lookupProps[prop.PropertyType.Name];
                            else
                                newPath = prop.Name;
                        }
                    }

                    if (!props.ContainsKey(newPath))
                    {
                        props.Add(newPath, prop);

                        if (prop.PropertyType != typeof(String) && collectionType != null)
                        {
                            Type collectionEntryType = collectionType.GetGenericArguments()[0];
                            //TODO: изменить поиск по коллекции
                            GetAttributedProperties(collectionEntryType, props, prop, true, depth: 1, path: newPath);
                        }

                    }
                }
            }

            return props;
        }
        private static object[] GetWhereParams(string searchString)
        {
            return searchString.Trim().Split(new[] { SearchDelimiter }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim()).Cast<object>()
                .ToArray();
        }

        private static string GetWhereString(string searchStr, Dictionary<string, PropertyInfo> props)
        {

            if (props.Count == 0)
                return null;

            var criteria = new StringBuilder();

            criteria.Append("(")
                .AddWhereCriteria(searchStr, props)
                .Remove(criteria.Length - 4, 4)
                .Append(") and ");


            criteria.Remove(criteria.Length - 5, 5);

            return criteria.ToString();

        }

        private static StringBuilder AddWhereCriteria(this StringBuilder criteria, string searchWord, IDictionary<string, PropertyInfo> props)
        {
            if (criteria == null || searchWord == null || props == null)
            {
                return criteria;
            }

            foreach (KeyValuePair<string, PropertyInfo> prop in props)
            {
                if (searchWord.Trim().Any(c => c != SearchDelimiter) && searchWord.Contains(SearchDelimiter))
                {
                    var words = searchWord.Split(new[] { SearchDelimiter }, StringSplitOptions.RemoveEmptyEntries);
                    var i = 0;
                    foreach (var word in words)
                    {
                        criteria.Append(String.Format(prop.Key + " or ", $"@{i++}"));
                    }
                }
                else
                {
                    criteria.Append(String.Format("( " + prop.Key + ")" + " or ", "@0"));
                }
            }

            return criteria;
        }

        //private static Expression<Func<T, bool>> BuildLambda<T>(Type type, string str) where T : class
        //{
        //    Dictionary<PropertyInfo, string> props = WrapQuery(GetAttributedProperties(type));

        //    var innerItem = Expression.Parameter(type, "f");

        //    var innerProperty = Expression.Property(innerItem, "Content");

        //    MethodInfo innerMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

        //    var innerSearchExpression = Expression.Constant(str, typeof(string));

        //    var innerMethodExpression = Expression.Call(innerProperty, innerMethod, new[] { innerSearchExpression });

        //    Expression<Func<T, bool>> innerLambda = Expression.Lambda<Func<T, bool>>(innerMethodExpression, innerItem);

        //    return null;
        //}
    }
}

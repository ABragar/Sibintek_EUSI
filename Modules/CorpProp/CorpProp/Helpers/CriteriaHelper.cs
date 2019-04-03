using Base;
using Base.DAL;
using CorpProp.Common;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.Security;
using CorpProp.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AppContext = Base.Ambient.AppContext;

namespace CorpProp.Helpers
{
    /// <summary>
    /// Системные переменные в критериях.
    /// </summary>
    public static class CriteriaValiables
    {
        public const string currentUserID = "@currentUserID";
        public const string currentSibUserID = "@currentSibUserID";
        public const string userIDEUP = "@userIDEUP";
        public const string isFromCauk = "@isFromCauk";
        public const string isFromService = "@isFromService";
        public const string isAdmin = "@isAdmin";
        public const string userTerritory = "@userTerritory";
        public const string userAgents = "@userAgents";
        public const string userBelows = "@userBelows";
        public const string datetimeNow = "@Now";
        public const string datetimeNowDate = "@currentDate";
        public const string pattern = @"\@[^\d]\w*";
    }

    public static class CriteriaHelper
    {

        public static IQueryable<T> AddPermCriteria<T>(this IQueryable<T> source, IUnitOfWork uow, List<ObjectPermission> perms) where T : BaseObject
        {
            var criteries = perms.Select(s => s.Criteria).Distinct().ToList<string>();
            var condition = "";
            foreach (var criteria in criteries)
            {
                //выстраиваем критерии группами выражений с оператором ИЛИ 
                if (!String.IsNullOrEmpty(condition))
                    condition += " || ";
                condition += $"( {criteria} )";
            }
            if (!String.IsNullOrEmpty(condition))
                return source.SetCriteria(uow, condition);

            return source;
        }


        public static IQueryable<T> SetCriteria<T>(this IQueryable<T> source, IUnitOfWork uow, string criteria) where T : BaseObject
        {

            if (String.IsNullOrEmpty(criteria)) return source;
            List<object> listParams = new List<object>();
            MatchCollection matches = Regex.Matches(criteria, CriteriaValiables.pattern);

            for (int i = 0; i < matches.Count; i++)
            {
                criteria = criteria.ReplaceVariable(uow, ref listParams);
            }

            return source.Where(criteria, listParams.ToArray());
        }


        public static string ReplaceVariable(this string source, IUnitOfWork uow, ref List<object> parms)
        {
            MatchCollection matches = Regex.Matches(source, CriteriaValiables.pattern);
            var str = source;
            if (matches.Count > 0)
            {

                var item = matches[0];
                str = str.ReplaceAt(item.Index, item.Length, "@" + parms.Count.ToString());

                var t = item.Value == CriteriaValiables.userIDEUP;
                switch (item.Value)
                {
                    case CriteriaValiables.currentSibUserID:
                        parms.Add(GetSibUserID(uow));
                        break;
                    case CriteriaValiables.currentUserID:
                        parms.Add(AppContext.SecurityUser.ID);
                        break;
                    case CriteriaValiables.datetimeNow:
                        parms.Add(DateTime.Now);
                        break;
                    case CriteriaValiables.datetimeNowDate:
                        parms.Add(DateTime.Now.Date);
                        break;
                    case CriteriaValiables.isAdmin:
                        parms.Add(AppContext.SecurityUser.IsAdmin);
                        break;
                    case CriteriaValiables.isFromCauk:
                        parms.Add(GetIsFromCauk(uow));
                        break;
                    case CriteriaValiables.isFromService:
                        parms.Add(GetIsFromService(uow));
                        break;
                    case CriteriaValiables.userIDEUP:
                        parms.Add(GetUserIDEUP(uow));
                        break;
                    case CriteriaValiables.userTerritory:
                        str = source;
                        paramsAdd(parms, ref str, item, uow, GetUserTerritory);
                        break;
                    case CriteriaValiables.userAgents:
                        str = source;
                        paramsAdd(parms, ref str, item, uow, GetUserAgents);
                        break;
                    case CriteriaValiables.userBelows:
                        str = source;
                        paramsAdd(parms, ref str, item, uow, GetUserBelows);
                        break;
                    default:
                        break;
                }
            }
            return str;
        }

        private static string paramsAdd(List<object> parms, ref string str, Match item, IUnitOfWork uow, Func<IUnitOfWork, string[]> GetValuesAction)
        {
            var userTerritory = GetValuesAction(uow);
            if (CheckContains(str, item))
                FixContains(parms, ref str, item, userTerritory);
            else
                parms.Add(userTerritory);
            return str;
        }

        private static bool CheckContains(string str, Match item)
        {
            var contains = ".Contains";
            return str.IndexOf(contains, item.Index, item.Length + contains.Length) > -1;
        }

        private static void FixContains(List<object> parms, ref string str, Match item, string[] items)
        {
            var currentparamIndex = parms.Count;
            int from;
            int to;
            string param = ContainsParam(item, str, out from, out to);
            var condition = string.Join(" || ", items.Select(xx => $"{param} == @{currentparamIndex++}"));
            str = str.Remove(from, to - from + 1);
            str = str.Insert(from, string.IsNullOrWhiteSpace(condition) ? "false" : $"({condition})");

            if (items.Length <= 0)
                parms.Add(Array.Empty<string>());
            else
                parms.AddRange(items);

        }

        private static string ContainsParam(Match match, string source, out int from, out int to)
        {
            var sourceCrop = source.Substring(match.Index, source.Length - match.Index);
            var subsIndex = sourceCrop.IndexOf(")");
            from = match.Index;

            var valueIndex = sourceCrop.IndexOf("(");
            var value = sourceCrop.Substring(valueIndex + 1, subsIndex - valueIndex - 1);
            to = match.Index + subsIndex;
            return value;

        }

        private static string GetUserIDEUP(IUnitOfWork uow)
        {
            return AppContext.SecurityUser.GetUserIDEUP(uow);
        }

        private static bool GetIsFromCauk(IUnitOfWork uow)
        {
            return AppContext.SecurityUser.IsFromCauk(uow);
        }
        private static bool GetIsFromService(IUnitOfWork uow)
        {
            return AppContext.SecurityUser.IsFromService(uow);
        }


        private static int GetSibUserID(IUnitOfWork uow)
        {
            return AppContext.SecurityUser.GetSibUserID(uow);
        }

        private static string[] GetUserTerritory(IUnitOfWork uow)
        {
            return AppContext.SecurityUser.GetTerritorys(uow).ToArray<string>();
        }

        private static string[] GetUserAgents(IUnitOfWork uow)
        {
            return AppContext.SecurityUser.GetUserAgents(uow).ToArray<string>();
        }

        private static string[] GetUserBelows(IUnitOfWork uow)
        {
            return AppContext.SecurityUser.GetUserBelows(uow).ToArray<string>();
        }


        public static string ReplaceAt(this string str, int index, int length, string replace)
        {
            return str.Remove(index, Math.Min(length, str.Length - index))
                    .Insert(index, replace);
        }
    }

}

using System;
using System.Linq.Expressions;
using Base;
using CorpProp.Entities.Request;
using CorpProp.Entities.Request.ResponseCells;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;

namespace CorpProp.Services.Response.Fasade
{
    public abstract class RequestQuery
    {
        protected RequestQuery()
        {
        }

        const string ColumnPrefix = "column";

        public static string ColumnPropertyAliasNameMaker(int columnId)
        {
            return $"{ColumnPrefix}{columnId}";
        }

        public static int ColumnIdFromColumnPropertyAlias(string columnAlias)
        {
            string id = columnAlias.Substring(ColumnPrefix.Length);
            return int.Parse(id);
        }

        public static string ColumnPropertyAliasNameMaker(IRequestColumn column)
        {
            return ColumnPropertyAliasNameMaker(column.ID);
        }

        protected const string JoinOuterFieldName = "outer";
        protected const string SelectorSortOrderFieldName = "OrderID";

        public static string GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        {
            return ((MemberExpression) memberAccess.Body).Member.Name;
        }

        protected static readonly string RequestCellLinkedColumnName =
            GetMemberName<ResponseCellBase<Type>, int>(x => x.LinkedColumnID);
        protected static readonly string RequestCellLinkedRowName =
            GetMemberName<ResponseCellBase<Type>, int>(x => x.LinkedRowID);
        protected static readonly string RequestValueTheValueName = GetMemberName<ResponseCellBase<Type>, Type>(x => x.Value);
        protected static readonly string RequestCellLinkedResponseIdName =
            GetMemberName<ResponseCellBase<Type>, int>(x => x.LinkedResponseID);
        protected static readonly string RequestCellRequestIdName =
            GetMemberName<ResponseCellBase<Type>, int?>(x => x.LinkedResponse.RequestID);
        protected static readonly string RequestSortOrderName =
            GetMemberName<ResponseCellBase<Type>, double>(x => x.LinkedResponse.SortOrder);
        protected static readonly string UserSocietyFieldName =
            GetMemberName<SibUser, Society>(user => user.Society);
        protected static readonly string BaseObjectIdFieldName =
            GetMemberName<BaseObject, int>(o => o.ID);
    }
}

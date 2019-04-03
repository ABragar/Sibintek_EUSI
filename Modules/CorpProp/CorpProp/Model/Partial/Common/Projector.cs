using Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Base.DAL;
using CorpProp.Common;

namespace CorpProp.Model
{
    /// <summary>
    /// Создание проекции одного типа на другой
    /// </summary>
    public class Projector
    {

        public static Expression<Func<TSource, TDest>> Selector<TSource, TDest>()
        {
            var destType = typeof(TDest);
            var srcType = typeof(TSource);
            var destPropertyInfos = destType.GetProperties(BindingFlags.Instance |
                                                   BindingFlags.Public |
                                                   BindingFlags.SetProperty);

            var sourceParamExpr = Expression.Parameter(typeof(TSource));
            var sourceExpr = Expression.Convert(sourceParamExpr, typeof(TSource));

            var memberBindings = new List<MemberBinding>();
            foreach (var resultProperty in destPropertyInfos)
            {
                var srcPropInfo = srcType.GetProperty(resultProperty.Name,
                                                      BindingFlags.Instance |
                                                      BindingFlags.Public |
                                                      BindingFlags.SetProperty);

                memberBindings.Add(Expression.Bind(resultProperty, Expression.Property(sourceExpr, srcPropInfo.Name)));
            }

            var resultExpr = Expression.MemberInit(Expression.New(typeof(TDest)), memberBindings);
            var mapperExpr = Expression.Lambda<Func<TSource, TDest>>(resultExpr, sourceParamExpr);

            return mapperExpr;
        }

        public static TSource Map<TSource, TDest>(IUnitOfWork unitOfWork, TSource source, Func<IUnitOfWork, TDest, TDest> predicate)
                where TDest : IBaseObject, new()
                where TSource : IBaseObject
        {
            var defaultDest = new TDest();
            if (source != null)
                MappingHelper.MapCopy(source, defaultDest);
            var created = predicate(unitOfWork, defaultDest);
            MappingHelper.MapCopy(created, source);
            return source;
        }
    }
}

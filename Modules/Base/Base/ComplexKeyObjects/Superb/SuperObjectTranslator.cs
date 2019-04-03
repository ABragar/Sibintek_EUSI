using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Base.ComplexKeyObjects.Common;
using Base.DAL;
using Base.Extensions;
using Base.Service;

namespace Base.ComplexKeyObjects.Superb
{
    public class SuperObjectTranslator
    {
        private static readonly MethodInfo AddTranslationMethod = typeof(SuperObjectTranslator).GetMethod(nameof(AddTranslation),
            BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo GetTypeMethod = typeof(SuperObjectTranslator).GetMethod(nameof(CheckType),
            BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(IUnitOfWork) }, null);

        private readonly ITypeNameResolver _name_resolver;
        private readonly ITypeRelationService _relation_service;
        private readonly IEntityTypeResolver _type_resolver;

        public SuperObjectTranslator(ITypeNameResolver name_resolver, ITypeRelationService relation_service, IEntityTypeResolver type_resolver)
        {
            _name_resolver = name_resolver;
            _relation_service = relation_service;
            _type_resolver = type_resolver;
        }

        private IEnumerable<Type> GetDescendantTypes(Type type)
        {

            return _type_resolver.GetEntityTypes().Where(x => x.IsSubclassOf(type));
        }

        public bool IsSuperRootObject(Type type)
        {
            return type.GetInterfaces()
                .Any(x => x.IsConstructedGenericType
                    && x.GetGenericTypeDefinition() == typeof(ISuperObject<>)
                    && x.GetGenericArguments().Single() == type);
        }

        public void InitSuperObject(IUnitOfWork unit_of_work, Type type)
        {

            if (!IsSuperRootObject(type))
                return;

            var checked_types = GetDescendantTypes(type).Where(x => CheckType(type, x, unit_of_work)).ToArray();



            TraceRelations(checked_types, type, type, new Stack<TraceItem>());
        }

        private class TraceItem
        {
            public readonly ParameterExpression Parameter;
            public Expression Body;

            public TraceItem(ParameterExpression parameter)
            {
                Parameter = parameter;
            }
        }

        private void TraceRelations(IReadOnlyCollection<Type> all_types, Type root, Type current, Stack<TraceItem> items)
        {

            if (!current.IsAbstract)
                _relation_service.AddRelation(current, current);

            var name = _name_resolver.GetName(current);

            var item = new TraceItem(Expression.Parameter(root, name))
            {
                Body = Expression.Constant(name, typeof(string))
            };


            items.ForEach(x =>
            {

                var is_type = Expression.TypeIs(x.Parameter, current);


                x.Body = Expression.Condition(is_type, item.Body, x.Body);

            });



            items.Push(item);

            GetFirstDescendantTypes(all_types, current).ForEach(x =>
            {
                _relation_service.AddRelation(current, x);
                TraceRelations(all_types, root, x, items);
            });

            items.Pop();

            AddTranslationMethod.MakeGenericMethod(current, root).Invoke(null, new object[] { item });


        }



        private static IEnumerable<Type> GetFirstDescendantTypes(IReadOnlyCollection<Type> all_types, Type root)
        {

            return all_types.Where(x => x.IsSubclassOf(root) && !all_types.Where(d => d.IsSubclassOf(root)).Any(x.IsSubclassOf));
        }


        private static void AddTranslation<TCurrent, TBase>(TraceItem item)
            where TCurrent : TBase
            where TBase : class, ISuperObject<TBase>
        {

            var property = RemoveConvertVisitor.Instance
                .VisitAndConvert<Expression<Func<TBase, string>>>(x => x.ExtraID, "add_translation");


            var lambda = Expression.Lambda<Func<TBase, string>>(item.Body, item.Parameter);

            Translations.TranslationMap.DefaultMap.AddStrict<TCurrent, TBase, string>(property, lambda);

            if (typeof (TCurrent) != typeof (TBase))
            {
                var property_ex =
                    RemoveConvertVisitor.Instance.VisitAndConvert<Expression<Func<TCurrent, string>>>(x => x.ExtraID,
                        "add_translation");

                var lambda_ex = Expression.Lambda<Func<TCurrent, string>>(item.Body, item.Parameter);

                Translations.TranslationMap.DefaultMap.AddStrict<TCurrent, TCurrent, string>(property_ex, lambda_ex);
            }
        }

        private static bool CheckType(Type base_type, Type current_type, IUnitOfWork unit_of_work)
        {
            return (bool)GetTypeMethod.MakeGenericMethod(base_type, current_type).Invoke(null, new[] { unit_of_work });
        }

        private static bool CheckType<TBase, TCurrent>(IUnitOfWork unit_of_work)
            where TBase : BaseObject
            where TCurrent : TBase
        {
            try
            {
                var test = unit_of_work.GetRepository<TBase>().All().Any(x => x is TCurrent);
                return true;
            }
            catch (NotSupportedException)
            {
                return false;
            }

        }
    }
}
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Base;
using Base.Attributes;
using Base.DAL;
using Base.Extensions;
using Base.UI.ViewModal;
using CorpProp.Entities.Common;
using CorpProp.Entities.Estate;
using CorpProp.Model.Partial.Entities.SeparLand;

namespace WebApi.Helper
{
    static class JoinMnemonicsHelper
    {
        private static ConcurrentDictionary<string, Type> _dictionary = new ConcurrentDictionary<string, Type>();

        private static string BuildSelector(ViewModelConfig config, string[] columns)
        {
            var childSelectors = config.ListView.Columns
                .Where(x => !string.IsNullOrEmpty(x.ChildMnemonic))
                .GroupBy(x => new {x.ChildMnemonic, x.ChildMnemonicType})
                .Select(x => new {Mnemonic = x.Key, Columns = x.ToList()})
                .SelectMany(x => x.Mnemonic.ChildMnemonicType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    //.Where(c => (columns?.Contains(c.PropertyName) ?? true) /* || c.PropertyName.Contains("ID")*/)
                    .Select(y => y.Name)
                    .Select(y => $"{x.Mnemonic.ChildMnemonicType.Name}.{y} as {x.Mnemonic.ChildMnemonicType.Name}_{y}")).Distinct();

            var baseSelectors = config
                .TypeEntity.GetProperties()
                //.Where(x => (columns?.Contains(x.Name) ?? true) || Attribute.IsDefined(x, typeof(SystemPropertyAttribute)))
                .Select(x => x.Name)
                .Select(x => $"BaseQuery.{x}")
                .ToList();

            return $"new( {string.Join(",", baseSelectors.Concat(childSelectors))} )";
        }

        private static Expression GetChildQueryExpression(IUnitOfWork uow, Type type)
        {
            var getRepoMethod = typeof(IUnitOfWork).GetMethod(nameof(IUnitOfWork.GetRepository))
                .MakeGenericMethod(type);
            var repo = getRepoMethod.Invoke(uow, null);
            var dbSet = repo.GetType().GetProperty(nameof(IRepository<BaseObject>.DbSet), BindingFlags.Instance | BindingFlags.Public).GetValue(repo) as IQueryable;
            return dbSet.Expression;
        }

        private static IEnumerable<Tuple<string, Type>> GetFieldsForAnonymousClass(Type basetype, Type childtype)
        {
            var baseFields =  basetype.GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Select(x => Tuple.Create(x.Name, x.FieldType));
            var baseProps = basetype.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(x => Tuple.Create(x.Name, x.PropertyType));
            var childFields = childtype.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Select(x => Tuple.Create($"{childtype.Name}_{x.Name}", x.PropertyType));

            return baseFields.Union(baseProps.Union(childFields));
        }

        private static IEnumerable<Tuple<string, Type>> GetFieldsForAnonymousClass2(Type basetype, Type childtype, bool isFirst)
        {
            var fields = new List<Tuple<string, Type>>();
            if (isFirst)
            {
                fields.Add(Tuple.Create("BaseQuery", basetype));
                fields.Add(Tuple.Create(childtype.Name, childtype));
            }
            else
            {
                fields.AddRange(basetype.GetFields(BindingFlags.Instance | BindingFlags.Public)
                    .Select(x => Tuple.Create(x.Name, x.FieldType)));
                fields.Add(Tuple.Create(childtype.Name, childtype));
            }
            return fields;
        }

        private static IEnumerable<MemberAssignment> CreateAndBindProperties(Type anonClass, ParameterExpression resultParamBase,
            ParameterExpression resultParamChild)
        {
            var resultExpressions = new List<MemberAssignment>();

            foreach (var propertyInfo in resultParamBase.Type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var parameter = Expression.PropertyOrField(resultParamBase, propertyInfo.Name);
                resultExpressions.Add(Expression.Bind(anonClass.GetField(propertyInfo.Name), parameter));
            }

            foreach (var propertyInfo in resultParamBase.Type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                var parameter = Expression.PropertyOrField(resultParamBase, propertyInfo.Name);
                resultExpressions.Add(Expression.Bind(anonClass.GetField(propertyInfo.Name), parameter));
            }

            foreach (var propertyInfo in resultParamChild.Type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                var parameter = Expression.PropertyOrField(resultParamChild, propertyInfo.Name);
                resultExpressions.Add(Expression.Bind(anonClass.GetField($"{resultParamChild.Type.Name}_{propertyInfo.Name}"), parameter));
            }

            return resultExpressions;
        }

        private static IEnumerable<MemberAssignment> CreateAndBindProperties2(Type anonClass, ParameterExpression resultParamBase,
            ParameterExpression resultParamChild, string propertyName, bool isFirst)
        {
            var resultExpressions = new List<MemberAssignment>();

            if (isFirst)
            {
                resultExpressions.Add(Expression.Bind(anonClass.GetField("BaseQuery"), resultParamBase));
                
            }
            else
            {
                foreach (var propertyInfo in resultParamBase.Type.GetFields(BindingFlags.Instance | BindingFlags.Public))
                {
                    var parameter = Expression.PropertyOrField(resultParamBase, propertyInfo.Name);
                    resultExpressions.Add(Expression.Bind(anonClass.GetField(propertyInfo.Name), parameter));
                }
            }

            resultExpressions.Add(Expression.Bind(anonClass.GetField(propertyName), resultParamChild));

            return resultExpressions;
        }

        public  static IQueryable LeftJoin<T>(IQueryable<T> queryIn, IUnitOfWork uow, ViewModelConfig config)
            where T : BaseObject
        {
            var resultExpression = queryIn.Expression;

            var startType = queryIn.ElementType;
            var childMnemonics = config.ListView.Columns
                .Where(x => !string.IsNullOrEmpty(x.ChildMnemonic))
                .Select(x => new {Type = x.ChildMnemonicType, x.ParentProperty, x.ChildProperty}).Distinct();

            foreach (var mnemonic in childMnemonics)
            {
                var anonType = CreateAnonymousType($"AnonymousClass_{mnemonic.GetHashCode()}", GetFieldsForAnonymousClass(startType, mnemonic.Type));

                var resultParamBase = Expression.Parameter(startType, "BaseQuery");
                var resultParamChild = Expression.Parameter(mnemonic.Type, mnemonic.Type.Name);

                var createExp = Expression.New(anonType.GetConstructor(Type.EmptyTypes));
                var init = Expression.MemberInit(createExp, CreateAndBindProperties(anonType, resultParamBase, resultParamChild));

                var selectExp = Expression.Lambda(
                    Expression.GetFuncType(startType, mnemonic.Type, anonType),
                    init, resultParamBase, resultParamChild);

                var param = Expression.Parameter(mnemonic.Type, "x");
                var left = Expression.Convert(Expression.PropertyOrField(param, mnemonic.ChildProperty), 
                    typeof(int?));
                var right = Expression.Convert(Expression.PropertyOrField(resultParamBase, mnemonic.ParentProperty),
                    typeof(int?));


                var body = Expression.Equal(left, right);
                var where = Expression.Lambda(body, param);

                var whereCall = Expression.Call(
                    typeof(Queryable),
                    nameof(Queryable.Where),
                    new[]
                    {
                        mnemonic.Type
                    }, GetChildQueryExpression(uow, mnemonic.Type), where);

                var defaultCall = Expression.Call(
                    typeof(Queryable),
                    nameof(Queryable.DefaultIfEmpty),
                    new[]
                    {
                        mnemonic.Type
                    }, whereCall);

                var defaultExp =
                    Expression.Lambda(Expression.GetFuncType(startType, typeof(IEnumerable<>).MakeGenericType(mnemonic.Type)),
                        defaultCall, resultParamBase);

                resultExpression = Expression.Call(
                    typeof(Queryable),
                    nameof(Queryable.SelectMany),
                    new[]
                    {
                        startType,
                        mnemonic.Type,
                        anonType
                    }, resultExpression, defaultExp, selectExp);

                startType = anonType;
            }

            return queryIn.Provider.CreateQuery(resultExpression);

        }

        public static IQueryable LeftJoin2<T>(IQueryable<T> queryIn, IUnitOfWork uow, ViewModelConfig config)
            where T : BaseObject
        {
            var resultExpression = queryIn.Expression;

            var startType = queryIn.ElementType;
            var childMnemonics = config.ListView.Columns
                .Where(x => !string.IsNullOrEmpty(x.ChildMnemonic))
                .GroupBy(x=>new{ Type = x.ChildMnemonicType, x.ParentProperty, x.ChildProperty })
                .Select(x => x.Key).Distinct();

            var isFirst = true;

            foreach (var mnemonic in childMnemonics)
            {
                var anonType = CreateAnonymousType($"AnonymousClass_{config.Mnemonic}_{mnemonic.GetHashCode()}", GetFieldsForAnonymousClass2(startType, mnemonic.Type, isFirst));

                var resultParamBase = Expression.Parameter(startType, "BaseQuery");
                var resultParamChild = Expression.Parameter(mnemonic.Type, mnemonic.Type.Name);

                var createExp = Expression.New(anonType.GetConstructor(Type.EmptyTypes));
                var init = Expression.MemberInit(createExp, CreateAndBindProperties2(anonType, resultParamBase, resultParamChild, mnemonic.Type.Name, isFirst));

                var selectExp = Expression.Lambda(
                    Expression.GetFuncType(startType, mnemonic.Type, anonType),
                    init, resultParamBase, resultParamChild);
                
                var param = Expression.Parameter(mnemonic.Type, "x");
                var left = Expression.Convert(Expression.PropertyOrField(param, mnemonic.ChildProperty),
                    typeof(int?));
                Expression right;
                if (isFirst)
                {
                    right = Expression.Convert(Expression.PropertyOrField(resultParamBase, mnemonic.ParentProperty),
                        typeof(int?));
                }
                else
                {
                    right = Expression.Convert(
                        Expression.PropertyOrField(Expression.PropertyOrField(resultParamBase, "BaseQuery"),
                            mnemonic.ParentProperty),
                        typeof(int?));
                }

                isFirst = false;
                var body = Expression.Equal(left, right);
                var where = Expression.Lambda(body, param);

                var whereCall = Expression.Call(
                    typeof(Queryable),
                    nameof(Queryable.Where),
                    new[]
                    {
                        mnemonic.Type
                    }, GetChildQueryExpression(uow, mnemonic.Type), where);

                var defaultCall = Expression.Call(
                    typeof(Queryable),
                    nameof(Queryable.DefaultIfEmpty),
                    new[]
                    {
                        mnemonic.Type
                    }, whereCall);

                var defaultExp =
                    Expression.Lambda(Expression.GetFuncType(startType, typeof(IEnumerable<>).MakeGenericType(mnemonic.Type)),
                        defaultCall, resultParamBase);

                resultExpression = Expression.Call(
                    typeof(Queryable),
                    nameof(Queryable.SelectMany),
                    new[]
                    {
                        startType,
                        mnemonic.Type,
                        anonType
                    }, resultExpression, defaultExp, selectExp);

                startType = anonType;
            }

            return queryIn.Provider.CreateQuery(resultExpression).Select(BuildSelector(config, null));
        }

        public static Type CreateAnonymousType(string name, IEnumerable<Tuple<string, Type>> properties)
        {
            
            AssemblyName dynamicAssemblyName = new AssemblyName("TempAssembly");
            AssemblyBuilder dynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(dynamicAssemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder dynamicModule = dynamicAssembly.DefineDynamicModule("TempAssembly");

            TypeBuilder dynamicAnonymousType = dynamicModule.DefineType(name, TypeAttributes.Public);

            foreach (var property in properties)
            {
                dynamicAnonymousType.DefineField(property.Item1, property.Item2, FieldAttributes.Public);
            }

            return _dictionary.GetOrAdd(name, dynamicAnonymousType.CreateType()) ;
        }
    }
}

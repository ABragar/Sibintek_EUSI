using System;
using System.Linq.Expressions;
using Base.ComplexKeyObjects;
using Base.ComplexKeyObjects.Common;
using Base.ComplexKeyObjects.Unions;
using Base.ComplexKeyObjects.Unions.Implementation;

namespace Base.UI
{
    public static class ConfiguratorExtensions
    {
        public static ViewModelConfigBuilder<T> AddToUnionSimple<T, TUnionEntry>(this ViewModelConfigBuilder<T> builder,
            Expression<Func<T, TUnionEntry>> selector,
            Expression<Func<T, bool>> filter = null)
            where T : BaseObject
            where TUnionEntry : class, IUnionEntry<TUnionEntry>
        {

  
        
            var mnemonic = builder.Config.Mnemonic;

            GetUnionConfig<T, TUnionEntry>(builder)
                .AddItem(new RepositoryQuerySource<T>(), selector, filter,
                    SelectorOverride<T, TUnionEntry>.Create(x => x.ID, x => x.ID),
                    SelectorOverride<T, TUnionEntry>.Create(x => x.Hidden, x => x.Hidden),
                    SelectorOverride<T, TUnionEntry>.Create(x => x.ExtraID, x => mnemonic));

            GetRelationService(builder).AddRelation(typeof(TUnionEntry), mnemonic);

            return builder;
        }




        public static ViewModelConfigBuilder<T> AddToUnionOtherUnion<T, TUnionEntry>(this ViewModelConfigBuilder<T> builder,
            Expression<Func<T, TUnionEntry>> selector,
            Expression<Func<T, bool>> filter = null)
            where T : class, IUnionEntry<T>
            where TUnionEntry : class, IUnionEntry<TUnionEntry>
        {

            IConfigurator<T> configurator = builder;



            var other_union = configurator.Context.GetChildContext<UnionConfig<T>>();


            GetUnionConfig<T, TUnionEntry>(builder)
                .AddItem(other_union, selector, filter,
                    SelectorOverride<T, TUnionEntry>.Create(x => x.ID, x => x.ID),
                    SelectorOverride<T, TUnionEntry>.Create(x => x.Hidden, x => x.Hidden),
                    SelectorOverride<T, TUnionEntry>.Create(x => x.ExtraID, x => x.ExtraID));

            GetRelationService(builder).AddRelation(typeof(TUnionEntry), typeof(T));

            return builder;
        }

        public static ViewModelConfigBuilder<T> AddToUnionComplex<T, TUnionEntry>(this ViewModelConfigBuilder<T> builder,
            Expression<Func<T, TUnionEntry>> selector,
            Expression<Func<T, bool>> filter = null)
                where T : BaseObject, IComplexKeyObject
                where TUnionEntry : class, IUnionEntry<TUnionEntry>
        {



            GetUnionConfig<T, TUnionEntry>(builder)
                .AddItem(new RepositoryQuerySource<T>(), selector, filter,
                    SelectorOverride<T, TUnionEntry>.Create(x => x.ID, x => x.ID),
                    SelectorOverride<T, TUnionEntry>.Create(x => x.Hidden, x => x.Hidden),
                    SelectorOverride<T, TUnionEntry>.Create(x => x.ExtraID, x => x.ExtraID));


            GetRelationService(builder).AddRelation(typeof(TUnionEntry), typeof(T));


            return builder;
        }


        private static ITypeRelationService GetRelationService<T>(IConfigurator<T> configurator)
        {
            return configurator.Context.GetChildContext<ITypeRelationService>();
        }


        private static UnionConfig<TUnionEntry> GetUnionConfig<T, TUnionEntry>(IConfigurator<T> configurator)
                            where TUnionEntry : class, IUnionEntry<TUnionEntry>
        {
            return configurator.Context.GetChildContext<UnionConfig<TUnionEntry>>();
        }

    }

}

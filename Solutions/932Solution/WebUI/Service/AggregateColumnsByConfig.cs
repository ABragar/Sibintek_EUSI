using System;
using System.Linq;
using System.Linq.Expressions;
using Base.Service;
using Base.UI;
using Base.UI.ViewModal;
using Base.Utils.Common;
using System.Reflection;

namespace WebUI.Service
{
    public interface IAggregateColumnsByConfig
    {
        void AggregateAll(AggregateType aggregateType, Predicate<Type> typeFilter);
    }

    public class AggregateColumnsByConfig : IAggregateColumnsByConfig
    {
        private IViewModelConfigService ViewModelConfigService { get; }
        private IColumnBoundRegisterService ColumnBoundRegisterService { get; }
        public AggregateColumnsByConfig(IServiceLocator locator)
        {
            ViewModelConfigService = locator.GetService<IViewModelConfigService>();
            ColumnBoundRegisterService = locator.GetService<IColumnBoundRegisterService>();
        }

        public void AggregateAll(AggregateType aggregateType, Predicate<Type> typeFilter)
        {
            var viewModelConfigs = ViewModelConfigService.GetAll();

            foreach (var config in viewModelConfigs)
            {
                var configListView = config.ListView;
                if (!typeFilter(configListView.TypeEntity))
                    continue;
                foreach (var column in configListView.Columns)
                {
                    //
                    // только видимые
                    if (!column.Visible)
                        continue;
                    if (!column.PropertyType.IsNumericType())
                        continue;

                    if (configListView.DataSource.Aggregates.Any(aggregate1 => aggregate1.Property == column.PropertyName))
                        continue;

                    var aggregate = new Aggregate()
                    {
                        Type = aggregateType,
                        Property = column.PropertyName
                    };

                    var parameter = Expression.Parameter(configListView.TypeEntity);
                    var memberExpression = Expression.Property(parameter, configListView.TypeEntity, column.PropertyName);
                    Expression lambdaExpression = Expression.Lambda(memberExpression, parameter);

                    //var bound = ColumnBoundRegisterService.GetBoundByGridTypeAndProperty(configListView.TypeEntity, column.PropertyName);
                    var methodInfo = GetType().GetMethod(nameof(Bound), BindingFlags.NonPublic | BindingFlags.Instance);
                    if (methodInfo == null)
                        continue;
                    var method = methodInfo.MakeGenericMethod(config.TypeEntity, column.ColumnType);
                    method.Invoke(this, new object[] { config.Mnemonic, lambdaExpression });
                    configListView.DataSource.Aggregates.Add(aggregate);
                }
            }
        }

        void Bound<T, TProperty>(string mnemonic, Expression<Func<T, TProperty>> property)
        {
                ColumnBoundRegisterService
                  .Register<T, TProperty>(mnemonic, property)
                  .Create((builder, preset, column, grid) =>
                  {
                      builder.ClientFooterTemplate(BoundsRegister.BoundsRegister.CreateClientFooterTemplate(column.PropertyName));
                      builder.ClientGroupFooterTemplate(BoundsRegister.BoundsRegister.CreateClientGroupFooterTemplate(column.PropertyName));
                  });
        }

    }
}
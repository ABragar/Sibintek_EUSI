using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Base.UI.ViewModal;

namespace Base.UI
{
    public class DataSourceBuilder<T> where T : class
    {
        private readonly DataSource _dataSource;

        public DataSourceBuilder(DataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public DataSourceBuilder<T> Filter(Expression<Func<T, bool>> predicate)
        {
            _dataSource.Filter = predicate;
            return this;
        }

        public DataSourceBuilder<T> Sort(Action<DataSourceSortsFactory<T>> action)
        {
            action(new DataSourceSortsFactory<T>(_dataSource.Sorts));
            return this;
        }

        //public DataSourceBuilder<T> Sort<TKey>(Expression<Func<T, TKey>> keySelector, ListSortDirection sortDirection)
        //{
            
        //        return this;
        //}

        public DataSourceBuilder<T> Groups(Action<DataSourceGroupsFactory<T>> action)
        {
            action(new DataSourceGroupsFactory<T>(_dataSource.Groups));
            return this;
        }

        public DataSourceBuilder<T> Aggregate(Action<DataSourceAggregatesFactory<T>> action)
        {
            action(new DataSourceAggregatesFactory<T>(_dataSource.Aggregates));
            return this;
        }

        //sib
        public DataSourceBuilder<T> PageSize(int size)
        {
            _dataSource.PageSize = size;
            return this;
        }
        //end sib
    }
}
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Base.Attributes;

namespace Base.UI.ViewModal
{
    public class DataSource
    {
        public LambdaExpression Filter { get; set; }
        public bool ServerOperation { get; set; }
        public int PageSize { get; set; }
        public GroupCollection Groups { get; set; }
        public List<Sort> Sorts { get; set; }
        public List<Aggregate> Aggregates { get; set; }
        
        public DataSource()
        {
            Groups = new GroupCollection();
            Sorts = new List<Sort>();
            Aggregates = new List<Aggregate>();
            PageSize = 50;
            ServerOperation = true;
        }

        public DataSource Copy()
        {
            var ds = new DataSource()
            {
                Filter = this.Filter,
                ServerOperation = this.ServerOperation,
                PageSize = this.PageSize,
                Groups = this.Groups.Copy(),
            };

            foreach (var sort in Sorts)
            {
                ds.Sorts.Add(sort.Copy());
            }

            foreach (var aggr in Aggregates)
            {
                ds.Aggregates.Add(aggr.Copy());
            }

            return ds;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Base.DAL;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.Request;
using CorpProp.Services.Response;
using CorpProp.Services.Response.Fasade;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace CorpProp.Tests.UnitTests
{
    [TestClass]
    public class RequestTest
    {
        public class RequestUpdateTester : RequestUpdate
        {
            public RequestUpdateTester(Response response, CellsData cells, TableData table, IUnitOfWork unitOfWork) : base(response, cells, table, unitOfWork)
            {
            }

            public new IEnumerable<RequestUpdate.KeyValuePair<RequestColumn, object>> ColumnValues(object src)
            {
                return base.ColumnValues(src);
            }
        }

        [TestMethod]
        public void GetColumnsFromModelTester()
        {
            object[] values = {"1", 2, (double) 3};

            var src = new {column1 = values[0], column2 = values[1], column3 = values[2]};

            var columns = new List<RequestColumn>()
            {
                new RequestColumn()
                {
                    ID = 1,
                    SortOrder = 1,
                    Name = "a"
                },
                new RequestColumn()
                {
                    ID = 2,
                    SortOrder = 2,
                    Name = "b"
                }

            };

            TableData table = new TableData()
            {
                ResponseColumns = columns.AsQueryable()
            };
            var request = new Response();

            var requestUpdate = new RequestUpdateTester(request, new CellsData(), table, null);

            var cv = requestUpdate.ColumnValues(src);

            var result = cv.ToList();
            Assert.AreSame(result[0].Key, columns[0]);
            Assert.AreSame(result[1].Key, columns[1]);
            Assert.AreSame(result[0].Value, values[0]);
            Assert.AreSame(result[1].Value, values[1]);

        }

        private Expression sss<T>(string field)
        {
            var xParameter = Expression.Parameter(typeof(T), field);
            var property = Expression.Property(xParameter, typeof(T).GetProperty(field));
            var lambda = Expression.Lambda(property, xParameter);
            return lambda;
        }
    }

}

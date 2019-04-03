using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Base.UI.ViewModal
{

    public class CalcFieldsBuilder<T> where T : class
    {
        private List<string> fields;
        public CalcFieldsBuilder(List<string> fields)
        {
            this.fields = fields;
        }

        public CalcFieldsBuilder<T> Add<TProp>(Expression<Func<T, TProp>> expr)
        {
            var exp = expr.Body as MemberExpression;
            fields.Add(exp.Member.Name);
            return this;
        }
    }
}

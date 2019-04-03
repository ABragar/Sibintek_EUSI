using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sib.Taxes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MultipleRegularExpressionAttribute : RegularExpressionAttribute
    {
        public MultipleRegularExpressionAttribute(string pattern) : base(pattern) { }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;

namespace Base.Entities.Complex
{
    [Serializable]
    [ComplexType]
    public class Icon
    {
        public Icon() { }

        public Icon(string value)
        {
            Value = value;
        }
        public string Color { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}

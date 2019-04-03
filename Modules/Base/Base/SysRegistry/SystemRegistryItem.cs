using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;

namespace Base.SysRegistry
{
    public class SystemRegistryItem: BaseObject
    {
        [UniqueIndex("Key")]
        [MaxLength(255)]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

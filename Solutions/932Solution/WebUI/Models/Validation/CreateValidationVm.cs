using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.Validation
{
    public class CreateValidationVm
    {
        public string ObjectType { get; set; }
        public ICollection<PropertyEditorVm> Properties { get; set; }
    }
}
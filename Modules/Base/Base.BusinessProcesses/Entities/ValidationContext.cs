using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Validation;

namespace Base.BusinessProcesses.Entities
{
    public class ValidationContext : IValidationContext
    {
        private readonly List<string> _validationRules;

        public ValidationContext()
        {
            _validationRules = new List<string>();
        }

        List<string> IValidationContext.ValidationRules => _validationRules;
    }
}

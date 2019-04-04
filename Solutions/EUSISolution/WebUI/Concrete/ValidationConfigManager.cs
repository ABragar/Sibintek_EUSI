using Base.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebUI.Concrete
{
    public class ValidationConfigManager : ValidationConfigBuilder, IValidationConfigManager
    {
        public override void Load()
        {
        }

        public ICollection<Type> GetRulesType(Type type)
        {
            return ValidationConfig.ConfigItems.Where(x => x.Target == type).Select(x => x.Source).ToList();
        }

        public ICollection<IValidationRule> GetRules(Type type)
        {
            return ValidationConfig.ConfigItems.Where(x => x.Target == type).Select(x => (IValidationRule)Activator.CreateInstance(x.Source)).ToList();
        }
    }
}
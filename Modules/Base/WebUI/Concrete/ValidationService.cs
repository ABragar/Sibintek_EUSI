using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting;
using Base;
using Base.Validation;

namespace WebUI.Concrete
{
    public class ValidationService : IValidationService
    {

        private readonly Dictionary<string, IValidationRule> _rules;

        public ValidationService(IValidationConfigManager configManager)
        {
            _rules = new Dictionary<string, IValidationRule>();

            foreach (var configItem in configManager.ValidationConfig.ConfigItems)
            {
                var ruleIntance = (IValidationRule)CreateRuleInstance(configItem.Source.GetTypeName()).Unwrap();
                _rules.Add(configItem.Source.FullName, ruleIntance);
            }
        }



        public List<ValidationResult> Validate<T>(IValidationContext context, T obj) where T : BaseObject
        {
            List<ValidationResult> results = new List<ValidationResult>();

            foreach (var valBinding in context.ValidationRules)
            {
                var rule = _rules[valBinding];
                var ruleType = rule.GetType();
                results.AddRange((List<ValidationResult>)ruleType.InvokeMember("Validate", BindingFlags.InvokeMethod, null, rule, null));
            }
            return results;
        }

        private ObjectHandle CreateRuleInstance(string valRuleName)
        {
            string assembly = valRuleName.Split(',')[1].Trim();
            string type = valRuleName.Split(',')[0].Trim();
            return Activator.CreateInstance(assembly, type);
        }
    }
}
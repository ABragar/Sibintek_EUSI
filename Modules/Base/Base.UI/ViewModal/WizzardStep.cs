using System;
using System.Collections.Generic;

namespace Base.UI.ViewModal
{
    public class WizzardStep
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public List<StepProperty> StepProperties { get; set; } = new List<StepProperty>();
        public Func<object, StepRuleResult> CheckStepRule { get; set; }

        public WizzardStep Copy()
        {
            var step = new WizzardStep
            {
                Name = this.Name,
                Description = this.Description,
                Title = this.Title,
                CheckStepRule = this.CheckStepRule
            };

            foreach (var stepProperty in StepProperties)
            {
                step.StepProperties.Add(stepProperty.Copy());
            }

            return step;
        }
    }

    public enum StepRuleResult
    {
        Backward,
        Forward,
        End,
        Ok
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Base.UI.ViewModal;

namespace Base.UI
{
    public class WizzardDetailViewBuilder<T> : DetailViewBuilder<T> where T : class
    {
        private readonly WizardDetailView _detailViewdetailView;

        public WizzardDetailViewBuilder(WizardDetailView detailview)
            : base(detailview)
        {
            _detailViewdetailView = detailview;
        }

        public WizzardDetailViewBuilder<T> FirstStep(string firstStep)
        {
            _detailViewdetailView.FirstStep = firstStep;
            return this;
        }

        public WizzardDetailViewBuilder<T> HasSummary(bool hasSummary)
        {
            _detailViewdetailView.HasSummary = hasSummary;
            return this;
        }

        public WizzardDetailViewBuilder<T> Steps(Action<WizzardDetailViewStepsFactory> fact)
        {
            fact(new WizzardDetailViewStepsFactory(_detailViewdetailView.Steps));
            return this;
        }

        public class WizzardDetailViewStepsFactory
        {
            private readonly List<WizzardStep> _steps;

            public WizzardDetailViewStepsFactory(List<WizzardStep> steps)
            {
                _steps = steps;
            }

            public WizzardDetailViewStepsFactory Add(string name, Action<WizzardStepBuilder> act)
            {
                if (_steps.Any(x => x.Name == name))
                    throw new Exception($"Шаг с идентификатором [{name}] уже существует");

                var step = new WizzardStep()
                {
                    StepProperties = new List<StepProperty>(),
                    Name = name,
                    //Index = _steps.Count(),
                };

                _steps.Add(step);
                act(new WizzardStepBuilder(step));
                return this;
            }
        }

        public class WizzardStepBuilder
        {
            private readonly WizzardStep step;

            public WizzardStepBuilder(WizzardStep step)
            {
                this.step = step;
            }


            public WizzardStepBuilder Description(string descr)
            {
                step.Description = descr;
                return this;
            }

            public WizzardStepBuilder Title(string title)
            {
                step.Title = title;
                return this;
            }

            public WizzardStepBuilder StepProperties(Action<StepPropertiesFactory> act)
            {
                act(new StepPropertiesFactory(step.StepProperties));
                return this;
            }

            public WizzardStepBuilder Rule(Func<T, StepRuleResult> func)
            {

                step.CheckStepRule = x => func((T)x);

                return this;
            }

            public class StepPropertiesFactory
            {
                private readonly List<StepProperty> stepProp;
                public StepPropertiesFactory(List<StepProperty> stepProp)
                {
                    this.stepProp = stepProp;
                }

                public StepPropertiesFactory Add<TValue>(Expression<Func<T, TValue>> property)
                {
                    var propertyExpression = property.Body as MemberExpression;

                    if (propertyExpression == null)
                        throw new Exception("propertyExpression");

                    var propertyInfo = propertyExpression.Member as PropertyInfo;

                    var prop = stepProp.FirstOrDefault(x => x.Name == propertyInfo.Name);

                    if (prop == null)
                    {
                        prop = new StepProperty { Name = propertyInfo.Name };
                        stepProp.Add(prop);
                    }
                    return this;
                }
            }
        }

    }
}
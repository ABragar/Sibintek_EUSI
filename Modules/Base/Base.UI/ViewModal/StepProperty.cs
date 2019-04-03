using System;

namespace Base.UI.ViewModal
{
    public class StepProperty
    {
        public string Name { get; set; }

        public StepProperty Copy()
        {
            return new StepProperty()
            {
                Name = this.Name
            };
        }
    }
}
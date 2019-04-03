using System;

namespace Base.Validation
{
    public class ValidationConfigBuilder
    {
        public ValidationConfig ValidationConfig { get; }

        public ValidationConfigBuilder()
        {
            ValidationConfig = new ValidationConfig();
        }

        public BindingTargetBuilder<T> Bind<T>() where T : class
        {
            ValidationBinding binding = new ValidationBinding { Target = typeof(T) };
            ValidationConfig.Add(binding);

            var tb = new BindingTargetBuilder<T>(binding);
            return tb;
        }

        public virtual void Load() { }



    }
    public class BindingTargetBuilder<T>
    {
        private ValidationBinding Binding { get; set; }

        public BindingTargetBuilder(ValidationBinding binding)
        {
            Binding = binding;


        }

        public ValidationBinding To<TRule>() where TRule : class, IValidationRule<T>, new()
        {
            Type t = typeof(TRule);
            Binding.Source = t;
            return Binding;
        }
    }
}
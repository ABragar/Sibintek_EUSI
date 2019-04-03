using System.Collections.Generic;

namespace Base.Validation
{
    public interface IValidationRule
    {
        string Title { get; }
        string Description { get; }
    }

    public interface IValidationRule<in T> : IValidationRule
    {
        ICollection<ValidationResult> Validate(T obj);
    }

    public enum ValidationRuleOperation
    {
        Below = 0,  // < 
        BelowOrEqual = 1,  // <=
        Equals = 2,    // == 
        GreaterOrEqual = 3, //   >=
        Greater = 4 // >
    }
}


using System.Collections.Generic;

namespace Base.Validation
{
    public interface IValidationContext
    {
        List<string> ValidationRules { get; }
    }
}

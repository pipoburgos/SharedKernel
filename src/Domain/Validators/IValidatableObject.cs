using System.Collections.Generic;

namespace SharedKernel.Domain.Validators
{
    /// <summary> Validate entity or agregate root. </summary>
    public interface IValidatableObject
    {
        /// <summary> Validate. </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }
}

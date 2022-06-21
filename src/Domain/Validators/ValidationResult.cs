using System;
using System.Collections.Generic;

namespace SharedKernel.Domain.Validators
{
    /// <summary> </summary>
    public class ValidationResult
    {
        private readonly IEnumerable<string> _memberNames;

        /// <summary> </summary>
        /// <param name="errorMessage"></param>
        /// <param name="memberNames"></param>
        public ValidationResult(string errorMessage, IEnumerable<string> memberNames = null)
        {
            ErrorMessage = errorMessage;
            _memberNames = (IEnumerable<string>)((object)memberNames ?? new string[0]);
        }

        /// <summary> </summary>
        /// <param name="validationResult"></param>
        protected ValidationResult(ValidationResult validationResult)
        {
            ErrorMessage = validationResult != null
                ? validationResult.ErrorMessage
                : throw new ArgumentNullException(nameof(validationResult));
            _memberNames = validationResult._memberNames;
        }

        /// <summary> </summary>
        public IEnumerable<string> MemberNames => _memberNames;

        /// <summary> </summary>
        public string ErrorMessage { get; }

        /// <summary> </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ErrorMessage ?? base.ToString();
        }
    }
}
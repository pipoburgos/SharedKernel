using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharedKernel.Application.Validator
{
    /// <summary>An exception that represents failed validation</summary>
    [Serializable]
    public class ValidationFailureException : Exception
    {
        /// <summary>Validation errors</summary>
        public IEnumerable<ValidationFailure> Errors { get; private set; }

        /// <summary>Creates a new ValidationException</summary>
        /// <param name="message"></param>
        public ValidationFailureException(string message)
            : this(message, Enumerable.Empty<ValidationFailure>())
        {
        }

        /// <summary>Creates a new ValidationException</summary>
        /// <param name="message"></param>
        /// <param name="errors"></param>
        public ValidationFailureException(string message, IEnumerable<ValidationFailure> errors)
            : base(message)
        {
            Errors = errors;
        }

        /// <summary>Creates a new ValidationException</summary>
        /// <param name="errors"></param>
        public ValidationFailureException(IList<ValidationFailure> errors)
            : base(BuildErrorMessage(errors))
        {
            Errors = errors;
        }

        private static string BuildErrorMessage(IList<ValidationFailure> errors)
        {
            var values = errors.Select(x => Environment.NewLine + " -- " + x.PropertyName + ": " + x.ErrorMessage);
            return "Validation failed: " + string.Join(string.Empty, values);
        }

        protected ValidationFailureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Errors = info.GetValue("errors", typeof(IEnumerable<ValidationFailure>)) as IEnumerable<ValidationFailure>;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue("errors", Errors);
            base.GetObjectData(info, context);
        }
    }
}
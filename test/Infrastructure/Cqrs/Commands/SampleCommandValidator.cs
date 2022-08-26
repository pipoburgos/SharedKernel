using FluentValidation;

namespace SharedKernel.Integration.Tests.Cqrs.Commands
{
    internal class SampleCommandValidator : AbstractValidator<SampleCommand>
    {
        public SampleCommandValidator(
            IntValidator intValidator)
        {
            RuleFor(x => x.Value)
                .SetValidator(intValidator);
        }
    }
}

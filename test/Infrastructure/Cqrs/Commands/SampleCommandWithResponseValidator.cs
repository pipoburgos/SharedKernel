using FluentValidation;

namespace SharedKernel.Integration.Tests.Cqrs.Commands
{
    internal class SampleCommandWithResponseValidator : AbstractValidator<SampleCommandWithResponse>
    {
        public SampleCommandWithResponseValidator()
        {
            RuleFor(x => x.Value).NotEmpty();
        }
    }
}

using FluentValidation;

namespace SharedKernel.Integration.Tests.Cqrs.Commands
{
    internal class SampleCommandValidator : AbstractValidator<SampleCommand>
    {
        public SampleCommandValidator()
        {
            RuleFor(x => x.Value).NotEmpty();
        }
    }
}

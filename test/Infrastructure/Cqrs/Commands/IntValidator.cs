using FluentValidation;

namespace SharedKernel.Integration.Tests.Cqrs.Commands;

internal class IntValidator : AbstractValidator<int>
{
    public IntValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage("0 is invalid.");
    }
}
using FluentValidation;
using SharedKernel.Application.Cqrs.Queries.Entities;

namespace SharedKernel.Infrastructure.FluentValidation;

/// <summary> . </summary>
public class PageOptionsValidator : AbstractValidator<PageOptions>
{
    /// <summary> . </summary>
    public PageOptionsValidator(
        OrderValidator orderValidator,
        FilterPropertyValidator filterPropertyValidator)
    {
        RuleForEach(a => a.Orders)
            .NotEmpty()
            .SetValidator(orderValidator);

        RuleForEach(a => a.FilterProperties)
            .SetValidator(filterPropertyValidator);
    }
}

/// <summary> . </summary>
public class OrderValidator : AbstractValidator<Order>
{
    /// <summary> . </summary>
    public OrderValidator()
    {
        RuleFor(a => a.Field)
            .NotEmpty();
    }
}

/// <summary> . </summary>
public class FilterPropertyValidator : AbstractValidator<FilterProperty>
{
    /// <summary> . </summary>
    public FilterPropertyValidator()
    {
        RuleFor(a => a.Field)
            .NotEmpty();

        RuleFor(a => a.Value)
            .NotEmpty();
    }
}
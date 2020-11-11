using System.Linq;
using FluentValidation;
using SharedKernel.Application.Cqrs.Queries.Entities;

namespace SharedKernel.Infrastructure.Validators
{
    public class PageOptionsValidator : AbstractValidator<PageOptions>
    {
        public PageOptionsValidator()
        {
            RuleFor(a => a.Take).NotEmpty();

            RuleFor(a => a.Orders).Must(orders =>
            {
                if (orders == null)
                    return false;

                var ordersList = orders.ToList();
                return ordersList.Any() && ordersList.All(o => !string.IsNullOrWhiteSpace(o.Field));
            });

            RuleFor(a => a.FilterProperties)
                .Must(pro => pro == null || pro.All(o => !string.IsNullOrWhiteSpace(o.Field)));
        }
    }
}

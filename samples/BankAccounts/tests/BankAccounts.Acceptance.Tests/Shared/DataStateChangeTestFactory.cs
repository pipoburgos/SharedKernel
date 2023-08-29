using SharedKernel.Application.Cqrs.Queries.Kendo;

namespace BankAccounts.Acceptance.Tests.Shared;

public static class DataStateChangeTestFactory
{
    public static DataStateChange Create(string sortProperty = default, int? skip = default, int? take = 10,
        IEnumerable<SortDescriptor> sort = default, IEnumerable<GroupDescriptor> group = default,
        CompositeFilterDescriptor filter = default)
    {
        return new DataStateChange
        {
            Skip = skip,
            Take = take,
            Sort = (sortProperty != default!
                ? new List<SortDescriptor> { new() { Field = sortProperty, Dir = "asc" } }
                : sort)!,
            Filter = filter,
            Group = group
        };
    }
}

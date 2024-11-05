using SharedKernel.Application.Cqrs.Queries.Entities;

namespace SharedKernel.Application.Cqrs.Queries.Kendo;

/// <summary> . </summary>
public static class KendoExtensions
{
    /// <summary> . </summary>
    public static PageOptions ToPageOptions(this DataStateChange state)
    {
        var sort = state.Sort.Select(s => new Order(s.Field, s.Dir != "desc"));

        var filterProperties = state.Filter?.Filters
            .Select(f => new FilterProperty(f.Field, f.Value, CastToFilterOperator(f.Operator), f.IgnoreCase));

        return new PageOptions(state.Skip, state.Take, null, false, false, sort, filterProperties);
    }

    private static FilterOperator? CastToFilterOperator(string @operator)
    {
        return @operator switch
        {
            "eq" => FilterOperator.EqualTo,
            "neq" => FilterOperator.NotEqualTo,
            "isnull" => FilterOperator.IsEqualToNull,
            "isnotnull" => FilterOperator.IsNotEqualToNull,
            "lt" => FilterOperator.LessThan,
            "lte" => FilterOperator.LessThanOrEqualTo,
            "gt" => FilterOperator.GreaterThan,
            "gte" => FilterOperator.GreaterThanOrEqualTo,
            "startswith" => FilterOperator.StartsWith,
            "endswith" => FilterOperator.EndsWith,
            "contains" => FilterOperator.Contains,
            "doesnotcontain" => FilterOperator.NotContains,
            "isempty" => FilterOperator.IsEmpty,
            "isnotempty" => FilterOperator.IsNotEmpty,
            "dateeq" => FilterOperator.DateEqual,
            "dateneq" => FilterOperator.NotDateEqual,
            _ => default,
        };
    }
}
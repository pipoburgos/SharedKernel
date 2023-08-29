namespace SharedKernel.Application.Cqrs.Queries.Kendo;

/// <summary> Filtro </summary>
public class FilterDescriptor
{
    /// <summary> The data item field to which the filter operator is applied. </summary>
    public string Field { get; set; } = null!;

    /// <summary>
    /// The filter operator (comparison).
    /// The supported operators are:
    ///   `"eq"` (equal to)
    ///   `"neq"` (not equal to)
    ///   `"isnull"` (is equal to null)
    ///   `"isnotnull"` (is not equal to null)
    ///   `"lt"` (less than)
    ///   `"lte"` (less than or equal to)
    ///   `"gt"` (greater than)
    ///   `"gte"` (greater than or equal to)
    /// The following operators are supported for string fields only:
    ///   `"startswith"`
    ///   `"endswith"`
    ///   `"contains"`
    ///   `"doesnotcontain"`
    ///   `"isempty"`
    ///   `"isnotempty"`
    /// </summary>
    public string Operator { get; set; } = null!;

    /// <summary> The value to which the field is compared. Has to be of the same type as the field. </summary>
    public string Value { get; set; } = null!;

    /// <summary> Determines if the string comparison is case-insensitive. </summary>
    public bool IgnoreCase { get; set; }
}
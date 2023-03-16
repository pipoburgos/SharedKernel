using System.Collections.Generic;

namespace SharedKernel.Application.Cqrs.Queries.Kendo;

/// <summary> Filtros </summary>
public class CompositeFilterDescriptor
{
    /// <summary>
    /// The logical operation to use when the `filter.filters` option is set.
    ///
    /// The supported values are:
    /// * `"and"`
    /// * `"or"`
    /// </summary>
    public string Logic { get; set; }

    /// <summary>
    /// The nested filter expressions
    /// either [`FilterDescriptor`]({% slug api_kendo-data-query_filterdescriptor %}), or [`CompositeFilterDescriptor`]
    /// ({% slug api_kendo-data-query_compositefilterdescriptor %}).
    /// Supports the same options as `filter`. You can nest filters indefinitely.
    /// </summary>
    public IEnumerable<FilterDescriptor> Filters { get; set; }
}
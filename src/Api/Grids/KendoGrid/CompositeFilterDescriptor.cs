using System.Collections.Generic;

namespace SharedKernel.Api.Grids.KendoGrid
{
    /// <summary>
    /// 
    /// </summary>
    public class CompositeFilterDescriptor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="filters"></param>
        public CompositeFilterDescriptor(string logic, IEnumerable<FilterDescriptor> filters)
        {
            Logic = logic;
            Filters = filters;
        }

        /// <summary>
        /// The logical operation to use when the `filter.filters` option is set.
        ///
        /// The supported values are:
        /// * `"and"`
        /// * `"or"`
        /// </summary>
        public string Logic { get; }

        /// <summary>
        /// The nested filter expressions
        /// either [`FilterDescriptor`]({% slug api_kendo-data-query_filterdescriptor %}), or [`CompositeFilterDescriptor`]
        /// ({% slug api_kendo-data-query_compositefilterdescriptor %}).
        /// Supports the same options as `filter`. You can nest filters indefinitely.
        /// </summary>
        public IEnumerable<FilterDescriptor> Filters { get; }
    }
}
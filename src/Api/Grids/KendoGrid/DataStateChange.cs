using System.Collections.Generic;

namespace SharedKernel.Api.Grids.KendoGrid
{
    /// <summary>
    /// 
    /// </summary>
    public class DataStateChange
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="allRecords"></param>
        /// <param name="limitRecords"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="sort"></param>
        /// <param name="group"></param>
        /// <param name="filter"></param>
        public DataStateChange(bool allRecords, int? limitRecords, int skip, int take, IEnumerable<SortDescriptor> sort,
            IEnumerable<GroupDescriptor> group, CompositeFilterDescriptor filter)
        {
            AllRecords = allRecords;
            LimitRecords = limitRecords;
            Skip = skip;
            Take = take;
            Sort = sort;
            Group = group;
            Filter = filter;
        }

        /// <summary>
        /// Return all records without paging
        /// </summary>
        public bool AllRecords { get; }

        /// <summary>
        /// Exact number of records to display
        /// </summary>
        public int? LimitRecords { get; }

        /// <summary>
        /// The number of records to skip.
        /// </summary>

        public int Skip { get; }

        /// <summary>
        /// The number of records to take.
        /// </summary>
        public int Take { get; }

        /// <summary>
        /// The sort descriptors by which the data is sorted.
        /// </summary>
        public IEnumerable<SortDescriptor> Sort { get; }

        /// <summary>
        /// The group descriptors by which the data is grouped.
        /// </summary>
        public IEnumerable<GroupDescriptor> Group { get; }

        /// <summary>
        /// The filter descriptor by which the data is filtered.
        /// </summary>
        public CompositeFilterDescriptor Filter { get; }

    }
}
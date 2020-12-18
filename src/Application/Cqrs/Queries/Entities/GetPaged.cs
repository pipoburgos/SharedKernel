using System.Collections.Generic;

namespace SharedKernel.Application.Cqrs.Queries.Entities
{
    /// <summary>
    /// Base filter for Dapper queries
    /// </summary>
    public class PageOptions
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="searchText"></param>
        /// <param name="showDeleted"></param>
        /// <param name="showOnlyDeleted"></param>
        /// <param name="orders"></param>
        /// <param name="filterProperties"></param>
        public PageOptions(int skip, int take, string searchText, bool showDeleted, bool showOnlyDeleted, IEnumerable<Order> orders,
            IEnumerable<FilterProperty> filterProperties)
        {
            Skip = skip;
            Take = take;
            SearchText = searchText;
            ShowDeleted = showDeleted;
            ShowOnlyDeleted = showOnlyDeleted;
            Orders = orders;
            FilterProperties = filterProperties;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Skip { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Take { get; }

        /// <summary>
        /// 
        /// </summary>
        public string SearchText { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowDeleted { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowOnlyDeleted { get; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Order> Orders { get; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<FilterProperty> FilterProperties { get; }
    }
}

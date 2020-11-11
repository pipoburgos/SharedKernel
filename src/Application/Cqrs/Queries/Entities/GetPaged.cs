using System.Collections.Generic;

namespace SharedKernel.Application.Cqrs.Queries.Entities
{
    /// <summary>
    /// Base filter for Dapper queries
    /// </summary>
    public class PageOptions
    {
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

        public int Skip { get; }

        public int Take { get; }

        public string SearchText { get; }

        public bool ShowDeleted { get; }

        public bool ShowOnlyDeleted { get; }


        public IEnumerable<Order> Orders { get; }

        public IEnumerable<FilterProperty> FilterProperties { get; }
    }
}

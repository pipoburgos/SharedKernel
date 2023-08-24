using SharedKernel.Application.Cqrs.Queries.Contracts;

namespace SharedKernel.Api.Grids.DataTables
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataTablesResponse<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public DataTablesResponse(int draw, IPagedList<T> pagedList)
        {
            Draw = draw + 1;
            RecordsTotal = pagedList.TotalRecordsFiltered;
            RecordsFiltered = pagedList.TotalRecordsFiltered;
            Data = pagedList.Items;
            AdditionalParameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// 
        /// </summary>
        public int Draw { get; }

        /// <summary>
        /// Unfiltered records
        /// </summary>
        public int RecordsTotal { get; }

        /// <summary>
        /// Records once the filter is done
        /// </summary>
        public int RecordsFiltered { get; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<T> Data { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> AdditionalParameters { get; set; }
    }
}
using SharedKernel.Application.Cqrs.Queries.DataTables;

namespace SharedKernel.Api.Grids.DataTables
{
    /// <summary>
    /// Datatables request
    /// </summary>
    public class DataTablesRequest<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="draw"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="search"></param>
        /// <param name="columns"></param>
        /// <param name="order"></param>
        public DataTablesRequest(T filter, int draw, int start, int length, List<Column> columns, IEnumerable<DataTablesOrder> order,
            Search search = null)
        {
            Filter = filter;
            Draw = draw;
            Start = start;
            Length = length;
            Search = search;
            Columns = columns;
            Order = order;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Draw { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// 
        /// </summary>
        public Search Search { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<Column> Columns { get; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<DataTablesOrder> Order { get; }

        /// <summary>
        /// 
        /// </summary>
        public T Filter { get; }
    }

    /// <summary>
    /// Datatables request
    /// </summary>
    public class DataTablesRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="search"></param>
        /// <param name="columns"></param>
        /// <param name="order"></param>
        /// <param name="additionalParameters"></param>
        public DataTablesRequest(int draw, int start, int length, List<Column> columns, IEnumerable<DataTablesOrder> order,
            Search search = null, IDictionary<string, object> additionalParameters = null)
        {
            Draw = draw;
            Start = start;
            Length = length;
            Search = search;
            Columns = columns;
            Order = order;
            AdditionalParameters = additionalParameters ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// 
        /// </summary>
        public int Draw { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// 
        /// </summary>
        public Search Search { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<Column> Columns { get; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<DataTablesOrder> Order { get; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> AdditionalParameters { get; }
    }
}

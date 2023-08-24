namespace SharedKernel.Application.Cqrs.Queries
{
    /// <summary>
    /// Exception thrown if the query handler is not registered in the dependency container
    /// </summary>
    public class QueryNotRegisteredException : Exception
    {
        /// <summary>
        /// Exception thrown if the query handler is not registered in the dependency container
        /// </summary>
        /// <param name="query"></param>
        public QueryNotRegisteredException(string query) : base(
            $"The query {query} has not a query handler associated")
        {
        }
    }
}

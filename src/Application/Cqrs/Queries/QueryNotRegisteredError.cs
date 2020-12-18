using System;

namespace SharedKernel.Application.Cqrs.Queries
{
    /// <summary>
    /// Exception thrown if the query handler is not registered in the dependency container
    /// </summary>
    public class QueryNotRegisteredError : Exception
    {
        /// <summary>
        /// Exception thrown if the query handler is not registered in the dependency container
        /// </summary>
        /// <param name="query"></param>
        public QueryNotRegisteredError(string query) : base(
            $"The query {query} has not a query handler associated")
        {
        }
    }
}

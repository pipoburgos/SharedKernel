using System;

namespace SharedKernel.Application.Cqrs.Queries
{
    public class QueryNotRegisteredError : Exception
    {
        public QueryNotRegisteredError(string query) : base(
            $"The query {query} has not a query handler associated")
        {
        }
    }
}

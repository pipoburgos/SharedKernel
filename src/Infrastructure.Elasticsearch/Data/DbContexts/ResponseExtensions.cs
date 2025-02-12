using Elastic.Transport.Products.Elasticsearch;

namespace SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;

/// <summary> . </summary>
public static class ResponseExtensions
{
    /// <summary> . </summary>
    public static void ThrowOriginalExceptionIfIsNotValid(this ElasticsearchResponse response)
    {
        if (response.IsValidResponse)
            return;

        if (!response.TryGetOriginalException(out var ex))
            return;

        if (ex != null)
            throw ex;
    }
}
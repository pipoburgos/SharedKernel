using Elastic.Transport.Products.Elasticsearch;
using Elasticsearch.Net;

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

    /// <summary> . </summary>
    public static bool ThrowOriginalExceptionIfIsNotValid(this ElasticsearchResponseBase response)
    {
        if (response.HttpStatusCode == 404)
            return true;

        if (response.HttpStatusCode != 200)
            throw response.OriginalException;

        return false;
    }
}
using Nest;

namespace SharedKernel.Infrastructure.Elasticsearch.Client;

/// <summary>  </summary>
public class ElasticsearchClient
{
    private readonly string _indexPrefix;

    /// <summary> </summary>
    public ElasticClient Client { get; }

    /// <summary>  </summary>
    public ElasticsearchClient(ElasticClient client, string indexPrefix)
    {
        Client = client;
        _indexPrefix = indexPrefix;
    }

    /// <summary>  </summary>
    public async Task Persist(string index, string id, string json)
    {
        await Client.LowLevel.IndexAsync<IndexResponse>(index, id, json);
    }

    /// <summary>  </summary>
    public string IndexFor(string moduleName)
    {
        return $"{_indexPrefix}_{moduleName}".ToLower();
    }
}

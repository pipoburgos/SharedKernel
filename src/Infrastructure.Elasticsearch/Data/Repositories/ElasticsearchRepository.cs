using Nest;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Infrastructure.Elasticsearch.Client;

namespace SharedKernel.Infrastructure.Elasticsearch.Data.Repositories;

/// <summary>  </summary>
public abstract class ElasticsearchRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
{
    private readonly ElasticsearchClient _client;

    /// <summary>  </summary>
    protected ElasticsearchRepository(ElasticsearchClient client)
    {
        _client = client;
    }

    /// <summary>  </summary>
    protected abstract string ModuleName();

    /// <summary>  </summary>
    protected async Task<IReadOnlyCollection<Dictionary<string, object>>> SearchAllInElastic()
    {
        var searchDescriptor = new SearchDescriptor<TAggregateRoot>();
        searchDescriptor.MatchAll();
        searchDescriptor.Index(_client.IndexFor(ModuleName()));
        var value = await _client.Client.SearchAsync<Dictionary<string, object>>(searchDescriptor);
        return value.Documents!;
    }

    /// <summary>  </summary>
    protected async Task Persist(string id, string json)
    {
        await _client.Persist(_client.IndexFor(ModuleName()), id, json);
    }
}

using System.Threading.Tasks;
using Nest;

namespace SharedKernel.Infrastructure.Data.Elasticsearch.Client
{
    public class ElasticsearchClient
    {
        public ElasticClient Client { get; }
        private readonly string _indexPrefix;

        public ElasticsearchClient(ElasticClient client, string indexPrefix)
        {
            Client = client;
            _indexPrefix = indexPrefix;
        }

        public async Task Persist(string index, string id, string json)
        {
            await Client.LowLevel.IndexAsync<IndexResponse>(index, id, json);
        }

        public string IndexFor(string moduleName)
        {
            return $"{_indexPrefix}_{moduleName}".ToLower();
        }
    }
}

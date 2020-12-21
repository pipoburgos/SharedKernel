using System.Threading.Tasks;
using Nest;

namespace SharedKernel.Infrastructure.Data.Elasticsearch.Client
{
    /// <summary>
    /// 
    /// </summary>
    public class ElasticsearchClient
    {
        private readonly string _indexPrefix;

        /// <summary>
        /// 
        /// </summary>
        public ElasticClient Client { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="indexPrefix"></param>
        public ElasticsearchClient(ElasticClient client, string indexPrefix)
        {
            Client = client;
            _indexPrefix = indexPrefix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="id"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public async Task Persist(string index, string id, string json)
        {
            await Client.LowLevel.IndexAsync<IndexResponse>(index, id, json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public string IndexFor(string moduleName)
        {
            return $"{_indexPrefix}_{moduleName}".ToLower();
        }
    }
}

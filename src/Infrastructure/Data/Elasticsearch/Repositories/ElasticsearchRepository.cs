using Nest;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Repositories;
using SharedKernel.Infrastructure.Data.Elasticsearch.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Data.Elasticsearch.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    public abstract class ElasticsearchRepository<TAggregateRoot> :
        IPersistRepository
        where TAggregateRoot : class, IAggregateRoot // , IEntity<TKey>
    {
        private readonly ElasticsearchClient _client;
        //private readonly ElasticsearchCriteriaConverter<T> _criteriaConverter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        protected ElasticsearchRepository(ElasticsearchClient client)
        {
            _client = client;
            //_criteriaConverter = new ElasticsearchCriteriaConverter<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract string ModuleName();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected async Task<IReadOnlyCollection<Dictionary<string, object>>> SearchAllInElastic()
        {
            var searchDescriptor = new SearchDescriptor<TAggregateRoot>();
            searchDescriptor.MatchAll();
            searchDescriptor.Index(_client.IndexFor(ModuleName()));

            return (await _client.Client.SearchAsync<Dictionary<string, object>>(searchDescriptor))?.Documents;
        }

        //protected async Task<IReadOnlyCollection<Dictionary<string, object>>> SearchByCriteria(Criteria criteria)
        //{
        //    var searchDescriptor = _criteriaConverter.Convert(criteria, _client.IndexFor(ModuleName()));

        //    return (await _client.Client.SearchAsync<Dictionary<string, object>>(searchDescriptor))?.Documents;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        protected async Task Persist(string id, string json)
        {
            await _client.Persist(_client.IndexFor(ModuleName()), id, json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Rollback()
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return 0;
        }
    }
}

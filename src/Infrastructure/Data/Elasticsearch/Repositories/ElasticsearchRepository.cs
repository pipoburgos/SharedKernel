using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Repositories;
using SharedKernel.Infrastructure.Data.Elasticsearch.Client;

namespace SharedKernel.Infrastructure.Data.Elasticsearch.Repositories
{
    public abstract class ElasticsearchRepository<TAggregateRoot> : ICreateRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot // , IEntity<TKey>
    {
        private readonly ElasticsearchClient _client;
        //private readonly ElasticsearchCriteriaConverter<T> _criteriaConverter;

        protected ElasticsearchRepository(ElasticsearchClient client)
        {
            _client = client;
            //_criteriaConverter = new ElasticsearchCriteriaConverter<T>();
        }

        protected abstract string ModuleName();

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

        protected async Task Persist(string id, string json)
        {
            await _client.Persist(_client.IndexFor(ModuleName()), id, json);
        }

        public void Add(TAggregateRoot aggregate)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TAggregateRoot> aggregates)
        {
            throw new NotImplementedException();
        }
    }
}

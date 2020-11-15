using Nest;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Specifications.Common;
using SharedKernel.Infrastructure.Data.Elasticsearch.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Data.Elasticsearch.Repositories
{
    public abstract class ElasticsearchRepository<TAggregateRoot> : Domain.Repositories.IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot // , IEntity<TKey>
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

        public TAggregateRoot GetById<TKey>(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Any()
        {
            throw new NotImplementedException();
        }

        public bool Any<TKey>(TKey key)
        {
            throw new NotImplementedException();
        }

        public void Update(TAggregateRoot aggregate)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
        {
            throw new NotImplementedException();
        }

        public void Remove(TAggregateRoot aggregate)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<TAggregateRoot> aggregate)
        {
            throw new NotImplementedException();
        }

        public List<TAggregateRoot> Where(ISpecification<TAggregateRoot> spec)
        {
            throw new NotImplementedException();
        }

        public TAggregateRoot Single(ISpecification<TAggregateRoot> spec)
        {
            throw new NotImplementedException();
        }

        public TAggregateRoot SingleOrDefault(ISpecification<TAggregateRoot> spec)
        {
            throw new NotImplementedException();
        }

        public bool Any(ISpecification<TAggregateRoot> spec)
        {
            throw new NotImplementedException();
        }

        public int Rollback()
        {
            return 0;
        }

        public int SaveChanges()
        {
            return 0;
        }
    }
}

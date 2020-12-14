using Newtonsoft.Json;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Specifications.Common;
using System;
using System.Collections.Generic;
using System.IO;

#pragma warning disable 693

namespace SharedKernel.Infrastructure.Data.FileSystem.Repositories
{
    public abstract class FileSystemRepository<TAggregateRoot, TKey> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        protected readonly string FilePath;

        protected FileSystemRepository()
        {
            FilePath = Directory.GetCurrentDirectory() + typeof(TAggregateRoot);
        }

        protected string FileName(string id)
        {
            return $"{FilePath}.{id}.repository";
        }

        //public async Task Save(T course)
        //{
        //    await Task.Run(() =>
        //    {
        //        using (StreamWriter outputFile = new StreamWriter(FileName(course.Id.Value), false))
        //        {
        //            outputFile.WriteLine(JsonConvert.SerializeObject(course));
        //        }
        //    });
        //}

        //public async Task<T> Search(CourseId id)
        //{
        //    if (File.Exists(FileName(id.Value)))
        //    {
        //        var text = await File.ReadAllTextAsync(FileName(id.Value));
        //        return JsonConvert.DeserializeObject<Course>(text);
        //    }

        //    return null;
        //}

        public void Add(TAggregateRoot aggregate)
        {
            using var outputFile = new StreamWriter(FileName(aggregate.Id.ToString()), false);
            outputFile.WriteLine(JsonConvert.SerializeObject(aggregate));
        }

        public void AddRange(IEnumerable<TAggregateRoot> aggregates)
        {
            foreach (var aggregateRoot in aggregates)
            {
                Add(aggregateRoot);
            }
        }

        public TAggregateRoot GetById<TKey>(TKey key)
        {
            if (!File.Exists(FileName(key.ToString())))
                return null;

            var text = File.ReadAllText(FileName(key.ToString()));
            return JsonConvert.DeserializeObject<TAggregateRoot>(text);
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

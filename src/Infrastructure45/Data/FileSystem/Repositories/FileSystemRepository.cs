using Newtonsoft.Json;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using System.Collections.Generic;
using System.IO;

namespace SharedKernel.Infrastructure.Data.FileSystem.Repositories
{
    public abstract class FileSystemRepository<TAggregateRoot, TKey> : ICreateRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        private readonly string _filePath;

        protected FileSystemRepository()
        {
            _filePath = Directory.GetCurrentDirectory() + typeof(TAggregateRoot);
        }

        private string FileName(string id)
        {
            return $"{_filePath}.{id}.repository";
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
            using (var outputFile = new StreamWriter(FileName(aggregate.Id.ToString()), false))
            {
                outputFile.WriteLine(JsonConvert.SerializeObject(aggregate));
            }
        }

        public void AddRange(IEnumerable<TAggregateRoot> aggregates)
        {
            foreach (var aggregateRoot in aggregates)
            {
                Add(aggregateRoot);
            }
        }
    }
}

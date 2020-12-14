using System;
using System.Threading.Tasks;

namespace SharedKernel.Application.Caching
{
    public interface ICacheHelper
    {
        Task SetAsync<T>(string key, T value, TimeSpan? timeSpan = null);

        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> generator, TimeSpan? timeSpan = null);

        void Remove(string key);
    }
}

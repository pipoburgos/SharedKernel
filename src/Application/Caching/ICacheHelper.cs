namespace SharedKernel.Application.Caching
{
    /// <summary>
    /// Cache helper
    /// </summary>
    public interface ICacheHelper
    {
        /// <summary>
        /// Get from cache the T value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Save into cache the T value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T value, TimeSpan? timeSpan = null);

        /// <summary>
        /// Validate if key exists on cache, and if not, call generator function and cache response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="generator"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> generator, TimeSpan? timeSpan = null);

        /// <summary>
        /// Remove cache entry
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }
}

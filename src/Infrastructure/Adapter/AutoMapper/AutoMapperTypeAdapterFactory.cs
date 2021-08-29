using AutoMapper;
using SharedKernel.Application.Adapter;

namespace SharedKernel.Infrastructure.Adapter.AutoMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class AutoMapperTypeAdapterFactory : ITypeAdapterFactory
    {
        private readonly IConfigurationProvider _configurationProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationProvider"></param>
        public AutoMapperTypeAdapterFactory(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ITypeAdapter Create()
        {
            return new AutoMapperTypeAdapter(_configurationProvider);
        }
    }
}

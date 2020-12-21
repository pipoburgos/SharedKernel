using AutoMapper;
using SharedKernel.Application.Adapter;

namespace SharedKernel.Infrastructure.Adapter.AutoMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class AutoMapperTypeAdapterFactory : ITypeAdapterFactory
    {
        private readonly MapperConfiguration _config;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profiles"></param>
        public AutoMapperTypeAdapterFactory(params Profile[] profiles)
        {
            _config = new MapperConfiguration(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ITypeAdapter Create()
        {
            return new AutoMapperTypeAdapter(_config);
        }
    }
}

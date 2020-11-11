using AutoMapper;
using SharedKernel.Application.Adapter;

namespace SharedKernel.Infrastructure.AutoMapper
{
    public class AutoMapperTypeAdapterFactory : ITypeAdapterFactory
    {
        private readonly MapperConfiguration _config;
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

        public ITypeAdapter Create()
        {
            return new AutoMapperTypeAdapter(_config);
        }
    }
}

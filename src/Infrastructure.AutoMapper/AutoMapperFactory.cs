using AutoMapper;
using SharedKernel.Application.Mapper;

namespace SharedKernel.Infrastructure.AutoMapper;

/// <summary> . </summary>
public class AutoMapperFactory : IMapperFactory
{
    private readonly IConfigurationProvider _configurationProvider;

    /// <summary> . </summary>
    public AutoMapperFactory(IConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }

    /// <summary> . </summary>
    public Application.Mapper.IMapper Create()
    {
        return new AutoMapperAdapter(_configurationProvider);
    }
}


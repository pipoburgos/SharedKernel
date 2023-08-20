using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace SharedKernel.Infrastructure.AutoMapper;

/// <summary>  </summary>
public class AutoMapperAdapter : Application.Mapper.IMapper
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IMapper _mapper;

    /// <summary>  </summary>
    public AutoMapperAdapter(IConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider ?? throw new Exception("AutoMapper not initialized");

        _mapper = _configurationProvider.CreateMapper();
    }

    /// <summary>  </summary>
    public TResult MapTo<TResult>(object source)
    {
        return _mapper.Map<TResult>(source);
    }

    /// <summary>  </summary>
    public TDestination MapTo<TSource, TDestination>(TSource source)
    {
        return _mapper.Map<TSource, TDestination>(source);
    }

    /// <summary>  </summary>
    public TDestination MapTo<TSource, TDestination>(TSource source, TDestination destination)
    {
        return _mapper.Map(source, destination);
    }

    /// <summary>  </summary>
    public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source)
    {
        return source.ProjectTo<TDestination>(_configurationProvider);
    }
}

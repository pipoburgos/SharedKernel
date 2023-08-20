namespace SharedKernel.Application.Mapper;

/// <summary> Base contract for adapter factory. </summary>
public interface IMapperFactory
{
    /// <summary> Create a generic type adapter for mapping objects. </summary>
    IMapper Create();
}

using System;

namespace SharedKernel.Application.Mapper;

/// <summary> Mapper factory. </summary>
public static class MapperFactory
{
    #region Members

    private static IMapperFactory _currentTypeAdapterFactory;

    #endregion

    #region Public Static Methods

    /// <summary> Set the current type adapter factory. </summary>
    /// <param name="adapterFactory">The adapter factory to set</param>
    public static void SetCurrent(IMapperFactory adapterFactory)
    {
        _currentTypeAdapterFactory = adapterFactory;
    }

    /// <summary> Create a new type adapter from current factory. </summary>
    /// <returns>Created type adapter</returns>
    public static IMapper Create()
    {
        if (_currentTypeAdapterFactory == null)
            throw new ArgumentNullException(nameof(_currentTypeAdapterFactory));

        return _currentTypeAdapterFactory.Create();
    }

    #endregion
}

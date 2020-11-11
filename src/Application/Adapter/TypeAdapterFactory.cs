using System;

namespace SharedKernel.Application.Adapter
{
    /// <summary>
    /// Mapper factory
    /// </summary>
    public static class TypeAdapterFactory
    {
        #region Members

        private static ITypeAdapterFactory _currentTypeAdapterFactory;

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Set the current type adapter factory
        /// </summary>
        /// <param name="adapterFactory">The adapter factory to set</param>
        public static void SetCurrent(ITypeAdapterFactory adapterFactory)
        {
            _currentTypeAdapterFactory = adapterFactory;
        }

        /// <summary>
        /// Create a new type adapter from current factory
        /// </summary>
        /// <returns>Created type adapter</returns>
        public static ITypeAdapter Create()
        {
            if (_currentTypeAdapterFactory == null)
                throw new ArgumentNullException(nameof(_currentTypeAdapterFactory));

            return _currentTypeAdapterFactory.Create();
        }

        #endregion
    }
}

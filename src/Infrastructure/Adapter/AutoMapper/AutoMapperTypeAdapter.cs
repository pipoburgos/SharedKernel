using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SharedKernel.Application.Adapter;

namespace SharedKernel.Infrastructure.Adapter.AutoMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class AutoMapperTypeAdapter : ITypeAdapter
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationProvider"></param>
        public AutoMapperTypeAdapter(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider ?? throw new Exception("AutoMapper not initialized");

            _mapper = _configurationProvider.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public TResult MapTo<TResult>(object source)
        {
            return _mapper.Map<TResult>(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public TDestination MapTo<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public TDestination MapTo<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source)
        {
            return source.ProjectTo<TDestination>(_configurationProvider);
        }
    }
}

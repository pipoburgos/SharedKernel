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
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapperConfiguration"></param>
        public AutoMapperTypeAdapter(
            MapperConfiguration mapperConfiguration)
        {
            _mapperConfiguration = mapperConfiguration ?? throw new Exception("AutoMapper not initialized");

            _mapper = _mapperConfiguration.CreateMapper();
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
            return source.ProjectTo<TDestination>(_mapperConfiguration);
        }
    }
}

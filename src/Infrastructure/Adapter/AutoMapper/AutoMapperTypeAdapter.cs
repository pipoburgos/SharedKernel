using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SharedKernel.Application.Adapter;

namespace SharedKernel.Infrastructure.Adapter.AutoMapper
{
    public class AutoMapperTypeAdapter : ITypeAdapter
    {
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IMapper _mapper;

        public AutoMapperTypeAdapter(
            MapperConfiguration mapperConfiguration)
        {
            _mapperConfiguration = mapperConfiguration ?? throw new Exception("AutoMapper not initialized");

            _mapper = _mapperConfiguration.CreateMapper();
        }
        public TResult MapTo<TResult>(object source)
        {
            return _mapper.Map<TResult>(source);
        }

        public TDestination MapTo<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        public TDestination MapTo<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source)
        {
            return source.ProjectTo<TDestination>(_mapperConfiguration);
        }
    }
}

using System;
using AutoMapper;
using Data.Service.Abstract;

namespace Data.Service.Concrete
{
    public class AutoMapperConfiguration : IAutoMapperConfiguration
    {
        private readonly MapperConfiguration _mapper_configuration;

        public AutoMapperConfiguration(MapperConfiguration mapper_configuration)
        {
            _mapper_configuration = mapper_configuration;
        }

        private IMapper Mapper()
        {
            return _mapper_configuration.CreateMapper();
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            var map = Mapper();
            var dest = map.Map(source, sourceType, destinationType);
            return dest;
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            var map = Mapper();
            var dest = map.Map(source, destination, sourceType, destinationType);
            return dest;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            var map = Mapper();
            var dest = map.Map<TSource, TDestination>(source);
            return dest;
        }
    }
}
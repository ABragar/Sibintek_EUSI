using System;

namespace Data.Service.Abstract
{
    public interface IAutoMapperConfiguration
    {
        object Map(object source, Type sourceType, Type destinationType);
        object Map(object source, object destination, Type sourceType, Type destinationType);
        TDestination Map<TSource, TDestination>(TSource source);
    }
}
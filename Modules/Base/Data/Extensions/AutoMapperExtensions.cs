using AutoMapper;
using Base;

namespace Data
{
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<T, T> CreateMapForCopy<T>(
            this IMapperConfigurationExpression mapper_configuration) where T : IBaseObject
        {
            return mapper_configuration.CreateMap<T, T>()
                .ForMember(x => x.ID, x => x.Ignore())
                .ForMember(x => x.Hidden, x => x.Ignore())
                .ForMember(x => x.RowVersion, x => x.Ignore())
                .ForMember(x => x.SortOrder, x => x.Ignore());
        }
    }
}
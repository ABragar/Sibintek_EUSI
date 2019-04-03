using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Base;
using Base.Macros.Entities;
using Base.UI.DetailViewSetting;
using Data.Service.Abstract;

namespace Data.Service.Concrete
{
    public class AutoMapperCloner : IAutoMapperCloner
    {
        private readonly IAutoMapperConfiguration _mapper_configuration;

        public AutoMapperCloner(IAutoMapperConfiguration mapper_configuration)
        {
            _mapper_configuration = mapper_configuration;
        }

        public IBaseObject Copy(IBaseObject source)
        {
            var type_source = source.GetType().GetBaseObjectType();
            var retval = _mapper_configuration.Map(source, type_source, type_source);
            return (IBaseObject) retval;
        }

        public IBaseObject Copy(IBaseObject source, IBaseObject destination)
        {
            var type_source = source.GetType().GetBaseObjectType();
            var type_destination = destination.GetType().GetBaseObjectType();
            var retval = _mapper_configuration.Map(source, destination, type_source, type_destination);
            return (IBaseObject) retval;
        }

        public TDestination Copy<TSource, TDestination>(TSource source)
        {
            return _mapper_configuration.Map<TSource, TDestination>(source);
        }
    }
}
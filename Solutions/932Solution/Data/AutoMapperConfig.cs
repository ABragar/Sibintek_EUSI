using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.Data;
using Common.Data.Service.Abstract;
using Common.Data.Service.Concrete;

namespace Data
{
    public static class AutoMapperConfig
    {
        public static IAutoMapperConfiguration BuildConfig()
        {
            var config = new MapperConfiguration(x =>
            {
                AutoMapperCommonConfig.Init(x);


            });


            return new AutoMapperConfiguration(config);
        }
    }
}
using System;
using Base.UI;

namespace WebApi.Services
{
    public interface IGenericTypeResolver
    {
        Type Resolve(string name, string value);
    }

    class TestGenericTypeResolver : IGenericTypeResolver
    {
        public Type Resolve(string name, string value)
        {
            return Type.GetType(value);
        }
    }


    public class ViewModelConfigTypeResolver: IGenericTypeResolver
    {
        private readonly IViewModelConfigService _service;

        public ViewModelConfigTypeResolver(IViewModelConfigService service)
        {
            _service = service;
        }

        public Type Resolve(string name, string value)
        {
            return _service.Get(value)?.TypeEntity;
        }
    }
}
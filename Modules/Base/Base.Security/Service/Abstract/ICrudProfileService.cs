using Base.Service;

namespace Base.Security.Service
{
    public interface ICrudProfileService<T> : IBaseObjectService<T> where T : BaseProfile, new()
    {
        
    }
}
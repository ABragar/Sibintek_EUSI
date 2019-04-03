using System;
using System.Collections.Generic;

namespace Base.DAL
{
    public interface IRepositoryManager : IDisposable
    {
        IRepository<T> RepositoryOf<T>() where T : BaseObject;
        IBaseContext ContextOf<T>() where T : BaseObject;
        IReadOnlyList<IBaseContext> GetContexts();

        
    }
}

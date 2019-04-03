using System;
using System.Linq;
using Base.DAL;
using Base.Service;

namespace Base.Event.Service
{
    public interface IEventService<T> : IBaseObjectService<T>
        where T: Entities.Event
    {
        IQueryable<T> GetRange(IUnitOfWork uofw, DateTime start, DateTime end);
        int GetEventsToRemind();        
    }
}

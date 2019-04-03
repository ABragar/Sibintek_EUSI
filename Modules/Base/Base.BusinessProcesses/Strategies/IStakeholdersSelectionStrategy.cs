using Base.DAL;
using Base.Security;
using System.Collections.Generic;
using System.Linq;
using Base.BusinessProcesses.Entities;

namespace Base.BusinessProcesses.Strategies
{
    public interface IStakeholdersSelectionStrategy
    {
        string Name { get; }
        IQueryable<User> FilterStackholders(IUnitOfWork unitOfWork, IQueryable<User> users, IBPObject obj);
    }

    public interface IStakeholdersSelectionStrategy<T> : IStakeholdersSelectionStrategy where T : class
    {
        IQueryable<User> FilterStackholders(IUnitOfWork unitOfWork, IQueryable<User> users, T obj);
    }
}
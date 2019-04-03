using System;
using Base.DAL;
using Base.Security;
using Base.Security.Service;
using Base.Security.Service.Abstract;
using System.Collections.Generic;
using System.Linq;
using Base.BusinessProcesses.Entities;

namespace Base.BusinessProcesses.Strategies
{
    public class СreatorOnlyObjectStrategy : IStakeholdersSelectionStrategy<ICreateObject>
    {
        public string Name => "Только создатель объекта";



        public IQueryable<User> FilterStackholders(IUnitOfWork unitOfWork, IQueryable<User> users, ICreateObject obj)
        {
            if (obj == null)
                throw new Exception($"Для типа [{obj.GetType().FullName}] невозможно применить стратегию: \"Только создатели объекта\"");

            return users.Where(x => x.ID == obj.CreatorID);
        }

        public IQueryable<User> FilterStackholders(IUnitOfWork unitOfWork, IQueryable<User> users, IBPObject obj)
        {
            return this.FilterStackholders(unitOfWork, users, (ICreateObject)obj);
        }
    }

    public class AdminOnlyStrategy : IStakeholdersSelectionStrategy<IBPObject>
    {
        public string Name => "Только администратор";
        IQueryable<User> IStakeholdersSelectionStrategy<IBPObject>.FilterStackholders(IUnitOfWork unitOfWork, IQueryable<User> users, IBPObject obj)
        {
            return FilterStackholders(unitOfWork, users, obj);
        }

        public IQueryable<User> FilterStackholders(IUnitOfWork unitOfWork, IQueryable<User> users, IBPObject obj)
        {            
            return users.Where(x => x.Category_.ID == 1);
        }
    }

}
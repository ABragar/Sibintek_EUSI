using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.BusinessProcesses.Entities;
using Base.BusinessProcesses.Strategies;
using Base.DAL;
using Base.Security;

namespace Data.Entities.Test
{
    public class TestStrategy : IStakeholdersSelectionStrategy<TestObject>
    {
        public IQueryable<User> FilterStackholders(IUnitOfWork unitOfWork, IQueryable<User> users, TestObject obj)
        {
            return FilterStackholders(unitOfWork, users, (IBPObject) obj);
        }

        public string Name => "Тестовая страта";

        public IQueryable<User> FilterStackholders(IUnitOfWork unitOfWork, IQueryable<User> users, IBPObject obj)
        {
            return users.Where(x => x.ID%2 == 0);
        }
    }
}
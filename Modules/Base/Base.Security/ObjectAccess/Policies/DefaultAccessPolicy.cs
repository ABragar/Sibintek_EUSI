using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Security.ObjectAccess.Policies
{
    public class DefaultAccessPolicy : BaseAccessPolicy
    {
        private readonly ISystemUnitOfWork _systemUnitOfWork;

        public DefaultAccessPolicy(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _systemUnitOfWork = unitOfWorkFactory.CreateSystem();
        }

        protected IUnitOfWork UnitOfWork
        {
            get { return _systemUnitOfWork; }
        }

        public override ObjectAccessItem InitializeAccessItem(ObjectAccessItem accessItem, int userID, Type objectType)
        {
            accessItem.Users.Add(new UserAccess() { UserID = userID });

            return accessItem;
        }
    }
}
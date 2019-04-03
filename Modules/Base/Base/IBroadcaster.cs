using System;
using System.Collections.Generic;
using Base.DAL;
using Base.Security;

namespace Base
{
    public interface IBroadcaster
    {
        #region METHODS
        void UpdateCounters(Type type);
        void UpdateCounters(Type type, IUser user);
        void UpdateCounters(Type type, IEnumerable<IUser> users);
        void UpdateCounters(Type type, IUnitOfWork uofw, int userID);
        void UpdateCounters(Type type, IUnitOfWork uofw, IEnumerable<int> userIDs);
        #endregion

        #region EVENTS
        event UpdateCountersHandler OnUpdateCounters;
        #endregion
    }

    public delegate void UpdateCountersHandler(object sender, BroadcasterEventArgs e);

    public class BroadcasterEventArgs : EventArgs
    {
        public BroadcasterEventArgs(Type entity, IUser user = null)
        {
            Entity = entity;
            User = user;
        }

        public Type Entity { get; private set; }
        public IUser User { get; private set; }
    }
}

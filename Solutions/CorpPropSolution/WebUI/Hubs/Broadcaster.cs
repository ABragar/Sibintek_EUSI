using Base;
using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.Security;
using Base.Service;


namespace WebUI.Hubs
{
    public class Broadcaster : IBroadcaster
    {
        private readonly IUserService<User> _userService;

        public Broadcaster(IUserService<User> userService)
        {
            _userService = userService;
        }

        //public void SendNotification(IUnitOfWork unitOfWork, Notification notification)
        //{
        //    string user = notification.User != null ? notification.User.Login : null;

        //    if (user == null && notification.UserID != null)
        //    {
        //        user =
        //            unitOfWork.GetRepository<User>()
        //                .All()
        //                .Where(x => x.ID == notification.UserID)
        //                .Select(x => x.Login)
        //                .FirstOrDefault();
        //    }

        //    if(String.IsNullOrEmpty(user)) return;

        //    if (notification.Status == NotificationStatus.New)
        //        _hubContext.Clients.User(user).create(notification.Title);
        //    else
        //        _hubContext.Clients.User(user).update(notification.ID);
        //}

        public void UpdateCounters(Type type)
        {
            _updateCountersHandler?.Invoke(this, new BroadcasterEventArgs(type));
        }

        public void UpdateCounters(Type type, IUser user)
        {
            if (user != null)
            {
                _updateCountersHandler?.Invoke(this, new BroadcasterEventArgs(type, user));
            }
        }

        public void UpdateCounters(Type type, IEnumerable<IUser> users)
        {
            if (users == null) return;

            foreach (var user in users)
            {
                _updateCountersHandler?.Invoke(this, new BroadcasterEventArgs(type, user));
            }
        }
        
        public void UpdateCounters(Type type, IUnitOfWork uofw, int userID)
        {
            UpdateCounters(type, _userService.Get(uofw, userID));
        }
        //TODO Не вызывать синхронном коде
        public async void UpdateCounters(Type type, IUnitOfWork uofw, IEnumerable<int> userIDs)
        {
            IEnumerable<IUser> users = null;

            if (userIDs != null)
            {
                users = await _userService.GetAllAsync(uofw, userIDs);
            }

            UpdateCounters(type, users);
        }

        // ONLY ONE HANDLER MAY LISTEN THIS EVENT
        private UpdateCountersHandler _updateCountersHandler;
        public event UpdateCountersHandler OnUpdateCounters
        {
            add
            {
                _updateCountersHandler = value;
            }
            remove { /*EMPTY*/ }
        }
    }
}
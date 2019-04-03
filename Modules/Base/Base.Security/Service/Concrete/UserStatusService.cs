using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.DAL;
using Base.Extensions;
using Base.Security;
using Base.Security.Service.Abstract;

namespace Base.Security.Service
{
    public class UserStatusService : IUserStatusService
    {
        public UserStatusService()
        {
            UserStatuses = new Dictionary<int, IUserStatus>();
        }

        public Dictionary<int, IUserStatus> UserStatuses { get; set; }

        public IUserStatus GetUserStatus(int userId)
        {
            return UserStatuses.FirstOrDefault(x => x.Key == userId).Value;
        }

        public IUserStatus GetUserStatus(string connectionId)
        {
            return UserStatuses.FirstOrDefault(x => x.Value.ConnectionId == connectionId).Value;
        }

        public IUserStatus SetOnline(int userId, string connectionId)
        {
            var status = GetUserStatus(userId);

            if (status != null)
            {
                status.ConnectionId = connectionId;
                status.Online = true;
                return status;
            }

            var userStatus = new UserStatus()
            {
                UserId = userId,
                ConnectionId = connectionId,
                Online = true
            };

            UserStatuses.Add(userStatus.UserId, userStatus);

            return userStatus;
        }

        public IUserStatus SetOnline(IUserStatus userStatus)
        {
            if (userStatus == null)
                return null;

            var status = GetUserStatus(userStatus.UserId);

            if (status != null)
            {
                status.ConnectionId = userStatus.ConnectionId;
                status.Online = true;
                return status;
            }

            UserStatuses.Add(userStatus.UserId, userStatus);
            return userStatus;
        }

        private IUserStatus _setOffline(IUserStatus userStatus)
        {
            userStatus.Online = false;
            userStatus.LastDate = DateTime.Now;
            return userStatus;
        }

        public IUserStatus SetOffline(int userId)
        {
            var status = GetUserStatus(userId);
            return status == null ? null : _setOffline(status);
        }

        public IUserStatus SetOffline(string connectionId)
        {
            var status = GetUserStatus(connectionId);
            return status == null ? null : _setOffline(status);
        }

        public IUserStatus SetOffline(IUserStatus userStatus)
        {
            var status = GetUserStatus(userStatus.UserId);
            return status == null ? null : _setOffline(status);
        }

        public IUserStatus SetCustomStatus(IUnitOfWork uofw, int userId, CustomStatus status)
        {
            var userStatus = GetUserStatus(userId);

            if (userStatus == null)
                return null;

            userStatus.LastCustomStatus = userStatus.CustomStatus;

            if (status == CustomStatus.Disconnected)
            {
                userStatus.LastPublicDate = DateTime.Now;

                if (!userStatus.LastPublicCustomStatus.HasValue)
                {
                    userStatus.LastPublicCustomStatus = userStatus.LastCustomStatus != CustomStatus.Disconnected
                        ? userStatus.LastCustomStatus
                        : CustomStatus.Ready;
                }
            }
            
            userStatus.CustomStatus = status;

            // IF STATUS IS NOT "INCONVERSATION" - SAVE STATUS TO THE DB
            if (status != CustomStatus.InConversation)
            {
                var user = uofw.GetRepository<User>().Find(x => x.ID == userId);

                if (user == null)
                    return null;

                user.CustomStatus = userStatus.CustomStatus;

                uofw.SaveChanges();
            }

            return userStatus;
        }

        public IUserStatus SetCustomStatus(IUnitOfWork uofw, IUserStatus userStatus, CustomStatus status)
        {
            return userStatus != null
                ? SetCustomStatus(uofw, userStatus.UserId, status)
                : null;
        }

        public async Task<IUserStatus> SetCustomStatusAsync(IUnitOfWork uofw, int userId, CustomStatus status)
        {
            var userStatus = GetUserStatus(userId);

            if (userStatus == null)
                return null;

            userStatus.LastCustomStatus = userStatus.CustomStatus;

            if (status == CustomStatus.Disconnected)
            {
                userStatus.LastPublicDate = DateTime.Now;

                if (!userStatus.LastPublicCustomStatus.HasValue)
                {
                    userStatus.LastPublicCustomStatus = userStatus.LastCustomStatus != CustomStatus.Disconnected
                        ? userStatus.LastCustomStatus
                        : CustomStatus.Ready;
                }
            }

            userStatus.CustomStatus = status;

            // IF STATUS IS NOT "INCONVERSATION" - SAVE STATUS TO THE DB
            //if (status != CustomStatus.InConversation)
            //{
            var user = await uofw.GetRepository<User>().All().Where(u => u.ID == userId).FirstOrDefaultAsync();

            if (user == null)
                return null;

            user.CustomStatus = userStatus.CustomStatus;

            await uofw.SaveChangesAsync();
            //}

            return userStatus;
        }

        public async Task<IUserStatus> SetCustomStatusAsync(IUnitOfWork uofw, IUserStatus userStatus, CustomStatus status)
        {
            return userStatus != null
                ? await SetCustomStatusAsync(uofw, userStatus.UserId, status)
                : null;
        }

        private IEnumerable<IUserStatus> _getUserStatuses(bool online, bool ignoreCustomStatus = false)
        {
            var result = UserStatuses.Where(x => x.Value.Online == online);

            if (!ignoreCustomStatus)
                result = result.Where(x => x.Value.CustomStatus != CustomStatus.Disconnected);

            return result.Select(x => x.Value);
        }

        public IEnumerable<IUserStatus> GetUserStatuses(bool online = true)
        {
            return _getUserStatuses(online);
        }

        public IEnumerable<IUserStatus> GetSystemUserStatuses(bool online = true)
        {
            return _getUserStatuses(online, true);
        }

        public IEnumerable<int> GetOnlineIds()
        {
            return UserStatuses.Any()
                ? GetUserStatuses().Select(x => x.UserId)
                : new int[0];
        }

        public IEnumerable<int> GetSystemOnlineIds()
        {
            return UserStatuses.Any()
                ? GetSystemUserStatuses().Select(x => x.UserId)
                : new int[0];
        }

        public IEnumerable<int> GetOnlineIds(IEnumerable<int> containsIds)
        {
            return containsIds.Where(x => GetOnlineIds().Contains(x));
        }

        public IEnumerable<int> GetSystemOnlineIds(IEnumerable<int> containsIds)
        {
            return containsIds.Where(x => GetSystemOnlineIds().Contains(x));
        }
    }
}

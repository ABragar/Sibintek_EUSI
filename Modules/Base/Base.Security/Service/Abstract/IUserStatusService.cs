using System.Collections.Generic;
using System.Threading.Tasks;
using Base.DAL;

namespace Base.Security.Service.Abstract
{
    public interface IUserStatusService
    {
        Dictionary<int, IUserStatus> UserStatuses { get; set; }

        IUserStatus GetUserStatus(int userId);
        IUserStatus GetUserStatus(string connectionId);

        IUserStatus SetOnline(int userId, string connectionId);
        IUserStatus SetOnline(IUserStatus userStatus);

        IUserStatus SetOffline(int userId);
        IUserStatus SetOffline(string connectionId);
        IUserStatus SetOffline(IUserStatus userStatus);

        IUserStatus SetCustomStatus (IUnitOfWork uofw, int userId, CustomStatus status);
        IUserStatus SetCustomStatus(IUnitOfWork uofw, IUserStatus userStatus, CustomStatus status);

        Task<IUserStatus> SetCustomStatusAsync(IUnitOfWork uofw, int userId, CustomStatus status);
        Task<IUserStatus> SetCustomStatusAsync(IUnitOfWork uofw, IUserStatus userStatus, CustomStatus status);

        IEnumerable<IUserStatus> GetUserStatuses(bool online = true);
        IEnumerable<int> GetOnlineIds();
        IEnumerable<int> GetOnlineIds(IEnumerable<int> containsIds);

        IEnumerable<IUserStatus> GetSystemUserStatuses(bool online = true);
        IEnumerable<int> GetSystemOnlineIds();
        IEnumerable<int> GetSystemOnlineIds(IEnumerable<int> containsIds);
    }
}
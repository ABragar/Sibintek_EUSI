using System.Threading.Tasks;
using Base.PBX.Entities;
using Base.PBX.Models;
using Base.Utils.Common;

namespace Base.PBX.Services.Abstract
{
    public interface IPBXUserService
    {
        PBXUser CreateUser(IPBXServer server, PBXUser user);
        Task<PBXUser> CreateUserAsync(IPBXServer server, PBXUser user);

        void DeleteUser(IPBXServer server, string extension);
        Task DeleteUserAsync(IPBXServer server, string extension);

        PBXUser UpdateUser(IPBXServer server, PBXUser user);
        Task<PBXUser> UpdateUserAsync(IPBXServer server, PBXUser user);

        PBXUser GetUser(IPBXServer server, string extension);
        Task<PBXUser> GetUserAsync(IPBXServer server, string extension);

        IPageResult GetPagedUsers(IPBXServer server, int page, int limit);
        Task<IPageResult> GetPagedUsersAsync(IPBXServer server, int page, int limit);

        void ApplyChanges(IPBXServer server);
        Task ApplyChangesAsync(IPBXServer server);

        void Reboot(IPBXServer server);
        Task RebootAsync(IPBXServer server);

        PBXServerStatus GetServerStatus(IPBXServer server);
        Task<PBXServerStatus> GetServerStatusAsync(IPBXServer server);

        string GetAvailableNumber(IPBXServer server);
        Task<string> GetAvailableNumberAsync(IPBXServer server);

        bool IsNumberExist(IPBXServer server, int number);
        Task<bool> IsNumberExistAsync(IPBXServer server, int number);
    }
}
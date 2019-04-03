using Base.Security;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using Base.Entities;
using Base.Events.Auth;

namespace Base.Identity.Core
{
    public interface IAccountManager
    {

        IAuthSettings Settings { get; }

        Task<AccountInfo> GetAccountInfoAsync(int user_id);

        Task<AuthResult> AuthenticateByLoginAsync(UserLoginInfo login_info);
        Task<AuthResult> AuthenticateByLoginAsync(string login);

        Task<AuthResult> AuthenticateByPasswordAsync(string login, string password, bool check_lockout);
        Task<AuthResult> SignOutByLogin(string login);

        Task<AuthResult> ConfirmEmailAsync(string code);
        Task<AuthResult> ResetPasswordAsync(string code, string new_password);

        Task<RegisterResult> RegisterByUserIdAsync(int userId, UserInfo info);
        Task<AuthResult> RegisterByLoginAsync(UserInfo info, UserLoginInfo login_info, bool createUser = false);
        Task<AuthResult> RegisterByPasswordAsync(UserInfo info, string password, bool createUser = false);

        Task<IdentityResult> AddLoginAsync(int user_id, UserLoginInfo login_info);
        Task<IdentityResult> RemoveLoginAsync(int user_id, UserLoginInfo login_info);
        Task<IdentityResult> AddPasswordAsync(int user_id, string password);

        Task<IdentityResult> RemovePasswordAsync(int user_id);

        Task<IdentityResult> ChangePasswordAsync(int user_id, string old_password, string new_password);

        Task<IdentityResult> SetPasswordAsync(int user_id, string new_password);

        Task<IdentityResult> SendConfirmEmailCodeAsync(string login, string title, Func<string, string> body_func);

        Task<IdentityResult> SendResetPasswordCodeAsync(string login, string title, Func<string, string> body_func);

        void RegisterAuthEvent(AuthResult authResult);
    }
}

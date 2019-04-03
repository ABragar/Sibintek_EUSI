using System;
using System.CodeDom;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Base.DAL;
using Base.Identity.Entities;
using Base.Security;
using Base.Security.Service.Abstract;
using Microsoft.AspNet.Identity;

namespace Base.Identity.Core
{
    public class UserManager : UserManager<AccountEntry, int>
    {


        public static IdentityResult UserLoginExistResult = IdentityResult.Failed("Метод входа занят");

        private readonly IUserInfoService _info_service;
        private readonly ITransactionUnitOfWork _unit_of_work;

        private readonly ICustomPasswordStorage _password_storage;

        public UserManager(ITransactionUnitOfWork unit_of_work, IUserManagerOptions options) : base(new AccountStore(unit_of_work, options.UserInfoService))
        {

            _password_storage = options.CustomPasswordStorage;

            EmailService = options.EmailService;

            UserTokenProvider = options.UserTokenProvider;


            UserValidator = new UserValidator<AccountEntry, int>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
            };

            _unit_of_work = unit_of_work;
            _info_service = options.UserInfoService;
            AccountStore = (AccountStore)Store;
        }

        protected readonly AccountStore AccountStore;

        public AccountEntry GetAccount(UserInfo info)
        {
            return new AccountEntry
            {
                Email = info.Email,
                UserName = info.Login ?? info.Email,
                FirstName = info.FirstName,
                LastName = info.LastName,
            };
        }

        public AccountEntry GetAccount(int user_id, UserInfo info)
        {
            return SetUserId(GetAccount(info), user_id);

        }

        public Task<AccountEntry> FindByUserIdAsync(int user_id)
        {
            return AccountStore.FindByUserIdAsync(user_id);
        }

        public AccountEntry GetSystemAccount(int user_id, string name)
        {
            return SetUserId(new AccountEntry
            {
                IsSystem = true,
                UserName = name
            }, user_id);


        }

        public bool IsUser(AccountEntry account)
        {
            return account.IsUser;
        }

        private AccountEntry SetUserId(AccountEntry account, int user_id)
        {
            account.UserId = user_id;
            account.IsUser = true;

            return account;
        }

        public async Task<AccountEntry> CreateUserIdAsync(AccountEntry account)
        {

            var user_id = await _info_service.CreateUserAsync(_unit_of_work,
                new UserInfo
                {
                    Email = account.EmailConfirmed ? account.Email : null,
                    FirstName = account.FirstName,
                    LastName = account.LastName,

                });

            return SetUserId(account, user_id);
        }

        public async Task<IdentityResult> CreateAsync(AccountEntry account, UserLoginInfo login_info)
        {
            var user = await FindAsync(login_info);

            if (user != null)
                return UserLoginExistResult;


            await AccountStore.AddLoginAsync(account, login_info);

            return await CreateAsync(account);
        }

        public bool IsSystem(AccountEntry account)
        {
            return account.IsSystem;
        }

        public async Task<AccountInfo> GetAccountInfoAsync(AccountEntry account)
        {
            //TODO
            if (account == null)
                return null;

            return new AccountInfo
            {
                UserId = account.UserId,
                Login = account.UserName,
                ExternalLogins = (await AccountStore.GetLoginsAsync(account)).ToArray(),
                HasPassword = await AccountStore.HasPasswordAsync(account),
                HasEmail = await AccountStore.GetEmailAsync(account) != null,
                EmailConfirmed = await AccountStore.GetEmailConfirmedAsync(account),
            };
        }

        public async Task SetEmailConfirmedAsync(int userId)
        {
            var account = await AccountStore.FindByIdAsync(userId);

            await AccountStore.SetEmailConfirmedAsync(account, true);

            await this.UpdateAsync(account);
        }

        //public async Task<IdentityResult> CreateAsync(Account account, UserLoginInfo login_info)
        //{


        //    await CreateAsync(account);
        //}


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Store.Dispose();
        }

        public override Task<IdentityResult> RemovePasswordAsync(int userId)
        {
            return base.RemovePasswordAsync(userId);
        }
        public override Task<IdentityResult> AddPasswordAsync(int userId, string password)
        {
            return base.AddPasswordAsync(userId, password);
        }

        public override Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            return base.ChangePasswordAsync(userId, currentPassword, newPassword);
        }

        public override Task<bool> CheckPasswordAsync(AccountEntry user, string password)
        {
            return base.CheckPasswordAsync(user, password);
        }



        public override Task<string> GeneratePasswordResetTokenAsync(int userId)
        {
            return base.GeneratePasswordResetTokenAsync(userId);
        }

        public override Task<bool> HasPasswordAsync(int userId)
        {
            return base.HasPasswordAsync(userId);
        }

        public override Task<IdentityResult> ResetPasswordAsync(int userId, string token, string newPassword)
        {
            return base.ResetPasswordAsync(userId, token, newPassword);
        }

        protected override async Task<IdentityResult> UpdatePassword(IUserPasswordStore<AccountEntry, int> passwordStore, AccountEntry user, string newPassword)
        {

            if (_password_storage != null)
            {

                var operatuion_result = await _password_storage.SetPasswordAsync(user.UserName, newPassword);
                switch (operatuion_result.Item1)
                {
                    // TODO need reset
                    case SetPasswordResult.Success:
                        return IdentityResult.Success;
                    case SetPasswordResult.Failed:
                        return IdentityResult.Failed("Ошибка установки пароля");
                    case SetPasswordResult.UseHash:
                        passwordStore = new UserPasswordStore(passwordStore, operatuion_result.Item2);
                        break;
                    case SetPasswordResult.UseDefault:
                        break;
                    default: throw new NotSupportedException();

                }

            }

            var result = await base.UpdatePassword(passwordStore, user, newPassword);


            return result;
        }


        private class UserPasswordStore : IUserPasswordStore<AccountEntry, int>
        {
            private readonly IUserPasswordStore<AccountEntry, int> _inner;
            private readonly string _hash;

            public UserPasswordStore(IUserPasswordStore<AccountEntry, int> inner, string hash)
            {
                _inner = inner;
                _hash = hash;
            }

            public void Dispose()
            {
                _inner.Dispose();

            }

            public Task CreateAsync(AccountEntry user)
            {
                return _inner.CreateAsync(user);

            }

            public Task UpdateAsync(AccountEntry user)
            {
                return _inner.UpdateAsync(user);
            }

            public Task DeleteAsync(AccountEntry user)
            {
                return _inner.DeleteAsync(user);
            }

            public Task<AccountEntry> FindByIdAsync(int userId)
            {
                return _inner.FindByIdAsync(userId);
            }

            public Task<AccountEntry> FindByNameAsync(string userName)
            {
                return _inner.FindByNameAsync(userName);
            }

            public Task SetPasswordHashAsync(AccountEntry user, string passwordHash)
            {
                return _inner.SetPasswordHashAsync(user, _hash);
            }

            public Task<string> GetPasswordHashAsync(AccountEntry user)
            {
                return Task.FromResult(_hash);
            }

            public Task<bool> HasPasswordAsync(AccountEntry user)
            {
                return Task.FromResult(_hash != null);
            }
        }

        protected override async Task<bool> VerifyPasswordAsync(IUserPasswordStore<AccountEntry, int> store, AccountEntry user, string password)
        {


            if (_password_storage != null)
            {
                var operatuion_result =
                    await
                        _password_storage.VerifyPasswordAsync(user.UserName, await store.GetPasswordHashAsync(user),
                            password);


                switch (operatuion_result)
                {
                    // TODO need reset
                    case VerifyPasswordResult.NeedReset:
                        return true;
                    case VerifyPasswordResult.Failed:
                        return false;
                    case VerifyPasswordResult.Success:
                        return true;
                    case VerifyPasswordResult.UseDefault:
                        break;
                    default: throw new NotSupportedException();

                }
            }

            return await base.VerifyPasswordAsync(store, user, password);

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Base.DAL;
using Base.Extensions;
using Base.Identity.Entities;
using Base.Security.Service.Abstract;
using Microsoft.AspNet.Identity;

namespace Base.Identity.Core
{
    public class AccountStore :
        IUserEmailStore<AccountEntry, int>,
        IUserPasswordStore<AccountEntry, int>,
        IUserLoginStore<AccountEntry, int>,
        IUserSecurityStampStore<AccountEntry, int>

    {
        private ITransactionUnitOfWork _unit_of_work;
        private IUserInfoService _info_service;

        private readonly IRepository<AccountEntry> _repository;
        private readonly IRepository<ExternalLogin> _logins_repository;

        public AccountStore(ITransactionUnitOfWork unit_of_work, IUserInfoService info_service)
        {
            _unit_of_work = unit_of_work;
            _info_service = info_service;
            _repository = unit_of_work.GetRepository<AccountEntry>();
            _logins_repository = unit_of_work.GetRepository<ExternalLogin>();
        }


        public Task SetEmailAsync(AccountEntry user, string email)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(AccountEntry user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(AccountEntry user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(AccountEntry user, bool confirmed)
        {
            if (user.Email == null)
                throw new InvalidOperationException();

            user.EmailConfirmed = confirmed;

            return Task.CompletedTask;

        }

        public Task<AccountEntry> FindByEmailAsync(string email)
        {

            return FindSingleAsync(x => x.Email == email);
        }

        public void Dispose()
        {
            _unit_of_work = null;
            _info_service = null;
        }

        public Task CreateAsync(AccountEntry user)
        {
            if (user.ID != 0)
                throw new InvalidOperationException();

            _repository.Create(user);
            foreach (var login in user.ExternalLogins)
            {
                _logins_repository.Create(login);
            }
            

            return _unit_of_work.SaveChangesAsync();

        }

        public Task UpdateAsync(AccountEntry user)
        {
            if (user.ID == 0)
                throw new InvalidOperationException();

            _repository.Update(user);


            foreach (var login in user.ExternalLogins)
            {
                if (login.ID == 0)
                    _logins_repository.Create(login);
            }
            
            foreach (var login in _logins_repository.All().Where(x=>x.AccountId == user.ID))
            {
                if (user.ExternalLogins.All(x => x.ID != login.ID))
                    _logins_repository.Delete(login);
            }

            return _unit_of_work.SaveChangesAsync();

        }

        public Task DeleteAsync(AccountEntry user)
        {

            if (user.ID == 0)
                throw new InvalidOperationException();

            _repository.Delete(user);

            foreach (var login in _logins_repository.All().Where(x => x.AccountId == user.ID))
            {
                _logins_repository.Delete(login);
            }

            return _unit_of_work.SaveChangesAsync();
        }


        public Task<AccountEntry> FindByUserIdAsync(int user_id)
        {
            return FindSingleAsync(x => x.UserId == user_id && x.IsUser, true);
        }

        public Task<AccountEntry> FindByIdAsync(int userId)
        {
            return Task.FromResult(_repository.Find(userId));
        }

        public Task<AccountEntry> FindByNameAsync(string userName)
        {
            return FindSingleAsync(x => x.UserName == userName);
        }

        public Task<bool> GetLockoutEnabledAsync(AccountEntry user)
        {
            return Task.FromResult(true);
        }

        public Task SetLockoutEnabledAsync(AccountEntry user, bool enabled)
        {

            return Task.CompletedTask;
        }

        public Task AddLoginAsync(AccountEntry user, UserLoginInfo login)
        {
            user.ExternalLogins.Add(new ExternalLogin { ProviderKey = login.LoginProvider, UserKey = login.ProviderKey });

            return Task.CompletedTask;
        }

        public Task RemoveLoginAsync(AccountEntry user, UserLoginInfo login)
        {
            var entry = user.ExternalLogins.SingleOrDefault(x => x.ProviderKey == login.LoginProvider && x.UserKey == login.ProviderKey);
            if (entry != null)
                user.ExternalLogins.Remove(entry);

            return Task.CompletedTask;

        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(AccountEntry user)
        {
            var logins = user.ExternalLogins.Select(x => new UserLoginInfo(x.ProviderKey, x.UserKey)).ToList();

            return Task.FromResult<IList<UserLoginInfo>>(logins);

        }

        public Task<AccountEntry> FindAsync(UserLoginInfo login)
        {
            var user_key = login.ProviderKey;
            var provider_key = login.LoginProvider;

            return
                FindSingleAsync(
                    x =>
                        x.ExternalLogins.Any(
                            external => external.ProviderKey == provider_key && external.UserKey == user_key));

        }

        public Task SetPasswordHashAsync(AccountEntry user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(AccountEntry user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(AccountEntry user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetSecurityStampAsync(AccountEntry user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        public Task<string> GetSecurityStampAsync(AccountEntry user)
        {
            return Task.FromResult(user.SecurityStamp);

        }

        protected async Task<AccountEntry> CheckExistAsync(AccountEntry account)
        {
            if (account.IsUser && !await _info_service.UserExistsAsync(_unit_of_work, account.UserId))
            {
                await DeleteAsync(account);
                return null;
            }
            return account;
        }

        protected async Task<AccountEntry> FindSingleAsync(Expression<Func<AccountEntry, bool>> func, bool check_exist = true)
        {
            var account = await _repository.All().Where(func).SingleOrDefaultAsync();

            if (account == null)
                return null;

            if (check_exist)
                return await CheckExistAsync(account);

            return account;
            

            
        }
    }
}
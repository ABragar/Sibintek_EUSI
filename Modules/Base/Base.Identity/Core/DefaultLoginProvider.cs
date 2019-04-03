using System;
using System.Linq;
using System.Threading.Tasks;
using Base.DAL;
using Base.Security;
using Base.Security.Service.Abstract;
using Base.Service;

namespace Base.Identity.Core
{
    public class DefaultLoginProvider : ILoginProvider
    {
        private readonly IUserManagerOptions _options;
        private readonly IExecutionContextScopeManager _scope_manager;


        public DefaultLoginProvider(IUserManagerOptions options, IExecutionContextScopeManager scope_manager)
        {
            _options = options;
            _scope_manager = scope_manager;

        }

        private TResult UsingManager<TResult>(IUnitOfWork unit_of_work,
            Func<UserManager, Task<TResult>> func)
        {
            if (!_scope_manager.InScope)
                throw new InvalidOperationException();

            return Task.Run(async () =>
            {

                using (var manager = new UserManager((ITransactionUnitOfWork)unit_of_work, _options))
                {
                    return await func(manager);
                }
            }).Result;

        }

        public bool Exist(IUnitOfWork unit_of_work, string email)
        {

            return UsingManager(unit_of_work, x => x.FindByNameAsync(email)) != null;
        }


        public void AttachPassword(IUnitOfWork unit_of_work, int user_id, string email, string password)
        {

            UsingManager(unit_of_work, async x =>
            {
                var account = x.GetAccount(user_id, new UserInfo { Email = email });

                var result = password != null ? await x.CreateAsync(account, password) : await x.CreateAsync(account);

                if (!result.Succeeded)
                    throw new OperationException(result.Errors.ToArray());

                return result;
            });


        }

        public void AttachSystemPassword(IUnitOfWork unit_of_work, int user_id, string email, string password)
        {
            UsingManager(unit_of_work, async x =>
            {
                var account = x.GetSystemAccount(user_id, email);

                var result = await x.CreateAsync(account, password);

                if (!result.Succeeded)
                    throw new OperationException(result.Errors.ToArray());

                return result;
            });
        }
    }
}
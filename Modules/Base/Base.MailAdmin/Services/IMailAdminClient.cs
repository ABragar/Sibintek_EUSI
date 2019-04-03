using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Proxies.Models;

namespace Base.MailAdmin.Services
{
    public interface IMailAdminClient
    {

        /// <param name="search"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>

        /// <returns></returns>
        Task<IReadOnlyCollection<AccountListModel>> SearchAccountsAsync(String search, Int32? take, Int32? skip);

        /// <returns></returns>
        Task<GetCountModel> GetCountAsync();


        /// <param name="account_id"></param>

        /// <returns></returns>
        Task<AccountDetailModel> GetAccountAsync(String account_id);

        /// <param name="name"></param>

        /// <returns></returns>
        Task<AccountDetailModel> GetAccountByNameAsync(String name);


        /// <returns></returns>
        Task<AccountDetailModel> CreateAccountAsync(CreateAccountModel model);

        /// <param name="account_id"></param>

        /// <returns></returns>
        Task AddAliasAsync(String account_id, AccountAlias model);

        /// <param name="account_id"></param>

        /// <returns></returns>
        Task RemoveAliasAsync(String account_id, AccountAlias model);

        /// <param name="account_id"></param>

        /// <returns></returns>
        Task SetAccountStatusAsync(String account_id, SetAccountStatusModel model);

        /// <param name="account_id"></param>

        /// <returns></returns>
        Task SetPasswordAsync(String account_id, SetPasswordModel model);

        /// <param name="account_id"></param>

        /// <returns></returns>
        Task SetForwardingListAsync(String account_id, SetForwardingListModel model);

        /// <param name="account_id"></param>

        /// <returns></returns>
        Task SetNameAsync(String account_id, SetNameModel model);

        /// <param name="account_id"></param>

        /// <returns></returns>
        Task SetQuotaAsync(String account_id, SetQuotaModel model);

    }
}
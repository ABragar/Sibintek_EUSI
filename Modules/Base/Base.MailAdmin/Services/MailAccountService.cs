using System;
using System.Linq;
using System.Threading.Tasks;
using Base.Identity.Core;
using Base.MailAdmin.Entities;
using WebApi.Proxies.Models;

namespace Base.MailAdmin.Services
{
    public class MailAccountService : IMailAccountService
    {
        private readonly IMailAdminClient _mail_admin_client;
        private readonly IAccountManager _account_manager;

        public MailAccountService(IMailAdminClient mailAdminClient, IAccountManager account_manager)
        {
            _mail_admin_client = mailAdminClient;
            _account_manager = account_manager;
        }

        public async Task<MailAccount> GetByIserId(int userid)
        {
            var account_info = await _account_manager.GetAccountInfoAsync(userid);

            var account = await _mail_admin_client.GetAccountByNameAsync(account_info.Login);

            return new MailAccount()
            {
                Name = account.Name,
                AccountId = account.AccountId,
                CreatedDate = account.CreatedDate,
                LastLogonDate = account.LastLogonDate,
                Status = account.Status,
                Size = account.Size,
                Quota = account.Quota,
                AliasList = account.AliasList?.Select(x => new VmAccountAlias() { Name = x.Name }),
                ForwardingList = account.ForwardingList?.Select(x => new VmForwardingList() { Name = x })
            };
        }

        public async Task<MailAccount> Save(MailAccount mailAccount)
        {
            if (mailAccount.Status != null)
                await _mail_admin_client.SetAccountStatusAsync(mailAccount.AccountId, new SetAccountStatusModel() { Status = mailAccount.Status.Value });

            await _mail_admin_client.SetQuotaAsync(mailAccount.AccountId, new SetQuotaModel() { Quota = mailAccount.Quota });

            return mailAccount;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Base.Identity;
using Base.MailAdmin.Entities;
using Base.Settings;
using WebApi.Proxies.Models;

namespace Base.MailAdmin.Services
{
    public class MailAdminClient : IMailAdminClient
    {
        private readonly HttpClient _http_client;

        private readonly ISettingService<MailAdminSettings> _settings;
        private readonly TokenService _token_service;

        public MailAdminClient(ISettingService<MailAdminSettings> settings, TokenService token_service)
        {
            _settings = settings;
            _token_service = token_service;

            var handler = new HttpClientHandler();

            _http_client = new HttpClient(handler,true);
        }



        protected async Task CheckAsync(HttpResponseMessage responce)
        {
            if (!responce.IsSuccessStatusCode)
                throw new InvalidOperationException(responce.StatusCode+":"+await responce.Content.ReadAsStringAsync());
        }

        protected async Task<HttpResponseMessage> SendAsync(HttpMethod method, string uri_string, object model)
        {
            var base_address = _settings.Get().BaseAddress;


            var uri = new Uri(new Uri(base_address, UriKind.Absolute), uri_string);


            var request = new HttpRequestMessage(method, uri);


            if (model != null)
            {
                request.Content = new ObjectContent(model.GetType(), model, new JsonMediaTypeFormatter());
            }
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token_service.GetToken(Ambient.AppContext.SecurityUser));

            var response = await _http_client.SendAsync(request);

            await CheckAsync(response);

            return response;
        }
        protected Task<HttpResponseMessage> GetAsync(string uri_string)
        {
            return SendAsync(HttpMethod.Get, uri_string, null);
        }



        public async Task<IReadOnlyCollection<AccountListModel>> SearchAccountsAsync(string search, int? take, int? skip)
        {
            var responce = await GetAsync("admin/list?search=" + search + (take == null ? null : ("&take=" + take)) + (skip == null ? null : ("&skip=" + skip)));

            return await responce.Content.ReadAsAsync<AccountListModel[]>();
        }

        public async Task<GetCountModel> GetCountAsync()
        {
            var responce = await GetAsync("admin/count");

            return await responce.Content.ReadAsAsync<GetCountModel>();

        }


        public async Task<AccountDetailModel> GetAccountAsync(string account_id)
        {
            var responce = await GetAsync("admin/by_id/" + account_id);

            return await responce.Content.ReadAsAsync<AccountDetailModel>();
        }

        public async Task<AccountDetailModel> GetAccountByNameAsync(string name)
        {
            var responce = await GetAsync("admin/by_name/" + name);

            return await responce.Content.ReadAsAsync<AccountDetailModel>();

        }

        public async Task<AccountDetailModel> CreateAccountAsync(CreateAccountModel model)
        {
            var responce = await SendAsync(HttpMethod.Post, "admin", model);


            return await responce.Content.ReadAsAsync<AccountDetailModel>();

        }

        public async Task AddAliasAsync(string account_id, AccountAlias model)
        {
            var responce = await SendAsync(HttpMethod.Post, "admin/by_id/" + account_id + "/alias", model);

        }



        public async Task RemoveAliasAsync(string account_id, AccountAlias model)
        {

            var responce = await SendAsync(HttpMethod.Delete, "admin/by_id/" + account_id + "/alias", model);

        }

        public async Task SetAccountStatusAsync(string account_id, SetAccountStatusModel model)
        {
            var responce = await SendAsync(HttpMethod.Post, "admin/by_id/" + account_id + "/status", model);


        }

        public async Task SetPasswordAsync(string account_id, SetPasswordModel model)
        {
            var responce = await SendAsync(HttpMethod.Post, "admin/by_id/" + account_id + "/password", model);

        }

        public async Task SetForwardingListAsync(string account_id, SetForwardingListModel model)
        {
            var responce = await SendAsync(HttpMethod.Post, "admin/by_id/" + account_id + "/forwarding", model);


        }

        public async Task SetNameAsync(string account_id, SetNameModel model)
        {
            var responce = await SendAsync(HttpMethod.Post, "admin/by_id/" + account_id + "/name", model);
        }

        public async Task SetQuotaAsync(string account_id, SetQuotaModel model)
        {
            var responce = await SendAsync(HttpMethod.Post, "admin/by_id/" + account_id + "/quota", model);

        }
    }
}
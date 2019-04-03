using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Base.Extensions;
using Base.PBX.Entities;
using Base.PBX.Models;
using Base.PBX.Services.Abstract;
using Base.PBX.Web.Helpers;
using Base.Utils.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Base.PBX.Web.Services.Concrete
{
    public class WebPBXUserService : IPBXUserService
    {
        private readonly IAutoMapperCloner _autoMapperCloner;
        private readonly ConcurrentDictionary<string, HttpClient> _connections = new ConcurrentDictionary<string, HttpClient>();

        public WebPBXUserService(IAutoMapperCloner autoMapperCloner)
        {
            _autoMapperCloner = autoMapperCloner;
        }

        #region Private methods

        private static string CalculateMD5Hash(string input, bool upperCase = false)
        {

            var md5 = MD5.Create();

            var bytes = Encoding.ASCII.GetBytes(input);

            var hash = md5.ComputeHash(bytes);

            var sb = new StringBuilder();

            hash.ForEach(t =>
            {
                sb.Append(t.ToString(upperCase ? "X2" : "x2"));
            });

            return sb.ToString();
        }

        private static void ValidateResult(JToken jToken)
        {
            var resultCode = jToken["status"].Value<int>();

            if (resultCode >= 0) return;

            switch (resultCode)
            {
                case -6:
                    throw new Exception("Ошибка авторизации. Попробуйте выполнить операцию еще раз, либо обратитесь к администратору.");
                case -45:
                    throw new Exception("Выполнение операций слишком частое или другие пользователи делают ту же операцию. Пожалуйста, повторите попытку через 15 секунд.");
                default:
                    throw new Exception("Invalid response status");
            }
        }

        private async Task<HttpClient> GetAuthorizedHttpClientAsync(IPBXServer server)
        {
            HttpClient httpClient;

            if (_connections.TryGetValue(server.Url, out httpClient))
            {
                return httpClient;
            }

            httpClient = await CreateHttpClientAsync(server);

            _connections.TryAdd(server.Url, httpClient);

            return httpClient;
        }

        private static async Task<HttpClient> CreateHttpClientAsync(IPBXServer server)
        {
            var cookieContainer = new CookieContainer(1000, 1000, 20000);

            var httpClientHandler = new HttpClientHandler()
            {
                CookieContainer = cookieContainer
            };

            var httpClient = new HttpClient(httpClientHandler);

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36");

            var uri = new Uri(new Uri(server.Url), "/cgi");

            var checkResponse = await httpClient.PostAsync(uri, new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"action", "challenge"},
                {"user", server.User},
            }));

            var checkResult = await checkResponse.Content.ReadAsAsync<JToken>();

            ValidateResult(checkResult);

            var key = checkResult["response"]["challenge"];

            var token = CalculateMD5Hash(key + server.Password);

            var tokenResponse = await httpClient.PostAsync(uri, new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"action", "login"},
                {"user", server.User},
                {"token", token}
            }));

            var tokenResult = await tokenResponse.Content.ReadAsAsync<JToken>();

            ValidateResult(tokenResult);

            var privilege = tokenResult["response"]["user"]["html_privilege"].ToString(Formatting.Indented);

            cookieContainer.Add(uri, new Cookie("username", server.User));
            cookieContainer.Add(uri, new Cookie("user_id", tokenResult["response"]["user"]["user_id"].Value<string>()));
            cookieContainer.Add(uri, new Cookie("html", HttpUtility.UrlEncode(privilege)));
            cookieContainer.Add(uri, new Cookie("role", tokenResult["response"]["user"]["user_role"].Value<string>()));
            cookieContainer.Add(uri, new Cookie("is_strong_password", tokenResult["response"]["user"]["is_strong_password"].Value<string>()));
            cookieContainer.Add(uri, new Cookie("first_login", "no"));

            return httpClient;
        }

        private async Task<PBXUser> GetPBXUserAsync(IPBXServer server, string extension)
        {
            var httpClient = await GetAuthorizedHttpClientAsync(server);

            var accountResponse = await httpClient.PostAsync(new Uri(new Uri(server.Url), "/cgi"), new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"action", "getSIPAccount"},
                {"extension", extension}
            }));

            var accountResult = await accountResponse.Content.ReadAsAsync<JToken>();

            ValidateResult(accountResult);

            var account = accountResult["response"]["extension"].ToObject<PBXUser>(new JsonSerializer()
            {
                Converters = { new PBXBoolConverter() }
            });

            var userResponse = await httpClient.PostAsync(new Uri(new Uri(server.Url), "/cgi"), new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"action", "getUser"},
                {"user_name", extension},
                {"_location", "extension" }
            }));

            var userResult = await userResponse.Content.ReadAsAsync<JToken>();

            ValidateResult(userResult);

            var user = userResult["response"]["user_name"].ToObject<PBXUser>(new JsonSerializer()
            {
                Converters = { new PBXBoolConverter() }
            });

            return (PBXUser)_autoMapperCloner.Copy(user, account);
        }

        private async Task<int[]> GetCurrentNumbersAsync(IPBXServer server)
        {
            var httpClient = await GetAuthorizedHttpClientAsync(server);

            var numberListResponse = await httpClient.PostAsync(new Uri(new Uri(server.Url), "/cgi"), new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"action", "getNumberList"},
                {"_location", "extension"}
            }));

            var numberListResult = await numberListResponse.Content.ReadAsAsync<JToken>();

            ValidateResult(numberListResult);

            return numberListResult["response"]["number"].ToObject<List<string>>().Where(x => !x.StartsWith("*")).Select(int.Parse).OrderBy(x => x).ToArray();
        }

        //TODO: Need test
        private async Task<string> GetAvailableNumberAsync(IPBXServer server, int minNumber)
        {
            var number = minNumber > 0 ? minNumber : 0;

            try
            {
                var numbers = (await GetCurrentNumbersAsync(server)).Where(x => x >= minNumber).ToArray();

                if (numbers.Any()) //&& numbers.FirstOrDefault() == minNumber
                {
                    number = numbers.FirstOrDefault();

                    foreach (var num in numbers.Skip(1))
                    {
                        if (num - number > 1)
                            break;

                        number = num;
                    }

                    number++;
                }
            }
            catch (Exception)
            {
                //throw;
            }

            return number.ToString();
        }

        #endregion


        #region IPBXUserService

        public PBXUser CreateUser(IPBXServer server, PBXUser user)
        {
            return AsyncHelpers.RunSync(() => CreateUserAsync(server, user));
        }

        public async Task<PBXUser> CreateUserAsync(IPBXServer server, PBXUser user)
        {
            var httpClient = await GetAuthorizedHttpClientAsync(server);

            #region Action:getExtenPrefSettings

            //var extenPrefSettingsResponse = await httpClient.PostAsync("/cgi", new FormUrlEncodedContent(new Dictionary<string, string>()
            //{
            //    {"action", "getExtenPrefSettings"},
            //    {"_location", "extension"}
            //}));

            //var extenPrefSettingsResult = await extenPrefSettingsResponse.Content.ReadAsAsync<JToken>();

            //if (!IsSuccessResult(extenPrefSettingsResult)) throw new Exception("Invalid response status");

            #endregion

            #region Action:getLanguage
            //getLanguage
            #endregion

            #region Action:getNumberList
            //var numberListResponse = await httpClient.PostAsync("/cgi", new FormUrlEncodedContent(new Dictionary<string, string>()
            //{
            //    {"action", "getNumberList"},
            //    {"_location", "extension"}
            //}));

            //var numberListResult = await numberListResponse.Content.ReadAsAsync<JToken>();

            //if (!IsSuccessResult(numberListResult)) throw new Exception("Invalid response status");

            //var number =
            //    (numberListResult["response"]["number"].ToObject<List<string>>().Where(x => !x.StartsWith("*")).Select(int.Parse).Max() + 1).ToString();

            string number;

            if (string.IsNullOrEmpty(user.extension))
            {
                number = await GetAvailableNumberAsync(server, server.MinNumber);
            }
            else
            {
                number = user.extension;
            }
            #endregion

            #region Action:getAccountList
            //getAccountList
            #endregion

            #region Action:getGeneralPrefSettings
            //var generalPrefSettingsResponse = await httpClient.GetAsync("/cgi?action=getGeneralPrefSettings&_location=extension");

            //var generalPrefSettingsResult = await generalPrefSettingsResponse.Content.ReadAsAsync<JToken>();

            //if (!IsSuccessResult(generalPrefSettingsResult)) throw new Exception("Invalid response status");
            #endregion

            #region Action:addSIPAccountAndUser

            var addAccountResponse = await httpClient.PostAsync(new Uri(new Uri(server.Url), "/cgi"), new CustomFormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"action", "addSIPAccountAndUser"},
                {"first_name", user.first_name },
                {"last_name", user.last_name },
                {"email", user.email },
                {"language", "ru" },
                {"user_password", user.user_password },
                {"fullname", user.fullname },

                {"extension", number},
                {"cidnumber", user.cidnumber},

                {"permission", "internal-local-national-international" },
                {"secret", user.secret },
                {"authid", "" },
                {"hasvoicemail", user.hasvoicemail ? "yes" : "no" },
                {"vmsecret", user.vmsecret },
                {"skip_vmsecret", user.skip_vmsecret ? "yes": "no" },
                {"out_of_service", user.out_of_service ? "yes" : "no" },
                {"max_contacts",  "1"},
                {"nat", "yes" },
                {"directmedia", "no" },
                {"dtmfmode", "rfc2833" },
                {"tel_uri", "disabled" },
                {"enable_qualify", "no" },
                {"alertinfo", "" },
                {"t38_udptl", "no" },
                {"encryption", "no" },
                {"strategy_ipacl", "0" },
                {"allow", "ulaw,alaw,gsm,g726,g722,g729,h264,ilbc,vp8" },
                {"cfu", "" },
                {"cfu_timetype", "0" },
                {"cfn", "" },
                {"cfn_timetype", "0" },
                {"cfb", "" },
                {"cfb_timetype", "0" },
                {"dnd", "no" },
                {"dnd_timetype", "0" },
                {"en_ringboth", "no" },
                {"ring_timeout", "" },
                {"auto_record", user.auto_record ? "yes": "no" },
                {"bypass_outrt_auth", "no" },
                {"user_outrt_passwd", "" },
                {"enablehotdesk", "no" },
                {"enable_ldap", "yes" },
                {"enable_webrtc", user.enable_webrtc ? "yes" : "no" },
                {"mohsuggest", "default" },
                {"faxdetect", "no" },
                {"local_network1", "" },
                {"local_network2", "" },
                {"local_network3", "" },
                {"local_network4", "" },
                {"local_network5", "" },
                {"local_network6", "" },
                {"local_network7", "" },
                {"local_network8", "" },
                {"local_network9", "" },
                {"local_network10", "" },
                {"cc_agent_policy", "never" },
                {"cc_monitor_policy", "never" },
                {"media_encryption", "no" },
                {"account_type", user.account_type },
                //{"time_condition", "[]" }, //TODO
                {"limitime", "" },
                {"_location", "extension" }
            }));

            var addAccountResult = await addAccountResponse.Content.ReadAsAsync<JToken>();

            ValidateResult(addAccountResult);

            #endregion

            return await GetPBXUserAsync(server, number);
        }

        public void DeleteUser(IPBXServer server, string extension)
        {
            AsyncHelpers.RunSync(() => DeleteUserAsync(server, extension));
        }

        public async Task DeleteUserAsync(IPBXServer server, string extension)
        {
            var httpClient = await GetAuthorizedHttpClientAsync(server);

            var deleteResponse = await httpClient.PostAsync(new Uri(new Uri(server.Url), "/cgi"), new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"action", "deleteUser"},
                {"user_name", extension},
                {"_location", "extension"}
            }));

            var deleteResult = await deleteResponse.Content.ReadAsAsync<JToken>();

            ValidateResult(deleteResult);
        }

        public PBXUser UpdateUser(IPBXServer server, PBXUser user)
        {
            return AsyncHelpers.RunSync(() => UpdateUserAsync(server, user));
        }

        public async Task<PBXUser> UpdateUserAsync(IPBXServer server, PBXUser user)
        {
            var httpClient = await GetAuthorizedHttpClientAsync(server);

            var accountResponse = await httpClient.PostAsync(new Uri(new Uri(server.Url), "/cgi"), new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"action", "getSIPAccount"},
                {"extension", user.extension}
            }));

            var accountResult = await accountResponse.Content.ReadAsAsync<JToken>();

            ValidateResult(accountResult);

            var userResponse = await httpClient.PostAsync(new Uri(new Uri(server.Url), "/cgi"), new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"action", "getUser"},
                {"user_name", user.extension},
                {"_location", "extension" }
            }));

            var userResult = await userResponse.Content.ReadAsAsync<JToken>();

            ValidateResult(userResult);

            var updateAccountResponse = await httpClient.PostAsync(new Uri(new Uri(server.Url), "/cgi"), new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"action", "updateSIPAccount"},
                {"extension", user.extension},
                {"cidnumber", user.cidnumber },
                {"permission", accountResult["response"]["extension"]["permission"]?.ToString() },
                {"secret", user.secret },
                {"authid", accountResult["response"]["extension"]["authid"]?.ToString() },
                {"hasvoicemail", user.hasvoicemail ? "yes" : "no" },
                {"vmsecret", user.vmsecret },
                {"skip_vmsecret", user.skip_vmsecret ? "yes": "no" },
                {"out_of_service", user.out_of_service ? "yes" : "no" },
                {"max_contacts", accountResult["response"]["extension"]["max_contacts"]?.ToString() },
                {"nat", accountResult["response"]["extension"]["nat"]?.ToString() },
                {"directmedia", accountResult["response"]["extension"]["directmedia"]?.ToString() },
                {"dtmfmode", accountResult["response"]["extension"]["dtmfmode"]?.ToString() },
                {"tel_uri", accountResult["response"]["extension"]["tel_uri"]?.ToString() },
                {"enable_qualify", accountResult["response"]["extension"]["enable_qualify"]?.ToString() },
                {"alertinfo", accountResult["response"]["extension"]["alertinfo"]?.ToString() },
                {"t38_udptl", accountResult["response"]["extension"]["t38_udptl"]?.ToString() },
                {"encryption", accountResult["response"]["extension"]["encryption"]?.ToString() },
                {"strategy_ipacl", accountResult["response"]["extension"]["strategy_ipacl"]?.ToString() },
                {"allow", accountResult["response"]["extension"]["allow"]?.ToString() },
                {"cfu", accountResult["response"]["extension"]["cfu"]?.ToString() },
                {"cfu_timetype", accountResult["response"]["extension"]["cfu_timetype"]?.ToString() },
                {"cfn", accountResult["response"]["extension"]["cfn"]?.ToString() },
                {"cfn_timetype", accountResult["response"]["extension"]["cfn_timetype"]?.ToString() },
                {"cfb", accountResult["response"]["extension"]["cfb"]?.ToString() },
                {"cfb_timetype", accountResult["response"]["extension"]["cfb_timetype"]?.ToString() },
                {"dnd", accountResult["response"]["extension"]["dnd"]?.ToString() },
                {"dnd_timetype", accountResult["response"]["extension"]["dnd_timetype"]?.ToString() },
                {"en_ringboth", accountResult["response"]["extension"]["en_ringboth"]?.ToString() },
                {"ring_timeout", accountResult["response"]["extension"]["ring_timeout"]?.ToString() },
                {"auto_record", user.auto_record ? "yes": "no" },
                {"bypass_outrt_auth", accountResult["response"]["extension"]["bypass_outrt_auth"]?.ToString() },
                {"user_outrt_passwd", accountResult["response"]["extension"]["user_outrt_passwd"]?.ToString() },
                {"enablehotdesk", accountResult["response"]["extension"]["enablehotdesk"]?.ToString() },
                {"enable_ldap", accountResult["response"]["extension"]["enable_ldap"]?.ToString() },
                {"enable_webrtc", user.enable_webrtc ? "yes" : "no" },
                {"mohsuggest", accountResult["response"]["extension"]["mohsuggest"]?.ToString() },
                {"faxdetect", accountResult["response"]["extension"]["faxdetect"]?.ToString() },
                {"local_network1", accountResult["response"]["extension"]["local_network1"]?.ToString() },
                {"local_network2", accountResult["response"]["extension"]["local_network2"]?.ToString() },
                {"local_network3", accountResult["response"]["extension"]["local_network3"]?.ToString() },
                {"local_network4", accountResult["response"]["extension"]["local_network4"]?.ToString() },
                {"local_network5", accountResult["response"]["extension"]["local_network5"]?.ToString() },
                {"local_network6", accountResult["response"]["extension"]["local_network6"]?.ToString() },
                {"local_network7", accountResult["response"]["extension"]["local_network7"]?.ToString() },
                {"local_network8", accountResult["response"]["extension"]["local_network8"]?.ToString() },
                {"local_network9", accountResult["response"]["extension"]["local_network9"]?.ToString() },
                {"local_network10", accountResult["response"]["extension"]["local_network10"]?.ToString() },
                {"cc_agent_policy", accountResult["response"]["extension"]["cc_agent_policy"]?.ToString() },
                {"cc_monitor_policy", accountResult["response"]["extension"]["cc_monitor_policy"]?.ToString() },
                {"media_encryption", accountResult["response"]["extension"]["media_encryption"]?.ToString() },
                {"account_type", user.account_type },
                {"time_condition", accountResult["response"]["extension"]["time_condition"]?.ToString() },
                {"limitime", accountResult["response"]["extension"]["limitime"]?.ToString() },
                {"_location", "extension" }
            }));

            var updateAccountResult = await updateAccountResponse.Content.ReadAsAsync<JToken>();

            ValidateResult(updateAccountResult);

            var updateUserResponse = await httpClient.PostAsync(new Uri(new Uri(server.Url), "/cgi"), new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"action", "updateUser"},
                {"user_id", user.user_id.ToString()},
                {"first_name", user.first_name},
                {"last_name", user.last_name},
                {"email", user.email},
                {"phone_number", user.phone_number},
                {"user_password", user.user_password},
                {"language", userResult["response"]["user_name"]["language"]?.ToString()},
                {"email_to_user", user.email_to_user ? "yes" : "no"},
                {"_location", "extension"}
            }));

            var updateUserResult = await updateUserResponse.Content.ReadAsAsync<JToken>();

            ValidateResult(updateUserResult);

            return await GetPBXUserAsync(server, user.extension);
        }

        public PBXUser GetUser(IPBXServer server, string extension)
        {
            return AsyncHelpers.RunSync(() => GetUserAsync(server, extension));
        }

        public async Task<PBXUser> GetUserAsync(IPBXServer server, string extension)
        {
            return await GetPBXUserAsync(server, extension);
        }

        public IPageResult GetPagedUsers(IPBXServer server, int page, int limit)
        {
            return AsyncHelpers.RunSync(() => GetPagedUsersAsync(server, page, limit));
        }

        public async Task<IPageResult> GetPagedUsersAsync(IPBXServer server, int page, int limit)
        {
            var users = new PBXUsersPageResult();

            var httpClient = await GetAuthorizedHttpClientAsync(server);

            var usersResponse = await httpClient.PostAsync(new Uri(new Uri(server.Url), "/cgi"), new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"action", "listAccount"},
                {"auto-refresh", ""},
                {"options", "extension,status,addr,account_type,fullname,out_of_service,email_to_user"},
                {"item_num", limit.ToString()},
                {"page", page.ToString()},
                {"sidx", "extension"},
                {"sord", "asc"},
                {"_location", "extension"}
            }));


            var usersResult = await usersResponse.Content.ReadAsAsync<JToken>();

            ValidateResult(usersResult);

            users.Result = usersResult["response"]["account"].ToObject<List<PBXAccount>>(new JsonSerializer()
            {
                Converters = { new PBXBoolConverter() }
            });

            users.Total = usersResult["response"]["total_item"].Value<int>();
            //users.TotalPages = usersResult["response"]["total_page"].Value<int>();

            return users;
        }

        public void ApplyChanges(IPBXServer server)
        {
            AsyncHelpers.RunSync(() => ApplyChangesAsync(server));
        }

        public async Task ApplyChangesAsync(IPBXServer server)
        {
            var httpClient = await GetAuthorizedHttpClientAsync(server);

            var applyResponse = await httpClient.GetAsync(new Uri(new Uri(server.Url), "/cgi?action=applyChanges&settings="));

            var applyResult = await applyResponse.Content.ReadAsAsync<JToken>();

            ValidateResult(applyResult);
        }

        public void Reboot(IPBXServer server)
        {
            AsyncHelpers.RunSync(() => RebootAsync(server));
        }

        public async Task RebootAsync(IPBXServer server)
        {
            var httpClient = await GetAuthorizedHttpClientAsync(server);

            var rebootResponse = await httpClient.GetAsync(new Uri(new Uri(server.Url), "/cgi?action=rebootSystem"));

            var rebootResult = await rebootResponse.Content.ReadAsAsync<JToken>();

            ValidateResult(rebootResult);
        }

        public PBXServerStatus GetServerStatus(IPBXServer server)
        {
            return AsyncHelpers.RunSync(() => GetServerStatusAsync(server));
        }

        public async Task<PBXServerStatus> GetServerStatusAsync(IPBXServer server)
        {
            var httpClient = await GetAuthorizedHttpClientAsync(server);

            var statusResponse = await httpClient.PostAsync(new Uri(new Uri(server.Url), "/cgi"), new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"action", "checkInfo"},
                {"user", server.User}
            }));

            var statusResult = await statusResponse.Content.ReadAsAsync<JToken>();

            ValidateResult(statusResult);

            return statusResult["response"].ToObject<PBXServerStatus>(new JsonSerializer()
            {
                Converters = { new PBXBoolConverter() }
            });
        }

        public string GetAvailableNumber(IPBXServer server)
        {
            return AsyncHelpers.RunSync(() => GetAvailableNumberAsync(server));
        }

        public async Task<string> GetAvailableNumberAsync(IPBXServer server)
        {
            return await GetAvailableNumberAsync(server, server.MinNumber);
        }

        public bool IsNumberExist(IPBXServer server, int number)
        {
            return AsyncHelpers.RunSync(() => IsNumberExistAsync(server, number));
        }

        public async Task<bool> IsNumberExistAsync(IPBXServer server, int number)
        {
            return (await GetCurrentNumbersAsync(server)).Contains(number);
        }

        #endregion

    }
}
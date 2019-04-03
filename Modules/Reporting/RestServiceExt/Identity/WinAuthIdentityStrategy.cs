using System;
using System.Collections.Concurrent;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using DAL.EF;
using NLog;

namespace RestService.Identity
{
    public class WinAuthIdentityStrategy : IAuthUserInfoStrategy
    {

        private static ILogger _log = LogManager.GetCurrentClassLogger();
        private static ConcurrentDictionary<string,UserInfo> _cashe=new ConcurrentDictionary<string, UserInfo>();


        public UserInfo GetUserInfo(ClaimsPrincipal principal)
        {
            try
            {
                string rawLogin = principal?.Identity?.Name;
                string login = CurrentLogin(rawLogin);
                UserInfo user;
                if (!_cashe.TryGetValue(login, out user))
                {
                    user = GetUserInfoByLogin(login);
                    _cashe.TryAdd(login, user);
                }
                return user;
            }
            catch (Exception ex)
            {
                var allinone = String.Join(", \n",
                    principal.FindAll(_ => true).Select(_ => $"{_.Subject}, value= {_.Value}"));
                _log.Debug(allinone);
                throw ex;
            }
        }

        public static void ClearCash()
        {
            _cashe.Clear();
        }
        private UserInfo GetUserInfoByLogin(string login)
        {
            using (var context = new ReportDbContext())
            {
                DbRawSqlQuery<DAL.Entities.UserInfo> rawSqlQuery = context.pGetUserInfo(login);
                return rawSqlQuery.Select(info =>
                {
                    return new UserInfo()
                        {CategoryIds = info.CategoryIds??"", IsAdmin = info.IsAdmin, UserId = info.UserId};
                }).First<UserInfo>();
            }
        }

        private string CurrentLogin(string fullName)
        {
            var indexOfBackslash = fullName.LastIndexOf("\\");
            if (indexOfBackslash > 0)
            {
                return fullName.Substring(indexOfBackslash + 1);
            }
            return fullName;
        }
    }
}
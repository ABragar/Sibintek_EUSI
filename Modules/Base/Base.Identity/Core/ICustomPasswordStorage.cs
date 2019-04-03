using System;
using System.Threading.Tasks;

namespace Base.Identity.Core
{
    public interface ICustomPasswordStorage
    {

        Task<Tuple<SetPasswordResult, string>> SetPasswordAsync(string login, string password);

        Task<VerifyPasswordResult> VerifyPasswordAsync(string login, string hash, string password);

        Task<HasPasswordResult> HasPasswordAsync(string login, string hash);
    }

    public enum HasPasswordResult
    {
        UseDefault,
        True,
        False
    }


    public enum SetPasswordResult
    {
        UseDefault,
        UseHash,
        Success,
        Failed
    }


    public enum VerifyPasswordResult
    {
        UseDefault,
        NeedReset,
        Success,
        Failed
    }

}
using Base.Enums;
using Base.Events.Registration;
using Base.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Identity
{
    public class RegisterResult : IRegisterResult
    {
        #region Fabric Methods
        internal static RegisterResult AccountRegistered(AccountEntry account, IEnumerable<string> messages = null)
        {
            return new RegisterResult(RegisterStatus.AccountRegistered, account.UserName, account.UserId, messages);
        }

        internal static RegisterResult Failure(AccountEntry account, IEnumerable<string> errors = null)
        {
            return new RegisterResult(RegisterStatus.Failure, account.UserName, account.UserId, errors);
        }

        internal static RegisterResult Failure(string login, IEnumerable<string> errors = null)
        {
            return new RegisterResult(RegisterStatus.Failure, login, null, errors);
        }
        #endregion


        public RegisterResult(RegisterStatus status, string login, int? userId, IEnumerable<string> messages = null)
        {
            Status = status;
            Login = login ?? string.Empty;
            UserId = userId.HasValue && userId.Value > 0 ? userId.Value : new int?();

            if ( messages != null && messages.Any())
            {
                Messages = new ReadOnlyCollection<String>(messages.ToList());
            }
        }

        public string Login { get; }
        public int? UserId { get; }
        public RegisterStatus Status { get; }
        public IReadOnlyCollection<string> Messages { get; }

        #region Helpers
        public bool IsSucceeded
        {
            get
            {
                return Status == RegisterStatus.AccountRegistered;
            }
        }

        public bool IsFailure
        {
            get
            {
                return Status == RegisterStatus.Failure;
            }
        }
        #endregion
    }
}

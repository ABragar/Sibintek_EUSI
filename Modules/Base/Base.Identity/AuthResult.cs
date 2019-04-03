using System;
using System.Collections.Generic;
using Base.Enums;
using Base.Events.Auth;
using Base.Identity;

namespace Base.Entities
{
    public class AuthResult : IAuthResult
    {
        #region Fabric Methods
        public static AuthResult Failed(params string[] messages)
        {
            return new AuthResult(null, null, AuthStatus.Failure, messages);
        }

        public static AuthResult Failed(int? userId, string login, params string[] messages)
        {
            return new AuthResult(userId, login, AuthStatus.Failure, messages);
        }

        public static AuthResult NotFound(int? userId, string login, params string[] messages)
        {
            return new AuthResult(userId, login, AuthStatus.NotFound, messages);
        }

        public static AuthResult NotFound(AccountInfo accountInfo = null, params string[] messages)
        {
            return new AuthResult(accountInfo?.UserId, accountInfo?.Login, AuthStatus.NotFound, messages);
        }
        #endregion

        private readonly int? _userId;
        private readonly string _login;

        public AuthResult(int? userId, string login, AuthStatus status, params string[] messages)
        {
            _userId = userId;
            _login = login;
            Status = status;
            Messages = messages;
        }

        public int? UserId => _userId;

        public int UserIdValue
        {
            get
            {
                if (_userId == null)
                    throw new InvalidOperationException();

                return _userId.Value;
            }
        }

        public string Login
        {
            get
            {
                if (_login == null)
                //throw new InvalidOperationException();
                {
                    return string.Empty;
                }

                return _login;
            }
        }

        public AuthStatus Status { get; }
        public IReadOnlyCollection<string> Messages { get; }

        public bool IsSuccess
        {
            get
            {
                return Status == AuthStatus.Success;
            }
        }

        public bool IsFailure
        {
            get
            {
                return Status == AuthStatus.Failure;
            }
        }

        public bool IsNotFound
        {
            get
            {
                return Status == AuthStatus.NotFound;
            }
        }
    }
}
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;
using Base.Identity.Core;
using Base.Settings;
using Newtonsoft.Json;

namespace Base.Identity
{
    [Serializable]
    public class AuthSettings : SettingItem, ITokenOptions, IAuthSettings
    {
        [DetailView("Разрешить сброс пароля через почту")]
        public bool ResetPasswordByTokenAllowed { get; set; } = false;

        [DetailView("Разрешить вход через внешнии сервисы")]
        public bool ExternalLoginAllowed { get; set; } = true;

        [DetailView("Разрешить регистрацию новых пользователей")]
        public bool RegistrationAllowed { get; set; } = true;

        [DetailView("Разрешить вход с не подтвержденной электронной почтой")]
        public bool NotConfirmedLoginAllowed { get; set; } = true;

        [DetailView("Разрешить подтверждение электронной почты")]
        public bool ConfirmAllowed { get; set; } = false;

        [DetailView()]
        public int MaxTokenCount { get; set; } = 3;

        
        
        public TimeSpan MaxTokenCountTimeSpan
        {
            get { return TimeSpan.FromHours(MaxTokenCountHours); }
        }

        [DetailView()]
        public double MaxTokenCountHours { get; set; } = TimeSpan.FromDays(3).Hours;

        
        
        public TimeSpan TokenTimeSpan
        {
            get { return TimeSpan.FromHours(TokenHours); }
        }

        [DetailView()]
        public double TokenHours { get; set; } = TimeSpan.FromDays(3).Hours;

    }
}
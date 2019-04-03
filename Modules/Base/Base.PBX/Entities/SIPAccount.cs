using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Base.Attributes;
using Base.Enums;
using Base.PBX.Models;
using Base.Security;
using Base.Translations;
using Base.Utils.Common.Attributes;

namespace Base.PBX.Entities
{
    [EnableFullTextSearch]
    public class SIPAccount : BaseObject, IPBXUser, IPBXAccount
    {
        private static readonly CompiledExpression<SIPAccount, string> _first_name =
            DefaultTranslationOf<SIPAccount>.Property(x => x.first_name).Is(x => x.User != null && x.User.Profile != null ? x.User.Profile.FirstName : "");

        private static readonly CompiledExpression<SIPAccount, string> _last_name =
            DefaultTranslationOf<SIPAccount>.Property(x => x.last_name).Is(x => x.User != null && x.User.Profile != null ? x.User.Profile.LastName : "");

        private static readonly CompiledExpression<SIPAccount, string> _fullname =
            DefaultTranslationOf<SIPAccount>.Property(x => x.fullname).Is(x => x.User != null && x.User.Profile != null ? x.User.Profile.FullName : "");

        private static readonly CompiledExpression<SIPAccount, string> _email =
            DefaultTranslationOf<SIPAccount>.Property(x => x.email).Is(x => x.User != null && x.User.Profile != null && x.User.Profile.Emails.FirstOrDefault(y => y.IsPrimary) != null ? x.User.Profile.Emails.FirstOrDefault(y => y.IsPrimary).Email : "");

        private static readonly CompiledExpression<SIPAccount, string> _phone_number =
            DefaultTranslationOf<SIPAccount>.Property(x => x.phone_number).Is(x => x.User != null && x.User.Profile != null && x.User.Profile.Phones.FirstOrDefault(y => y.Phone.Type == PhoneType.Mobile) != null ? x.User.Profile.Phones.FirstOrDefault().Phone.Code + x.User.Profile.Phones.FirstOrDefault().Phone.Number: "");
        
        private static readonly CompiledExpression<SIPAccount, string> _account_type =
            DefaultTranslationOf<SIPAccount>.Property(x => x.account_type).Is(x => x.enable_webrtc ? "SIP(WebRTC)" : "SIP");

        public int UserID { get; set; }

        [ForeignKey("UserID")]
        [DetailView("Пользователь", TabName = "[0]Основная информация", Required = true), ListView]
        public virtual User User { get; set; }

        public int ServerID { get; set; }

        [ForeignKey("ServerID")]
        [DetailView("Сервер телефонии", TabName = "[0]Основная информация", Required = true), ListView]
        public virtual PBXServer PBXServer { get; set; }

        #region IPBXUser

        [SystemProperty]
        public int user_id { get; set; }
        public string first_name => _first_name.Evaluate(this);
        public string last_name => _last_name.Evaluate(this);
        public string email => _email.Evaluate(this);
        public string phone_number => _phone_number.Evaluate(this);

        [DetailView("Пароль", TabName = "[1]SIP Настройки", Required = true, Order = 22)]
        [PropertyDataType(PropertyDataType.Password)]
        public string user_password { get; set; }

        [DetailView("Отправлять e-mail", TabName = "[1]SIP Настройки", Order = 23)]
        public bool email_to_user { get; set; }
        #endregion

        #region IPBXAccount

        [DetailView("Добавочный номер", TabName = "[1]SIP Настройки", Order = 20), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        public string extension { get; set; }

        [DetailView("Номер CallerID", TabName = "[1]SIP Настройки", Order = 21)]
        [PropertyDataType(PropertyDataType.Text)]
        public string cidnumber { get; set; }

        //SIP(WebRTC) if WebRTC is True, this is just readonly systemdescription
        [FullTextSearchProperty]
        [ListView("Тип аккаунта")]
        public string account_type => _account_type.Evaluate(this);

        [FullTextSearchProperty]
        public string fullname => _fullname.Evaluate(this);

        [DetailView("Пароль SIP/IAX", TabName = "[1]SIP Настройки", Required = true, Order = 24)]
        [PropertyDataType(PropertyDataType.Password)]
        public string secret { get; set; }

        [DetailView("Пароль голосовой почты (0-9)", TabName = "[1]SIP Настройки", Required = true, Order = 25)]
        [PropertyDataType(PropertyDataType.Password)]
        public string vmsecret { get; set; } //just numbers


        [DetailView("Пропустить проверку пароля голосовой почты", TabName = "[1]SIP Настройки", Order = 26)]
        public bool skip_vmsecret { get; set; }


        [DetailView("Использовать WebRTC", TabName = "[1]SIP Настройки", Order = 27)]
        public bool enable_webrtc { get; set; } = true;


        [DetailView("Голосовая почта", TabName = "[1]SIP Настройки", Order = 28)]
        public bool hasvoicemail { get; set; }


        [DetailView("Аудио запись", TabName = "[1]SIP Настройки", Order = 29)]
        public bool auto_record { get; set; } = true;

        [DetailView("Отключен", TabName = "[1]SIP Настройки", Order = 30)]
        public bool out_of_service { get; set; }
        #endregion

    }
}
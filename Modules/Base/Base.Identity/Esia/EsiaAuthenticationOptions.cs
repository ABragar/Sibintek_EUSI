using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Base.Identity.Esia
{
    public class EsiaAuthenticationOptions : AuthenticationOptions
    {
        public EsiaAuthenticationOptions() : base("ESIA")
        {
            Caption = "ЕСИА";
            this.AuthenticationMode = AuthenticationMode.Passive;
        }

        /// <summary>
        /// URL-ссылка на сервис единого входа ЕСИА
        /// </summary>
        /// <remarks>
        /// Значение по умолчанию - https://esia.gosuslugi.ru/idp/profile/SAML2/Redirect/SSO
        /// </remarks>
        public string IdpSSOTargetURL { get; set; }

        /// <summary>
        /// Идентификатор поставщика услуг, отправляюего запрос
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Путь к PFX-файлу сертификата безопасности
        /// </summary>
        public string PFXFileName { get; set; }

        public string PFXPassword { get; set; }


        public string Caption
        {
            get
            {
                return Description.Caption;
            }
            set
            {
                Description.Caption = value;
            } 
        } 
        public PathString CallbackPath { get; set; } = new PathString("/esia-callback");
    }
}
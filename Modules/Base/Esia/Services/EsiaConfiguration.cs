using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Esia.Services
{
    /// <summary>
    /// Конфигурация для работы с системой ЕСИА
    /// </summary>
    public class EsiaConfiguration
    {
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

        /// <summary>
        /// Путь к log-файлу в который будет записываться отладочная информация
        /// </summary>
        /// <remarks>
        /// Необязательный параметр
        /// </remarks>
        public string LogFileName { get; set; }

        private X509Certificate2 _certificate;

        /// <summary>
        /// Возвращает сертификат безопасности, если необходимо загружая его из файла.
        /// Путь к файлу сертификата задается в свойстве PFXFileName.
        /// </summary>
        /// <returns>Сертификат безопасности</returns>
        public X509Certificate2 GetCertificate()
        {
            if (File.Exists(PFXFileName) == false)
                throw new FileNotFoundException("Не найден файл сертификата безопасности", PFXFileName);

            if (_certificate == null)
                _certificate = EsiaUtils.LoadCertificate(PFXFileName, PFXPassword);

            return _certificate;
        }


        public string GetIdpSSOTargetURL()
            => IdpSSOTargetURL ?? "https://esia.gosuslugi.ru/idp/profile/SAML2/Redirect/SSO";
    }
}

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace Esia.Services
{
    /// <summary>
    /// Класс для формирования GET-запроса единому центру аутентификации ЕСИА.
    /// Для каждого отдельного запроса должен быть создат свой собственный экземпляр данного класса,
    /// т.к. сам класс содержит уникальный код запроса.
    /// </summary>
    public class EsiaRequest
    {
        /// <summary>
        /// Создает класс запроса на основе представленной конфигурации
        /// </summary>
        /// <param name="configuration">Конфигурация для обращения к ЕСИА</param>
        public EsiaRequest(EsiaConfiguration configuration)
        {

            if (String.IsNullOrWhiteSpace(configuration.Issuer))
                throw new ArgumentException("Пустой идентификатор провайдера", "configuration.Issuer");

            this.Configuration = configuration;


            this.Certificate = configuration.GetCertificate();
            this.Logger = new EsiaLogger(Configuration.LogFileName);
        }

        /// <summary>
        /// Конфигурация для обращения к ЕСИА
        /// </summary>
        public EsiaConfiguration Configuration { get; private set; }

        /// <summary>
        /// Сертификат безопасности, полученный на основе конфигурации
        /// </summary>
        public X509Certificate2 Certificate { get; private set; }

        /// <summary>
        /// Журнал, для записи отладочной информации
        /// </summary>
        private EsiaLogger Logger { get; set; }

        /// <summary>
        /// Создает URL для GET-запроса к сервису аутентификации ЕСИА с текущей временной меткой
        /// </summary>
        /// <returns>Строка, содержащая URL GET-запроса</returns>
        public string CreateLoginUrl(string callback_url, string relay_state)
        {
            var now = DateTime.Now.ToUniversalTime();

            var uuid = Guid.NewGuid().ToString().ToUpper();

            var xmltext = CreateSamlXml(now, uuid, callback_url, "");
            Logger.Log(xmltext, "Начальное значение");

            var digest = EsiaUtils.GenerateBase64Digest(xmltext);
            Logger.Log(digest, "Digest (хэш функция)");

            var signedInfo = CreateSignInfoXml(digest, uuid);
            Logger.Log(signedInfo, "SignedInfo XML Node");

            var signature = SignXml(signedInfo, Certificate);
            Logger.Log(signature, "Signature itself");

            var signature_xml = CreateSignatureXml(signedInfo, signature);
            Logger.Log(signature_xml, "Signature XML Node");

            var request_xml = CreateSamlXml(now, uuid, callback_url, signature_xml);
            Logger.Log(request_xml, "Full reqiest XML document");

            var request_param = CreateRequestParam(request_xml);
            var url = CreateEsiaLoginUrl(request_param, relay_state);
            Logger.Log(url, "Request URL");

            return url;
        }

        #region Generate request SAML XML

        /// <summary>
        /// Формирует текст узла AuthnRequest (основной узел запроса) SAML-документа
        /// </summary>
        /// <param name="timeStamp">Временная метка запроса</param>
        /// <param name="signature">Текст узла Signature цифровой подписи (необязательный параметр)</param>
        /// <returns>Текст узла AuthnRequest</returns>
        private string CreateSamlXml(DateTime timeStamp, string uuid, string callback_url, string signature)
        {
            const string SAML_TEMPLATE =
@"<saml2p:AuthnRequest xmlns:saml2p=""urn:oasis:names:tc:SAML:2.0:protocol"" AssertionConsumerServiceURL=""{0}"" Destination=""{1}"" ForceAuthn=""false"" ID=""{2}"" IsPassive=""false"" IssueInstant=""{3}"" ProtocolBinding=""urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST"" Version=""2.0"">
    <saml2:Issuer xmlns:saml2=""urn:oasis:names:tc:SAML:2.0:assertion"">{4}</saml2:Issuer>
{5}</saml2p:AuthnRequest>";

            var now = DateTime.Now;
            var xml_text = String.Format(SAML_TEMPLATE,
                callback_url,
                Configuration.GetIdpSSOTargetURL(),
                uuid,
                String.Format("{0}.{1:000}Z", timeStamp.ToString("s"), timeStamp.Millisecond),
                Configuration.Issuer,
                signature);

            return xml_text;
        }

        /// <summary>
        /// Формирует текст узла SignInfo цифровой подписи XML-документа
        /// </summary>
        /// <param name="digest64">Дайджест основного документа в формате Base64</param>
        /// <returns>Текст узла SignInfo</returns>
        private string CreateSignInfoXml(string digest64, string uuid)
        {
            const string SIGNEDINFO_TEMPLATE =
@"<ds:SignedInfo xmlns:ds=""http://www.w3.org/2000/09/xmldsig#"">
    <ds:CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#""></ds:CanonicalizationMethod>
    <ds:SignatureMethod Algorithm=""http://www.w3.org/2000/09/xmldsig#rsa-sha1""></ds:SignatureMethod>
    <ds:Reference URI=""#{0}"">
        <ds:Transforms>
            <ds:Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature""></ds:Transform>
            <ds:Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#""></ds:Transform>
        </ds:Transforms>
        <ds:DigestMethod Algorithm=""http://www.w3.org/2000/09/xmldsig#sha1""></ds:DigestMethod>
        <ds:DigestValue>{1}</ds:DigestValue>
    </ds:Reference>
</ds:SignedInfo>";

            var signedInfo = String.Format(SIGNEDINFO_TEMPLATE, uuid, digest64);

            return signedInfo;
        }

        /// <summary>
        /// Формирует текст узла Signature цифровой подписи SAML-документа
        /// </summary>
        /// <param name="signInfo">Текст узла SignInfo</param>
        /// <param name="signValue">Цифровая подпись в формате Base64</param>
        /// <returns></returns>
        private string CreateSignatureXml(string signInfo, string signValue)
        {
            const string SIGNATURE_TEMPLATE =
@"<ds:Signature xmlns:ds=""http://www.w3.org/2000/09/xmldsig#"">
    {0}
    <ds:SignatureValue>{1}</ds:SignatureValue>
</ds:Signature>";

            var sign_xml = String.Format(SIGNATURE_TEMPLATE, signInfo, signValue);
            return sign_xml;
        }

        #endregion

        #region Utils...

        /// <summary>
        /// Подписывает текст XML-документа закрытым ключом сертификата
        /// </summary>
        /// <param name="xml">Подписываемый текст XML-документа</param>
        /// <param name="cert">Сертификат безопасности</param>
        /// <returns>Цифровая подпись в формате Base64</returns>
        private string SignXml(string xml, X509Certificate2 cert)
        {
            var xmlu8 = EsiaUtils.ToUnixUtf8(xml.Trim());

            var rsa = (RSACryptoServiceProvider)cert.PrivateKey;
            var sign = rsa.SignData(xmlu8, SHA1.Create());

            var sign64 = EsiaUtils.ToBase64(sign);

            return sign64;
        }

        /// <summary>
        /// Формирует параметр SAMLRequest запроса к ЕСИА, сжимая текст SAML2-документа 
        /// и преобразуя сжатые данные в формат Base64
        /// </summary>
        /// <param name="requestXml">Текст SAML2-запроса</param>
        /// <returns>Значение параметра SAMLRequest в формате Base64</returns>
        private string CreateRequestParam(string requestXml)
        {
            var requestu8 = EsiaUtils.ToUnixUtf8(requestXml);
            var compressed64 = EsiaUtils.ToBase64(EsiaUtils.GZDeflate(requestu8));

            return compressed64;
        }

        /// <summary>
        /// Создает URL для GET-запроса к сервису аутентификации ЕСИА
        /// </summary>
        /// <param name="requestParam">Значение параметра SAMLRequest запроса</param>
        /// <returns>Строка, содержащая URL GET-запроса</returns>
        private string CreateEsiaLoginUrl(string requestParam, string relay_state)
        {
            var get_params = HttpUtility.ParseQueryString(string.Empty);
            get_params["SAMLRequest"] = requestParam;
            get_params["Signature"] = "-";
            get_params["SigAlg"] = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";

            if (!string.IsNullOrEmpty(relay_state))
                get_params["RelayState"] = relay_state;

            return string.Format("{0}?{1}", Configuration.GetIdpSSOTargetURL(), get_params);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using Esia.Model;

namespace Esia.Services
{
    /// <summary>
    /// Описывает дешифрованный ответ системы ЕСИА в случае успешной аутентификации
    /// </summary>
    public sealed class EsiaResponse
    {
        /// <summary>
        /// Создает объект на основе строки ответа сервиса ЕСИА (параметр SAMLResponse)
        /// <remarks>
        /// Недоступен для использования вне класса. Для создания объекта необходимо использовать метод <see cref="EsiaResponse.Create"/>
        /// </remarks>
        /// </summary>
        /// <param name="response">Значение ответа сервиса (параметр SAMLResponse) в формате Base64</param>
        private EsiaResponse(string response)
        {
            this._response = response;
        }

        /// <summary>
        /// Дешифрованная строка ответа сервиса ЕСИА
        /// </summary>
        private readonly string _response;

        /// <summary>
        /// Возвращает дешифрованный ответ в форме XML-документа
        /// </summary>
        /// <returns>XML-документ</returns>
        public XmlDocument GetAsXml()
        {
            return EsiaUtils.LoadXml(_response);
        }

        /// <summary>
        /// Возвращает дешифрованный ответ в форме тестовой строки
        /// </summary>
        /// <returns>Строка ответа сервиса ЕСИА</returns>
        public string GetAsString()
        {
            return _response;
        }

        /// <summary>
        /// Возвращает дешифрованный набор утверждений о пользователе в форме словаря "Ключ=Значение"
        /// </summary>
        /// <returns>Словарь утверждений</returns>
        public Dictionary<string, string> GetAsDictionary()
        {
            var xml = GetAsXml();
            var name_table = new NameTable();
            XmlNamespaceManager saml2mngr = new XmlNamespaceManager(name_table);
            saml2mngr.AddNamespace("saml2", "urn:oasis:names:tc:SAML:2.0:assertion");

            var attributes =
                from attr in xml.SelectNodes("//saml2:Attribute", saml2mngr).Cast<XmlElement>()
                let value = attr.SelectSingleNode("saml2:AttributeValue", saml2mngr).InnerText
                select new { Key = attr.GetAttribute("FriendlyName"), Value = value };

            return attributes.ToDictionary(x => x.Key, x => x.Value);
        }

        #region Create

        /// <summary>
        /// Создает объект на основе строки ответа сервиса ЕСИА (параметр SAMLResponse) и
        /// заданной конфигурации обращения
        /// </summary>
        /// <param name="configuration">Конфигурация обращения к сервису ЕСИА</param>
        /// <param name="response64">Значение ответа сервиса (параметр SAMLResponse) в формате Base64</param>
        /// <returns>Сформированный объект ответа</returns>
        public static EsiaResponse Create(EsiaConfiguration configuration, string response64)
        {
            var samlResponse = EsiaUtils.StringFromBase64(response64);
            var rsa = configuration.GetCertificate().PrivateKey as RSACryptoServiceProvider;
            var responseDocument = EsiaUtils.LoadXml(samlResponse);

            //найдем в документе узлы ключа дешифрации и зашифрованных данных
            var cipherValues = responseDocument.GetElementsByTagName("CipherValue", "http://www.w3.org/2001/04/xmlenc#");
            if (cipherValues.Count < 2)
                throw new ArgumentException("Неверный формат ответа сервиса", "response64");

            var passwordNode = cipherValues[0] as XmlElement;
            var encXmlNode = cipherValues[1] as XmlElement;

            var encPassword = EsiaUtils.FromBase64(passwordNode.InnerText);
            var encVectorXml = EsiaUtils.FromBase64(encXmlNode.InnerText);

            //дешифруем ключ симметричного алгоритма с использованием закрытого ключа сертификата
            var aesKey = rsa.Decrypt(encPassword, true);

            //разделим данные ответа на инициализирующий вектор дешифрации и собственно защиврованные данные
            var initialVector = encVectorXml.Take(16).ToArray();
            var encXml = encVectorXml.Skip(16).ToArray();

            //расшифруем основное тело XML-документа с утверждениями о пользователе
            var responseXml = EsiaUtils.AesDecryptText(encXml, aesKey, initialVector);

            return new EsiaResponse(responseXml);
        }

        #endregion
    }
}

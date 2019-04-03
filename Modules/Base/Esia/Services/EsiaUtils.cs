using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace Esia.Services
{
    /// <summary>
    /// Класс вспомогательных функций для реализации взаимодействия с ЕСИА
    /// </summary>
    public static class EsiaUtils
    {
        /// <summary>
        /// Кодировка по умолчанию, используемая при работе с SAML-документами
        /// </summary>
        public static Encoding DefaultEncoding = Encoding.UTF8;

        /// <summary>
        /// Загружает сертификат и закрытый ключ из указанного PFX-файла.
        /// </summary>
        /// <param name="fileName">Имя PFX-файла</param>
        /// <returns>Загруженный сертификат</returns>
        public static X509Certificate2 LoadCertificate(string fileName,string password)
        {
            var cert = new X509Certificate2(fileName, password);

            return cert;
        }

        /// <summary>
        /// Создает Aes-дешифратор данных для ЕСИА по заданным ключу и начальному вектору
        /// </summary>
        /// <param name="key">Ключ дешифрации</param>
        /// <param name="iv">Начальный вектор (для режима CBC)</param>
        /// <returns>Дешифратор</returns>
        public static ICryptoTransform CreateEsiaAesDecryptor(byte[] key, byte[] iv)
        {
            var aes = new AesCryptoServiceProvider();
            aes.KeySize = 128;
            aes.Padding = PaddingMode.ISO10126;
            aes.Mode = CipherMode.CBC;

            return aes.CreateDecryptor(key, iv);
        }

        /// <summary>
        /// Расшифровывает блок данных, полученных от ЕСИА
        /// </summary>
        /// <param name="encryptedText">Зашифрованные данные</param>
        /// <param name="key">Ключ дешифрации</param>
        /// <param name="initialVector">Начальный вектор (для режима CBC)</param>
        /// <param name="encoding">Кодировка дешифрованного текста</param>
        /// <returns>Дешифрованные текстовые данные</returns>
        public static string AesDecryptText(byte[] encryptedText, byte[] key, byte[] initialVector, Encoding encoding = null)
        {
            encoding = encoding ?? EsiaUtils.DefaultEncoding;

            var text = String.Empty;
            using (var decryptor = EsiaUtils.CreateEsiaAesDecryptor(key, initialVector))
            {
                text = encoding.GetString(decryptor.TransformFinalBlock(encryptedText, 0, encryptedText.Length));
            }

            return text;
        }

        /// <summary>
        /// Сжимает данные с использованием алгоритмом Deflate
        /// </summary>
        /// <param name="data">Исходные данные</param>
        /// <returns>Сжатые данные</returns>
        public static byte[] GZDeflate(byte[] data)
        {
            using (var resultStream = new MemoryStream())
            {
                using (var deflateStream = new DeflateStream(resultStream, CompressionMode.Compress, true))
                {
                    deflateStream.Write(data, 0, data.Length);
                }
                return resultStream.ToArray();
            }
        }

        /// <summary>
        /// Генерирует дайджест текстовых данных в кодировке UTF8 с использованием хэш-функции SHA1
        /// </summary>
        /// <param name="saml">Текстовые данные</param>
        /// <returns>Дайджест, записанные в кодировке Base64</returns>
        public static string GenerateBase64Digest(string saml)
        {
            var saml_utf8 = ToUnixUtf8(saml);
            var digest = SHA1.Create().ComputeHash(saml_utf8);
            var digest64 = ToBase64(digest);

            return digest64;
        }

        /// <summary>
        /// Преобразует двоичные данные в формат Base64
        /// </summary>
        /// <param name="data">Двоичные данные</param>
        /// <returns>Текстовое представление данные в формате Base64</returns>
        public static string ToBase64(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// Преобразует запись в формате Base64 в двочные данные
        /// </summary>
        /// <param name="base64">Текстовая запись в формате Base64</param>
        /// <returns>Двоичные данные</returns>
        public static byte[] FromBase64(string base64)
        {
            return Convert.FromBase64String(base64);
        }

        /// <summary>
        /// Преобразует двоичные данные в формате Base64 в текст согласно указанной кодировке
        /// </summary>
        /// <param name="base64">Данные в формате Base64</param>
        /// <param name="encoding">Кодировка</param>
        /// <returns>Декодированные текст</returns>
        public static string StringFromBase64(string base64, Encoding encoding = null)
        {
            encoding = encoding ?? EsiaUtils.DefaultEncoding;

            var bytes = FromBase64(base64);
            var text = encoding.GetString(bytes);

            return text;
        }

        /// <summary>
        /// Преобразует текст в кодировку UTF8 с учетом переносов в Unix-like стиле
        /// (без использования символа \xD)
        /// </summary>
        /// <param name="text">Преобразуемые текст</param>
        /// <returns>Данные в кодировке UTF8</returns>
        public static byte[] ToUnixUtf8(string text)
        {
            return Encoding.UTF8.GetBytes(text).Where(x => x != 13).ToArray();
        }

        /// <summary>
        /// Загружает XML-документ из текстовой строки
        /// </summary>
        /// <param name="xml_text">Текст XML-документа</param>
        /// <returns>XML-документ</returns>
        public static XmlDocument LoadXml(string xml_text)
        {
            var xml = new XmlDocument();
            xml.LoadXml(xml_text);

            return xml;
        }
    }
}

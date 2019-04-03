using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Base.PBX.Web.Helpers
{
    public class CustomFormUrlEncodedContent : ByteArrayContent
    {
        public CustomFormUrlEncodedContent(byte[] content) : base(content)
        {

        }

        public CustomFormUrlEncodedContent(byte[] content, int offset, int count) : base(content, offset, count)
        {

        }

        public CustomFormUrlEncodedContent(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        : base(GetContentByteArray(nameValueCollection))
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        }

        private static byte[] GetContentByteArray(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            if (nameValueCollection == null)
            {
                throw new ArgumentNullException(nameof(nameValueCollection));
            }

            var stringBuilder = new StringBuilder();

            foreach (var current in nameValueCollection)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append('&');
                }

                stringBuilder.Append(Encode(current.Key));
                stringBuilder.Append('=');
                stringBuilder.Append(Encode(current.Value));
            }

            return Encoding.Default.GetBytes(stringBuilder.ToString());
        }

        private static string Encode(string data)
        {
            return string.IsNullOrEmpty(data) ? string.Empty : System.Net.WebUtility.UrlEncode(data)?.Replace("+", "%20").Replace(" ", "%20");
        }
    }
}

using System.Collections.Generic;

namespace Esia.Model
{
    public class EsiaAccountModel
    {
        private readonly IDictionary<string, string> _dict;
        public string AuthToken => GetValue("authToken");
        public string UserId => GetValue("userId");
        public string UserName => GetValue("userName");
        public string AuthnMethod => GetValue("authnMethod");
        public string DeviceType => GetValue("deviceType");
        public string PersonType => GetValue("personType");
        public string GlobalRole => GetValue("globalRole");
        public string LastName => GetValue("lastName");
        public string FirstName => GetValue("firstName");
        public string MiddleName => GetValue("middleName");
        public string PersonInn => GetValue("personINN");
        public string PersonSnils => GetValue("personSNILS");
        public string PersonEmail => GetValue("personEMail");

        public EsiaAccountModel(IDictionary<string, string> dict)
        {
            _dict = dict;
        }

        private string GetValue(string name)
        {
            string result;

            if (_dict.TryGetValue(name, out result))
                return result;

            return null;
        }
    }
}

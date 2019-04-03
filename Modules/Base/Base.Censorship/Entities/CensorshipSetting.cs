using Base.Attributes;
using System;
using System.Linq;
using Base.Settings;

namespace Base.Censorship.Entities
{
    [Serializable]
    public class CensorshipSetting : SettingItem
    {
        [NonSerialized]
        private string[] _whiteListArray = null;

        [DetailView("Установить цензуру")]
        public bool TurnOn { get; set; }

        [DetailView("Регулярное выражение", Description = "Сервисы для проверки: debuggex.com или regexper.com")]
        public string Regex { get; set; }

        [DetailView("Белый список", Description = "Слова-исключения, разделитель - пробел")]
        public string WhiteList { get; set; }
        
        public string[] WhiteListArray 
        {
            get
            {
                if (_whiteListArray == null)
                    _whiteListArray = this.WhiteList.Split(' ', ';').Where(x => !string.IsNullOrEmpty(x)).ToArray();

                return _whiteListArray;
            }
        }
    }
}

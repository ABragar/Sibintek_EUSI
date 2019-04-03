using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sib.Taxes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ShemaPathAttribute : Attribute
    {
        private string _shemaPath;

        /// <summary>
        /// Инициализирует новый экземпляр класса ShemaPathAttribute.
        /// </summary>
        /// <param name="shemaPath"></param>
        public ShemaPathAttribute(string shemaPath) : base()
        {
            _shemaPath = shemaPath;
        }

        /// <summary>
        /// Получает путь ресурса с файлом схемы.
        /// </summary>
        public string ShemaPath { get { return _shemaPath; } }
    }
}

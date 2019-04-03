using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr
{
    public class RosReestrXmlUrlResolver : System.Xml.XmlUrlResolver
    {
        
        private Assembly resourceAssembly = null;

        private string source = "";

        /// <summary>
        /// Инициализирует новый экземпляр класса RosReestrXmlUrlResolver.
        /// </summary>
        /// <param name="resourceAssembly"></param>
        public RosReestrXmlUrlResolver(Assembly resourceAssembly, string resourceSource): base()
        {
            this.resourceAssembly = resourceAssembly;
            this.source = resourceSource;
        }

        //public string Resource { get; set; }

        /// <summary>
        /// Переопределяет GetEntity().
        /// </summary>
        /// <param name="absoluteUri">Абсолютный Uri.</param>
        /// <param name="role">Не используется.</param>
        /// <param name="ofObjectToReturn">Тип возвращаемого объекта.</param>
        /// <returns>Запрашиваемый ресурс сборки.</returns>
        override public object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri.IsFile)
            {
                string file = Path.GetFileName(absoluteUri.AbsolutePath);
                int length = GetNthIndex(source, '.', 2);
                if (length == -1)
                    length = source.Length;
                string folder = source.Substring(0, length);
                Stream stream = resourceAssembly.GetManifestResourceStream(
                   String.Format("{0}.{1}", folder, file));
                return stream;
                
            }
            return null;
        }


        /// <summary>
        /// Возвращает n-ое вхождение символа t в строке s с конца.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private int GetNthIndex(string s, char t, int n)
        {
            int count = 0;
            for (int i = s.Length-1; i > -1; i--)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
   
}

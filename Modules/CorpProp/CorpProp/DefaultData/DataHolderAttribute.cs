using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CorpProp.DefaultData
{

    public class DataHolderAttribute: Attribute
    {
        public DataHolderAttribute(string resourcesFolder)
        {
            ResourcesFolder = resourcesFolder;
        }
        /// <summary>
        /// Наименование пути до папки с файлами, содержащего дефолтные данные для БД.
        /// </summary>
        public string ResourcesFolder { get; set; }
    }
}

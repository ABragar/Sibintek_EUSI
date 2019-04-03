using Base.DAL;
using Base.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Common
{
    //TODO: перенести IXmlDataImport в п/с импорта/экспорта.
    /// <summary>
    /// Предоставляет методы импорта объектов из файлов Excel с помощью библиотеки ExcelDataReader.
    /// </summary>
    public interface IXmlDataImport : IService
    {
        /// <summary>
        /// Импорт объектов из XML.
        /// </summary>       
        void ImportXML(IUnitOfWork unitOfWork, Stream stream, ref string error);
    }
}

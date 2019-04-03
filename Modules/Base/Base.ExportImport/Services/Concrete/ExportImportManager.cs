using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Xsl;
using Base.ExportImport.Entities;
using Base.ExportImport.Services.Abstract;
using Base.Service;

namespace Base.ExportImport.Services.Concrete
{
    public class ExportImportManager : IExportImportManager
    {
        private readonly IFileSystemService _fileSystemService;
        public ExportImportManager(IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
        }

        public XDocument ExportToXML(IEnumerable items, Package package)
        {
            var t = Type.GetType(package.ObjectType);
            if (t == null)
                throw new Exception($"Тип { package.ObjectType} не найден ");
            
            var document = new XDocument();

            using (var writer = document.CreateWriter())
            {                
                writer.WriteStartDocument();
                writer.WriteStartElement("Items");

                foreach (var item in items)
                {                    
                    var serializer = new XmlSerializer(item.GetType());
                    serializer.Serialize(writer, item);                    
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();                
            }

            if (package.Transform != null)
            {
                var xslt = new XslCompiledTransform();
                xslt.Load(XmlReader.Create(new StreamReader(_fileSystemService.GetFilePath(package.Transform.FileID))));
                xslt.Transform(document.CreateReader(), document.CreateWriter());
            }
            return document;
        }

        public MemoryStream GetExportStream(IEnumerable items, Package package)
        {
            var doc = ExportToXML(items, package);
            var s = new MemoryStream();
            doc.Save(s);
            return s;
        }

        public string GetExportString(IEnumerable items, Package package)
        {
            var doc = ExportToXML(items, package);
            var reader = doc.CreateReader();
            return reader.Value;
        }


    }
}

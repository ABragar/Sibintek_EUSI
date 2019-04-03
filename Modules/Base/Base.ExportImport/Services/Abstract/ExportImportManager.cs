using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Base.ExportImport.Entities;

namespace Base.ExportImport.Services.Abstract
{
    public interface IExportImportManager
    {
        XDocument ExportToXML(IEnumerable items, Package package);
        MemoryStream GetExportStream(IEnumerable items, Package package);
        string GetExportString(IEnumerable items, Package package);
    }
}

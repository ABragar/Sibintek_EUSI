using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CorpProp.RosReestr.Helpers
{
    public static class ImportChecker
    {
        //TODO: проверять схемы
        public static string CheckShema(MemoryStream stream)
        {              
            try
            {
                XDocument xdoc = XDocument.Load(stream);
                if (xdoc != null)
                {
                    System.Xml.Schema.XmlSchemaSet schemas = new System.Xml.Schema.XmlSchemaSet();
                    string resource = "SibRosReestr.Shemas.EGRP.04.04_EXTRACT_SBJ.XSD";
                        
                    System.Reflection.Assembly assembly = typeof(SibRosReestr.EGRP.V04.ExtractSubj.Extract).Assembly;
                    //schemas.XmlResolver = new RosReestrXmlUrlResolver(assembly);
                    using (Stream rstream = assembly.GetManifestResourceStream(resource))
                    {
                        if (rstream != null)
                        {
                            schemas.Add("", System.Xml.XmlReader.Create(rstream));
                            System.Xml.Schema.Extensions.Validate(xdoc, schemas, (o, e) =>
                            {                                
                                if (e != null)
                                {
                                   //ошибка
                                }

                            });
                        }
                    }
                }               

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
            return "";
        }

       

    }
}

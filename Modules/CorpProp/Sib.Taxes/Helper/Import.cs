using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sib.Taxes.Helper
{

    /// <summary>
    /// Предоставляет методы и свойства для импорта экземпляра налоговой декларации.
    /// </summary>
    public class DeclarationImport
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса DeclarationImport.
        /// </summary>
        public DeclarationImport()
        {

        }

        /// <summary>
        /// Возвращает тип объекта сборки в соответсвии с наименованием ресурса (путь схемы XSD).
        /// </summary>
        /// <param name="resourceName">Наименование ресурса манифеста.</param>
        /// <returns></returns>
        public Type GetTypeByShema(string resourceName)
        {
            return this.GetType().Assembly.GetTypes()
                .Where(f =>
                !f.IsAbstract
                && !f.IsInterface
                && f.GetCustomAttributes(false).Any()
                && f.GetCustomAttributes(false).Where(w => w.GetType().Equals(typeof(ShemaPathAttribute))
                && ((ShemaPathAttribute)w).ShemaPath == resourceName).Any()
                ).DefaultIfEmpty()
                .FirstOrDefault();
        }

        /// <summary>
        /// Десериализация объекта типа T из потока.        
        /// </summary>
        /// <param name="stream">Поток.</param>
        /// <returns>Десериализованный объект.</returns>
        public T DeserializeFromStream<T>(
             Stream stream            
            ) where T : class
        {
            T obj = null;
            try
            {
                if (stream != null && stream.Length > 0)
                {
                    stream.Position = 0;
                    System.Xml.Serialization.XmlSerializer reader =
                        new System.Xml.Serialization.XmlSerializer(typeof(T));
                    obj = (T)reader.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }


        /// <summary>
        /// Вовзращает наименование ресурса манифеста, являющейся верной схемой XSD для передаваемого в потоке файла XML.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>Наименование ресурса сборки файла XSD.</returns>
        public string FindValidShema(Stream stream)
        {
            stream.Position = 0;
            XDocument xdoc = XDocument.Load(stream);
            if (xdoc != null)
            {
                var resources = this.GetType().Assembly.GetManifestResourceNames().Where(f => f.Contains(".xsd"));
                foreach (var resource in resources)
                {
                    if (CheckByShema(xdoc, resource))
                        return resource;
                }
            }           
            return "";
        }

        /// <summary>
        /// Проверяет файл на соответствие схеме.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="xdoc"></param>
        /// <param name="resource"></param>
        private bool CheckByShema(            
             XDocument xdoc
            , string resource)
        {
            bool res = true;
            try
            {

                System.Xml.Schema.XmlSchemaSet schemas = new System.Xml.Schema.XmlSchemaSet();               
                using (Stream rstream = this.GetType().Assembly.GetManifestResourceStream(resource))
                {
                    if (rstream != null)
                    {
                        schemas.Add("", System.Xml.XmlReader.Create(rstream));
                        System.Xml.Schema.Extensions.Validate(xdoc, schemas, (o, e) =>
                        {
                            if (e != null)                            
                                res = false;
                        });
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return false;
            }           
        }
    }
}

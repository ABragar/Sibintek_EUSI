using Base.DAL;
using Base.Service;
using Base.Utils.Common.Wrappers;
using CorpProp.Entities.Document;
using CorpProp.Entities.Import;
using CorpProp.Entities.Law;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.Security;
using CorpProp.Helpers.Import.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CorpProp.RosReestr.Helpers
{
    public partial class ImportHolder
    {
        /// <summary>
        /// Создает экземпляр виписки заданного типа.
        /// </summary>
        /// <typeparam name="T">Тип выписки.</typeparam>
        public void CreateExtract<T>() where T : Extract
        {
            T ext = Activator.CreateInstance<T>();
            string typeName = typeof(T).Name;
            ext.ExtractType = this.UnitOfWork.GetRepository<ExtractType>().All().Where(x => x.Code == typeName).FirstOrDefault();
            ext.ExtractFormat = this.UnitOfWork.GetRepository<ExtractFormat>().All().Where(x => x.Code == "Electronic").FirstOrDefault();
            this.Extract = this.UnitOfWork.GetRepository<T>().Create(ext);
            this.ImportHistory.Mnemonic = this.Extract.GetType().Name.ToString();

            if (this._ImportHistory.FileCard != null)
            {
                var fileID = this._ImportHistory.FileCard.ID;
                var link = this.UnitOfWork.GetRepository<FileCardAndExtract>().Create(new FileCardAndExtract());
                link.ObjLeft = this.UnitOfWork.GetRepository<FileCard>().Filter(f => f.ID == fileID).FirstOrDefault();
                link.ObjRigth = this.Extract;
            }
            
        }

        /// <summary>
        /// Создает экземпляр истории импорта.
        /// </summary>
        public void CreateImportHistory( )
        {  
            _ImportHistory = this.UofWHistory.GetRepository<ImportHistory>()
                .Create(new ImportHistory() { FileName = _fileName, Mnemonic = "Extract" });

            
            _ImportHistory.FileCard = _file;

            if (_userID != null)
            {
                SibUser us = UofWHistory.GetRepository<SibUser>().Filter(x=>x.UserID == _userID).FirstOrDefault();
                if (us != null)
                    _ImportHistory.SibUser = us;
            }
           
        }

       
        public void ReadXML(Stream stream)
        {
            try
            {
                stream.Position = 0;
                XDocument xdoc = XDocument.Load(stream);
                if (xdoc != null)
                {
                    System.Reflection.Assembly assembly = typeof(SibRosReestr.EGRN.Unknown.ExtractBaseParamsLand).Assembly;
                    var resources = assembly.GetManifestResourceNames();
                    foreach (var resource in resources)
                    {
                        if (resource.ToLower().Contains("extract_base_params") 
                            || resource.ToLower().Contains("extract_sbj"))
                        {
                            CheckByShema(assembly, xdoc, resource);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //
            }
            return;
        }


        /// <summary>
        /// Проверяет файл на соответствие схеме.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="xdoc"></param>
        /// <param name="resource"></param>
        public bool CheckByShema(
            System.Reflection.Assembly assembly
            , XDocument xdoc
            , string resource)
        {
            bool res = true;        
            try
            {
                
                System.Xml.Schema.XmlSchemaSet schemas = new System.Xml.Schema.XmlSchemaSet();
                schemas.XmlResolver = new SibRosReestr.RosReestrXmlUrlResolver(assembly, resource);
                using (Stream rstream = assembly.GetManifestResourceStream(resource))
                {
                    if (rstream != null)
                    {
                        schemas.Add("", System.Xml.XmlReader.Create(rstream));
                        System.Xml.Schema.Extensions.Validate(xdoc, schemas, (o, e) =>
                        {
                            if (e != null)
                            {
                                res = false;
                                if (e.Exception != null)
                                    ImportHistory.ImportErrorLogs.AddError(
                                        e.Exception.LineNumber
                                        , e.Exception.LinePosition
                                        , ""
                                        , e.Exception.Message
                                        , CorpProp.Helpers.ErrorType.XML
                                        );                                
                            }

                        });
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                ImportHistory.ImportErrorLogs.AddError(ex);
            }
            return res;
        }

        

        public bool ValidateShema(Stream stream, string resource)
        {
            try
            {
                stream.Position = 0;
                XDocument xdoc = XDocument.Load(stream);
                if (xdoc != null)
                {
                    System.Reflection.Assembly assembly = typeof(SibRosReestr.EGRN.Unknown.ExtractBaseParamsLand).Assembly;
                    return CheckByShema(assembly, xdoc, resource);
                }

            }
            catch (Exception ex)
            {
                ImportHistory.ImportErrorLogs.AddError(ex);
            }
            return false;
        }
    }
}

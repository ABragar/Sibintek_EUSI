using Base.DAL;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.RosReestr.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using CorpProp.RosReestr.Extentions.EGRP.V04;
using CorpProp.RosReestr.Extentions.EGRP.V06;
using CorpProp.Entities.Law;
using Base.Utils.Common;

namespace CorpProp.RosReestr.Helpers
{
    public static class ImportStarter
    {        
        private static string ShemaExtractSubjV06 = "SibRosReestr.Shemas.EGRP.06.04_EXTRACT_SBJ.XSD";
        private static string ShemaExtractSubjV04 = "SibRosReestr.Shemas.EGRP.04.04_EXTRACT_SBJ.XSD";

        private static string ShemaBuild = "SibRosReestr.Shemas.EGRN.Unknown.extract_base_params_build_v01.xsd";
        private static string ShemaCarParking = "SibRosReestr.Shemas.EGRN.Unknown.extract_base_params_car_parking_space_v01.xsd";
        private static string ShemaConstruction = "SibRosReestr.Shemas.EGRN.Unknown.extract_base_params_construction_v01.xsd";
        private static string ShemaLand = "SibRosReestr.Shemas.EGRN.Unknown.extract_base_params_land_v01.xsd";
        private static string ShemaRoom = "SibRosReestr.Shemas.EGRN.Unknown.extract_base_params_room_v01.xsd";
        private static string ShemaUnderConstruction = "SibRosReestr.Shemas.EGRN.Unknown.extract_base_params_under_construction_v01.xsd";
        private static string ShemaRealEstateComplex = "SibRosReestr.Shemas.EGRN.Unknown.extract_base_params_unified_real_estate_complex_v01.xsd";

        public  static string InvalidShema = "Файл не соответсвует схеме.";
        private static string InvalidFile = "Невозможно определить тип импортируемой информации. Файл не соответсвует схеме ни одной из схем.";
        public static string InvalidNumber = @"Выписка с регистрационным номером <{0}> уже существует в Системе.";
        private static string ErrorDeserialize = "Невозможно десериализовать выписку: {0}";



        public static void Start(this ImportHolder holder, Stream stream)
        {
            XmlDocument doc = LoadFromStream(stream);
            var root = doc.DocumentElement;
            switch (root.Name)
            {
                case "Extract":                   
                    holder.StartEGRP(stream, doc);
                    break;
                case "extract_base_params_build":
                    holder.StartBuild(stream);
                    break;
                case "extract_base_params_land":
                    holder.StartLand(stream);
                    break;
                case "extract_base_params_car_parking_space":
                    holder.StartCarParkingSpace(stream);
                    break;
                case "extract_base_params_construction":
                    holder.StartConstruction(stream);
                    break;
                case "extract_base_params_room":
                    holder.StartRoom(stream);
                    break;
                case "extract_base_params_under_construction":
                    holder.StartUnderConstruction(stream);
                    break;
                case "extract_base_params_unified_real_estate_complex":
                    holder.StartRealEstateComplex(stream);
                    break;
                default:
                    holder.ImportHistory.ImportErrorLogs.AddError(InvalidFile);
                    break;
            }
        }


        private static void StartEGRP(this ImportHolder holder, Stream stream, XmlDocument doc)
        {
            int vers = GetVersion(GetXmlVersion(doc));
            if (vers == 6)
            {
                if (holder.ValidateShema(stream, ShemaExtractSubjV06))
                    holder.CreateExtractV06(stream);
            }             
            else
            {
                if (holder.ValidateShema(stream, ShemaExtractSubjV04))
                    holder.CreateExtractV04(stream);
            }           
                
            
        }

        private static void StartBuild(this ImportHolder holder, Stream stream)
        {
            if (holder.ValidateShema(stream, ShemaBuild))
                holder.CreateExtractBuild(stream);
        }

        private static void StartLand(this ImportHolder holder, Stream stream)
        {
            if (holder.ValidateShema(stream, ShemaLand))
                holder.CreateExtractLand(stream);
        }

        private static void StartCarParkingSpace(this ImportHolder holder, Stream stream)
        {
            if (holder.ValidateShema(stream, ShemaCarParking))
                holder.CreateExtractCarParking(stream);
        }

        private static void StartConstruction(this ImportHolder holder, Stream stream)
        {
            if (holder.ValidateShema(stream, ShemaConstruction))
                holder.CreateExtractCostruction(stream);
        }

        private static void StartRoom(this ImportHolder holder, Stream stream)
        {
            if (holder.ValidateShema(stream, ShemaRoom))
                holder.CreateExtractRoom(stream);
        }

        private static void StartUnderConstruction(this ImportHolder holder, Stream stream)
        {
            if (holder.ValidateShema(stream, ShemaUnderConstruction))
                holder.CreateExtractUnderCostruction(stream);
        }

        private static void StartRealEstateComplex(this ImportHolder holder, Stream stream)
        {
            if (holder.ValidateShema(stream, ShemaRealEstateComplex))
                holder.CreateExtractRealEstateComplex(stream);
        }



        private static bool ContainsXmlVersion(XmlDocument xml)
        {
            if (xml == null) return false;
            XmlNodeList els = xml.GetElementsByTagName("eDocument");
            if (els == null || els.Count == 0) return false;
            var vers = els.Item(0).Attributes.GetNamedItem("Version");
            if (vers != null)
                return true;
            return false;
        }

        private static string GetXmlVersion(XmlDocument xml)
        {
            if (xml == null) return null;
            XmlNodeList els = xml.GetElementsByTagName("eDocument");
            if (els == null || els.Count == 0) return null;
            var vers = els.Item(0).Attributes.GetNamedItem("Version");
            if (vers != null && !String.IsNullOrEmpty(vers.Value))
                return vers.Value;
            return null;
        }

        private static int GetVersion(string vers)
        {
            //TODO: пока только две версии
            int v = ImportHelper.GetInt(vers.ToString());
            if (v >= 6)            
                return 6;            
            else return 4;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>stream.Position=0; return static XmlDocument</returns>
        private static XmlDocument LoadFromStream(Stream stream)
        {
            XmlDocument doc = new XmlDocument();
            using (XmlReader reader = XmlReader.Create(stream))
            {
                doc.Load(reader);
            }
            stream.Position = 0;
            return doc;
        }

        public static void CreateExtractV04(this ImportHolder holder, Stream stream)
        {
            SibRosReestr.EGRP.V04.ExtractSubj.Extract obj = 
                ImportHelper.DeserializeFromStream<SibRosReestr.EGRP.V04.ExtractSubj.Extract>(stream);

            if (obj != null)
            {
                if (holder.CheckExtractNumber(obj))
                {
                    holder.CreateExtract<Entities.ExtractSubj>();
                    ((Entities.ExtractSubj)holder.Extract).ImportV04(obj, ref holder);
                }
                    
            } 
            
            if (obj == null)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(InvalidShema);
            }
        }

        public static void CreateExtractV06(this ImportHolder holder, Stream stream)
        {
            SibRosReestr.EGRP.V06.ExtractSubj.Extract obj = 
                ImportHelper.DeserializeFromStream<SibRosReestr.EGRP.V06.ExtractSubj.Extract>(stream);

            if (obj != null )
            {
                if (holder.CheckExtractNumber(obj))
                {
                    holder.CreateExtract<Entities.ExtractSubj>();
                    ((Entities.ExtractSubj)holder.Extract).ImportV06(obj, ref holder);
                }                
                
            }
            if (obj == null)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(InvalidShema);
            }

        }

        private static bool CheckExtractNumber(this ImportHolder holder, SibRosReestr.EGRP.V06.ExtractSubj.Extract obj)
        {
            if (obj == null) return true;
            var numb = obj.ReestrExtract?.DeclarAttribute?.ExtractNumber;
            if (!String.IsNullOrEmpty(numb))
            {
                var res = holder.UnitOfWork.GetRepository<Extract>().Filter(f => !f.Hidden && f.ExtractNumber == numb).Any();
                if (res)
                {
                    holder.ImportHistory.Mnemonic = "ExtractSubj";
                    holder.ImportHistory.ImportErrorLogs.AddError(String.Format(InvalidNumber, numb));
                    return false;
                }
            }
            return true;
        }

        private static bool CheckExtractNumber(this ImportHolder holder, SibRosReestr.EGRP.V04.ExtractSubj.Extract obj)
        {
            if (obj == null) return true;
            var numb = obj.ReestrExtract?.DeclarAttribute?.ExtractNumber;
            if (!String.IsNullOrEmpty(numb))
            {
                var res = holder.UnitOfWork.GetRepository<Extract>().Filter(f => !f.Hidden && f.ExtractNumber == numb).Any();
                if (res)
                {
                    holder.ImportHistory.Mnemonic = "ExtractSubj";
                    holder.ImportHistory.ImportErrorLogs.AddError(String.Format(InvalidNumber, numb));
                    return false;
                }
            }
            return true;
        }
        private static bool CheckExtractNumber(this ImportHolder holder, SibRosReestr.EGRN.Unknown.ExtractBaseParamsBuild obj)
        {
            if (obj == null) return true;
            var numb = obj.Details_statement?.Group_top_requisites?.Registration_number;
            if (!String.IsNullOrEmpty(numb))
            {
                var res = holder.UnitOfWork.GetRepository<Extract>().Filter(f => !f.Hidden && f.ExtractNumber == numb).Any();
                if (res)
                {
                    holder.ImportHistory.Mnemonic = "ExtractObject";
                    holder.ImportHistory.ImportErrorLogs.AddError(String.Format(InvalidNumber, numb));
                    return false;
                }
            }
            return true;
        }

        private static bool CheckExtractNumber(this ImportHolder holder, SibRosReestr.EGRN.Unknown.ExtractBaseParamsLand obj)
        {
            if (obj == null) return true;
            var numb = obj.Details_statement?.Group_top_requisites?.Registration_number;
            if (!String.IsNullOrEmpty(numb))
            {
                var res = holder.UnitOfWork.GetRepository<Extract>().Filter(f => !f.Hidden && f.ExtractNumber == numb).Any();
                if (res)
                {
                    holder.ImportHistory.Mnemonic = "ExtractObject";
                    holder.ImportHistory.ImportErrorLogs.AddError(String.Format(InvalidNumber, numb));
                    return false;
                }
            }
            return true;
        }

              

        public static void CreateExtractBuild(this ImportHolder holder, Stream stream)
        {
            SibRosReestr.EGRN.Unknown.ExtractBaseParamsBuild build =
                holder.DeserializeFromStream<SibRosReestr.EGRN.Unknown.ExtractBaseParamsBuild>(stream);

            if (build != null)
            {
                if (holder.CheckExtractNumber(build))
                {
                    holder.CreateExtract<Entities.ExtractBuild>();
                    ((Entities.ExtractBuild)holder.Extract).Import(build, ref holder);
                    return;
                }
                else return;

            } 
        }
        public static void CreateExtractLand(this ImportHolder holder, Stream stream)
        {
            SibRosReestr.EGRN.Unknown.ExtractBaseParamsLand land =
             holder.DeserializeFromStream<SibRosReestr.EGRN.Unknown.ExtractBaseParamsLand>(stream);

            if (land != null)
            {
                if (holder.CheckExtractNumber(land))
                {
                    holder.CreateExtract<Entities.ExtractLand>();
                    ((Entities.ExtractLand)holder.Extract).Import(land, ref holder);
                    return;
                }
                else return;
            }
        }
        public static void CreateExtractCarParking(this ImportHolder holder, Stream stream)
        {
            //TODO: реализовать
        }
        public static void CreateExtractCostruction(this ImportHolder holder, Stream stream)
        {
            //TODO: реализовать
            throw new NotImplementedException();
        }
        public static void CreateExtractRoom(this ImportHolder holder, Stream stream)
        {
            //TODO: реализовать
            throw new NotImplementedException();
        }
        public static void CreateExtractUnderCostruction(this ImportHolder holder, Stream stream)
        {
            //TODO: реализовать
            throw new NotImplementedException();
        }
        public static void CreateExtractRealEstateComplex(this ImportHolder holder, Stream stream)
        {
            //TODO: реализовать
            throw new NotImplementedException();
        }


        public static T DeserializeFromStream<T>(
            this ImportHolder holder
            , Stream stream

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
                holder.ImportHistory.ImportErrorLogs.AddError(
                    String.Format(ErrorDeserialize, ex.ToStringWithInner()
                    ));
            }
            return obj;
        }



    }
}

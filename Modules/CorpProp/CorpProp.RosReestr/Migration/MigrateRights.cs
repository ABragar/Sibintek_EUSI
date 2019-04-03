using Base.DAL;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Document;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Law;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.Mapping;
using CorpProp.Entities.Subject;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Migration
{
    public static class MigrateRights
    {
        public static int GetCount(this MigrateHolder holder, string state, string mnemonic)
        {
            int count = 0;
            try
            {
               count = (holder.MigrateLogs.Where(x => x != null && x.MigrateState.Code == state && x.Mnemonic == mnemonic).Any()) ?
               holder.MigrateLogs.Where(x => x != null && x.MigrateState.Code == state && x.Mnemonic == mnemonic).Count() : 0;

            }
            catch { }
            return count;
        }


        public static string GetCheckingReport(this MigrateHolder holder)
        {
            string mess = "";
            var createOI = holder.GetCount("101", "Cadastral") + holder.GetCount("101", "ObjectRecord");
            var updateOI = holder.GetCount("102", "Cadastral") + holder.GetCount("102", "ObjectRecord");

            var createRR = holder.GetCount("101", "Right") + holder.GetCount("101", "RightRecord");
            var updateRR = holder.GetCount("102", "Right") + holder.GetCount("102", "RightRecord");

            var createEnc = holder.GetCount("101", "Encumbrance"); 
            var updateEnc = holder.GetCount("102", "Encumbrance");

            var createDoc = holder.GetCount("101", "FileCardOne");
            var updateDoc = holder.GetCount("102", "FileCardOne");


            mess += "<div class='sib-popup-content'>";
            mess += @"<div>В результате миграции</div>";
            if (createOI > 0)
                mess += @"<div>Будет создано " + createOI + " объектов имущества</div>";
            if (updateOI > 0)
                mess += @"<div>Будет обновлено " + updateOI + " объектов имущества</div>";
            if (createRR > 0)
                mess += @"<div>Будет создано " + createRR + " объектов Права</div>";
            if (updateRR > 0)
                mess += @"<div>Будет обновлено " + updateRR + " объектов Права</div>";
            if (createEnc > 0)
                mess += @"<div>Будет создано " + createEnc + " объектов Обременений/ограничений</div>";
            if (updateEnc > 0)
                mess += @"<div>Будет обновлено " + updateEnc + " объектов Обременений/ограничений</div>";
            if (createDoc > 0)
                mess += @"<div>Будет создано " + createDoc + " документов</div>";
            if (updateDoc > 0)
                mess += @"<div>Будет обновлено " + updateDoc + " документов</div>";

            if (createOI == 0 && updateOI == 0 
                && createRR == 0 && updateRR == 0 
                && createEnc == 0 && updateEnc == 0
                && createDoc == 0 && updateDoc == 0)
                mess += "планируемых изменений не выявлено";
            mess += "</div>";
            

            return mess;
        }

        public static MigrateHolder StartMigrateRights(IUnitOfWork uow, IUnitOfWork uowHistory, string[] oids, int? userID, bool save = true)
        {           
            MigrateHolder holder = new MigrateHolder(uow, uowHistory, null, userID);
            try
            {               
                foreach (var strId in oids)
                {
                    MigrateFromRight(uow, strId, holder);
                }
                holder.SetResult(save);
            }
            catch (Exception ex)
            {
                holder.AddError(nameof(ExtractSubj), "", ex.Message);
            }
            return holder;

        }

        public static void MigrateFromRight(IUnitOfWork uow, string strId, MigrateHolder holder)
        {
            int id = 0;
            int.TryParse(strId, out id);
            if (id != 0)
            {
                bool upd = false;
                var rr = uow.GetRepository<RightRecord>().Filter(x => !x.Hidden && x.ID == id).FirstOrDefault();
                if (rr != null)
                {
                    if (holder.Extract == null)
                        holder.SetExtract(rr.Extract);

                    upd = rr.MigrateRight(uow, holder);
                    rr.UpdateCPDateTime = DateTime.Now;
                    rr.isAccept = true;
                    if (upd)
                        rr.UpdateCPStatus = StatusCorpProp.Updated;
                    else
                        rr.UpdateCPStatus = StatusCorpProp.NotUpdated;
                }
                
            }
        }
        public static bool MigrateRight(this RightRecord obj, IUnitOfWork uow, MigrateHolder holder)
        {
            if (obj == null || uow == null) return false;

            try
            {
                var rId = obj.ID;
                var list = uow.GetRepository<RightHolder>().Filter(f => !f.Hidden && f.RightRecordID == rId).ToList();

                foreach (var item in list)
                {
                    var og = ImportHelper.FindSociety(uow, item.Inn, item.Ogrn);
                    
                    //если право не наше, не мигрируем
                    if (og == null)
                    {
                        holder.AddInfo(nameof(RightRecord), obj.RegNumber + " " + item.Name, "Не найдено ОГ, соответствующее правообладателю.");
                        continue;
                    } 

                    Right rr = obj.Migrate(uow, holder, og);
                    rr.Society = og;
                    Estate est = rr.MigrateEstate(uow, obj.ObjectRecordID, holder);
                    rr.Estate = est;
                    obj.Estate = est;
                    if (est != null && est is Cadastral)
                    {
                        Cadastral cad = est as Cadastral;
                        rr.Address = cad.Address;
                        rr.Appointments = cad.UsesKind;
                        rr.RealEstateKind = cad.RealEstateKind;
                        rr.RegionCode = cad.RegionCode;
                    }

                    rr.MigrateDocRights(uow, obj, est, holder);
                    rr.MigrateEncumbrances(uow, obj, est, holder);
                    rr.MigrateExtract(uow, obj, holder);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                holder.AddError(nameof(RightRecord), obj.RegNumber, ex.Message);
            }
            return false;
        }

      
        public static void MigrateEncumbrances(this Right rr, IUnitOfWork uow, RightRecord obj, Estate est, MigrateHolder holder)
        {
            if (obj == null || est == null) return;
            int rightId = obj.ID;
            int estId = (obj.ObjectRecord == null) ? 0: obj.ObjectRecord.ID ;

            var list = uow.GetRepository<RestrictRecord>()
                .Filter(f => !f.Hidden && (f.RightRecordID == rightId || f.ObjectRecordID == estId)).ToList();

            foreach (var item in list)
            {
                Encumbrance enc  = MigrateEncumbrance.FindOrCreateEncumbrance(uow, item, holder);
                enc.Estate = est;
                enc.Right = rr;
                enc.Migrate(uow, rr, item);
            }
        }


        public static void MigrateExtract(this Right rr, IUnitOfWork uow, RightRecord obj, MigrateHolder holder)
        {
            //выписка как правоудостоверяющий документ
            if (obj == null || obj.Extract == null) return;

            Extract ext = obj.Extract;

            FileCard file = rr.FindFileCardExtract(uow, ext, holder);
            if (file == null)
            {
                file = uow.GetRepository<FileCardOne>().Create(new FileCardOne());
                file.CategoryID = 1;
                file.Name = ext.Name;
                file.Number = ext.ExtractNumber;
                file.DateCard = ext.ExtractDate;
                file.Description = ext.Name;

                FileCardAndCertificateRight doc =
                      uow.GetRepository<FileCardAndCertificateRight>()
                      .Create(new FileCardAndCertificateRight());
                doc.ObjRigth = rr;
                doc.ObjLeft = file;
            }    
            
        }

        public static FileCard FindFileCardExtract(this Right rr, IUnitOfWork uow, Extract ext, MigrateHolder holder)
        {
            var extID = ext.ID;
            var numb = ext.ExtractNumber;           

            FileCardAndExtract fl = uow.GetRepository<FileCardAndExtract>()
                .Filter(x => !x.Hidden && x.ObjRigthId == extID && x.ObjLeft.Number == numb)
                .FirstOrDefault();

            if (fl != null)
            {
                var flid = fl.ObjLeftId;
                var rrID = rr.ID;
                FileCardAndCertificateRight doc =
                  uow.GetRepository<FileCardAndCertificateRight>()
                  .Filter(f => !f.Hidden && f.ObjLeftId == flid && f.ObjRigth.ID == rrID)
                  .FirstOrDefault();
                if (doc == null)
                {
                    doc = uow.GetRepository<FileCardAndCertificateRight>().Create(new FileCardAndCertificateRight());
                    doc.ObjRigth = rr;
                    doc.ObjLeft = fl.ObjLeft;
                }
                return fl.ObjLeft;                
            }
            return null;
        }

        public static void MigrateDocRights(this Right rr, IUnitOfWork uow, RightRecord obj, Estate est, MigrateHolder holder)
        {
            if (obj == null) return;
            int id = obj.ID;
            var list = uow.GetRepository<DocumentRecord>()
                .Filter(f => !f.Hidden && f.RightRecordID == id).ToList();

            foreach (var item in list)
            {

                FileCardOne file = rr.FindFileCard(uow, item);
                if (file == null)
                {
                    file = rr.CreateFileCard(uow);
                    holder.AddLog(nameof(FileCardOne), item.Number+" "+ item.Content, "101");
                }
                else
                    holder.AddLog(nameof(FileCardOne), item.Number + " " + item.Content, "102");
                file.Migrate(item);

            } 

        }

        private static  FileCardOne FindFileCard(this Right rr, IUnitOfWork uow, DocumentRecord doc)
        {
            int id = rr.ID;
            string docID = doc.ID_Document;
            var file = uow.GetRepository<FileCardOne>()
               .Filter(f => !f.Hidden && f.DocumentID == docID)
               .FirstOrDefault();

            if (file != null)
            {
                var fid = file.ID;
                FileCardAndLegalRight flink =
               uow.GetRepository<FileCardAndLegalRight>()
               .Filter(f => !f.Hidden && !f.ObjLeft.Hidden && !f.ObjRigth.Hidden && f.ObjLeftId == fid && f.ObjRigthId == id)
               .FirstOrDefault();
                if (flink == null)
                {
                    FileCardAndLegalRight fr =
                    uow.GetRepository<FileCardAndLegalRight>()
                    .Create(new FileCardAndLegalRight());
                        fr.ObjRigth = rr;
                        fr.ObjLeft = file;
                }
                
                var estateID = rr.EstateID;
                FileCardAndEstate fflink =
               uow.GetRepository<FileCardAndEstate>()
               .Filter(f => !f.Hidden && !f.ObjLeft.Hidden && !f.ObjRigth.Hidden && f.ObjLeftId == fid && f.ObjRigthId == estateID)
               .FirstOrDefault();
                if (fflink == null)
                {
                    FileCardAndEstate doc2 =
                      uow.GetRepository<FileCardAndEstate>()
                      .Create(new FileCardAndEstate());
                            doc2.ObjRigth = rr.Estate;
                            doc2.ObjLeft = file;
                }               
            }
            return file;
        }


        public static FileCardOne CreateFileCard(this Right rr, IUnitOfWork uow)
        {

            FileCardOne file =
                uow.GetRepository<FileCardOne>()
                .Create(new FileCardOne());

            FileCardAndLegalRight doc =
                uow.GetRepository<FileCardAndLegalRight>()
                .Create(new FileCardAndLegalRight());
            doc.ObjRigth = rr;
            doc.ObjLeft = file;

            FileCardAndEstate doc2 =
                uow.GetRepository<FileCardAndEstate>()
                .Create(new FileCardAndEstate());
            doc2.ObjRigth = rr.Estate;
            doc2.ObjLeft = file;
            return file;
        }


        public static Right Migrate(this RightRecord obj, IUnitOfWork uow, MigrateHolder holder, Society og)
        {
            if (obj == null || uow == null) return null;
            Right r = obj.FindRight(uow, og);
            if (r == null)
                r = obj.CreateRight(uow, holder);
            else
               r = obj.UpdateRight(uow, r, holder);
            return r;
        }

        /// <summary>
        /// Проверяет, существует ли в Системе ОГ = правообладателю.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="uow"></param>
        /// <returns></returns>
        public static bool CheckRightHolder(this RightRecord obj, IUnitOfWork uow)
        {
            if (obj == null || obj.SubjectRecord == null)
                return false;

            var subj = ImportHelper.FindSociety(uow, obj.SubjectRecord.Inn, obj.SubjectRecord.Ogrn);
            if (subj != null)
                return true;

            return false;
        }


        public static Right FindRight(this RightRecord obj, IUnitOfWork uow, Society og)
        {
            if (obj == null || String.IsNullOrEmpty(obj.RegNumber)) return null;
            
            var numb = obj.RegNumber;
            return uow.GetRepository<Right>().Filter(x => !x.Hidden && x.RegNumber == numb).FirstOrDefault();

           
        }

        public static Right CreateRight(this RightRecord obj, IUnitOfWork uow, MigrateHolder holder)
        {
            if (obj == null ) return null;
           
            var rr = uow.GetRepository<Right>().Create(new Right());
            
            //TODO: Estate            
            rr.Society = obj.GetHolder(uow);
            rr.CadastralNumber = obj.CadastralNumber;
            rr.RightKind = ImportHelper.FindOrCreateDictByName<RightKind>(uow, obj.RightTypeName);
           
            rr.Share = obj.ShareText;
            rr.ShareRightNumerator = (obj.Numerator == null)? 1 : obj.Numerator.Value;
            rr.ShareRightDenominator = (obj.Denominator == null) ? 1 : obj.Denominator.Value;
            rr.SetShare();            
            rr.RegDate = obj.RegDate;
            rr.RegNumber = obj.RegNumber;
            rr.SetKindAndShare();
            holder.AddLog(nameof(Right), rr.RegNumber, "101");
            return rr;
        }

        public static Right UpdateRight(this RightRecord obj, IUnitOfWork uow, Right rr, MigrateHolder holder)
        {
            if (obj == null || rr == null) return null;
                        
            rr.Society = obj.GetHolder(uow);
            rr.CadastralNumber = obj.CadastralNumber;
            rr.RightKind = ImportHelper.FindOrCreateDictByName<RightKind>(uow, obj.RightTypeName);

            rr.Share = obj.ShareText;
            rr.ShareRightNumerator = (obj.Numerator == null) ? 1 : obj.Numerator.Value;
            rr.ShareRightDenominator = (obj.Denominator == null) ? 1 : obj.Denominator.Value;
            rr.SetShare();
            rr.RegDate = obj.RegDate;
            rr.RegNumber = obj.RegNumber;
            rr.RegDateEnd = obj.EndDate;
            rr.SetKindAndShare();
            uow.GetRepository<Right>().Update(rr);
            holder.AddLog(nameof(Right), rr.RegNumber, "102");
            return rr;
        }

        public static Society GetHolder(this RightRecord obj, IUnitOfWork uow)
        {
            if (obj == null || obj.SubjectRecord == null)
                return null;

            return ImportHelper.FindSociety(uow, obj.SubjectRecord.Inn, obj.SubjectRecord.Ogrn);
            
        }


        public static Estate MigrateEstate(this Right rr, IUnitOfWork uow, int? id, MigrateHolder holder)
        {
            if (id == null) return null;
            ObjectRecord obj = uow.GetRepository<ObjectRecord>().Filter(x => !x.Hidden && x.ID == id.Value).FirstOrDefault();
            if (obj == null) return null;
            Estate est = CorpProp.RosReestr.Migration.MigrateEstate.FindOrCreateEstate(uow, obj, rr, holder);
            rr.Estate = est;
            //TODO: миграция ОИ
           
            est.StartMigrateEstate(uow, obj, holder);
            return est;
        }




        
        
        
    }
}

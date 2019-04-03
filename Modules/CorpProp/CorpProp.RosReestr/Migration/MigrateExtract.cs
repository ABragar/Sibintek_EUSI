using Base.DAL;
using CorpProp.Entities.Law;
using CorpProp.RosReestr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Migration
{
    public static class MigrateExtract
    {
        public static MigrateHolder CheckMigrateObjects(IUnitOfWork uow, IUnitOfWork uowHistory, int extractID, int? userID)
        {
            MigrateHolder holder = new MigrateHolder(uow, uowHistory, extractID, userID);
            try
            {

                holder.StartMigrate(uow);
                holder.SetResult();

            }
            catch (Exception ex)
            {

                var str = ex.Message + " ";
                if (ex.InnerException != null)
                    str += ex.InnerException.Message;
                holder.AddError(nameof(ExtractObject), "", str);
                holder.MigrateHistory.ResultText = "Завершено с ошибками.";
                holder.MigrateHistory.ResultText += str;
                //uowHistory.SaveChanges();
                return holder;
            }
            return holder;
        }


        public static MigrateHolder MigrateObjects(IUnitOfWork uow, IUnitOfWork uowHistory, int extractID, int? userID)
        {   
            MigrateHolder holder = new MigrateHolder(uow, uowHistory, extractID, userID);
            try
            {

                holder.StartMigrate(uow);
                holder.SetResult();

            }
            catch (Exception ex)
            {

                var str = ex.Message +" ";
                if (ex.InnerException != null)
                    str += ex.InnerException.Message;
                holder.AddError(nameof(ExtractObject), "", str);
                holder.MigrateHistory.ResultText = "Завершено с ошибками.";
                holder.MigrateHistory.ResultText += str;
                uowHistory.SaveChanges();
                return holder;
            }
            return holder;
        }

        public static void SetResult(this MigrateHolder holder, bool save = true)
        {            
            if (!holder.MigrateLogs.Where(x => x.MigrateState.Code == "103").Any())
            {
                holder.MigrateHistory.ResultText = "Завершено успешно.";
                holder.MigrateHistory.ResultText += System.Environment.NewLine;
                holder.MigrateHistory.ResultText += "Создано объектов: ";
                holder.MigrateHistory.ResultText += holder.MigrateLogs.Where(x => x.MigrateState.Code == "101").Count();
                holder.MigrateHistory.ResultText += System.Environment.NewLine;
                holder.MigrateHistory.ResultText += "Обновлено объектов: ";
                holder.MigrateHistory.ResultText += holder.MigrateLogs.Where(x => x.MigrateState.Code == "102").Count();

                if (save)
                {
                    holder.UofWHistory.SaveChanges();
                    holder.UnitOfWork.SaveChanges();
                }
                
            }
            else
            {
                holder.MigrateHistory.ResultText = "Завершено с ошибками. Миграция отклонена.";
                holder.MigrateHistory.ResultText += System.Environment.NewLine;
                holder.MigrateHistory.ResultText += "Не удалось создать/обновить : ";
                holder.MigrateHistory.ResultText += holder.MigrateLogs.Where(x => x.MigrateState.Code == "103").Count();
                holder.MigrateHistory.ResultText += " объектов.";
                if (save)
                    holder.UofWHistory.SaveChanges();
                return ;
            }
        }

        private static bool MigrateObject(this ExtractObject ext, IUnitOfWork uow, MigrateHolder holder)
        {
            if (ext == null) return false;
            var extId = ext.ID;
            var list = uow.GetRepository<RightRecord>()
                .Filter(x => !x.Hidden && x.ExtractID == extId).ToList();
            var upd = false;
            foreach (var rr in list)
            {
                upd = rr.MigrateRight(uow, holder);
                rr.UpdateCPDateTime = DateTime.Now;
                if (upd)
                    rr.UpdateCPStatus = StatusCorpProp.Updated;
                else
                    rr.UpdateCPStatus = StatusCorpProp.NotUpdated;
                rr.isAccept = true;
            }
            return true;
        }

        public static void UpdateState(this ExtractObject ext, bool state)
        {
            ext.UpdateCPStatus = (state) ? StatusCorpProp.Updated : StatusCorpProp.NotUpdated;
            ext.UpdateCPDateTime = DateTime.Now;
            ext.isAccept = true;
        }


        public static void StartMigrate(this MigrateHolder holder, IUnitOfWork uow)
        {
            try
            {
                var obj = holder.Extract as ExtractObject;
                if (obj == null) return;
                var val = obj.MigrateObject(uow, holder);
                obj.UpdateState(val);
            }
            catch { return ; }
            
        }

        public static void UpdateState(IUnitOfWork uow, string rightId, int state)
        {
            ExtractSubj es = null;            
            int id = 0;
            int.TryParse(rightId, out id);
            if (id != 0)
                es = uow.GetRepository<ExtractSubj>()
                    .Filter(x => !x.Hidden && x.ID == id)
                    .FirstOrDefault();            
            if (es != null)
            {
                es.UpdateCPStatus = GetSate(state);
                es.UpdateCPDateTime = DateTime.Now;
                es.isAccept = true;
            }

        }

        private static StatusCorpProp GetSate(int state)
        {
            StatusCorpProp st = StatusCorpProp.NotUpdated;
            switch (state)
            {
                case 0:
                    st= StatusCorpProp.Updated;
                    break;
                case 1:
                    st = StatusCorpProp.NotUpdated;
                    break;
                case 2:
                    st = StatusCorpProp.NotAll;
                    break;
                default:
                    break;
            }
            return st;
        }
    }
}

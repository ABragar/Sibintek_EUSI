using Base.DAL;
using Base.Service;
using Base.Utils.Common.Wrappers;
using CorpProp.Entities.Document;
using CorpProp.Helpers.Import.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Helpers
{
    public static class ImportLoader
    {

        public static ImportHolder Import(
            FileCard attachFile 
            , IUnitOfWork uow
            , IUnitOfWork uowHistory
            , Stream stream
            , string fileName
            , int? userID)
        {
            ImportHolder holder = new ImportHolder( attachFile, uow, uowHistory, fileName, userID);
            try
            {

                if (CorpProp.Helpers.ImportHelper.CheckRepeatImport(uow, stream))
                    holder.ImportHistory.ImportErrorLogs.AddError("Идентичный файл импортировался ранее.");
                else
                {
                    holder.Start(stream);
                    if (holder.Extract != null)
                    {
                        holder.Extract.SibUserID = holder.ImportHistory?.SibUser?.ID;
                    }
                }

                if (String.IsNullOrEmpty(holder.ImportHistory.Mnemonic))
                    holder.ImportHistory.Mnemonic = "Extract";

                if (holder.ImportHistory.ImportErrorLogs.Count == 0)
                {
                    holder.ImportHistory.ResultText = "Завершено успешно. Создано объектов:";
                    holder.ImportHistory.ResultText += System.Environment.NewLine;
                    holder.ImportHistory.ResultText += "Прав - ";
                    holder.ImportHistory.ResultText += (holder.Extract == null || holder.Extract?.CountRights == null) ? "0": holder.Extract.CountRights.ToString();
                    holder.ImportHistory.ResultText += System.Environment.NewLine;
                    holder.ImportHistory.ResultText += "ОНИ - ";
                    holder.ImportHistory.ResultText += (holder.Extract == null || holder.Extract?.CountObjects == null) ? "0" : holder.Extract.CountObjects.ToString();

                    uowHistory.SaveChanges();
                    uow.SaveChanges();
                }
                else
                {
                    holder.ImportHistory.ResultText = "Завершено с ошибками.";
                    if (holder.ImportHistory.ImportErrorLogs.Where(f => f.ErrorType.Contains("схеме")).Any())
                    {
                        holder.ImportHistory.ResultText += System.Environment.NewLine;
                        holder.ImportHistory.ResultText += ImportStarter.InvalidShema;
                    }
                    else if (holder.ImportHistory.ImportErrorLogs.Count == 1)
                    {
                        holder.ImportHistory.ResultText += System.Environment.NewLine;
                        holder.ImportHistory.ResultText += holder.ImportHistory.ImportErrorLogs.First().ErrorText;
                    }
                    uowHistory.SaveChanges();
                    return holder;
                }
                    
            }
            catch (Exception ex)
            {
                
                var str = ex.Message;
                holder.ImportHistory.ImportErrorLogs.AddError(str);
                holder.ImportHistory.ResultText = "Завершено с ошибками.";
                holder.ImportHistory.ResultText += str;
                uowHistory.SaveChanges();
                return holder;
            }
            return holder;
        }


    }
}

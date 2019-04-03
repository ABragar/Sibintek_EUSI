using Base.DAL;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Extentions.EGRN.Unknown;
using CorpProp.RosReestr.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Extentions
{
    /// <summary>
    /// Предоставляет методы расширения для импорта выписки о ЗУ.
    /// </summary>
    public static class ExtractLandExtentions
    {
        /// <summary>
        /// Инициация импорта xml-выписки о ЗУ в модуль Росреестра.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="ext"></param>
        /// <param name="holder"></param>
        public static void Import(
              this ExtractLand extract
            , SibRosReestr.EGRN.Unknown.ExtractBaseParamsLand land
            , ref ImportHolder holder
            )
        {
            if (land == null) return;

            extract.SetInfo(holder.UnitOfWork, land);
            LandRecord obj = extract.CreateLand(land, ref holder);
            extract.SetOwnerless(land.Ownerless_right_record);
            obj.CreateRights(land.Right_records, ref holder);
            obj.CreateRestrictRecords(land.Restrict_records, ref holder);
            extract.CountObjects++;
        }

        private static LandRecord CreateLand(
             this ExtractLand extract
            , SibRosReestr.EGRN.Unknown.ExtractBaseParamsLand obj
            , ref ImportHolder holder
            )
        {
            if (obj == null) return null;

            LandRecord br = holder.UnitOfWork.GetRepository<LandRecord>().Create(new LandRecord());
            br.Extract = extract;
            br.Import(obj, ref holder);

            extract.SetLandParams(obj.Land_record?.Params);
            extract.Address = br.Address;
            extract.Permitted_usesStr = br.Permitted_usesStr;
            extract.Cost = br.Cost = obj.Land_record?.Cost?.Value;
            extract.Special_notes = br.Special_notes = obj.Land_record?.Special_notes;


            return br;
        }

    


        public static void SetInfo(
         this ExtractLand extract
         , IUnitOfWork uow
         , SibRosReestr.EGRN.Unknown.ExtractBaseParamsLand land
       )
        {
            extract.SetDetailState(land.Details_statement);
            extract.SetDetailRequest(land.Details_request);

            extract.RecipientName = land.Recipient_statement;
            extract.Status = land.Status;
            extract.Guid = land.Guid;
            extract.Name = $"{extract.ExtractNumber} {extract.DateUpload?.ToString("d")}";

            extract.SetRecordInfo(land.Land_record.Record_info);
            extract.SetObjectCommonData(uow, land.Land_record?.Object);
        }
        private static void SetObjectCommonData(
          this ExtractLand extract
         , IUnitOfWork uow
         , SibRosReestr.EGRN.Unknown.ObjectLandAndWithdrawal obj
      )
        {
            if (obj == null) return;
            extract.SubtypeCode = obj.Subtype?.Code;
            extract.SubtypeName = obj.Subtype?.Value;
            extract.SubtypeCode = $"{extract.SubtypeCode} {extract.SubtypeName}";
            if (obj.Date_removed_cad_account != null
                && obj.Date_removed_cad_account.Withdrawal_state_munic_needs != null
                && obj.Date_removed_cad_account.Withdrawal_state_munic_needs.Decision_attribute != null)
                extract.Date_removed_cad_account =
                    DocExtentions.CreateRemoveAccountDocument(uow, obj.Date_removed_cad_account.Withdrawal_state_munic_needs.Decision_attribute);
            extract.Reg_date_by_doc = ImportHelper.GetDate(obj.Reg_date_by_doc);
            extract.SetCommonData(obj.Common_data);
        }

        private static void SetLandParams(
           this ExtractLand extract
         , SibRosReestr.EGRN.Unknown.ParamsLandCategoryUses obj
     )
        {
            if (obj == null) return;
            if (obj.Category != null)
            {
                extract.CategoryCode = obj.Category.Type?.Code;
                extract.CategoryName = obj.Category.Type?.Value;
                extract.Category = $"{extract.CategoryCode} {extract.CategoryName}";
            }
            if (obj.Permitted_use != null)
            {
                extract.CategoryCode = obj.Permitted_use.Permitted_use_established?.Land_use?.Code;
                extract.CategoryName = obj.Permitted_use.Permitted_use_established?.Land_use?.Value;
                extract.Category = $"{extract.PermittedLand_useCode} {extract.PermittedLand_useName}";
                extract.CategoryCode = obj.Permitted_use.Permitted_use_established?.Land_use_mer?.Code;
                extract.CategoryName = obj.Permitted_use.Permitted_use_established?.Land_use_mer?.Value;
                extract.Category = $"{extract.PermittedLand_use_merCode} {extract.PermittedLand_use_merName}";
                extract.PermittedBy_document = obj.Permitted_use.Permitted_use_established?.By_document;
                extract.Permitted_usesStr = extract.PermittedLand_use_mer;
            }

            if (obj.Permittes_uses_grad_reg != null)
            {
                extract.Permittes_Grad_Land_useCode = obj.Permittes_uses_grad_reg.Land_use?.Code;
                extract.Permittes_Grad_Land_useName = obj.Permittes_uses_grad_reg.Land_use?.Value;
                extract.Permittes_Grad_Land_use = $"{extract.Permittes_Grad_Land_useCode} {extract.Permittes_Grad_Land_useName}";
                extract.Permittes_Grad_Reg_numb_border = obj.Permittes_uses_grad_reg.Reg_numb_border;
                extract.Permittes_Grad_use_text = obj.Permittes_uses_grad_reg.Permitted_use_text;
                extract.Permitted_usesStr = extract.Permittes_Grad_Land_use;
            }
            if (obj.Area != null)
            {
                extract.Area = obj.Area.Value;
                extract.Inaccuracy = obj.Area.Inaccuracy;
            }

        }
    }
}

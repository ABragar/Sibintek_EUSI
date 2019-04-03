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
    /// Предоставляет методы расширения для импорта объекта выписки на ОНИ - земля.
    /// </summary>
    public static class LandExtentions
    {
        public static void Import(
          this LandRecord br
        , SibRosReestr.EGRN.Unknown.ExtractBaseParamsLand obj
        , ref ImportHolder holder
        )
        {
            try
            {
                if (obj == null) return;             

                br.SetRecordInfo(obj.Land_record?.Record_info);
                br.SetLandCommonData(holder.UnitOfWork, obj.Land_record?.Object);

                br.SetLandParams(obj.Land_record?.Params);
                br.SetLandCadLinks(holder.UnitOfWork, obj.Land_record?.Cad_links);
                br.SetAddress(obj.Land_record?.Address_location);
                br.SetParts(holder.UnitOfWork, obj.Land_record?.Object_parts);
                br.SetContours(holder.UnitOfWork, obj.Land_record?.Contours_location?.Contours);
                br.SetOwnerless(obj.Ownerless_right_record);               

            }
            catch (Exception ex)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }


        private static void SetLandCommonData(
            this LandRecord br
          , IUnitOfWork uow
          , SibRosReestr.EGRN.Unknown.ObjectLandAndWithdrawal obj       
        )
        {
            if (obj == null) return;
            br.SubtypeCode = obj.Subtype?.Code;
            br.SubtypeName = obj.Subtype?.Value;
            br.SubtypeCode = $"{br.SubtypeCode} {br.SubtypeName}";
            if (obj.Date_removed_cad_account != null
                && obj.Date_removed_cad_account.Withdrawal_state_munic_needs != null
                && obj.Date_removed_cad_account.Withdrawal_state_munic_needs.Decision_attribute != null)
                br.Date_removed_cad_account = DocExtentions.CreateRemoveAccountDocument(uow, obj.Date_removed_cad_account.Withdrawal_state_munic_needs.Decision_attribute);

            br.Reg_date_by_doc = ImportHelper.GetDate(obj.Reg_date_by_doc);
            if (obj.Common_data != null)
            {
                br.CadastralNumber = obj.Common_data.Cad_number;
                br.Quarter_cad_number = obj.Common_data.Quarter_cad_number;
                br.TypeCode = obj.Common_data.Type?.Code;
                br.TypeValue = obj.Common_data.Type?.Value;
                br.TypeStr = $"{br.TypeCode} {br.TypeValue}";
            }
        }

        private static void SetLandParams(
             this LandRecord br        
           , SibRosReestr.EGRN.Unknown.ParamsLandCategoryUses obj      
       )
        {
            if (obj == null) return;
            if (obj.Category != null)
            {
                br.CategoryCode = obj.Category.Type?.Code;
                br.CategoryName = obj.Category.Type?.Value;
                br.Category = $"{br.CategoryCode} {br.CategoryName}";
            }
            if (obj.Permitted_use != null)
            {
                br.CategoryCode = obj.Permitted_use.Permitted_use_established?.Land_use?.Code;
                br.CategoryName = obj.Permitted_use.Permitted_use_established?.Land_use?.Value;
                br.Category = $"{br.PermittedLand_useCode} {br.PermittedLand_useName}";
                br.CategoryCode = obj.Permitted_use.Permitted_use_established?.Land_use_mer?.Code;
                br.CategoryName = obj.Permitted_use.Permitted_use_established?.Land_use_mer?.Value;
                br.Category = $"{br.PermittedLand_use_merCode} {br.PermittedLand_use_merName}";
                br.PermittedBy_document = obj.Permitted_use.Permitted_use_established?.By_document;
                br.Permitted_usesStr = br.PermittedLand_use_mer;
            }

            if (obj.Permittes_uses_grad_reg != null)
            {
                br.Permittes_Grad_Land_useCode = obj.Permittes_uses_grad_reg.Land_use?.Code;
                br.Permittes_Grad_Land_useName = obj.Permittes_uses_grad_reg.Land_use?.Value;
                br.Permittes_Grad_Land_use = $"{br.Permittes_Grad_Land_useCode} {br.Permittes_Grad_Land_useName}";
                br.Permittes_Grad_Reg_numb_border = obj.Permittes_uses_grad_reg.Reg_numb_border;
                br.Permittes_Grad_use_text = obj.Permittes_uses_grad_reg.Permitted_use_text;
                br.Permitted_usesStr = br.Permittes_Grad_Land_use;
            }
            if (obj.Area != null)
            {
                br.Area = obj.Area.Value;
                br.Inaccuracy = obj.Area.Inaccuracy;
            }

        }

        private static void SetLandCadLinks(
             this LandRecord br
            , IUnitOfWork uow
            , SibRosReestr.EGRN.Unknown.CadLinksLandIncludedOld obj          
    )
        {
            if (obj == null) return;
            br.SetRoomCadNumbers(uow, obj.Included_objects);
            br.SetOldNumbers(uow, obj.Old_numbers);
        }
    }
}

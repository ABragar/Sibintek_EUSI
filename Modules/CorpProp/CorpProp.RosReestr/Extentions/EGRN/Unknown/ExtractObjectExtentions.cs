using Base.DAL;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Extentions
{
    /// <summary>
    /// Предоставляет методы расширения для импорта выписки на ОНИ.
    /// </summary>
    public static class ExtractObjectExtentions
    {
        /// <summary>
        /// Устанавливает основные данные выписки на ОНИ.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="build"></param>
        public static void SetInfo(
            this ExtractObject extract
          , SibRosReestr.EGRN.Unknown.ExtractBaseParamsBuild build
          )
        {
            extract.SetDetailState(build.Details_statement);
            extract.SetDetailRequest(build.Details_request);

            extract.RecipientName = build.Recipient_statement;
            extract.Status = build.Status;
            extract.Guid = build.Guid;
            extract.Name = $"{extract.ExtractNumber} {extract.DateUpload?.ToString("d")}";

            extract.SetRecordInfo(build.Build_record?.Record_info);
            extract.SetCommonData(build.Build_record?.Object?.Common_data);
        }

      


        public static void SetDetailState(
            this ExtractObject extract
           , SibRosReestr.EGRN.Unknown.DetailsStatementRealty obj
         )
        {
            if (obj == null) return;
            if (obj.Group_top_requisites != null)
            {
                extract.SenderName = obj.Group_top_requisites.Organ_registr_rights;
                extract.DateUpload = ImportHelper.GetDate(obj.Group_top_requisites.Date_formation);
                extract.ExtractNumber = obj.Group_top_requisites.Registration_number;
            }
            if (obj.Group_lower_requisites != null)
            {
                extract.Appointment = obj.Group_lower_requisites.Full_name_position;
                extract.FIO = obj.Group_lower_requisites.Initials_surname;
            }
        }


        public static void SetDetailRequest(
           this ExtractObject extract
          , SibRosReestr.EGRN.Unknown.DetailsRequest obj)
        {
            if (obj == null) return;
            extract.DateRequest = ImportHelper.GetDate(obj.Date_received_request);
            extract.DateReceipt = ImportHelper.GetDate(obj.Date_receipt_request_reg_authority_rights);
        }


        public static void SetRecordInfo(
           this ExtractObject extract
         , SibRosReestr.EGRN.Unknown.RecordInfo rr
         )
        {
            if (rr == null) return;
            extract.RegistrationDate = ImportHelper.GetDate(rr.Registration_date);
            extract.CancelDate = ImportHelper.GetDate(rr.Cancel_date);
        }


      

        public static void SetCommonData(
           this ExtractObject extract
          , SibRosReestr.EGRN.Unknown.CommonData obj
       )
        {
            if (obj == null) return;

            extract.CadNumber = obj.Cad_number;
            extract.Quarter_cad_number = obj.Quarter_cad_number;
            extract.TypeCode = obj.Type?.Code;
            extract.TypeValue = obj.Type?.Value;
            extract.TypeStr = $"{extract.TypeCode} {extract.TypeValue}";
        }


        public static void SetBuildParams(
          this ExtractObject extract
        , SibRosReestr.EGRN.Unknown.ParamsBuildWithoutMaterials obj
        , IUnitOfWork uow
        )
        {
            if (obj == null) return;
            extract.Area = obj.Area;
            extract.Floors = obj.Floors;
            extract.Underground_floors = obj.Underground_floors;
            extract.PurposeCode = obj.Purpose?.Code;
            extract.PurposeName = obj.Purpose?.Value;
            extract.PurposeStr = $"{extract.PurposeCode} {extract.PurposeName}";
            extract.ObjectName = obj.Name;
            extract.Year_built = obj.Year_built;
            extract.Year_commisioning = obj.Year_commisioning;

        }


        public static void SetOwnerless(
             this ExtractObject extract
           , SibRosReestr.EGRN.Unknown.OwnerlessRightRecordOut ownerless
           )
        {
            if (ownerless == null) return;
            extract.OwnerlessRightRecordRegDate = ImportHelper.GetDate(ownerless.Record_info?.Registration_date);
            extract.Ownerless_right_number = ownerless.Ownerless_right_data?.Ownerless_right_number;
            extract.Authority_name = ownerless.Ownerless_right_data?.Authority_name;
        }




      


    }
}

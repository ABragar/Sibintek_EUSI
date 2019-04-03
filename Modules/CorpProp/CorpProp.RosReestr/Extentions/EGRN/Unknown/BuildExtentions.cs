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
    /// Предоставляет методы расширения для импорта объекта выписки на ОНИ - здание.
    /// </summary>
    public static class BuildExtentions
    {
        public static void Import(
           this BuildRecord br
         , SibRosReestr.EGRN.Unknown.ExtractBaseParamsBuild obj
         , ref ImportHolder holder
         )
        {
            try
            {
                if (obj == null) return;

                br.SetRecordInfo(obj.Build_record?.Record_info);
                br.SetObjectCommonData(obj.Build_record?.Object);
                br.SetBuildParams(obj.Build_record?.Params, holder.UnitOfWork);

                br.SetAddress(obj.Build_record?.Address_location);
                br.SetParts(holder.UnitOfWork, obj.Build_record?.Object_parts);
                br.SetContours(holder.UnitOfWork, obj.Build_record?.Contours);
                br.SetOwnerless(obj.Ownerless_right_record);
                br.SetCadLinks(holder.UnitOfWork, obj.Build_record?.Cad_links);

                br.SetRoomRecords(holder.UnitOfWork, obj.Room_records);
                br.SetParkingRecords(holder.UnitOfWork, obj.Car_parking_space_records);



            }
            catch (Exception ex)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }


    }
}

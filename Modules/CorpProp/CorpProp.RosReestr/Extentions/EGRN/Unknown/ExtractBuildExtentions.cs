using Base.DAL;
using CorpProp.Entities.Law;
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
    /// Предоставляет методы расширения для импорта выписки о здании.
    /// </summary>
    public static class ExtractBuildExtentions
    {
        /// <summary>
        /// Инициация импорта xml-выписки о здании в модуль Росреестра.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="ext"></param>
        /// <param name="holder"></param>
        public static void Import(
              this ExtractBuild extract
            , SibRosReestr.EGRN.Unknown.ExtractBaseParamsBuild build
            , ref ImportHolder holder
            )
        {
            if (build == null) return;

            extract.SetInfo(build);            
            BuildRecord obj = extract.CreateBuild(build, ref holder);
            extract.SetOwnerless(build.Ownerless_right_record);
            obj.CreateRights( build.Right_records, ref holder);
            obj.CreateRestrictRecords(build.Restrict_records, ref holder);            
            extract.CountObjects++;
        }

        /// <summary>
        /// Создает Здание.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="obj"></param>
        /// <param name="holder"></param>
        private static BuildRecord CreateBuild(
             this ExtractBuild extract
            , SibRosReestr.EGRN.Unknown.ExtractBaseParamsBuild obj
            , ref ImportHolder holder
            )
        {
            if (obj == null) return null;

            BuildRecord br = holder.UnitOfWork.GetRepository<BuildRecord>().Create(new BuildRecord());
            br.Extract = extract;
            br.Import(obj, ref holder);

            extract.SetBuildParams(obj.Build_record?.Params, holder.UnitOfWork);            
            extract.Address = br.Address;
            extract.Permitted_usesStr = br.Permitted_usesStr;
            extract.Cost = br.Cost = obj.Build_record?.Cost?.Value;
            extract.Special_notes = br.Special_notes = obj.Build_record?.Special_notes;
            

            return br;
        }
        

    }
}

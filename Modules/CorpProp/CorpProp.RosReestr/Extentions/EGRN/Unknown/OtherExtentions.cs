using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Extentions
{
    /// <summary>
    /// Методы расширения для иных объектов модуля.
    /// </summary>
    public static class OtherExtentions
    {

        public static void SetLocationInBuild(
            this Entities.RoomLocationInBuildPlans cad
          , IUnitOfWork uow
          , List<SibRosReestr.EGRN.Unknown.LevelAll> objs
            )
        {
            if (objs == null || cad == null) return;
            for (int i = 0; i < objs.Count; i++)
            {
                SibRosReestr.EGRN.Unknown.LevelAll level = objs[i];
                if (i == 0)
                {
                    cad.Floor = level.Floor;
                    cad.Floor_typeCode = level.Floor_type?.Code;
                    cad.Floor_typeName = level.Floor_type?.Value;
                    cad.Plan_number = level.Plan_number;
                    cad.Description = level.Description;
                    if (level.Plans != null)
                        cad.SetPlans(uow, level.Plans);
                }
                else
                {
                    Entities.RoomLocationInBuildPlans ks =
                        uow.GetRepository<Entities.RoomLocationInBuildPlans>()
                        .Create(new Entities.RoomLocationInBuildPlans());
                    ks.ObjectCadNumber = cad.ObjectCadNumber;
                    ks.Floor = level.Floor;
                    ks.Floor_typeCode = level.Floor_type?.Code;
                    ks.Floor_typeName = level.Floor_type?.Value;
                    ks.Plan_number = level.Plan_number;
                    ks.Description = level.Description;
                    ks.Extract = cad.Extract;
                    ks.ObjectRecord = cad.ObjectRecord;
                    if (level.Plans != null)
                        ks.SetPlans(uow, level.Plans);
                    
                }
            }
        }

        private static void SetPlans(
             this Entities.RoomLocationInBuildPlans cad
           , IUnitOfWork uow
           , List<SibRosReestr.EGRN.Unknown.Plan> objs          
          )
        {
            if (objs == null) return;
            for (int i = 0; i < objs.Count; i++)
            {
                SibRosReestr.EGRN.Unknown.Plan plan = objs[i];
                if (i == 0)
                {
                    cad.Floor = plan.File_link;
                }
                else
                {
                    Entities.RoomLocationInBuildPlans ks =
                        uow.GetRepository<Entities.RoomLocationInBuildPlans>()
                        .Create(new Entities.RoomLocationInBuildPlans());
                    ks.ObjectCadNumber = cad.ObjectCadNumber;
                    ks.Floor = cad.Floor;
                    ks.Floor_typeCode = cad.Floor_typeCode;
                    ks.Floor_typeName = cad.Floor_typeName;
                    ks.Plan_number = cad.Plan_number;
                    ks.Description = cad.Description;
                    ks.Floor = plan.File_link;
                    ks.Extract = cad.Extract;
                    ks.ObjectRecord = cad.ObjectRecord;                   
                }
            }
        }


        public static void SetParkingPlans(
            this Entities.CarParkingSpaceLocationInBuildPlans cad
          , IUnitOfWork uofw
          , List<SibRosReestr.EGRN.Unknown.Plan> objs       
        )
        {
            if (objs == null) return;
            for (int i = 0; i < objs.Count; i++)
            {
                SibRosReestr.EGRN.Unknown.Plan plan = objs[i];
                if (i == 0)
                {
                    cad.Floor = plan.File_link;
                }
                else
                {
                    Entities.CarParkingSpaceLocationInBuildPlans ks =
                        uofw.GetRepository<Entities.CarParkingSpaceLocationInBuildPlans>()
                        .Create( new Entities.CarParkingSpaceLocationInBuildPlans());
                    ks.ObjectCadNumber = cad.ObjectCadNumber;
                    ks.Floor = cad.Floor;
                    ks.Floor_typeCode = cad.Floor_typeCode;
                    ks.Floor_typeName = cad.Floor_typeName;
                    ks.Plan_number = cad.Plan_number;
                    ks.Description = cad.Description;
                    ks.Floor = plan.File_link;
                    ks.Extract = cad.Extract;
                    ks.ObjectRecord = cad.ObjectRecord;
                    
                }
            }
        }



        
    }
}

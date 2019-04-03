using Base.DAL;
using CorpProp.Entities.Import;
using CorpProp.Helpers.Import.Extentions;
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
    /// Предоставляет методы расширения дл импорта ОНИ из xml-выписки.
    /// </summary>
    public static class ObjectExtentions
    {
        /// <summary>
        /// Номера на поэтажном плане.
        /// </summary>
        /// <param name="oni"></param>
        /// <param name="objs"></param>
        public static void SetFloorPlan(
           this ObjectRecord oni
         , List<string> objs
          )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {
                if (!String.IsNullOrEmpty(oni.FloorPlan_No))
                    oni.FloorPlan_No += "; ";
                oni.FloorPlan_No += item;
            }
        }

        /// <summary>
        /// Создает сведения о частях ОНИ.
        /// </summary>
        /// <param name="oni"></param>
        /// <param name="objs"></param>
        /// <param name="uow"></param>
        public static void CreateParts(
           this ObjectRecord oni
         , List<string> objs
         , IUnitOfWork uow
          )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {
                ObjectPartNumberRestrictions part = 
                    uow.GetRepository<ObjectPartNumberRestrictions>().Create(new ObjectPartNumberRestrictions());
                part.ObjectRecord = oni;
                part.Extract = oni.Extract;
                part.Number = item;
            }
        }


      

        

    }
}

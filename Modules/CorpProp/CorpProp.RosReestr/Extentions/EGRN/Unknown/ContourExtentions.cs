using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Extentions
{
    /// <summary>
    /// Методы расширения для контура.
    /// </summary>
    public static class ContourExtentions
    {
        /// <summary>
        /// Устанавливает координаты.
        /// </summary>      
        public static void SetOrdinates(
              this CorpProp.RosReestr.Entities.ContourOKSOut cont
            , IUnitOfWork uow          
            , List<SibRosReestr.EGRN.Unknown.OrdinateOKSOut> ordinates
          )
        {
            if (ordinates != null)
                for (int j = 0; j < ordinates.Count; j++)
                {
                    SibRosReestr.EGRN.Unknown.OrdinateOKSOut ord = ordinates[j];
                    if (j == 0 && cont != null)
                    {
                        cont.X = ord.X;
                        cont.Y = ord.Y;
                        cont.Z = ord.Z;
                        cont.Ord_nmb = ord.Ord_nmb;
                        cont.Num_geopoint = ord.Num_geopoint;
                        cont.Delta_geopoint = ord.Delta_geopoint;
                        cont.R = ord.R;                       
                    }
                    else
                    {
                        CorpProp.RosReestr.Entities.ContourOKSOut ss =
                            uow.GetRepository<CorpProp.RosReestr.Entities.ContourOKSOut>().Create(
                                new CorpProp.RosReestr.Entities.ContourOKSOut());
                        ss.X = ord.X;
                        ss.Y = ord.Y;
                        ss.Z = ord.Z;
                        ss.Ord_nmb = ord.Ord_nmb;
                        ss.Num_geopoint = ord.Num_geopoint;
                        ss.Delta_geopoint = ord.Delta_geopoint;
                        ss.R = ord.R;
                        ss.Extract = cont.Extract;
                        ss.ObjectRecord = cont.ObjectRecord;                        
                    }
                }
        }

        public static void SetOrdinates(
            this CorpProp.RosReestr.Entities.ContourOKSOut cont
          , IUnitOfWork uow
          , List<SibRosReestr.EGRN.Unknown.OrdinateZUOut> ordinates
        )
        {
            if (ordinates != null)
                for (int j = 0; j < ordinates.Count; j++)
                {
                    SibRosReestr.EGRN.Unknown.OrdinateZUOut ord = ordinates[j];
                    if (j == 0 && cont != null)
                    {
                        cont.X = ord.X;
                        cont.Y = ord.Y;
                        cont.Z = ord.Z;
                        cont.Ord_nmb = ord.Ord_nmb;
                        cont.Num_geopoint = ord.Num_geopoint;
                        cont.Delta_geopoint = ord.Delta_geopoint;                       
                    }
                    else
                    {
                        Entities.ContourOKSOut ss =
                            uow.GetRepository<Entities.ContourOKSOut>()
                            .Create(new Entities.ContourOKSOut());
                        ss.X = ord.X;
                        ss.Y = ord.Y;
                        ss.Z = ord.Z;
                        ss.Ord_nmb = ord.Ord_nmb;
                        ss.Num_geopoint = ord.Num_geopoint;
                        ss.Delta_geopoint = ord.Delta_geopoint;

                        ss.Extract = cont.Extract;
                        ss.ObjectRecord = cont.ObjectRecord;
                        
                    }
                }
        }
    }
}

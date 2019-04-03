using System;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using Base.DAL;
using CorpProp.DefaultData;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;

namespace CorpProp.Analyze.DefaultData
{
    public class FillPolygonsDataStrategy : IFillDataStrategy<PolygonsDataHolder>
    {
        public void FillData(IDefaultDataHelper helper, IUnitOfWork uow, PolygonsDataHolder data)
        {
            if (data == null) return;
            try
            {
                if (data.SibRegionPolygons != null)
                {
                    var repository = uow.GetRepository<SibRegion>();
                    foreach (SibRegion sibRegion in repository.All())
                    {
                        SibRegionPolygon regionPolygon = data.SibRegionPolygons.FirstOrDefault(polygon => polygon.Code.Trim().Equals(sibRegion.Code));
                        if (regionPolygon == null)continue;
                        string pol = regionPolygon.Polygon.Trim();
                        sibRegion.Location.Disposition = DbGeography.FromText(pol);
                        repository.Update(sibRegion);
                    }
                }

                if (data.SibFederalDistrictPolygons != null)
                {
                    var repository = uow.GetRepository<SibFederalDistrict>();
                    foreach (SibFederalDistrict sibFederalDistrict in repository.All())
                    {
                        SibFederalDistrictPolygon sibFederalDistrictPolygon = data.SibFederalDistrictPolygons.FirstOrDefault(polygon => polygon.Name.Trim().Equals(sibFederalDistrict.Name, StringComparison.OrdinalIgnoreCase));
                        if(sibFederalDistrictPolygon == null) continue;
                        string pol = sibFederalDistrictPolygon.Polygon.Trim();
                        sibFederalDistrict.Location.Disposition = DbGeography.FromText(pol);
                        repository.Update(sibFederalDistrict);
                    }
                }

                uow.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }

        public void FillContext(IDefaultDataHelper helper, DbContext context, PolygonsDataHolder data)
        {
            if (data == null) return;
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }
    }
}
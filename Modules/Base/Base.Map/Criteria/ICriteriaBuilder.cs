using Base.Map.Filters;
using Base.Map.Spatial;
using System.Linq;

namespace Base.Map.Criteria
{
    public interface ICriteriaBuilder
    {
        IQueryable BuildFilterWhereClause(IQueryable query, string mnemonic, FilterValues filters);

        IQueryable<IGeoObject> BuildIntersectsBoundsWhereClause(IQueryable<IGeoObject> query, GeoBounds bbox);

        IQueryable<IGeoObject> BuildIntersectsBoundsWhereClause(IQueryable<IGeoObject> query, Bounds bbox);

        IQueryable<IGeoObject> BuildContainsBoundsWhereClause(IQueryable<IGeoObject> query, GeoBounds bbox);

        IQueryable<IGeoObject> BuildContainsBoundsWhereClause(IQueryable<IGeoObject> query, Bounds bbox);

        IQueryable<IGeoObject> BuildOverlapBoundsWhereClause(IQueryable<IGeoObject> query, GeoBounds bbox);

        IQueryable<IGeoObject> BuildOverlapBoundsWhereClause(IQueryable<IGeoObject> query, Bounds bbox);

        IQueryable<IGeoObject> BuildBoundsWhereClause(IQueryable<IGeoObject> query, GeoBounds bbox, BoundsMode mode);

        IQueryable<IGeoObject> BuildBoundsWhereClause(IQueryable<IGeoObject> query, Bounds bbox, BoundsMode mode);

        IQueryable<IGeoObject> BuildAroundOnDistancePointWhereClause(IQueryable<IGeoObject> query, GeoPoint point, int? zoom,
            double? distanceRatioInPixels = null, double? minDistance = null, double? maxDistance = null);

        IQueryable<IGeoObject> BuildLocationNotNullWhereClause(IQueryable<IGeoObject> query);

        IQueryable BuildBaseObjectTypeMnemonicWhereClause(IQueryable query, string mnemonic);

        IQueryable BuildBaseObjectTypeIdWhereClause(IQueryable query, int id);

        IQueryable<IGeoObject> BuildTitleMatchWhereClause(IQueryable<IGeoObject> query, string title);

        IQueryable<IGeoObject> BuildDescriptionMatchWhereClause(IQueryable<IGeoObject> query, string description);

        IQueryable<IGeoObject> BuildSearchStringWhereClause(IQueryable<IGeoObject> query, string searchString);
    }
}
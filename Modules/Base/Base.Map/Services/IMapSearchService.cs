using Base.Map.MapObjects;
using Base.Map.Search;
using System.Collections.Generic;

namespace Base.Map.Services
{
    public interface IMapSearchService
    {
        IEnumerable<GeoObjectBase> Search(SearchParameters searchParameters);
    }
}
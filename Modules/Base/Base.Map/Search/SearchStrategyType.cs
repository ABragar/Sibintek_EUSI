using Base.Attributes;

namespace Base.Map.Search
{
    [UiEnum]
    public enum SearchStrategyType
    {
        Default = 0,
        SearchOnClick = 1,
        ClusteringSearch = 3,
        SearchByCluster = 5,
        SearchByBounds = 6
    }
}
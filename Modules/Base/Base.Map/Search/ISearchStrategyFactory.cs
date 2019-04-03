namespace Base.Map.Search
{
    internal interface ISearchStrategyFactory
    {
        SearchStrategyBase Create(SearchStrategyType searchStrategyType);

        SearchStrategyBase Create(SearchParameters searchParameters);
    }
}
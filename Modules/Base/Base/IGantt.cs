using System;

namespace Base
{
    public interface IGantt: IDateTimeRange
    {
        string Title { get; }
        int OrderId { get; }
        decimal PercentComplete { get; }
    }
}

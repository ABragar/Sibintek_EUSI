using System;

namespace Base.Identity
{
    public interface ITokenOptions
    {
        int MaxTokenCount { get; }
        TimeSpan MaxTokenCountTimeSpan { get; }

        TimeSpan TokenTimeSpan { get; }
    }
}
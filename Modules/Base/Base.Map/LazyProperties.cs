using System;
using System.Collections.Generic;
using Base.Attributes;

namespace Base.Map
{
    public static class LazySettings
    {
        public const string StartDisposition = "START_DISPOSITION";
    }

    [UiEnum]
    public enum LazyProperties
    {

        [UiEnumValue("Нет")]
        None = 0,
        [UiEnumValue("При наведении")]
        OnHover = 10,
        [UiEnumValue("При выборе")]
        OnSelect = 20
    }

    public interface ILazyProperty : IDictionary<string, object>
    {
        
    }

    public class LazyPropery : Dictionary<string, object>, ILazyProperty
    {
        
    }
}
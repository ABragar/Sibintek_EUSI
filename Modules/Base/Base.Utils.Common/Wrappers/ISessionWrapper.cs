﻿
namespace Base.Utils.Common.Wrappers
{
    public interface ISessionWrapper
    {
        object this[string name] { get; set; }
        void Remove(string key);
        bool HasEntry { get; }
    }
}

using System;

namespace Base
{
    public interface IBaseObject
    {
        int ID { get; set; }
        bool Hidden { get; set; }
        double SortOrder { get; set; }
        byte[] RowVersion { get; set; }
    }
}
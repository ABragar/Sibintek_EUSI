using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.QueryBuilderFilter
{
    /// <summary>
    /// Types of jquery query builder (don't change)
    /// </summary>
    public enum QueryBuilderType
    {       
        STRING,
        INTEGER,
        DOUBLE,
        DATE,
        TIME,
        DATETIME,
        BOOLEAN
    }
}
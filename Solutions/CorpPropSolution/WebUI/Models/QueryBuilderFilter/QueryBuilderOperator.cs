using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.QueryBuilderFilter
{
    public enum QueryBuilderOperator
    {
        equal,
        not_equal,
        less,
        less_or_equal,
        greater,
        greater_or_equal,
        contains,
        not_contains,
        is_null,
        is_not_null,
        any,
        @in,
        between,
        not_between
    }
}
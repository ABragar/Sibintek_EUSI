using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.QueryBuilderFilter
{
    public class QueryFilterDataVm
    {
        /// <summary>
        /// real type of property
        /// </summary>
        public string system_type { get; set; }

        /// <summary>
        /// id of html editor
        /// not used on server
        /// </summary>
        public string editor_id { get; set; }

        /// <summary>
        /// other info for type
        /// </summary>
        public string additional_info { get; set; }
    }
}
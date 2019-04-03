using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.QueryBuilderFilter
{
    public class QueryFilterVm
    {
        public string id { get; set; }
        public string label { get; set; }
        public string type { get; set; }        
        /// <summary>
        /// Свойство 'data' в filter
        /// </summary>
        public QueryFilterDataVm data { get; set; }
        public List<string> operators { get; set; }
    }
}
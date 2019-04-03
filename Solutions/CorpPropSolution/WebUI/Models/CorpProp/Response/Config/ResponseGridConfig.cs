using System.Collections.Generic;

namespace WebUI.Models.CorpProp.Response.Config
{
    public class ResponseGridConfig
    {
        public int? RowsCount { get; set; }

        public IList<ResponseGridConfigProperty> Columns { get; set; }

    }
}
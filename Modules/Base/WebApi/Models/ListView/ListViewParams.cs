using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models.ListView
{
    public class ListViewParams
    {
        public string SearchStr { get; set; }
        public int? MnemonicFilterId { get; set; }
        public string Extrafilter { get; set; }

        //sib
        public string Date { get; set; }

        public string CustomParams { get; set; }

        //end sib
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Base.UI.ViewModal;

namespace WebUI.Models.Links
{
    public class LinkMapItemModel
    {
        public string Mnemonic { get; set; }
        public string ID { get; set; }
        public string Title { get; set; }

        public int RealID { get; set; }
        public bool Hidden { get; set; }        
    }

    public class LinkConnection
    {
        public int ID { get; set; }
        public string FromObject { get; set; }
        public string ToObject { get; set; }
    }
}
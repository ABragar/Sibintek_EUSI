using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.Print
{
    public class PrintDocumentContentViewModel
    {
        public int Id { get; set; }
        public string Mnemonic { get; set; }
        public int TemplateId { get; set; }
    }
}
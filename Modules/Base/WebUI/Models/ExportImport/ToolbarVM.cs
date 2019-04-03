using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.ExportImport
{
    public class ExportImportToolbarVM
    {
        public ExportImportToolbarVM(string mnemonic)
        {
            Mnemonic = mnemonic;            
        }

        public string Mnemonic { get; set; }

    }    
}
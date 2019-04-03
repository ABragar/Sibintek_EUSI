using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Base;
using Base.UI.ViewModal;

namespace WebUI.Models.Links
{
    public class LinksToolbarVm
    {
        public List<LinksToolbarItem> CanCreate
        {
            get;
            set;
        }

        public string CurrentItemID { get; set; }
    }

    public class LinksToolbarItem
    {
        public string Title { get; set; }
        public string Mnemonic { get; set; }

        public LinksToolbarItem(string title, string mnemonic)
        {
            Title = title;
            Mnemonic = mnemonic;

        }
    }



}
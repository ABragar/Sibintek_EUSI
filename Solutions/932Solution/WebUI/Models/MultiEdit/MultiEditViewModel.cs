using System.Collections.Generic;
using Base.UI;

namespace WebUI.Models.MultiEdit
{
    public class MultiEditViewModel
    {
        public string Property { get; set; }
        public IReadOnlyCollection<TabVm> Tabs { get; set; }
    }
}
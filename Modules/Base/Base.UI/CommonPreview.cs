using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Base.UI.Helpers;
using Base.UI.ViewModal;

namespace Base.UI
{
    public class CommonPreview
    {
        private List<TabPreview> _tabs;

        public CommonPreview(ViewModelConfig viewModelConfig, IEnumerable<PreviewField> fields)
        {
            UID = UIHelper.CreateSystemName("preview");

            ViewModelConfig = viewModelConfig;

            Fields = fields.ToList();
        }

        public string UID { get; private set; }
        public ViewModelConfig ViewModelConfig { get; private set; }
        public List<PreviewField> Fields { get; private set; }

        public List<TabPreview> Tabs
        {
            get
            {
                return _tabs ?? (_tabs = Fields
                    .Where(e => e.Visible)
                    .OrderBy(e => e.SortOrder)
                    .GroupBy(x => x.TabName).OrderBy(x => x.Key)
                    .Select(x => new TabPreview(x.Key, fields: x)).ToList());
            }
        }
    }

    //TODO: Remove regexp [0] [1] for TAB names
    public class TabPreview
    {
        const string regex = "(\\[.*\\])|(\".*\")|('.*')|(\\(.*\\))";

        public TabPreview(string tabName, IGrouping<string, PreviewField> fields)
        {
            UID = UIHelper.CreateSystemName("tab");
            Name = Regex.Replace(tabName, regex, "");
            TabName = tabName;
            Fields = fields;
        }

        public string UID { get; private set; }
        public string TabName { get; private set; }
        public string Name { get; private set; }
        public IGrouping<string, PreviewField> Fields { get; private set; }
    }
}
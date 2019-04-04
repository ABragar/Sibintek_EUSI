using System;
using Base;
using Base.BusinessProcesses.Entities;
using Base.UI;
using Base.UI.ViewModal;

namespace WebUI.Models.BusinessProcess
{
    public class TaskToolbarViewModel
    {
        public int TaskID { get; set; }
        public string InfoString { get; set; }
        public string Title { get; set; }
        public string Mnemonic { get; set; }
        public int ObjectID { get; set; }
        public bool DisableStatus { get; set; }
        public bool IconOnly { get; set; }
        public ViewModelConfig Config { get; set; }

        public TaskToolbarViewModel(ViewModelConfig config, BaseObject baseObject)
        {
            Mnemonic = config.Mnemonic;
            Config = config;
            Title = config.Title;
            ObjectID = baseObject.ID;
            InfoString = string.Format("{0}: {1}", Title,
                baseObject.GetType().GetProperty(config.LookupProperty.Text).GetValue(baseObject));
        }
    }
}
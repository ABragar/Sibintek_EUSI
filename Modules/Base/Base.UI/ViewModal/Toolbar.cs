using System;
using Base.Entities.Complex;
using Base.Extensions;

namespace Base.UI.ViewModal
{
    public class Toolbar: BaseObject
    {
        public Guid ToolbarID { get; private set; }
        public string Title { get; set; }
        public AjaxAction AjaxAction { get; set; }
        public Icon EdtIcon { get; set; }
        public string Icon => EdtIcon.Value;
        public bool IsAjax { get; set; }
        public string Url { get; set; }
        public bool IsMaximize { get; set; }
        public bool OnlyForSelected { get; set; }

        public Toolbar()
        {
            EdtIcon = new Icon();
            ToolbarID = Guid.NewGuid();
            OnlyForSelected = true;
        }

        public Toolbar Copy()
        {
            return new Toolbar()
            {
                Title = this.Title,
                AjaxAction = this.AjaxAction.DeepCopy(),
                EdtIcon = this.EdtIcon.DeepCopy(),
                IsAjax = this.IsAjax,
                Url = this.Url,
                IsMaximize = this.IsMaximize,
                OnlyForSelected = this.OnlyForSelected
            };
        }
    }
}
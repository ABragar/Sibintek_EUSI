using System;
using Base.Attributes;

namespace Base.UI.ViewModal
{
    [Serializable]
    public class AjaxActionParam : BaseObject
    {
        private static readonly Random Rnd = new Random();

        public AjaxActionParam()
        {
            ID = Rnd.Next(0, 99999);
        }

        [DetailView("Key"), ListView]
        public string Key { get; set; }
        [DetailView("Value"), ListView]
        public string Value { get; set; }
    }
}
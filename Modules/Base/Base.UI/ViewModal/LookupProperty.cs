using System;

namespace Base.UI.ViewModal
{
    [Serializable]
    public class LookupProperty
    {
        public string Text { get; set; }
        public string Image { get; set; }
        public string Icon { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}

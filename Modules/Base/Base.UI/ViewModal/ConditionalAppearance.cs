using Base.Entities.Complex;
using Base.Extensions;

namespace Base.UI.ViewModal
{
    public class ConditionalAppearance
    {
        public ConditionalAppearance()
        {
            BtnIcon = new Icon();
        }

        public string Condition { get; set; }
        public Icon BtnIcon { get; set; }
        public string Color => BtnIcon.Color;
        public string Icon => BtnIcon.Value;
        public string Backgound { get; set; }

        public ConditionalAppearance Copy()
        {
            return new ConditionalAppearance()
            {
                Condition = this.Condition,
                BtnIcon = this.BtnIcon.DeepCopy(),
                Backgound = this.Backgound
            };
        }
    }
}
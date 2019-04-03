namespace Base.UI.ViewModal
{
    public class PreviewField : PropertyViewModel
    {
        public PreviewField() : base()
        {
        }

        public PreviewField(string name) : base(name)
        {
        }

        public string TabName { get; set; }

        public override T Copy<T>(T propertyView = null)
        {
            var pr = propertyView as PreviewField ?? new PreviewField();

            pr.TabName = TabName;

            return base.Copy(pr as T);
        }
    }
}
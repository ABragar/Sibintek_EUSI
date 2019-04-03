namespace WordTemplates
{
    public struct TemplateValue
    {
        private readonly string _title;
        private readonly string _url;

        public TemplateValue(string title, string url)
        {
            _title = title;
            _url = url;
        }

        public string Title => _title ?? _url ?? "";

        public string Url => _url;

        public static implicit operator TemplateValue(string val)
        {
            return new TemplateValue(val,null);
        }
    }
}
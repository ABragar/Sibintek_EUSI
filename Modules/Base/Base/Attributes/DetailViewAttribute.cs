using System;

namespace Base.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DetailViewAttribute : Attribute
    {
        private bool? _visible;
        private int? _width;
        private int? _height;

        public DetailViewAttribute() { }

        public DetailViewAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TabName { get; set; }

        public string Group { get; set; }

        public bool Visible
        {
            get { return _visible ?? true; }
            set { _visible = value; }
        }

        public bool ReadOnly { get; set; }

        public bool Required { get; set; }

        public int Order { get; set; }

        public bool HideLabel { get; set; }

        public string BgColor { get; set; }
    }
}

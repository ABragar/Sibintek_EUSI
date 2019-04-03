namespace Base.Map.MapObjects
{
    public class MapLayerStyle
    {
        public const double DefaultOpacity = 0.2;
        public const bool DefaultShowIcon = true;
        public const double DefaultBorderOpacity = 0.5;
        public const int DefaultBorderWidth = 5;

        #region Icon

        public string Icon { get; set; }
        public string Color { get; set; }

        #endregion Icon

        #region Background

        public string Background { get; set; }
        public double Opacity { get; set; }

        #endregion Background

        #region Border

        public string BorderColor { get; set; }
        public double BorderOpacity { get; set; }
        public int BorderWidth { get; set; }

        #endregion Border

        public bool ShowIcon { get; set; }
    }
}
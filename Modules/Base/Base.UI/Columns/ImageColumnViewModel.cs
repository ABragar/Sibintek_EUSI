using Base.Enums;

namespace Base.UI
{
    public class ImageColumnViewModel : ColumnViewModel
    {
        public ImageColumnViewModel() : base() { }
        public ImageColumnViewModel(string name) : base(name) { }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public bool Circle { get; set; }
        public DefaultImage DefaultImage { get; set; }

        public override T Copy<T>(T propertyView = null)
        {
            var col = propertyView as ImageColumnViewModel ?? new ImageColumnViewModel();
            col.ImageWidth = ImageWidth;
            col.ImageHeight = ImageHeight;
            col.Circle = Circle;
            col.DefaultImage = DefaultImage;
            return base.Copy(col as T);
        }
    }
}

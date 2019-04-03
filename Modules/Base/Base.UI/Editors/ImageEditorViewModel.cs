using System;
using Base.Enums;

namespace Base.UI
{
    [Serializable]
    public class ImageEditorViewModel : EditorViewModel
    {
        public ImageEditorViewModel() : base()
        {
        }

        public ImageEditorViewModel(string name) : base(name)
        {
        }

        public bool Crop { get; set; }
        public bool SelectFileStorage { get; set; }
        public bool Upload { get; set; }
        public bool Circle { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public DefaultImage DefaultImage { get; set; }

        public override T Copy<T>(T propertyView = null)
        {
            var editor = propertyView as ImageEditorViewModel ?? new ImageEditorViewModel();

            editor.Crop = this.Crop;
            editor.SelectFileStorage = this.SelectFileStorage;
            editor.Upload = this.Upload;
            editor.Circle = this.Circle;
            editor.Width = this.Width;
            editor.Height = this.Height;
            editor.DefaultImage = this.DefaultImage;

            return base.Copy(editor as T);
        }
    }
}
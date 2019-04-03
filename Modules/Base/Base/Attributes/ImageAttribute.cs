using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Enums;

namespace Base.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ImageAttribute : Attribute
    {
        private bool? _crop;
        private bool? _upload;
        private bool? _selectFileStorage;
        private bool? _circle;
        private int? _width;
        private int? _height;
        private int? _widthForListView;
        private int? _heightForListView;

        public bool Crop
        {
            get { return _crop ?? true; }
            set { _crop = value; }
        }

        public bool Upload
        {
            get { return _upload ?? true; }
            set { _upload = value; }
        }

        public bool SelectFileStorage
        {
            get { return _selectFileStorage ?? true; }
            set { _selectFileStorage = value; }
        }

        public bool Circle
        {
            get { return _circle ?? false; }
            set { _circle = value; }
        }

        /// <summary>
        /// Width of image. Value by defoult is 200
        /// </summary>
        public int Width
        {
            get { return _width ?? 200; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height ?? 200; }
            set { _height = value; }
        }

        public int WidthForListView
        {
            get { return _widthForListView ?? 48; }
            set { _widthForListView = value; }
        }

        public int HeightForListView
        {
            get { return _heightForListView ?? 48; }
            set { _heightForListView = value; }
        }

        public DefaultImage DefaultImage { get; set; } = DefaultImage.NoImage;
    }
}

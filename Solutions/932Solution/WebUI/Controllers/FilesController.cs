using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Base.DAL;
using Base.Service;
using Base.UI.Enums;
using Base.UI.Service;
using Base.Utils.Common.Caching;
using ImageResizer;

namespace WebUI.Controllers
{
    public class FilesController : Controller
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ISimpleCacheWrapper _cacheWrapper;
        private readonly IImageSettingService _imageSettingService;

        public FilesController(IFileSystemService fileSystemService, IUnitOfWorkFactory unitOfWorkFactory, ISimpleCacheWrapper cacheWrapper, IImageSettingService imageSettingService)
        {
            _fileSystemService = fileSystemService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _cacheWrapper = cacheWrapper;
            _imageSettingService = imageSettingService;
        }

        private static CacheAccessor<FileResult> GetFileGroup = new CacheAccessor<FileResult>(TimeSpan.FromHours(1));
        public FileResult GetFile(Guid fileid)
        {
            return _cacheWrapper.GetOrAdd(GetFileGroup, fileid.ToString(), () =>
            {

                using (var uofw = _unitOfWorkFactory.CreateSystem())
                {
                    var fileData = _fileSystemService.GetFileData(uofw, fileid);

                    if (fileData == null) return null;

                    string path = _fileSystemService.GetFilePath(fileid);

                    byte[] result;

                    if (!System.IO.File.Exists(path)) return null;

                    using (var sourceStream = System.IO.File.Open(path, FileMode.Open))
                    {
                        result = new byte[sourceStream.Length];
                        sourceStream.Read(result, 0, (int)sourceStream.Length);
                    }

                    return File(result, MimeMapping.GetMimeMapping(fileData.FileName),
                        fileData.FileName);
                }

            });
        }

        private static readonly CacheAccessor<FileResult> GetImageGroup = new CacheAccessor<FileResult>(TimeSpan.FromDays(5));

        private const int MAX_IMAGE_WIDTH = 3200;      

        [HttpGet, OutputCache(Duration = int.MaxValue, VaryByParam = "*", Location = OutputCacheLocation.Client)]
        public FileResult GetImage(Guid? id, int? width, int? height, string defImage = "", string type = "", string scale = "", string anchor = "")
        {
            var size = width > height ? width : height;
            ImageSize imageSize = _imageSettingService.GetClosestSize(size);           
            return this.GetImageThumbnail(id, imageSize, defImage, type, scale, anchor);
        }

        [HttpGet, OutputCache(Duration = int.MaxValue, VaryByParam = "*", Location = OutputCacheLocation.Client)]
        public FileResult GetImageThumbnail(Guid? id, ImageSize size, string defImage = "", string type = "",
            string scale = "", string anchor = "")
        {
            Guid imageID = id.GetValueOrDefault();

            string key = $"[{imageID}][{size}][{defImage}][{type}][{scale}][{anchor}]";

            return _cacheWrapper.GetOrAdd(GetImageGroup, key, () =>
            {
                using (var ms = new MemoryStream())
                {
                    var instructions = new NameValueCollection
                    {
                        {"mode", ImageMode.Resolve(type)},
                        {"anchor", ImageAnchor.Resolve(anchor)},
                        {"ignoreicc", "true"}
                    };

                    instructions.Add("scale", ImageScale.Resolve(scale));
                    string path = _fileSystemService.GetFilePath(imageID);
                    var defImageObject = defImage.ToLower() == "nophoto"
                        ? Properties.Resources.NoPhoto
                        : Properties.Resources.NoImage;

                    var mainParam = "width";
                    if (System.IO.File.Exists(path))
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromFile(path);
                        mainParam = img.Width > img.Height ? "width" : "height";                        
                    }
                    string sizeParam = _imageSettingService.GetImageSizeValue(size).ToString();
                    instructions.Add(mainParam, sizeParam);
                    var imageJob = id.HasValue && System.IO.File.Exists(path)
                        ? new ImageJob(path, ms, new Instructions(instructions))
                        : new ImageJob(defImageObject, ms, new Instructions(instructions));
                    ImageBuilder.Current.Build(imageJob);
                    return File(ms.ToArray(), imageJob.ResultMimeType);
                }
            });
        }


        [HttpGet, OutputCache(Duration = int.MaxValue, VaryByParam = "*", Location = OutputCacheLocation.Client)]
        public FileResult GetImageFromDocx(Guid? id, string imgIndex, int? width, int? height, string defImage = "", string type = "", string scale = "", string anchor = "")
        {
            Guid imageID = id.GetValueOrDefault();/////            
            string key = $"[{imageID}][{imgIndex}][{width ?? 0}][{height ?? 0}][{defImage}][{type}][{scale}][{anchor}]";

            return _cacheWrapper.GetOrAdd(GetImageGroup, key, () =>
            {

                using (var ms = new MemoryStream())
                {
                    var instructions = new NameValueCollection
                    {
                        {"mode", ImageMode.Resolve(type)},
                        {"anchor", ImageAnchor.Resolve(anchor)},
                        {"ignoreicc", "true"}
                    };

                    if (!width.HasValue && !height.HasValue)
                    {
                        instructions.Add("width", MAX_IMAGE_WIDTH.ToString());
                        instructions.Add("scale", ImageScale.DownScaleOnly);
                    }
                    else
                    {
                        instructions.Add("scale", ImageScale.Resolve(scale));

                        if (width.HasValue)
                            instructions.Add("width", width.ToString());

                        if (height.HasValue)
                            instructions.Add("height", height.ToString());
                    }

                    StringBuilder path = new StringBuilder();
                    path.Append(_fileSystemService.GetFilePath(imageID));
                    path.Append("_");
                    path.Append(imgIndex);

                    var defImageObject = defImage.ToLower() == "nophoto"
                        ? Properties.Resources.NoPhoto
                        : Properties.Resources.NoImage;

                    var imageJob = id.HasValue && System.IO.File.Exists(path.ToString())
                        ? new ImageJob(path.ToString(), ms, new Instructions(instructions))
                        : new ImageJob(defImageObject, ms, new Instructions(instructions));

                    ImageBuilder.Current.Build(imageJob);

                    return File(ms.ToArray(), imageJob.ResultMimeType);

                }
            });

        }
    }

    public static class ImageScale
    {
        private const string _default = "both";

        public const string Both = "both";
        public const string UpScaleOnly = "upscaleonly";
        public const string DownScaleOnly = "downscaleonly";
        public const string UpScaleCanvas = "upscalecanvas";

        public static string Default => _default;
        public static string Resolve(string scale)
        {
            if (string.IsNullOrEmpty(scale))
            {
                return _default;
            }

            switch (scale.ToLower())
            {
                case Both:
                case UpScaleOnly:
                case DownScaleOnly:
                case UpScaleCanvas:
                    return scale;
                case "prevent":
                    return DownScaleOnly;
                default:
                    return _default;
            }
        }
    }

    public static class ImageMode
    {
        private const string _default = Crop;

        public const string Max = "max";
        public const string Pad = "pad";
        public const string Crop = "crop";
        public const string Carve = "carve";
        public const string Stretch = "stretch";

        public static string Default => _default;
        public static string Resolve(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return _default;
            }

            switch (type.ToLower())
            {
                case Max:
                case Crop:
                case Carve:
                case Stretch:
                    return type;
                case Pad:
                case "frame":
                case "contain":
                    return Pad;
                default:
                    return _default;
            }
        }
    }

    public static class ImageAnchor
    {
        private const string _default = MiddleCenter;

        public const string TopLeft = "topleft";
        public const string TopCenter = "topcenter";
        public const string TopRight = "topright";
        public const string MiddleLeft = "middleleft";
        public const string MiddleCenter = "middlecenter";
        public const string MiddleRight = "middleright";
        public const string BottomLeft = "bottomleft";
        public const string BottomCenter = "bottomcenter";
        public const string BottomRight = "bottomright";

        public static string Default = _default;

        public static string Resolve(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return _default;
            }

            switch (type.ToLower())
            {
                case TopLeft:
                case TopCenter:
                case TopRight:
                case MiddleLeft:
                case MiddleCenter:
                case MiddleRight:
                case BottomLeft:
                case BottomCenter:
                case BottomRight:
                    return type;
                default:
                    return _default;
            }
        }
    }
}
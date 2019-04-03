using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Base.Extensions;
using Base.Multimedia.Models;
using Base.Multimedia.Service;
using Base.Service;

namespace WebUI.Controllers
{
    public class MultimediaController : BaseController
    {
        private readonly IMultimediaObjectService _multimediaObjectService;
        private readonly IFileSystemService _fileSystemService;


        public MultimediaController(IBaseControllerServiceFacade serviceFacade, IFileSystemService fileSystemService, IMultimediaObjectService multimediaObjectService) : base(serviceFacade)
        {
            _fileSystemService = fileSystemService;
            _multimediaObjectService = multimediaObjectService;
        }

        public async Task GetMedia(int id)
        {
            var filePath = "";
            var type = MultimediaType.Unknown;
            string fileExtension;

            using (var uofw = CreateSystemUnitOfWork())
            {
                var multimedia = await _multimediaObjectService.GetAll(uofw).Where(x => x.ID == id).FirstOrDefaultAsync();

                if (multimedia?.SourceFiles == null || !multimedia.SourceFiles.Any()) return;

                var file = multimedia.Type == MultimediaType.Audio ?
                    multimedia.SourceFiles.FirstOrDefault(x => x.Object.FileName.EndsWith(".wav") || x.Object.FileName.EndsWith(".ogg")) :
                    multimedia.SourceFiles.FirstOrDefault(x => x.Object.FileName.EndsWith(".webm"));

                if (file == null) return;

                type = multimedia.Type;
                fileExtension = Path.GetExtension(file.Object.FileName).Replace(".", "");
                filePath = _fileSystemService.GetFilePath(file.Object.FileID);
            }

            if (!System.IO.File.Exists(filePath)) return;

            long size, start, end, length, fp = 0;

            switch (type)
            {
                case MultimediaType.Video:
                    HttpContext.Response.ContentType = "video/webm";
                    break;
                case MultimediaType.Audio:
                    HttpContext.Response.ContentType = "audio/" + fileExtension;
                    break;
            }

            using (var reader = new StreamReader(filePath))
            {
                size = reader.BaseStream.Length;
                start = 0;
                end = size - 1;
                length = size;

                HttpContext.Response.AddHeader("Accept-Ranges", "0-" + size);

                if (!string.IsNullOrEmpty(HttpContext.Request.ServerVariables["HTTP_RANGE"]))
                {
                    var anotherStart = start;
                    var anotherEnd = end;
                    var arrSplit = HttpContext.Request.ServerVariables["HTTP_RANGE"].Split(Convert.ToChar("="));
                    var range = arrSplit[1];

                    if (range.IndexOf(",", StringComparison.Ordinal) > -1)
                    {
                        HttpContext.Response.AddHeader("Content-Range", "bytes " + start + "-" + end + "/" + size);
                        throw new HttpException(416, "Requested Range Not Satisfiable");
                    }

                    if (range.StartsWith("-"))
                    {
                        anotherStart = size - Convert.ToInt64(range.Substring(1));
                    }
                    else
                    {
                        arrSplit = range.Split(Convert.ToChar("-"));
                        anotherStart = Convert.ToInt64(arrSplit[0]);
                        long temp = 0;
                        anotherEnd = (arrSplit.Length > 1 && long.TryParse(arrSplit[1], out temp)) ? Convert.ToInt64(arrSplit[1]) : size;
                    }

                    anotherEnd = (anotherEnd > end) ? end : anotherEnd;

                    if (anotherStart > anotherEnd || anotherStart > size - 1 || anotherEnd >= size)
                    {
                        HttpContext.Response.AddHeader("Content-Range", "bytes " + start + "-" + end + "/" + size);
                        throw new HttpException(416, "Requested Range Not Satisfiable");
                    }

                    start = anotherStart;
                    end = anotherEnd;

                    length = end - start + 1; // Calculate new content length
                    fp = reader.BaseStream.Seek(start, SeekOrigin.Begin);
                    HttpContext.Response.StatusCode = 206;
                }
            }

            HttpContext.Response.AddHeader("Content-Range", "bytes " + start + "-" + end + "/" + size);
            HttpContext.Response.AddHeader("Content-Length", length.ToString());

            HttpContext.Response.WriteFile(filePath, fp, length);
            HttpContext.Response.Flush();
        }
    }
}
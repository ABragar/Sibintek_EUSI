using System;
using System.IO;
using System.Linq;
using System.Web;
using DAL.EF;
using DAL.Entities;
using ReportStorage.Exceptions;

namespace ReportStorage.Service
{
    public interface IFileService
    {
        Stream GetStream(Report obj);
        Guid CreateFile(HttpPostedFile file);
        void ClearFiles();
        void ToHistoryFile(Report obj);
    }

    public class FileService : IFileService
    {
        private readonly IPathHelper _pathHelper;

        public FileService(IPathHelper pathHelper)
        {
            _pathHelper = pathHelper;
        }

        public Stream GetStream(Report obj)
        {
            var filePath = Path.Combine(_pathHelper.GetFilesDirectory(), obj.GuidId.ToString("N") + obj.Extension);

            if (!File.Exists(filePath))
                return null;

            return File.OpenRead(filePath);
        }

        public Guid CreateFile(HttpPostedFile file)
        {
            var guidId = Guid.NewGuid();

            var folder = _pathHelper.GetFilesDirectory();

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var path = Path.Combine(folder, guidId.ToString("N") + "." + file.FileName.Split('.').Last());

//            Очень долгая и бессмысленная операция
//            if (System.IO.File.Exists(path))
//                throw new ObjectValidationException("Файл с таким идентификатором уже существует. Попробуйте загрузить еще раз.");

            file.SaveAs(path);

            return guidId;
        }

        public void ClearFiles()
        {
            var folder = _pathHelper.GetFilesDirectory();

            var files = Directory.GetFiles(folder, "*");

            using (var context = new ReportDbContext())
            {
                foreach (var file in files)
                {
                    var fi = new FileInfo(file);
                    if (!context.Reports.Any(x => x.GuidId + x.Extension == fi.Name))
                        File.Delete(file);
                }
            }
        }


        /// <summary>
        /// Move template file to history folder.
        /// </summary>
        /// <param name="obj"></param>
        public void ToHistoryFile(Report obj)
        {
            //TODO Убрать MagicString
            var folder = Path.Combine(_pathHelper.GetFilesDirectory(), "History");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string file = Path.Combine(_pathHelper.GetFilesDirectory(),
                obj.GuidId.ToString("N") + "." + obj.Extension.Split('.').Last());

            if (!File.Exists(file))
                throw new ObjectValidationException("Файл с таким именем не существует или уже удалён.");

            string newFile = Path.Combine(folder, obj.GuidId.ToString("N") + "." + obj.Extension.Split('.').Last());
            if (File.Exists(newFile))
                throw new ObjectValidationException("Файл с таким именем уже существует как исторический.");

            File.Move(file, newFile);
        }
    }
}
using Base.DAL;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Base.Extensions;
using Base.Utils.Common.Wrappers;

namespace Base.Service
{
    public class FileSystemService : IFileSystemService
    {
        private readonly IServiceFactory<IWebClientAdapter> _webClientAdapterFactory;

        public string FilesDirectory { get; }

        public string ContentDirectory { get; }

        public FileSystemService(IPathHelper pathHelper, IServiceFactory<IWebClientAdapter> webClientAdapterFactory)
        {
            _webClientAdapterFactory = webClientAdapterFactory;
            FilesDirectory = pathHelper.GetFilesDirectory();
            ContentDirectory = pathHelper.GetContentDirectory();
        }

        public FileData SaveFile(IPostedFileWrapper file)
        {
            if (file == null) return null;

            if (!Directory.Exists(FilesDirectory))
                Directory.CreateDirectory(FilesDirectory);

            var fileid = Guid.NewGuid();

            var result = new FileData()
            {
                FileID = fileid,
                FileName = Path.GetFileName(file.FileName),
                Size = file.ContentLength,
                CreationDate = DateTime.Now,
                ChangeDate = DateTime.Now,
                Extension = file.FileName.Contains(".") ? file.FileName.Substring(file.FileName.LastIndexOf(".", StringComparison.Ordinal) + 1).ToUpper() : ""
            };

            var path = GetFilePath(result.FileID);

            var dir = Directory.GetParent(path).FullName;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            else if (File.Exists(path))
                File.Delete(path);

            file.SaveAs(path);

            return result;
        }

        public FileData SaveFile(Uri address, out Exception error)
        {
            if (!Directory.Exists(FilesDirectory))
                Directory.CreateDirectory(FilesDirectory);

            var fileid = Guid.NewGuid();

            string path = GetFilePath(fileid);

            string dir = Directory.GetParent(path).FullName;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            else if (File.Exists(path))
                File.Delete(path);

            FileInfo fi = null;

            try
            {
                using (var client = _webClientAdapterFactory.GetService())
                {
                    client.DownloadFile(address, path);
                }

                fi = new FileInfo(path);

                error = null;
            }
            catch (Exception e)
            {
                fileid = Guid.Empty;
                error = e;
            }

            return new FileData()
            {
                FileID = fileid,
                FileName = fi?.Name ?? fileid.ToString(),
                Size = fi?.Length ?? 0,
                CreationDate = DateTime.Now,
                ChangeDate = DateTime.Now,
                Extension = fi != null && fi.Name.Contains(".") ? fi.Name.Substring(fi.Name.LastIndexOf(".", StringComparison.Ordinal) + 1).ToUpper() : ""
            };
        }

        public FileData SaveFile(Stream stream, out Exception error)
        {
            if (!Directory.Exists(FilesDirectory))
                Directory.CreateDirectory(FilesDirectory);

            var fileid = Guid.NewGuid();

            string path = GetFilePath(fileid);

            string dir = Directory.GetParent(path).FullName;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            else if (File.Exists(path))
                File.Delete(path);

            FileInfo fi = null;

            try
            {
                using (var fileStream = File.Create(path))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                }

                fi = new FileInfo(path);
                error = null;
            }
            catch (Exception e)
            {
                fileid = Guid.Empty;
                error = e;
            }

            return new FileData()
            {
                FileID = fileid,
                FileName = fi?.Name ?? fileid.ToString(),
                Size = fi?.Length ?? 0,
                CreationDate = DateTime.Now,
                ChangeDate = DateTime.Now,
                Extension = fi != null && fi.Name.Contains(".") ? fi.Name.Substring(fi.Name.LastIndexOf(".", StringComparison.Ordinal) + 1).ToUpper() : ""
            };
        }

        public FileData SaveFile(Stream stream, string extension, out Exception error)
        {
            if (!Directory.Exists(FilesDirectory))
                Directory.CreateDirectory(FilesDirectory);

            var fileid = Guid.NewGuid();

            string path = GetFilePath(fileid);

            string dir = Directory.GetParent(path).FullName;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            else if (File.Exists(path))
                File.Delete(path);

            FileInfo fi = null;

            try
            {
                using (var fileStream = File.Create(path))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                }

                fi = new FileInfo(path);
                error = null;
            }
            catch (Exception e)
            {
                fileid = Guid.Empty;
                error = e;
            }

            return new FileData()
            {
                FileID = fileid,
                FileName = fi != null ? fi.Name + extension : fileid + extension,
                Size = fi?.Length ?? 0,
                CreationDate = DateTime.Now,
                ChangeDate = DateTime.Now,
                Extension = fi != null && fi.Name.Contains(".") ? fi.Name.Substring(fi.Name.LastIndexOf(".", StringComparison.Ordinal) + 1).ToUpper() : ""
            };
        }

        public string GetFilePath(Guid fileid)
        {
            string guid = fileid.ToString("N");

            return Path.Combine(Path.Combine(FilesDirectory, guid.Substring(1, 4)), guid);
        }

        public string GetFilePathFromContent(string subFolder, string fileName, string extension)
        {
            return Path.Combine(Path.Combine(ContentDirectory, subFolder), fileName + "." + extension);
        }

        public string GetFilePath(Guid fileid, bool checkTemp)
        {
            string path = GetFilePath(fileid);

            var fi = new FileInfo(path);

            return fi.Exists ? path : null;
        }

        public FileData GetFileData(IUnitOfWork unitOfWork, int id)
        {
            return unitOfWork.GetRepository<FileData>().Find(f => f.ID == id);
        }

        public Task<FileData> GetFileDataAsync(IUnitOfWork unitOfWork, int id)
        {
            return unitOfWork.GetRepository<FileData>().All().Where(f => f.ID == id).FirstOrDefaultAsync();
        }

        public FileData GetFileData(IUnitOfWork unitOfWork, Guid fileid)
        {
            return unitOfWork.GetRepository<FileData>().Find(f => f.FileID == fileid);
        }

        public Task<FileData> GetFileDataAsync(IUnitOfWork unitOfWork, Guid fileid)
        {
            return unitOfWork.GetRepository<FileData>().All().Where(f => f.FileID == fileid).FirstOrDefaultAsync();
        }

        public string GetUrlFromFileName(string name)
        {
            string[] nameArr = name.Split('_');
            return $"/Files/GetImageFromDocx/{nameArr[0]}?imgIndex={nameArr[1]}";
        }
    }

    public class DefaultFileManager : IFileManager
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IFileSystemService _fileSystemService;

        public DefaultFileManager(IUnitOfWorkFactory unitOfWorkFactory, IFileSystemService fileSystemService)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _fileSystemService = fileSystemService;
        }

        public int DeleteFiles()
        {
            int i = 0;

            if (!Directory.Exists(_fileSystemService.FilesDirectory)) return 0;

            using (var uofw = _unitOfWorkFactory.CreateSystem())
            {
                var files = Directory.GetFiles(_fileSystemService.FilesDirectory, "*", SearchOption.AllDirectories);

                var repository = uofw.GetRepository<FileData>();

                foreach (string file in files)
                {
                    var fi = new FileInfo(file);

                    if (fi.LastWriteTime >= DateTime.Now.AddHours(-1)) continue;

                    Guid guid;

                    if (Guid.TryParse(fi.Name, out guid))
                    {
                        if (repository.All().Any(x => x.FileID == guid)) continue;
                        File.Delete(file);
                        i++;
                    }
                    else if (fi.Name.IndexOf("_", StringComparison.Ordinal) > 0)
                    {
                        //"a340b56f13254f42b156f70d18811339_0"
                        var name_file = fi.Name.Split('_');
                        if (!Guid.TryParse(name_file[0], out guid)) continue;
                        string path = _fileSystemService.GetFilePath(guid);
                        if (File.Exists(path)) continue;
                        File.Delete(file);
                        i++;
                    }
                }

                var dirs = Directory.GetDirectories(_fileSystemService.FilesDirectory);

                foreach (string dir in dirs)
                {
                    var di = new DirectoryInfo(dir);

                    if (!di.GetFiles().Any())
                    {
                        try
                        {
                            Directory.Delete(dir);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                }
            }

            return i;
        }

        public int DeleteFileData()
        {
            int i = 0;

            using (var uofw = _unitOfWorkFactory.CreateSystem())
            {
                var repository = uofw.GetRepository<FileData>();

                var items = repository.All().Select(x => new { ID = x.ID , RowVersion = x.RowVersion });

                foreach (var item in items)
                {
                    var file = new FileData() { ID = item.ID, RowVersion = item.RowVersion };

                    repository.Attach(file);
                    repository.Delete(file);

                    try
                    {
                        uofw.SaveChanges();
                        i++;
                    }
                    catch (Exception)
                    {
                        //Конфликт инструкции DELETE с ограничением REFERENCE. Выполнение данной инструкции было прервано.
                        repository.Detach(file);
                    }
                }
            }

            return i;
        }
    }
}

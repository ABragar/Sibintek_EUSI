using Base.DAL;
using System;
using System.IO;
using System.Threading.Tasks;
using Base.Utils.Common.Wrappers;

namespace Base.Service
{
    public interface IFileSystemService : IService
    {
        string FilesDirectory { get; }
        string ContentDirectory { get; }
        FileData SaveFile(IPostedFileWrapper file);
        FileData SaveFile(Uri address, out Exception error);
        FileData SaveFile(Stream stream, out Exception error);
        FileData SaveFile(Stream stream, string extension, out Exception error);
        string GetFilePath(Guid fileid);
        string GetFilePath(Guid fileid, bool checkTemp);
        string GetFilePathFromContent(string subFolder, string fileName, string extension);
        FileData GetFileData(IUnitOfWork unitOfWork, int id);
        FileData GetFileData(IUnitOfWork unitOfWork, Guid fileid);
        Task<FileData> GetFileDataAsync(IUnitOfWork unitOfWork, int id);
        Task<FileData> GetFileDataAsync(IUnitOfWork unitOfWork, Guid fileid);

        string GetUrlFromFileName(string name);
    }

    public interface IFileManager
    {
        int DeleteFiles();
        int DeleteFileData();
    }
}
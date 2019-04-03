using System.Linq;
using Base.DAL;
using Base.Service;
using CorpProp.Entities.Document;
using CorpProp.Entities.ManyToMany;
using AppContext = Base.Ambient.AppContext;
using CorpProp.Entities.Security;
using CorpProp.Services.Security;
using CorpProp.Services.Base;
using System;
using System.IO;
using System.Threading.Tasks;
using Base.Utils.Common.Wrappers;
using System.Data.Entity;

namespace CorpProp.Services.Document
{
    /// <summary>
    /// Предоставляет методы для работы с файлами в БД. 
    /// </summary>
    public interface IFileDBService : IService
    {       
        FileDB SaveFile(IUnitOfWork uow, IPostedFileWrapper file); 
        FileDB GetFile(IUnitOfWork uow, int id);
        FileDB GetFile(IUnitOfWork uow, Guid oid);
        Task<FileDB> GetFileAsync(IUnitOfWork uow, int id);
        Task<FileDB> GetFileAsync(IUnitOfWork uow, Guid fileid);
                
    }

    /// <summary>
    /// Представляет серви сдля работы с файлами в БД.
    /// </summary>
    public class FileDBService : IFileDBService
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса FileDBService.
        /// </summary>
        public FileDBService()
        {            
            
        }

        /// <summary>
        /// Сохраняет файл в БД.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="file">HTTP-объект, предоставляющий доступ к отдельным файлам, которые были отправлены клиентом.</param>
        /// <returns></returns>
        public FileDB SaveFile(IUnitOfWork uow, IPostedFileWrapper file)
        {
            if (file == null) return null;
            
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(file.InputStream))
            {
                bytes = br.ReadBytes(file.ContentLength);
            }

            var result = uow.GetRepository<FileDB>()
                .Create(new FileDB()
                {

                    Name = file.FileName,
                    Ext = file.FileName.Contains(".") ? file.FileName.Substring(file.FileName.LastIndexOf(".", StringComparison.Ordinal) + 1).ToUpper() : "",
                    Content = bytes

                });
            uow.SaveChanges();
            return result;
        }
        
     
        /// <summary>
        /// Возвращает файл из БД по ИД.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileDB GetFile(IUnitOfWork uow, int id)
        {
            return uow.GetRepository<FileDB>().Find(f => f.ID == id);
        }

        /// <summary>
        /// Возвращает файл из БД по его Oid.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="oid"></param>
        /// <returns></returns>
        public FileDB GetFile(IUnitOfWork uow, Guid oid)
        {
            return uow.GetRepository<FileDB>().Find(f => f.Oid == oid);
        }

        /// <summary>
        /// Возвращает файл из БД по ИД (асинхронный вызов).
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<FileDB> GetFileAsync(IUnitOfWork unitOfWork, int id)
        {
            return unitOfWork.GetRepository<FileDB>().All().Where(f => f.ID == id).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Возвращает файл из БД по его Oid (асинхронный вызов).
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="oid"></param>
        /// <returns></returns>
        public Task<FileDB> GetFileAsync(IUnitOfWork unitOfWork, Guid oid)
        {
            return unitOfWork.GetRepository<FileDB>().All().Where(f => f.Oid == oid).FirstOrDefaultAsync();
        }

        
    }

}

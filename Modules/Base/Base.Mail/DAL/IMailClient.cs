using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Mail.Entities;
using MailKit;
using MailFolder = Base.Mail.Entities.MailFolder;

namespace Base.Mail.DAL
{
    public interface IMailClient
    {                        
        Task Send(IUnitOfWork uow, MailMessageViewModel msg);
        void SystemSend(IEnumerable<string> mailto, string caption, string body);
        Task<IEnumerable<MailFolder>> GetFolders(IUnitOfWork uow, string id = null);
        Task<MailClientResult> GetMessages(IUnitOfWork uow,string folderPath, int skip, int take, MailSorting sorting = null);
        Task<MailMessageViewModel> GetMessage(IUnitOfWork uow, string folderPath, int id);
        Task<byte[]> GetFile(IUnitOfWork uow, string folder, uint messageId, string fileName);
        void DeleteMessage(IUnitOfWork uow, string folder, uint messageId);
        void MoveMessage(IUnitOfWork uow, string folder,string newFolder ,uint messageId);
        void SetFlags(IUnitOfWork uow, string folderName, uint messageId, string flags);
        void RemoveFlags(IUnitOfWork uow, string folderName, uint messageId, string flags);
        Task<int> GetFolderCount(IUnitOfWork uow, string folderName);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Mail.DAL;
using Base.Mail.Entities;
using Base.Service;

namespace Base.Mail.Service
{
    public interface IMailMessageService
    {
        Task<MailClientResult> GetMessages(IUnitOfWork uow, string folder, int page, int take, MailSorting sorting = null);
        Task<MailMessageViewModel> GetMessage(IUnitOfWork uow, string folder, int index);
        Task<byte[]> GetFile(IUnitOfWork uow, string folder, uint messageId, string fileName);
        void DeleteMessage(IUnitOfWork uow, string folder, uint messageId);
        void MoveMessage(IUnitOfWork uow, string folder, string newFolder, uint messageId);
        void SetFlags(IUnitOfWork uow, string folderName, uint messageId, string flags);
        void RemoveFlags(IUnitOfWork uow, string folderName, uint messageId, string flags);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Base.DAL;
using Base.Mail.DAL;
using Base.Mail.Entities;
using Base.UI;
using Base.UI.Service;

namespace Base.Mail.Service
{

    public class MailMessageService : IMailMessageService
    {
        private readonly IMailClient _mailClient;

        public MailMessageService(IMailClient mailClient)
        {
            _mailClient = mailClient;
        }

        public async Task<MailClientResult> GetMessages(IUnitOfWork uow, string folder, int skip, int take, MailSorting sorting = null)
        {
            return await _mailClient.GetMessages(uow, folder, skip, take);
        }

        public async Task<MailMessageViewModel> GetMessage(IUnitOfWork uow, string folder, int index)
        {
            return await _mailClient.GetMessage(uow, folder, index);
        }

        public async Task<byte[]> GetFile(IUnitOfWork uow, string folder, uint messageId, string fileName)
        {
            return await _mailClient.GetFile(uow, folder, messageId, fileName);
        }

        public void DeleteMessage(IUnitOfWork uow, string folder, uint messageId)
        {
            _mailClient.DeleteMessage(uow, folder, messageId);
        }

        public void MoveMessage(IUnitOfWork uow, string folder, string newFolder, uint messageId)
        {
            _mailClient.MoveMessage(uow, folder, newFolder, messageId);
        }

        public async void SetFlags(IUnitOfWork uow, string folderName, uint messageId, string flags)
        {
            _mailClient.SetFlags(uow, folderName, messageId, flags);
        }

        public async void RemoveFlags(IUnitOfWork uow, string folderName, uint messageId, string flags)
        {
            _mailClient.RemoveFlags(uow, folderName, messageId, flags);
        }
    }
}

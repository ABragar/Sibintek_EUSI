using System.Collections.Generic;
using System.Threading.Tasks;
using Base.DAL;
using Base.Mail.DAL;
using Base.UI.Service;
using MailFolder = Base.Mail.Entities.MailFolder;

namespace Base.Mail.Service
{
    public class MailFolderService : IMailFolderService
    {
        private readonly IMailClient _mailClient;

        public MailFolderService(IMailClient mailClient)
        {
            _mailClient = mailClient;
        }

        public async Task<IEnumerable<MailFolder>> GetFolders(IUnitOfWork uow, string id = null)
        {            
            return await _mailClient.GetFolders(uow, id);
        }

        public async Task<int> UpdateCount(IUnitOfWork uow, string folder)
        {
            return await _mailClient.GetFolderCount(uow, folder);
        }
    }
}

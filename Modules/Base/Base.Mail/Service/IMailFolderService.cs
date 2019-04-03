using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Mail.Entities;
using Base.Service;

namespace Base.Mail.Service
{
    public interface IMailFolderService
    {
        Task<IEnumerable<MailFolder>> GetFolders(IUnitOfWork uow, string ID = null);
        Task<int> UpdateCount(IUnitOfWork uow, string folder);
    }
}

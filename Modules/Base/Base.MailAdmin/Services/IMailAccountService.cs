using System.Threading.Tasks;
using Base.MailAdmin.Entities;

namespace Base.MailAdmin.Services
{ 
    public interface IMailAccountService
    {
        Task<MailAccount> GetByIserId(int userid);
        Task<MailAccount> Save(MailAccount mailAccount);
    }
}
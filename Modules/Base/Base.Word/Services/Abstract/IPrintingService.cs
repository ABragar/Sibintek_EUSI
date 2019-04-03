using System.IO;
using System.Threading.Tasks;
using Base.DAL;
using Base.Service;
using Base.Word.Services.Concrete;
using WordTemplates;

namespace Base.Word.Services.Abstract
{
    public interface IPrintingService : IService
    {
        Task<PrintResult> PrintAsync(IUnitOfWork uow, string mnemonic, int id, int? template_settings_id);
        Template GetTemplateConfig(string mnemonic);
    }
}
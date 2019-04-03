using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.ExportImport.Entities;
using Base.Service;

namespace Base.ExportImport.Services.Abstract
{
    public interface IPackageService : IBaseObjectService<Package>
    {
        ICollection<Package> GetPackags (IUnitOfWork uow, string mnemonic);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.ExportImport.Entities;
using Base.ExportImport.Services.Abstract;
using Base.Service;

namespace Base.ExportImport.Services.Concrete
{
    public class PackageService : BaseObjectService<Package>, IPackageService
    {
        public PackageService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<Package> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<Package> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(x => x.Transform);
        }

        public ICollection<Package> GetPackags(IUnitOfWork uow, string mnemonic)
        {
            var retVal = GetAll(uow).Where(x => x.ObjectType == mnemonic);

            return retVal.ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using Base.DAL;
using Base.Service;

namespace CorpProp.Services.Document
{

    public interface IFileDataVersionsService : IBaseObjectService<FileData>
    {
        
    }
    public class FileDataVersionsService : BaseObjectService<FileData>, IFileDataVersionsService
    {
        public FileDataVersionsService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        public override IQueryable<FileData> GetAll(IUnitOfWork unitOfWork, bool? hidden)
        {
            return base.GetAll(unitOfWork, hidden);
        }
    }
}

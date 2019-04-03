using Base.DAL;
using Base.Service;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Import;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Accounting;
using CorpProp.Services.Base;
using EUSI.Entities.Accounting;
using EUSI.Entities.NSI;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace EUSI.Services.Accounting
{
    public interface IExportMovingService : IBaseObjectService<ExportMoving>
    {

    }

    public class ExportMovingService : BaseObjectService<ExportMoving>, IExportMovingService
    {
       

        public ExportMovingService(IBaseObjectServiceFacade facade) : base(facade)
        {
           
        }

        public override ExportMoving Create(IUnitOfWork unitOfWork, ExportMoving obj)
        {
            throw new NotImplementedException();
        }

        public override ExportMoving Update(IUnitOfWork unitOfWork, ExportMoving obj)
        {
            throw new NotImplementedException();
        }

        protected override IObjectSaver<ExportMoving> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<ExportMoving> objectSaver)
        {

            throw new NotImplementedException();

        }

    }
}

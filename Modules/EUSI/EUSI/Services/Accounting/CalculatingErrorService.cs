using Base.DAL;
using Base.Service;
using EUSI.Entities.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Services.Accounting
{
    public interface ICalculatingErrorService : IBaseObjectService<CalculatingError>
    {

    }

    public class CalculatingErrorService : BaseObjectService<CalculatingError>,
        ICalculatingErrorService
    {
        public CalculatingErrorService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<CalculatingError> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<CalculatingError> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                //.SaveOneObject(s => s.AccountingObject)
                ;
        }
    }
}

using Base.DAL;
using Base.Service;
using CorpProp.Common;
using EUSI.Entities.Estate;

namespace EUSI
{
    public interface IEstateRegistrationService: IBaseObjectService<EstateRegistration>
        , IExcelImportEntity            
        , IExportToZip
        , ISibUserNotification
        , IConfirmImportChecker
        , IExcelImportChecker

    {
        void InvokeWFStage(IUnitOfWork uow, int objID, string stageSysName);
        void DeleteRows(IUnitOfWork unitOfWork, EstateRegistration obj);
    }
}

using Base.DAL;
using Base.Service;
using CorpProp.Entities.Settings;

namespace CorpProp.Services.Settings
{
    public interface IExportImportSettingsService : IBaseObjectService<ExportImportSettings>
    {

    }
    public class ExportImportSettingsService : BaseObjectService<ExportImportSettings>, IExportImportSettingsService
    {
        public ExportImportSettingsService(IBaseObjectServiceFacade facade) : base(facade)
        {
        }

        protected override IObjectSaver<ExportImportSettings> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<ExportImportSettings> objectSaver)
        {
            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(s => s.FileCard)
                .SaveOneObject(s => s.Society)
                ;
        }
    }
}

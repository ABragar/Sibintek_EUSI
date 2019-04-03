using Base.Contact.Service.Abstract;
using Base.DAL;
using Base.Document.Entities;
using Base.Security;
using Base.Service;

namespace Base.Document.Services.Concrete
{
    public class UnifiedDocumentService : BaseDocumentService<UnifiedDocument>
    {
        public UnifiedDocumentService(IBaseObjectServiceFacade facade, IUserService<User> userService, IEmployeeUserService employeeUserService) : base(facade, userService, employeeUserService)
        {
        }

        protected override IObjectSaver<UnifiedDocument> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<UnifiedDocument> objectSaver)
        {
            if (string.IsNullOrEmpty(objectSaver.Src.Description))
                objectSaver.Src.Description = objectSaver.Src.File?.FileName;

            if (objectSaver.Src.File != null && objectSaver.Dest.File != null)
            {
                if (objectSaver.Src.File.FileID != objectSaver.Dest.File.FileID)
                {
                    UnifiedDocumentChangeHistory history = new UnifiedDocumentChangeHistory
                    {
                        File = objectSaver.Dest.File,
                        EditorUser =
                            unitOfWork.GetRepository<User>().Find(u => u.ID == Base.Ambient.AppContext.SecurityUser.ID),
                        ChangedDate = Base.Ambient.AppContext.DateTime.Now
                    };
                    objectSaver.Src.FileHistory.Add(history);
                }
            }



            return base.GetForSave(unitOfWork, objectSaver)
                .SaveOneObject(s => s.File)
                .SaveOneToMany(x => x.FileHistory,
                    x => x.SaveOneObject(o => o.File).SaveOneObject(o => o.EditorUser));
        }
    }
}
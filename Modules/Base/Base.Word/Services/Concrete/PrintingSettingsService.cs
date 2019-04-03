using System;
using Base.DAL;
using Base.Service;
using Base.Word.Entities;
using Base.Word.Services.Abstract;

namespace Base.Word.Services.Concrete
{
    public class PrintingSettingsService : BaseObjectService<PrintingSettings>, IPrintingSettingsService
    {
        private readonly IWordService _word_service;

        public PrintingSettingsService(IBaseObjectServiceFacade facade,IWordService word_service) : base(facade)
        {
            _word_service = word_service;
        }

        protected override IObjectSaver<PrintingSettings> GetForSave(IUnitOfWork unitOfWork,
            IObjectSaver<PrintingSettings> objectSaver)
        {
            base.GetForSave(unitOfWork, objectSaver);

            objectSaver.SaveOneObject(x => x.Template);

            if (!_word_service.HasContent(objectSaver.Dest.Template.FileID))
            {
                throw new InvalidOperationException("Шаблон не должен быть пустым");
            }

            return objectSaver;

                        
        }

    }
}
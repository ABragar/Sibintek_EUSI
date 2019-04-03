using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.DAL;
using Base.Entities.Complex;
using Base.Mail.DAL;
using Base.Mail.Entities;
using Base.Service;
using Base.UI;
using Base.Word.Entities;
using Base.Word.Services.Abstract;

namespace Base.Mail.Service
{
    public class MailQueueService : BaseObjectService<MailQueueItem>, IMailQueueService
    {

        private readonly IPrintingSettingsService _printingSettingsService;
        private readonly IPrintingService _printingService;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IMailClient _mailClient;
        private readonly IFileSystemService _fileSystemService;
        private readonly IWordService _wordService;
        private readonly IViewModelConfigService _viewModelConfigService;


        public MailQueueService(IBaseObjectServiceFacade facade,
            IPrintingSettingsService printingSettingsService,
            IPrintingService printingService,
            IUnitOfWorkFactory unitOfWorkFactory,
            IMailClient mailClient, IFileSystemService fileSystemService, IWordService wordService, IViewModelConfigService viewModelConfigService) :
                base(facade)
        {
            _printingSettingsService = printingSettingsService;
            _printingService = printingService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _mailClient = mailClient;
            _fileSystemService = fileSystemService;
            _wordService = wordService;
            _viewModelConfigService = viewModelConfigService;
        }

        public int SendQueue()
        {
            int count = 0;

            using (var uow = _unitOfWorkFactory.CreateSystem())
            {
                var queueu = this.GetAll(uow).Where(x => x.Processed == false);

                var grouped = queueu.GroupBy(x => new { x.Entity.ID, x.Entity.TypeName, x.Title })
                    .Select(x => new { EntitiID = x.Key.ID, x.Key.TypeName, x.Key.Title, To = x.Select(z => z.To) });

                foreach (var mailQueueItem in grouped)
                {
                    string mnemonic = _viewModelConfigService.Get(mailQueueItem.TypeName)?.Mnemonic;

                    if (!string.IsNullOrEmpty(mnemonic))
                    {
                        var printSetting = _printingSettingsService.GetAll(uow).ToList()
                            .FirstOrDefault(x => x.Mnemonic == mnemonic && x.TemplateType == TemplateType.Message);

                        if (printSetting != null)
                        {
                            //TODO ПИЗДЕЦ

                            var task = _printingService.PrintAsync(uow, mnemonic, mailQueueItem.EntitiID, printSetting.ID);

                            var originalPath = _fileSystemService.GetFilePath(printSetting.Template.FileID);

                            var content = _wordService.ConvertToHtml(task.Result.Result, originalPath);

                            _mailClient.SystemSend(mailQueueItem.To, mailQueueItem.Title, content);
                        }
                        else
                        {
                            _mailClient.SystemSend(mailQueueItem.To, mailQueueItem.Title, "not template");
                        }

                        count++;
                    }
                }

                foreach (var mailQueueItem in queueu)
                {
                    mailQueueItem.Processed = true;
                }

                uow.SaveChanges();
            }

            //TODO : remove processed optimize

            return count;
        }

        public Task<int> RemoveFromQueue(IUnitOfWork uow, IEnumerable<string> sourse)
        {
            if (sourse == null)
                throw new ArgumentNullException(nameof(sourse));

            var repository = uow.GetRepository<MailQueueItem>();

            var q = repository.All().Where(x => sourse.Contains(x.Source)).Select(x => new { x.ID, x.RowVersion });

            foreach (var item in q)
            {
                var mailQueueItem = new MailQueueItem() { ID = item.ID, RowVersion = item.RowVersion };
                repository.Attach(mailQueueItem);
                repository.Delete(mailQueueItem);
            }

            return uow.SaveChangesAsync();
        }

        public void AddToQueue(IUnitOfWork uow, string sourse, string title, LinkBaseObject obj, string emailTo, bool save = true)
        {
            if (string.IsNullOrEmpty(sourse))
                throw new ArgumentNullException(nameof(sourse));
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));
            if (string.IsNullOrEmpty(emailTo))
                throw new ArgumentNullException(nameof(emailTo));

            var repository = uow.GetRepository<MailQueueItem>();

            repository.Create(new MailQueueItem
            {
                Title = title,
                Date = DateTime.Now,
                To = emailTo,
                Entity = obj,
                Source = sourse
            });

            if (save)
                uow.SaveChanges();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Base.DAL;
using Base.Extensions;
using Base.Service;
using Base.Utils.Common.Caching;
using Base.Word.Entities;
using Base.Word.Services.Abstract;
using WebUI.Helpers;
using WebUI.Models.Print;
using WordTemplates;


namespace WebUI.Controllers
{
    public class PrintController : BaseController
    {

        private readonly IPrintingService _printing_service;

        private readonly IPrintingSettingsService _printing_settings_service;


        private readonly IFileSystemService _fileSystemService;
        private readonly ISimpleCacheWrapper _cacheWrapper;
        private readonly IWordService _wordService;
        

        public PrintController(IBaseControllerServiceFacade serviceFacade,
            IPrintingService printing_service,
            IPrintingSettingsService printing_settings_service,
            IFileSystemService fileSystemService,
            ISimpleCacheWrapper cacheWrapper,
            IWordService wordService)
                : base(serviceFacade)
        {
            _printing_service = printing_service;
            _printing_settings_service = printing_settings_service;
            _fileSystemService = fileSystemService;
            _cacheWrapper = cacheWrapper;
            _wordService = wordService;
            
        }

        const string DocxContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";



        public async Task<ActionResult> DocumentPreview(string mnemonic, int id, int templateId)
        {
            return PartialView("Word", new PrintDocumentContentViewModel()
            {
                Id = id,
                Mnemonic = mnemonic,
                TemplateId = templateId
            });
        }


        private static readonly CacheAccessor<string> PrintPreviewAccessor = new CacheAccessor<string>();
        public async Task<ActionResult> PreviewContent(string mnemonic, int id, int templateId)
        {
            using (var uow = CreateUnitOfWork())
            {

                var print_result = await _printing_service.PrintAsync(uow, mnemonic, id, templateId);

                var content = _cacheWrapper.GetOrAdd(PrintPreviewAccessor, print_result.ResultKey, () =>
                {

                    //TODO 
                    var template = uow.GetRepository<PrintingSettings>().All().FirstOrDefault(x => x.ID == templateId);
                    if (template == null || template.Template == null)
                        return null;

                    var originalPath = _fileSystemService.GetFilePath(template.Template.FileID);

                    return _wordService.ConvertToHtml(print_result.Result, originalPath);

                });
       
                return Content(content);
            }
        }

        public async Task<ActionResult> Print(string mnemonic, int id, int templateId)
        {
            using (var uow = CreateUnitOfWork())
            {
                var result = await _printing_service.PrintAsync(uow, mnemonic, id, templateId);

                return File(result.Result, DocxContentType, result.Name);
            }
        }

        public async Task<ActionResult> ToolBar(string mnemonic, int id)
        {
            using (var uow = CreateUnitOfWork())
            {
                if (GetTemplates(uow, mnemonic).Any())
                {
                    return PartialView(new PrintToolbarViewModel() { Id = id, Mnemonic = mnemonic, });

                }

                return new EmptyResult();

            }

        }

        public ActionResult DataSourceToolBar()
        {
            return PartialView();
        }


        public async Task<ActionResult> Templates(string mnemonic)
        {
            using (var uow = CreateUnitOfWork())
            {
                var result = await GetTemplates(uow, mnemonic).ToListAsync();

                return new JsonNetResult(result);
            }
        }


        private IQueryable<PrintTemplatesModel> GetTemplates(IUnitOfWork uow, string mnemonic)
        {

            return
                _printing_settings_service.GetAll(uow)
                    .Where(x => x.Mnemonic == mnemonic)
                    .Select(x => new PrintTemplatesModel { Id = x.ID, Title = x.TemplateName });


        }

        public ActionResult DataSource(string mnemonic)
        {
            var template = _printing_service.GetTemplateConfig(mnemonic);


            return new JsonNetResult(GetDataSourceModel(template));
        }

        private IEnumerable<DataSourceModel> GetDataSourceModel(Template template)
        {
            return template.Values.Select(x => new DataSourceModel { Name = x })
                .Concat(template.Items.Select(x => new DataSourceModel { Name = x.Key, Childrens = GetDataSourceModel(x.Value) }));
        }
    }
}
using System;
using System.Web.Mvc;
using Base.Service;
using CorpProp.Services.NSI;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class ExchangeRateController : BaseController
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IFileSystemService _fileSystemService;

        public ExchangeRateController(IBaseControllerServiceFacade serviceFacade,
            IExchangeRateService exchangeRateService, IFileSystemService fileSystemService) : base(serviceFacade)
        {
            _exchangeRateService = exchangeRateService;
            _fileSystemService = fileSystemService;
        }

        [HttpPost]
        public JsonNetResult Load(Guid fileId)
        {
           
            var error = 0;
            var message = string.Empty;

            try
            {
                
                using (var unitOfWork = CreateUnitOfWork())
                {
                    _exchangeRateService.Import(_fileSystemService.GetFilePath(fileId), unitOfWork);
                }
            }
            catch (Exception e)
            {
                error = 1;
                message = "Ошибка загрузки курсов валют из файла: " + e.Message;
            }

            return new JsonNetResult(new
            {
                error,
                message = error == 1 ? message : "Данные сохранены успешно."
            });
        }
    }
}
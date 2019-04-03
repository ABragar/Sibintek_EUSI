using System.Web.Http;
using Base;
using Base.DAL;
using Base.Service;
using Base.Service.Log;
using Base.UI;
using Base.UI.ViewModal;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    public abstract class BaseApiController: ApiController
    {
        private readonly IViewModelConfigService _view_model_config_service;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ILogService _logger;
        private ViewModelConfig _config;

        protected BaseApiController(IViewModelConfigService viewModelConfigService, IUnitOfWorkFactory unitOfWorkFactory, ILogService logger)
        {
            _logger = logger;
            _view_model_config_service = viewModelConfigService;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        protected ViewModelConfig GetConfig(string mnemonic = null)
        {
            if (mnemonic == null)
            {
                if (_config != null)
            return _config;
                return _config =
                    _view_model_config_service.Get((string) this.RequestContext.RouteData.Values["mnemonic"]);

            }
            else
            {
                return _view_model_config_service.Get(mnemonic);
            }
        }

        public IViewModelConfigService ViewModelConfigService { get { return _view_model_config_service; } }

        protected IBaseObjectService<T> GetBaseObjectService<T>(string mnemonic = null) where T : BaseObject
        {
            return GetConfig(mnemonic).GetService<IBaseObjectService<T>>();
        }


        protected IQueryService<T> GetQueryService<T>(string mnemonic = null) where T : IBaseObject
        {
            return GetConfig(mnemonic).GetService<IQueryService<T>>();
        }

        protected IBaseCategorizedItemService<T> GetBaseCategorizedItemService<T>() where T : ICategorizedItem
        {
            return GetConfig().GetService<IBaseCategorizedItemService<T>>();
        }

        protected IUnitOfWork CreateUnitOfWork()
        {
            return _unitOfWorkFactory.Create();
        }

        protected ITransactionUnitOfWork CreateTransactionUnitOfWork()
        {
            return _unitOfWorkFactory.CreateTransaction();
        }

        protected JsonSerializer CreateJsonSerializer()
        {
            return this.ControllerContext.Configuration.Formatters.JsonFormatter.CreateJsonSerializer();
        }
    }
}

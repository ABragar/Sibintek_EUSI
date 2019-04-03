using System;
using System.Threading.Tasks;
using System.Web.Http;
using Base;
using Base.DAL;
using Base.Service.Log;
using Base.UI;
using Base.UI.ViewModal;
using Base.UI.Wizard;
using WebApi.Attributes;

namespace WebApi.Controllers
{


    [CheckSecurityUser]
    [RoutePrefix("wizard/{wizard}")]
    internal class WizardController : BaseApiController
    {
        private ViewModelConfig _wizardConfig;
        private readonly IViewModelConfigService _view_model_config_service;
        private readonly ILogService _logger;

        public WizardController(IViewModelConfigService view_model_config_service, IUnitOfWorkFactory unit_of_work_factory, ILogService logger)
            : base(view_model_config_service, unit_of_work_factory, logger)
        {
            _logger = logger;
            _view_model_config_service = view_model_config_service;
        }

        private ViewModelConfig GetWizardConfig()
        {
            if (_wizardConfig != null)
                return _wizardConfig;

            var mnemonic = (string)this.RequestContext.RouteData.Values["wizard"];

            _wizardConfig = _view_model_config_service.Get(mnemonic);

            return _wizardConfig;
        }

        //TODO переделать
        private ViewModelConfig GetObjectViewModelConfig()
        {
            var config = GetWizardConfig();

            return _view_model_config_service.Get(m => !string.IsNullOrEmpty(m.DetailView?.WizardName) && string.Equals(m.DetailView.WizardName, config.Mnemonic, StringComparison.CurrentCultureIgnoreCase));
        }

        private IWizardService<T> GetWizardService<T>() where T : IWizardObject
        {
            return GetWizardConfig().GetService<IWizardService<T>>();

        }

        [GenericAction("wizard")]
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetInstance<T>(string wizard) where T : IWizardObject
        {
            var config = GetWizardConfig();
            var srv = GetWizardService<T>();

            try
            {
                var obj = Activator.CreateInstance(typeof(T));

                return Ok(new { model = config.DetailView.SelectObj(obj) });
            }
            catch (Exception e)
            {
                return Ok(new { error = 1, message = e.Message });
            }

        }

        [GenericAction("wizard")]
        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> Start<T>(string wizard,[FromBody]T model) where T : class, IWizardObject
        {
            try
            {
                var config = GetWizardConfig();
                var srv = GetWizardService<T>();

                if (config.ServiceType != null)
                {
                    using (var uofw = CreateTransactionUnitOfWork())
                    {
                        var obj = await srv.StartAsync(uofw, model, config);
                        uofw.Commit();

                        return Ok(new { model = config.DetailView.SelectObj(obj) });
                    }
                }
                else
                {
                    return Ok(new { model = config.DetailView.SelectObj(Activator.CreateInstance(config.TypeEntity) as IBaseObject) });
                }

            }
            catch (Exception e)
            {
                return Ok(new { error = 1, message = e.Message });
            }
        }

        [GenericAction("wizard")]
        [HttpPut]        
        [Route("next")]
        public async Task<IHttpActionResult> Next<T>(string wizard,[FromBody]T model) where T : IWizardObject
        {
            try
            {
                var config = GetWizardConfig();

                var srv = GetWizardService<T>();

                using (var uofw = CreateTransactionUnitOfWork())
                {
                    var obj = await srv.NextStepAsync(uofw, model, config);


                    if (obj.Step == WizardConfig.WIZARD_COMPLETE_KEY)
                    {
                        var baseConfig = GetObjectViewModelConfig();
                        var complete = await srv.CompleteAsync(uofw, model);

                        uofw.Commit();

                        return Ok(new
                        {
                            model = config.DetailView.SelectObj(model),
                            status = "Complete",
                            @base = baseConfig.DetailView.SelectObj(complete),
                            error = 0,
                            basemnemonic = baseConfig != null ? baseConfig.Mnemonic : string.Empty
                        });
                    }

                    uofw.Commit();

                    return Ok(new { model = config.DetailView.SelectObj(obj), status = "Success" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = "Error", message = ex.Message });
            }

        }

        [GenericAction("wizard")]
        [HttpPut]
        [Route("prev")]
        public async Task<IHttpActionResult> Prev<T>(string wizard, [FromBody] T model) where T : IWizardObject
        {
            try
            {
                var config = GetWizardConfig();
                var srv = GetWizardService<T>();

                using (var uofw = CreateTransactionUnitOfWork())
                {
                    var obj = await srv.PrevStepAsync(uofw, model, config);
                    uofw.Commit();

                    return Ok(new { model = config.DetailView.SelectObj(obj), status = "Success" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = "Error", message = ex.Message });
            }
        }

        [GenericAction("wizard")]
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Complete<T>(string wizard, [FromBody] T model) where T : IWizardObject
        {
            try
            {
                var config = GetWizardConfig();
                var baseConfig = GetObjectViewModelConfig();


                var srv = GetWizardService<T>();


                using (var uofw = CreateTransactionUnitOfWork())
                {
                    var obj = await srv.CompleteAsync(uofw, model);
                    uofw.Commit();

                    return Ok(new
                    {
                        model = config.DetailView.SelectObj(model),
                        @base = baseConfig.DetailView.SelectObj(obj),
                        error = 0,
                        basemnemonic = baseConfig != null ? baseConfig.Mnemonic : string.Empty
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { error = 1, message = ex.Message });
            }
        }
    }
}
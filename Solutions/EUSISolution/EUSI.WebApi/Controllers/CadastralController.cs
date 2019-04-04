using System;
using System.Web.Http;
using Base;
using Base.DAL;
using Base.Service;
using Base.Service.Log;
using Base.UI;
using Base.Utils.Common;
using EUSI.Entities.Accounting;
using EUSI.Entities.Models;
using EUSI.Services.Estate;
using WebApi.Attributes;
using WebApi.Controllers;
using WebApi.Models.Crud;

namespace EUSI.WebApi.Controllers
{
    /// <summary>
    /// Контроллер де
    /// </summary>
    internal class CadastralController : BaseApiController
    {
        private readonly ILogService _logger;

        public CadastralController(IViewModelConfigService viewModelConfigService, IUnitOfWorkFactory unitOfWorkFactory, ILogService logger) : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("eusi/updateTaxBaseCadastralObjects/{mnemonic}")]
        [GenericAction("mnemonic")]
        public IHttpActionResult UpdateTaxBaseCadastralObjects<T>(string mnemonic, [FromBody] SaveModel<T> save_model) where T : BaseObject
        {
            var sm = save_model as SaveModel<UpdateTaxBaseCadastralObjects>;

            if (sm?.model?.Year == null)
            {
                return Ok(new
                {
                    error = 1,
                    message = "Неправильный набор вводимых параметров.",
                });
            }

            try
            {
                var year = (int)sm?.model?.Year.Year;
                using (ITransactionUnitOfWork unitOfWork = CreateTransactionUnitOfWork())
                {
                    var tService = new UpdateTaxBaseCadastralObjectsServise();
                    tService.Update(year);
                }
                return Ok(new
                {
                    error = 0,
                    message = "ОИ обновлены."   
                });
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner(),
                });
            }
        }
    }
}
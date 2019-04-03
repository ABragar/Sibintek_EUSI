using System;
using System.Threading.Tasks;
using System.Web.Http;
using Base.Service;
using Base.UI;
using Base.UI.Service;
using WebApi.Attributes;

namespace WebApi.Controllers
{

    [CheckSecurityUser]
    [RoutePrefix("preset/{preset}")]
    class PresetController: ApiController
    {
        private readonly IServiceLocator _locator;

        public PresetController(IServiceLocator locator)
        {
            _locator = locator;
        }

        private IPresetService<T> GetPresetService<T>()
            where T: Preset
        {
            return _locator.GetService<IPresetService<T>>();
        }


        [GenericAction("preset")]
        [HttpDelete]
        [Route("")]
        public async Task<IHttpActionResult> Delete<T>(string preset,[FromBody]T model)
            where T : Preset
        {
            try
            {
                await GetPresetService<T>().DeleteAsync(model);

                return Ok(new { });
            }
            catch (Exception ex)
            {
                return Ok(new { error = ex.Message });
            }
        }

        [GenericAction("preset")]
        [HttpGet]
        [Route("{ownerName}")]
        public async Task<IHttpActionResult> Get<T>(string preset, string ownerName)
            where T : Preset
        {
            try
            {
                return Ok(await
                    GetPresetService<T>().GetAsync(ownerName));
            }
            catch (Exception ex)
            {
                return Ok(new { error = ex.Message });
            }
        }

        [GenericAction("preset")]
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Save<T>(string preset, T model)
            where T : Preset
        {
            try
            {
                return Ok(await
                    GetPresetService<T>().SaveAsync(model));
            }
            catch (Exception ex)
            {
                return Ok(new { error = ex.Message });
            }
        }
    }
}

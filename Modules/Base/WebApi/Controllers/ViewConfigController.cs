using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Http;
using Base;
using Base.ComplexKeyObjects;
using Base.ComplexKeyObjects.Common;
using Base.DAL;
using Base.Enums;
using Base.Extensions;
using Base.Service;
using Base.Service.Log;
using Base.UI;
using Base.UI.Extensions;
using Base.UI.Service;
using WebApi.Attributes;

namespace WebApi.Controllers
{
    [RoutePrefix("viewconfig")]
    public class ViewConfigController : BaseApiController
    {
        private readonly IUiFasade _uiFasade;
        private readonly ITypeRelationService _relationService;
        private readonly IViewModelConfigService _viewModelConfigService;
        private readonly ILogService _logger;

        public ViewConfigController(IViewModelConfigService viewModelConfigService, IUnitOfWorkFactory unitOfWorkFactory, IUiFasade uiFasade, ITypeRelationService relationService, ILogService logger) : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
            _uiFasade = uiFasade;
            _relationService = relationService;
            _viewModelConfigService = viewModelConfigService;
        }

        [HttpGet]
        [Route("getEditors")]
        public IHttpActionResult GetEditors([FromUri]string mnemonic)
        {
            return Ok(_uiFasade.GetEditors(mnemonic).Select(x => new
            {
                x.PropertyName,
                x.Title
            }));
        }

        [HttpGet]
        [Route("getColumns")]
        public IHttpActionResult GetColumns([FromUri]string mnemonic)
        {
            return Ok(_uiFasade.GetColumns(mnemonic).Select(x => new
            {
                x.PropertyName,
                x.Title
            }));
        }

        [HttpGet]
        [Route("getEditor/{objectType}/{propertyName}")]
        public IHttpActionResult GetEditor(string objectType, string propertyName)
        {
            var editor = _uiFasade.GetEditors(objectType).Select(x => new
            {
                x.PropertyName,
                x.Title,
                x.Description,
                x.Visible,
                x.IsReadOnly,
                Enable = !x.IsReadOnly,
                x.IsRequired,
                x.IsLabelVisible,
                x.TabName
            }).SingleOrDefault(x => x.PropertyName == propertyName);

            if (editor != null)
                return Ok(editor);

            return Ok(new {error = $"property [{propertyName}] is not found"});
        }

        [HttpGet]
        [Route("getColumn/{objectType}/{propertyName}")]
        public IHttpActionResult GetColumn(string objectType, string propertyName)
        {
            var column = _uiFasade.GetColumns(objectType).Select(x => new
            {
                x.PropertyName,
                x.Title,
                x.Visible,
                x.OneLine
            }).SingleOrDefault(x => x.PropertyName == propertyName);

            if (column != null)
                return Ok(column);

            return Ok(new { error = $"property [{propertyName}] is not found" });
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("lookupProperty/value/{mnemonic}/{id}")]
        public async Task<IHttpActionResult> GetLookupPropertyValue<T>(string mnemonic, int id)
            where T: BaseObject
        {
            var config = GetConfig();

            using (var uofw = CreateUnitOfWork())
            {
                var serv = GetQueryService<T>();
                var lookupProperty = config.LookupProperty.Text;
                if (serv == null)
                    return Ok(new
                    {
                        error = "service not found"
                    });

                var result = await serv.GetAll(uofw)
                    .Where(x => x.ID == id)
                    .Select("it." + lookupProperty)
                    .Cast<string>()
                    .SingleOrDefaultAsync();

                return Ok(new
                {
                    value = result
                });
            }
        }

        [HttpGet]
        [GenericAction("mnemonic")]
        [Route("lookupProperty/value/collection/{mnemonic}")]
        public async Task<IHttpActionResult> GetLookupPropertyValuesForCollection<T>(string mnemonic, [FromUri]int[] ids)
            where T: BaseObject
        {
            var config = GetConfig();

            using (var uofw = CreateUnitOfWork())
            {
                var serv = GetQueryService<T>();
                var lookupProperty = config.LookupProperty.Text;
                if (serv == null)
                    return Ok(new
                    {
                        error = "service not found"
                    });

                var result = await serv.GetAll(uofw)
                    .Where(x => ids.Contains(x.ID))
                    .Select("it." + lookupProperty)
                    .Cast<string>()
                    .ToListAsync();

                return Ok(new
                {
                    value = result
                });
            }
        }

        [HttpGet]
        [Route("extraid/{mnemonic}/{id}")]
        public IHttpActionResult GetExtraId(string mnemonic, int id)
        {
            var config = GetConfig();

            if (!config.Relations.Any())
            {
                return Ok(new { Mnemonic = mnemonic });
            }
            if (!typeof(IComplexKeyObject).IsAssignableFrom(config.TypeEntity))
            {
                return Ok(new { error = "not supported" });
            }

            try
            {
                using (var uofw = CreateUnitOfWork())
                {

                    IQueryable q = config.GetService<IQueryService<IComplexKeyObject>>().GetAll(uofw);

                    var m = q.Where("it.ID==@0", id).Select("it.ExtraID").SingleOrDefault();

                    if (m == null)
                    {
                        return Ok(new { error = "not found" });
                    }

                    return Ok(new { Mnemonic = m });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { error = "error" });
            }
        }

        [HttpGet]
        [Route("getRelations/{mnemonic}")]
        [GenericAction("mnemonic")]

        public IHttpActionResult GetRelations<T>(string mnemonic)
            where T : BaseObject
        {
            var relations = _relationService.GetRelations(typeof(T));

            return Ok(relations);
        }

        [HttpGet]
        [Route("{mnemonic}")]
        public IHttpActionResult GetViewModelConfig(string mnemonic)
        {
            var config = GetConfig();

            if (config == null)
                return Ok();

            return Ok(config.ToDto(_viewModelConfigService.Get));
        }

        [HttpGet]
        [Route("getMnemonics")]
        public IHttpActionResult GetMnemonics([FromUri]string search = null)
        {
            var configs = _viewModelConfigService.GetAll();

            if (!string.IsNullOrEmpty(search))
                configs = configs.Where(x => x.Title.Contains(search))
                    .Where(x => Base.Ambient.AppContext.SecurityUser.IsPermission(x.TypeEntity, TypePermission.Navigate));

            return Ok(configs
                .Select(x => new
                {
                    ID = x.Mnemonic,
                    Text = $"{x.Mnemonic} : {x.Title}"
                }));
        }

        [HttpGet]
        [Route("getTypes")]
        public IHttpActionResult GetTypes([FromUri]string search = null)
        {
            var configs = _viewModelConfigService.GetAll();

            if (!string.IsNullOrEmpty(search))
                configs = configs.Where(x => x.Title.StartsWith(search) || x.Mnemonic.StartsWith(search));
            
            return Ok(configs
                .Where(f => f.Mnemonic == f.TypeEntity.Name)
                .GroupBy(x => x.TypeEntity)                
                .Select(x => new
                {
                    ID = x.Key.GetTypeName(),
                    Text = x.First().Title + " - " + x.Key.GetTypeName()
                })
                .OrderBy(sort => sort.Text)
                );
        }
        [HttpGet]
        [Route("getTypeNames")]
        public IHttpActionResult GetTypeNames([FromUri]string search = null)
        {
            var configs = _viewModelConfigService.GetAll();

            if (!string.IsNullOrEmpty(search))
                configs = configs.Where(x => x.Title.StartsWith(search) || x.Mnemonic.StartsWith(search));

            return Ok(configs
                .Where(f => f.Mnemonic == f.TypeEntity.Name)
                .GroupBy(x => x.TypeEntity)
                .Select(x => new
                {
                    ID = x.Key.Name,
                    Text = x.Key.Name + " - " + x.First().Title
                })
                .OrderBy(sort => sort.Text)
                );
        }
    }
}
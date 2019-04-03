using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Base;
using Base.DAL;
using Base.Extensions;
using Base.Service;
using Base.Service.Log;
using Base.UI;
using Base.UI.DetailViewSetting;
using Base.UI.Service;
using WebApi.Attributes;

namespace WebApi.Controllers
{
    [CheckSecurityUser]
    [RoutePrefix("standard")]
    internal class StandardController : BaseApiController
    {
        private readonly IUiFasade _uiFasade;
        private readonly IAutoMapperCloner _cloner;
        private readonly IViewModelConfigService _viewModelConfigService;
        private readonly IDvSettingService<DvSettingForType> _dvSettingService;
        private readonly IDvSettingManager _dvSettingManager;
        private readonly ILogService _logger;

        public StandardController(IViewModelConfigService viewModelConfigService, IUnitOfWorkFactory unitOfWorkFactory, IUiFasade uiFasade, IAutoMapperCloner cloner, IDvSettingService<DvSettingForType> dvSettingService, IDvSettingManager dvSettingManager, ILogService logger) 
            : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
            _viewModelConfigService = viewModelConfigService;
            _uiFasade = uiFasade;
            _cloner = cloner;
            _dvSettingService = dvSettingService;
            _dvSettingManager = dvSettingManager;
        }

        //TODO fix api explorer
        [HttpGet]
        [Route("{mnemonic}/create_default")]
        [GenericAction("mnemonic")]
        public IHttpActionResult CreateDefault<T>([FromUri]string mnemonic)
            where T : BaseObject
        {

            // var x = Configuration.Services.GetApiExplorer().ApiDescriptions;

            try
            {
                var config = GetConfig();
                var service = config.GetService<IService>() as IBaseObjectService<T>;

                using (var unit_of_work = CreateUnitOfWork())
                {
                    var base_object = service != null ? service.CreateDefault(unit_of_work) : Activator.CreateInstance(config.TypeEntity);

                    var model = config.DetailView.SelectObj(base_object);

                    return Ok(new { model });
                }
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    error = 1,
                    message = e.Message
                });
            }
        }

        [HttpGet]
        [Route("getUiEnum/{type}")]
        public IHttpActionResult GetUiEnum(string type)
        {
            try
            {
                var uienum = _uiFasade.GetUiEnum(Type.GetType(type));

                return Ok(new
                {
                    Type = uienum.Type,
                    Title = uienum.Title,
                    Values = uienum.Values.ToDictionary(x => x.Value, x => new
                    {
                        Value = x.Value,
                        Title = x.Title,
                        Color = x.Icon.Color ?? "#428bca",
                        Icon = x.Icon.Value ?? "",// "mdi mdi-multiplication",
                    })
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    error = 1,
                    message = e.Message
                });
            }
        }

        [HttpPost]
        [Route("clone/{mnemonic}/{id}")]
        [GenericAction("mnemonic")]
        public IHttpActionResult Clone<T>(string mnemonic, int id)
            where T:BaseObject
        {
            try
            {
                using (var uow = CreateUnitOfWork())
                {
                    var serv = GetBaseObjectService<T>();
                    var source = serv.Get(uow, id);
                    var dest = (T)_cloner.Copy(source);
                    var clone = serv.Create(uow, dest);
                    return Ok(new
                    {
                        model = GetConfig().DetailView.GetData(uow, serv, clone.ID)
                    });
                }
            }
            catch (Exception error)
            {
                return Ok(new
                {
                    error = error.Message,
                    code = -1,
                });
            }
        }

        [HttpGet]
        [Route("dvSettings/tree")]
        public async Task<IHttpActionResult> GetDvSettings_Tree([FromUri] int? id = null, [FromUri] string objectType = null)
        {
            try
            {
                using (var uow = CreateUnitOfWork())
                {
                    var config = _viewModelConfigService.Get(nameof(DvSettingForType));

                    var pifo = config.TypeEntity.GetProperty(config.LookupProperty.Text);
                    var settings = _dvSettingService.GetDvSettings(uow, objectType);

                    var ids = settings.Select(x => x.ID).ToList();

                    var parents = settings
                        .Where(x => x.ParentID != null)
                        .Select(x => x.ParentID.Value)
                        .Where(x => ids.Contains(x))
                        .Distinct().ToDictionary(x => x);

                    settings = id != null
                        ? settings.Where(x => x.ParentID == id)
                        : settings.Where(x => x.ParentID == null);

                    var res = (await settings.ToListAsync()).Select(a =>
                        new
                        {
                            id = a.ID,
                            Title = pifo.GetValue(a),
                            hasChildren = parents.ContainsKey(a.ID),
                            isRoot = a.IsRoot
                        });

                    return Ok(res);
                }
            }
            catch (Exception error)
            {
                return Ok(new { error = error.Message });
            }
        }

        [HttpGet]
        [Route("dvSettings/{mnemonic}/{id}")]
        [GenericAction("mnemonic")]
        public IHttpActionResult GetDvSettings<T>(string mnemonic, int id)
            where T: BaseObject
        {
            try
            {
                using (var uow = CreateUnitOfWork())
                {
                    var config = GetConfig();

                    var serv = GetBaseObjectService<T>();

                    var model = serv.Get(uow, id);

                    var settings =
                        _dvSettingManager.GetSettingsForType(uow, config.TypeEntity, model).Select(x => new { x.Name, x.ID });

                    return Ok(settings);
                }
            }
            catch (Exception error)
            {
                return Ok(new { error = error.Message });
            }
        }
    }
}
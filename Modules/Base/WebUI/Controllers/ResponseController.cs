using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using Base.UI.Presets;
using Base.UI.Service;
using System.Web.Mvc;
using Base.DAL;
using Base.Service;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Request;
using CorpProp.Entities.Request.ResponseCells;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using CorpProp.Services.Response;
using CorpProp.Services.Response.Fasade;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using WebUI.Helpers;
using WebUI.Models.CorpProp.Response;
using EnumerableExtensions = Base.Extensions.EnumerableExtensions;
using WebUI.Models.CorpProp.Response.Config;

namespace WebUI.Controllers
{
    public sealed class DynamicModelBinder : IModelBinder
    {
        private const string ContentType = "application/json";

        public DynamicModelBinder(bool useModelName = false)
        {
            this.UseModelName = useModelName;
        }

        public bool UseModelName { get; private set; }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            dynamic data = null;

            var any = false;
            if (controllerContext.HttpContext.Request.AcceptTypes != null)
                if (controllerContext.HttpContext.Request.AcceptTypes.Any(x => x.StartsWith(ContentType, StringComparison.OrdinalIgnoreCase)))
                    any = true;
            if (any || controllerContext.HttpContext.Request.ContentType.StartsWith(ContentType, StringComparison.OrdinalIgnoreCase))
            {
                controllerContext.HttpContext.Request.InputStream.Position = 0;

                using (var reader = new StreamReader(controllerContext.HttpContext.Request.InputStream))
                {
                    var payload = reader.ReadToEnd();

                    if (string.IsNullOrWhiteSpace(payload) == false)
                    {
                        data = JsonConvert.DeserializeObject<dynamic>(payload);

                        if (this.UseModelName == true)
                        {
                            data = data[bindingContext.ModelName];
                        }
                    }
                }
            }

            return data;
        }
    }

    [Serializable]
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class DynamicModelBinderAttribute : CustomModelBinderAttribute
    {
        public DynamicModelBinderAttribute(bool useModelName = false)
        {
            this.UseModelName = useModelName;
        }

        public bool UseModelName { get; private set; }

        public override IModelBinder GetBinder()
        {
            return new DynamicModelBinder(this.UseModelName);
        }
    }

    public class ParameterBinder : IModelBinder
    {
        public string ActualParameter { get; private set; }

        public ParameterBinder(string actualParameter)
        {
            this.ActualParameter = actualParameter;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object id = controllerContext.RouteData.Values[this.ActualParameter];
            return id;
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class BindParameterAttribute : CustomModelBinderAttribute
    {
        public string ActualParameter { get; private set; }

        public BindParameterAttribute(string actualParameter)
        {
            this.ActualParameter = actualParameter;
        }

        public override IModelBinder GetBinder()
        {
            return new ParameterBinder(this.ActualParameter);
        }
    }

    public class ResponseController : BaseController
    {
        private readonly IPresetService<MenuPreset> _menuPresetService;
        private readonly IResponseDynamicQueryService _dynamicQueryService;

        public ResponseController(IBaseControllerServiceFacade serviceFacade, IPresetService<MenuPreset> menuPresetService, IResponseDynamicQueryService dynamicQueryService)
            : base(serviceFacade)
        {
            _menuPresetService = menuPresetService;
            _dynamicQueryService = dynamicQueryService;
        }

        public static IQueryable<RequestColumn> GetColumns(IUnitOfWork uofw, int? responseId, int? requestId)
        {
            IQueryable<RequestColumn> columns = null;
            if (responseId != null)
            {
                columns = ResponseGridConfigFasade.GetColumnsByResponse(uofw, responseId);
            }

            if (requestId != null)
            {
                if (columns == null)
                {
                    columns = ResponseGridConfigFasade.GetColumnsByRequest(requestId, uofw);
                }
            }
            return columns;
        }

        private const string SocietyIdFieldName = "columnSocietyID";

        public PartialViewResult GetResponseGrid(string widgetId, int? requestId = null, int? responseId = null)
        {

            if (requestId == null && responseId == null)
                throw new ArgumentNullException("ids");

            IList<ResponseGridConfigProperty> configProperties = new List<ResponseGridConfigProperty>();

            using (var uofw = CreateUnitOfWork())
            {
                var columns = GetColumns(uofw, responseId, requestId);
                var lcolumns = columns.ToList();

                ResponseGridConfigPropertyFabric responseGridConfigPropertyFabric =
                    new ResponseGridConfigPropertyFabric(uofw);
                EnumerableExtensions.ForEach(lcolumns,
                    column => configProperties.Add(responseGridConfigPropertyFabric.Create(column)));

                int? rowsCount = null;
                if (responseId != null)
                {
                    var rCount = ResponseGridConfigFasade.ResponseRows(uofw, responseId.Value)?.Count();
                    if (rCount != null)
                        rowsCount = rCount <= 0 ? null : rCount;
                }

                if (requestId != null)
                {
                    var societyColumn = new RequestColumn()
                    {
                        TypeData = new TypeData()
                                   {
                                       Code = "Society"
                                   },
                         Name = "ОГ"
                    };
                    configProperties.Insert(0, responseGridConfigPropertyFabric.Create(societyColumn, SocietyIdFieldName));
                    if (configProperties.Count > 1)
                        configProperties[1].Visible = false;
                }

                var model = new ResponseGridModel(this)
                {
                    ResponseID = responseId,
                    RequestID = requestId,

                    Config = new ResponseGridConfig()
                    {
                        Columns = configProperties,
                        RowsCount = rowsCount
                    }
                };

                return PartialView("ResponseGrid", model);
            }

        }

        public JsonNetResult GetRequestColumnItems([DataSourceRequest] DataSourceRequest request, int? columnId)
        {
            if (columnId == null)
                throw new ArgumentException(nameof(columnId));
            using (var uow = this.CreateUnitOfWork())
            {
                var requestColumnItems = uow.GetRepository<RequestColumnItems>().All()
                                                                                .Where(items => items.RequestColumnID == columnId)
                                                                                .Select(items => new {Item = items.Item, ID = items.ID });
                return new JsonNetResult(requestColumnItems.ToDataSourceResult(request));
            }
        }

        public JsonNetResult GetGridResponseData([DataSourceRequest] DataSourceRequest request, int? requestId = null, int? responseId = null)
        {
            if (requestId == null && responseId == null)
                throw new ArgumentNullException($"{nameof(requestId)} and {nameof(responseId)}");

            using (var uofw = CreateUnitOfWork())
            {
                IQueryable<RequestColumn> columns = null;
                CellsData cells = new CellsData();


                if (responseId != null)
                {
                    columns = ResponseGridConfigFasade.GetColumnsByResponse(uofw, responseId);

                    cells.InitRepositoryes(uofw, $"LinkedResponseID = {responseId.Value}");
                    //TODO перенести в инициализатор сопоставление определения типов
                }

                if (requestId != null)
                {
                    if (columns == null)
                    {
                        columns = ResponseGridConfigFasade.GetColumnsByRequest(requestId, uofw);
                    }

                    cells.Booleans = ResponseGridConfigFasade.ResponseCells<ResponseCellBoolean, bool?>(uofw, requestId);
                    cells.DateTimes = ResponseGridConfigFasade.ResponseCells<ResponseCellDateTime, DateTime?>(uofw, requestId);
                    cells.Decimals = ResponseGridConfigFasade.ResponseCells<ResponseCellDecimal, decimal?>(uofw, requestId);
                    cells.Dicts = ResponseGridConfigFasade.ResponseCells<ResponseCellDict, int?>(uofw, requestId);
                    cells.Doubles = ResponseGridConfigFasade.ResponseCells<ResponseCellDouble, double?>(uofw, requestId);
                    cells.Floats = ResponseGridConfigFasade.ResponseCells<ResponseCellFloat, float?>(uofw, requestId);
                    cells.Ints = ResponseGridConfigFasade.ResponseCells<ResponseCellInt, int?>(uofw, requestId);
                    cells.Strings = ResponseGridConfigFasade.ResponseCells<ResponseCellString, string>(uofw, requestId);
                }

                var table = new TableData()
                {
                    ResponseColumns = columns
                };

                var requestSelector = new RequestSelector(cells, table, uofw);

                IQueryable data = requestSelector.GetResponseData();

                if (data == null)
                    return new JsonNetResult(new object[0]);

                if (requestId != null)
                {
                    table.Request = uofw.GetRepository<Request>().All();
                    table.Response = uofw.GetRepository<Response>().All();
                    table.ResponseRow = uofw.GetRepository<ResponseRow>().All();
                    table.SibUser = uofw.GetRepository<SibUser>().All();
                    table.Society = uofw.GetRepository<Society>().All();

                    data = requestSelector.AppendSociety(data, SocietyIdFieldName);
                    if (data == null)
                        return new JsonNetResult(new object[0]);
                }

                try
                {
                    return new JsonNetResult(data.ToDataSourceResult(request));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonNetResult SaveDynamicObject([DynamicModelBinder]dynamic savedObject, string id)
        {
            var iid = int.Parse(id);

            using (var uofw = CreateUnitOfWork())
            {
                var columns = ResponseGridConfigFasade.GetColumnsByResponse(uofw, iid);

                var response = uofw.GetRepository<Response>().Find(iid);

                CellsData cells = new CellsData();
                if (response.RequestID == null)
                    return new JsonNetResult(new { error = 1 });

                cells.InitRepositoryes(uofw, $"LinkedResponse.RequestID = {response.RequestID.Value}");

                TableData table = new TableData()
                {
                    ResponseColumns = columns
                };

                RequestUpdate requestUpdate;
                try
                {
                    requestUpdate = new RequestUpdate(response, cells, table, uofw);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                var rowId = requestUpdate.SaveRowModel(savedObject);
                return new JsonNetResult(new { error = 0, rowId });
            }
        }


        [System.Web.Mvc.HttpDelete]
        public JsonNetResult DeleteRow(int? responseId, int? rowId)
        {
            Debug.Assert(responseId != null, "responseId != null");
            var responseIdInt = responseId.Value;
            Debug.Assert(rowId != null, "rowId != null");
            var rowIdInt = rowId.Value;

            using (var uofw = CreateUnitOfWork())
            {
                var columns = ResponseGridConfigFasade.GetColumnsByResponse(uofw, responseIdInt);

                var response = uofw.GetRepository<Response>().Find(responseIdInt);

                if (response.RequestID == null)
                    return new JsonNetResult(new { error = 1 });

                CellsData cells = new CellsData();
                cells.InitRepositoryes(uofw, $"LinkedResponse.RequestID = {response.RequestID.Value}");

                TableData table = new TableData()
                {
                    ResponseColumns = columns
                };

                var requestUpdate = new RequestUpdate(response, cells, table, uofw);
                rowIdInt = requestUpdate.DeleteRow(rowIdInt);
                return new JsonNetResult(new { error = 0, rowId = rowIdInt });
            }
        }

        //[System.Web.Mvc.HttpPost]
        public JsonNetResult GetCellObjectLookupTitleById([DataSourceRequest] DataSourceRequest request, int id,
            string mnemonic)
        {
            using (var uofw = CreateUnitOfWork())
            {
                var serv = GetService<IQueryService<object>>(mnemonic);
                var titleField = GetViewModelConfig(mnemonic).LookupProperty.Text;
                var q = serv.GetAll(uofw).Where("$.ID = @0", id).Select($"new ({titleField} as Title)");
                return new JsonNetResult(q.ToDataSourceResult(request));

            }
        }

        public PartialViewResult GetResponseInstructionToolbar(int? id)
        {
            return PartialView("ResponseInstructionToolbar", id);
        }

        public JsonNetResult GetInstructionByResponseId([DataSourceRequest] DataSourceRequest rq, int? id)
        {
            using (var uow = CreateUnitOfWork())
            {
                var columns = ResponseGridConfigFasade.GetColumnsByResponse(uow, id);
                var columnsInstruction = columns.Select(column => new {Name = column.Name, Instruction = column.Instruction});

                return new JsonNetResult(columnsInstruction.ToDataSourceResult(rq));
            }

        }
    }
}
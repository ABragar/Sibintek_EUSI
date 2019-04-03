using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using DAL.EF;
using DAL.Entities;
using NLog;
using ReportStorage.Exceptions;
using ReportStorage.Service;
using RestService.Identity;
using RestService.Models;
using RestService.Service;
namespace RestService.Controllers
{
    [Authorize]
    [RoutePrefix("manager")]
    public class ReportManagerController : ApiController
    {
        private static ILogger _log = LogManager.GetCurrentClassLogger();
        private readonly IFileService _fileService;
        private readonly IReportStorageService _reportStorageService;
               
        public ReportManagerController(IReportStorageService reportStorageService, IFileService fileService)
        {
            _reportStorageService = reportStorageService;
            _fileService = fileService;
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage Interface()
        {
            var sb = new StringBuilder("<html><body>");
            var methods = typeof(ReportManagerController).GetMethods();
            sb.AppendLine("<ul>");
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(RouteAttribute), false).Cast<RouteAttribute>().Where(attribute => attribute.Template != String.Empty);
                foreach (var attribute in attributes)
                    sb.AppendLine(
                        $"<li><a href=\"manager\\{attribute.Template}\">\\manager\\{attribute.Template}</a></li>");
            }

            sb.AppendLine("</ul>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(sb.ToString());
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        [HttpPost]
        [Route("upload")]
        public IHttpActionResult Upload()
        {
            HttpPostedFile file = null;

            var request = HttpContext.Current.Request;

            if (!HttpContext.Current.Request.GetOwinContext().Authentication.User.GetUserInfo().IsAdmin)
                return StatusCode(HttpStatusCode.Forbidden);

            if (request.Files.Count > 0) {file = request.Files[0];}

            if (file == null) {return Ok(new {error = "Нет файла"});}

            try
            {
                return Ok(new {GuidId = _fileService.CreateFile(file)});
            }
            catch (ObjectValidationException e)
            {
                return Ok(new {error = e.Message});
            }
            catch (Exception)
            {
                return Ok(new {error = "Произошла непредвиденная ошибка. Обратитесь к администратору"});
            }
        }

        [HttpPost]
        [Route("create")]
        public IHttpActionResult Create(Report obj)
        {
            if (!HttpContext.Current.Request.GetOwinContext().Authentication.User.GetUserInfo().IsAdmin)
                return StatusCode(HttpStatusCode.Forbidden);

            try
            {
                using (var context = new ReportDbContext())
                {
                    _reportStorageService.Create(context, obj);
                }

                return Ok(new {message = "Объект успешно создан"});
            }
            catch (ObjectValidationException e)
            {
                return Ok(new {error = e.Message});
            }
            catch (Exception)
            {
                return Ok(new {error = "Произошла непредвиденная ошибка. Обратитесь к администратору"});
            }
        }

        [HttpGet]
        [Route("list")]
        public IHttpActionResult GetAll()
        {
            _log.Debug("Begin method");
            var request = HttpContext.Current.Request;
            _log.Debug("Request: {0}", request.InputStream);
            try
            {
                IEnumerable<ReportModel> result;
                using (var context = new ReportDbContext())
                {
                    var q = _reportStorageService.GetAll(context);

                    _log.Debug("Count of reports from storage: {0}", q.Count());

                    var user = request.GetOwinContext().Authentication.User.GetUserInfo();
                    _log.Debug("UserId: {0}; IsAdmin: {1}; CategoryIds: {2}", user.UserId, user.IsAdmin,
                        user.CategoryIds);
                    if (!user.IsAdmin)
                    {
                        var categories = q.Where(x => x.UserCategories != null && x.UserCategories != "")
                            .Select(c => c.UserCategories).AsEnumerable();

                        var tokenCategories = user.CategoryIds.Replace("@", "").Split(';');

                        categories = categories.Where(x => x.Split(',').Intersect(tokenCategories).Any());

                        q = q.Where(x => categories.Contains(x.UserCategories));
                    }

                    result = q.ToList().Where(r => r.Hidden != true).Select(c => new ReportModel
                    {
                        ID = c.ID,
                        Name = c.Name,
                        GuidId = c.GuidId.ToString("N"),
                        Extension = c.Extension,
                        Description = c.Description,
                        Code = c.Code,
                        Params = c.Params,
                        ReportType = c.ReportType,
                        Module = c.Module,
                        Number = c.Number,
                        ReportPublishCode = c.ReportPublishCode,
                        ReportVersion = c.ReportVersion
                    });

                    _log.Debug("Count of result: {0}", result.Count());
//                    if (_log.IsDebugEnabled)
//                    {
//                        StringBuilder resultReports = new StringBuilder("Result reports:").AppendLine();
//                        foreach (ReportModel model in result)
//                        {
//                            resultReports.AppendLine($"GUID: {model.GuidId}; Name: {model.Name}");
//                        }
//                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return InternalServerError(new Exception("Произошла непредвиденная ошибка. Обратитесь к администратору",
                    ex));
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var user = HttpContext.Current.Request.GetOwinContext().Authentication.User.GetUserInfo();

            _log.Debug("UserId: {0}; IsAdmin: {1}; CategoryIds: {2}", user.UserId, user.IsAdmin, user.CategoryIds);

            if (!user.IsAdmin)
                return StatusCode(HttpStatusCode.Forbidden);

            try
            {
                using (var context = new ReportDbContext())
                {
                    Report report = _reportStorageService.Get(context, id);
                    _reportStorageService.Delete(context, id);
                    _fileService.ToHistoryFile(report);
                }

                return Ok(new {message = "Объект успешно удален"});
            }
            catch (ObjectValidationException e)
            {
                return Ok(new {error = e.Message});
            }
            catch (Exception e)
            {
                _log.Error(e);
                return Ok(new {error = "Произошла непредвиденная ошибка. Обратитесь к администратору"});
            }
        }

        [HttpPost]
        [Route("update")]
        public IHttpActionResult Update(Report obj)
        {
            if (!HttpContext.Current.Request.GetOwinContext().Authentication.User.GetUserInfo().IsAdmin)
                return StatusCode(HttpStatusCode.Forbidden);

            try
            {
                using (var context = new ReportDbContext())
                {
                    _reportStorageService.Update(context, obj);
                }

                return Ok(new {message = "Объект успешно обновлен"});
            }
            catch (ObjectValidationException e)
            {
                return Ok(new {error = e.Message});
            }
            catch (Exception)
            {
                return Ok(new {error = "Произошла непредвиденная ошибка. Обратитесь к администратору"});
            }
        }

        [HttpGet]
        [Route("download/{id}")]
        public HttpResponseMessage Download(int id)
        {
            if (!HttpContext.Current.Request.GetOwinContext().Authentication.User.GetUserInfo().IsAdmin)
                return Request.CreateResponse(HttpStatusCode.Forbidden);

            using (var context = new ReportDbContext())
            {
                var obj = _reportStorageService.Get(context, id);

                if (obj == null)
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));

                var stream = _fileService.GetStream(obj);

                if (stream == null)
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));

                var responce = Request.CreateResponse(HttpStatusCode.OK);

                responce.Content = new StreamContent(stream);
                responce.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                     FileName = obj.Name + obj.Extension
                };

                return responce;
            }
        }

        [HttpGet]
        [Route("get/{id}")]
        public IHttpActionResult Get(int id)
        {
            var user = HttpContext.Current.Request.GetOwinContext().Authentication.User.GetUserInfo();

            _log.Debug("UserId: {0}; IsAdmin: {1}; CategoryIds: {2}, ReportId:{3}", user?.UserId, user?.IsAdmin,
                user?.CategoryIds, id);

            Report result;
            using (var context = new ReportDbContext())
            {
                result = _reportStorageService.Get(context, id);

                if (result == null)
                {
                    _log.Debug("Report with id = {id} not found.");
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                }
            }

            _log.Debug(
                $"Return report: {{Name:{result.Name}, Guid:{result.GuidId}, Params:[{string.Join(", ", result.Params)}]}}");
            return Ok(new ReportModel()
            {
                Name = result.Name,
                ID = result.ID,
                GuidId = result.GuidId.ToString("N"),
                Extension = result.Extension,
                Description = result.Description,
                UserCategories = result.UserCategories,
                Params = result.Params,
                Code = result.Code,
                RelativePath = result.RelativePath,
                ReportType = result.ReportType,
                Module = result.Module,
                Number = result.Number,
                ReportPublishCode = result.ReportPublishCode,
                ReportVersion = result.ReportVersion
            });
        }

        [HttpGet]
        [Route("getByCode/{code}")]
        public IHttpActionResult Get(string code)
        {
            Report result;
            using (var context = new ReportDbContext())
            {               

                var reports = _reportStorageService.GetAll(context).Where(w => w.Hidden != true && w.Code == code);
                if (reports.Count() > 1)
                {
                    _log.Debug("getByCode / More than one report was found in the system");
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.MultipleChoices));
                }
                result = reports.FirstOrDefault();

                if (result == null)
                {
                    _log.Debug("Report with id = {id} not found.");
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                }
            }

            _log.Debug(
                $"Return report: {{Name:{result.Name}, Guid:{result.GuidId}, Params:[{string.Join(", ", result.Params)}]}}");

            return Ok(new ReportModel()
            {
                Name = result.Name,
                ID = result.ID,
                GuidId = result.GuidId.ToString("N"),
                Extension = result.Extension,
                Description = result.Description,
                UserCategories = result.UserCategories,
                Params = result.Params,
                Code = result.Code,
                RelativePath = result.RelativePath,
                ReportType = result.ReportType,
                Module = result.Module,
                Number = result.Number,
                ReportPublishCode = result.ReportPublishCode,
                ReportVersion = result.ReportVersion
            });
        }
    }
}
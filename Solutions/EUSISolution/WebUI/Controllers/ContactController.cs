using System;
using System.Collections.Specialized;
using System.IO;
using Base.Contact.Entities;
using Base.Service;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Base.Contact.Service.Abstract;
using Base.DAL;
using Base.Extensions;
using Base.Security;
using Base.UI.Filter;
using Base.UI.Service.Abstract;
using Base.Utils.Common;
using Base.Utils.Common.Caching;
using ImageResizer;
using Kendo.Mvc.UI;
using WebUI.Extensions;
using WebUI.Helpers;
using WebUI.Models.Contact;

namespace WebUI.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IBaseContactService<Company> _baseCompanyService;
        private readonly IEmployeeService _baseEmployeeService;
        private readonly IEmployeeUserService _employeeUserService;
        private readonly IUserService<User> _userService;
        private readonly ISimpleCacheWrapper _cacheWrapper;
        private readonly IFileSystemService _fileSystemService;
        private readonly IBaseObjectService<Department> _baseObjectServiceDepartment;
        private readonly IMnemonicFilterService<MnemonicFilter> _mnemonicFilterService;

        public ContactController(IBaseControllerServiceFacade serviceFacade, IEmployeeUserService employeeUserService,
            IUserService<User> userService, ISimpleCacheWrapper cacheWrapper, IFileSystemService fileSystemService,
            IBaseContactService<Company> baseCompanyService, IEmployeeService baseEmployeeService, IBaseObjectService<Department> baseObjectServiceDepartment, IMnemonicFilterService<MnemonicFilter> mnemonicFilterService) : base(serviceFacade)
        {
            _employeeUserService = employeeUserService;
            _userService = userService;
            _cacheWrapper = cacheWrapper;
            _fileSystemService = fileSystemService;
            _baseCompanyService = baseCompanyService;
            _baseEmployeeService = baseEmployeeService;
            _baseObjectServiceDepartment = baseObjectServiceDepartment;
            _mnemonicFilterService = mnemonicFilterService;
        }

        public ActionResult GetCompanyToolbar(int? companyID)
        {
            return PartialView("_CompanyToolbar", new CompanyToolbarModel(companyID));
        }

        public JsonNetResult GetCompanyDepartments(int companyID)
        {
            using (var uow = CreateUnitOfWork())
            {
                var config = GetViewModelConfig(nameof(Department));

                var service = config.GetService<IBaseObjectService<Department>>();

                var departments = service.GetAll(uow).Where(x => x.CompanyID == companyID);

                var parents = departments.Where(x => x.ParentID.HasValue).Select(x => x.ParentID.Value).ToArray();

                var res = departments.Select(x => new
                {
                    x.ID,
                    x.Name,
                    hasChildren = parents.Contains(x.ID),
                    isRoot = x.IsRoot
                });

                return new JsonNetResult(res);
            }
        }

        public JsonNetResult GetUserCompany()
        {
            using (var uow = CreateUnitOfWork())
            {
                var company = _employeeUserService.GetUserCompany(uow);

                if (company != null)
                    return new JsonNetResult(new { ID = company.ID });
                else
                    return new JsonNetResult(new { error = "Компаний не найдено" });
            }
        }


        //todo: перенести в сервис
        private class ContactInfo
        {
            public string ID { get; set; }
            public string Title { get; set; }
            public Guid ImageUid { get; set; }
            public string Email { get; set; }
        }

        //todo: перенести в сервис
        private IQueryable<ContactInfo> GetContacts(IUnitOfWork uow)
        {
            return _userService.GetAll(uow).SelectMany(x => x.Profile.Emails.Select(e=>new  { e,x })).Select(x => new ContactInfo()
            {
                ID = "User:" + x.x.ID,
                Title = x.e.BaseProfile.FullName,
                ImageUid = x.e.BaseProfile.Image != null ? x.e.BaseProfile.Image.FileID : Guid.Empty,
                Email = x.e.Email,
            }).Union(_baseCompanyService.GetAll(uow).SelectMany(x => x.Emails.Select(e => new { e, x })).Select(x => new ContactInfo()
            {
                ID = "Company:" + x.x.ID,
                Title = x.e.Contact.Title,
                ImageUid = x.e.Contact.Image != null ? x.e.Contact.Image.FileID : Guid.Empty,
                Email = x.e.Email
            })).Union(_baseEmployeeService.GetAll(uow).SelectMany(x => x.Emails.Select(e => new { e, x })).Select(x => new ContactInfo()
            {
                ID = "Employee:" + x.x.ID,
                Title = x.e.Contact.Title,
                ImageUid = x.e.Contact.Image != null ? x.e.Contact.Image.FileID : Guid.Empty,
                Email = x.e.Email
            }));
        }

        public ActionResult GetEmails(string title, string email)
        {
            using (var uow = CreateUnitOfWork())
            {
                var q = GetContacts(uow)
                    .Distinct()
                    .OrderBy(x => x.Title)
                    .Skip(0)
                    .Take(50);

                if (!string.IsNullOrWhiteSpace(title) || !string.IsNullOrWhiteSpace(email))
                {
                    q = q.Where(x => x.Title.Contains(title) || x.Email.Contains(email));
                }

                return new JsonNetResult(q.ToList());
            }
        }

        public async Task<ActionResult> GetByEmail(string email)
        {
            using (var uow = CreateUnitOfWork())
            {
                var contact = await GetContacts(uow).Where(x => x.Email == email).FirstOrDefaultAsync();

                return new JsonNetResult(contact);
            }
        }

        private static readonly CacheAccessor<FileResult> GetImageGroup =
            new CacheAccessor<FileResult>(TimeSpan.FromDays(5));

        private const int MaxImageWidth = 3200;

        public FileResult GetPhotoByEmail(string email, int? width, int? height)
        {
            using (var uow = CreateUnitOfWork())
            {
                var image = GetContacts(uow).Where(x => x.Email == email).Select(x => x.ImageUid).FirstOrDefault();

                string key = $"[{image}][{width ?? 0}][{height ?? 0}]";

                return _cacheWrapper.GetOrAdd(GetImageGroup, key, () =>
                {
                    using (var ms = new MemoryStream())
                    {
                        var instructions = new NameValueCollection
                        {
                            {"mode", ImageMode.Resolve("crop")},
                            {"anchor", ImageAnchor.Resolve("middlecenter")},
                            {"ignoreicc", "true"}
                        };

                        if (!width.HasValue && !height.HasValue)
                        {
                            instructions.Add("width", MaxImageWidth.ToString());
                            instructions.Add("scale", ImageScale.DownScaleOnly);
                        }
                        else
                        {
                            instructions.Add("scale", ImageScale.Resolve("both"));

                            if (width.HasValue)
                                instructions.Add("width", width.ToString());

                            if (height.HasValue)
                                instructions.Add("height", height.ToString());
                        }

                        string path = _fileSystemService.GetFilePath(image);

                        var defImageObject = Properties.Resources.NoPhoto;

                        var imageJob = System.IO.File.Exists(path)
                            ? new ImageJob(path, ms, new Instructions(instructions))
                            : new ImageJob(defImageObject, ms, new Instructions(instructions));

                        ImageBuilder.Current.Build(imageJob);

                        return File(ms.ToArray(), imageJob.ResultMimeType);
                    }
                });
            }
        }

        [Obsolete]
        //TODO: => webapi
        public async Task<JsonNetResult> KendoUI_CollectionRead([DataSourceRequest] DataSourceRequest request, string mnemonic, int? categoryID, bool? allItems, string searchStr, string extrafilter, string[] columns, string mnemonicFilterId)
        {
            var serv = GetService<IQueryService<Company>>(mnemonic);
            try
            {
                using (var uofw = this.CreateUnitOfWork())
                {
                    //var config = this.GetViewModelConfig(mnemonic);

                    //var allCompany = serv.GetAll(uofw).Select(x => new { x.ID, x.Title });
                    //var allDepartmnetn = _baseObjectServiceDepartment.GetAll(uofw).GroupBy(x => x.Company.ID).Select(x => new { ID = x.Key, Count = x.Count() });

                    //var join = from l in allCompany
                    //           join r in allDepartmnetn on l.ID equals r.ID into lrs
                    //           from lr in lrs.DefaultIfEmpty()
                    //           select new { Company = l, Count = lr == null ? 0 : lr.Count };

                    //IQueryable q = join.OrderByDescending(x => x.Count).Select(x => x.Company);
                    //string mnemonicFilter = await _mnemonicFilterService.GetMnemonicFilter(mnemonic, mnemonicFilterId, extrafilter);
                    //q = q.Filter(this, uofw, config, mnemonicFilter);

                    //q = q.FullTextSearch(searchStr, CacheWrapper);

                    //return new JsonNetResult(await q.ToDataSourceResultAsync(request, config));

                    return new JsonNetResult(new {});
                }
            }
            catch (Exception e)
            {
                var res = new DataSourceResult()
                {
                    Errors = e.Message
                };

                return new JsonNetResult(res);
            }
        }
    }
}
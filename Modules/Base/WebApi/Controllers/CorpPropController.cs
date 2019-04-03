using Base;
using Base.Audit.Entities;
using Base.DAL;
using Base.Entities.Complex;
using Base.Extensions;
using Base.Notification.Service.Abstract;
using Base.Security;
using Base.Security.ObjectAccess;
using Base.Security.Service.Abstract;
using Base.Service;
using Base.UI;
using Base.UI.DetailViewSetting;
using Base.UI.Extensions;
using Base.UI.Presets;
using Base.UI.Service;
using Base.Utils.Common;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Asset;
using CorpProp.Entities.Base;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Document;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Import;
using CorpProp.Entities.Law;
using CorpProp.Entities.NSI;
using CorpProp.Entities.ProjectActivity;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.RosReestr.Migration;
using CorpProp.Services.Base;
using CorpProp.Services.Settings;
using CorpProp.Services.Subject;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using CorpProp.Services.Import;
using WebApi.Attributes;
using AppContext = Base.Ambient.AppContext;
using WebHttp = System.Web.Http;
using Base.Service.Crud;
using Base.Service.Log;

namespace WebApi.Controllers
{
    [CheckSecurityUser]
    [WebHttp.RoutePrefix("corpProp")]
    internal class CorpPropController : BaseApiController
    {
        private readonly IExcelImportChecker _importChecker;
        private readonly IUiFasade _uiFasade;
        private readonly IAutoMapperCloner _cloner;
        private readonly IViewModelConfigService _viewModelConfigService;
        private readonly IDvSettingService<DvSettingForType> _dvSettingService;
        private readonly IDvSettingManager _dvSettingManager;
        private readonly INotificationService _notificationService;
        private readonly IFileSystemService _fileSystemService;
        private readonly ISecurityService _security_service;
        private readonly IAccessService _accessService;
        private ISecurityUser _securityUser;
        private ISibEmailService _emailService;
        private readonly ILogService _logger;

        public ISecurityUser SecurityUser => _securityUser ?? (_securityUser = Base.Ambient.AppContext.SecurityUser);

        public CorpPropController(
            IViewModelConfigService viewModelConfigService
            , IUnitOfWorkFactory unitOfWorkFactory
            , IUiFasade uiFasade
            , IAutoMapperCloner cloner
            , IDvSettingService<DvSettingForType> dvSettingService
            , IDvSettingManager dvSettingManager
            , INotificationService notificationService
            , IFileSystemService fileSystemService
            , ISecurityService security_service
            , ISibEmailService emailService
            , IAccessService accessService
            , IExcelImportChecker importChecker
            , ILogService logger
           )
            : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
            _viewModelConfigService = viewModelConfigService;
            _uiFasade = uiFasade;
            _cloner = cloner;
            _dvSettingService = dvSettingService;
            _dvSettingManager = dvSettingManager;
            _notificationService = notificationService;
            _fileSystemService = fileSystemService;
            _security_service = security_service;
            _emailService = emailService;
            _accessService = accessService;
            _importChecker = importChecker;

        }

        [HttpGet]
        [WebHttp.Route("getActiveSociety/{id}")]
        public WebHttp.IHttpActionResult GetActiveSociety(int id = 0)
        {
            var config = _viewModelConfigService.Get("Subject");

            if (config == null)
                return Ok();

            if (id != 0)
                using (var uofw = CreateUnitOfWork())
                {
                    var serv = config.GetService<SubjectService>();
                    var model = serv.Get(uofw, id);
                    if (model != null && model.Society != null)
                        return GetSocietyConfig(model);
                }

            return Ok(config.ToDto(_viewModelConfigService.Get));
        }

        [HttpPost]
        [WebHttp.Route("getMenuPreset/{presetFor}")]
        public WebHttp.IHttpActionResult GetMenuPreset(string presetFor = "")
        {
            ICollection<MenuElement> list = new List<MenuElement>() { };
            using (var uofw = CreateUnitOfWork())
            {
                if (String.IsNullOrEmpty(presetFor)) return Ok(list);
                var pr = uofw.GetRepository<PresetRegistor>()
                      .Filter(f => f.For == presetFor)
                      .FirstOrDefault();
                if (pr != null)
                {
                    MenuPreset mp = pr.Preset as MenuPreset;
                    if (mp != null && mp.MenuElements != null)
                    {
                        list = mp.MenuElements;
                    }
                }
            }
            return Ok(list);
        }

        /// <summary>
        /// Возвращает конфиг для ДП, являющегося актуальным ОГ.
        /// </summary>
        /// <param name="subj">Деловой партнер.</param>
        /// <returns>Конфиг общества группы, если деловой партнер является актуальным ОГ, иначе - конфиг ДП.</returns>
        public WebHttp.IHttpActionResult GetSocietyConfig(Subject subj)
        {
            var mnSociety = "Society";
            if (subj == null || subj.SocietyID == null)
                return Ok(GetConfig()?.ToDto(_viewModelConfigService.Get));

            var config = _viewModelConfigService.Get(mnSociety);
            var res = config.ToDto(_viewModelConfigService.Get);
            res.Ext.Add("ObjectID", ((subj != null && subj.SocietyID != null) ? subj.SocietyID : subj?.ID));
            return Ok(res);
        }

        [HttpPost]
        [WebHttp.Route("addInComplex/{complexID}/{objectIds}")]
        public WebHttp.IHttpActionResult AddInComplex(int complexID, string objectIds)
        {
            string mess = "";
            int count = 0;
            if (String.IsNullOrEmpty(objectIds))
                return Ok(new { error = $"Ошибка при выборе объектов для добавления в ИК." });
            var oids = objectIds.Split(';');
            if (oids != null && oids.Length > 0)
            {
                if (complexID != 0)
                {
                    using (var uofw = CreateUnitOfWork())
                    {
                        //var conf = _viewModelConfigService.Get("InventoryObject");
                        foreach (var strId in oids)
                        {
                            int id = 0;
                            int.TryParse(strId, out id);
                            if (id != 0)
                            {
                                var item = uofw.GetRepository<CorpProp.Entities.Estate.InventoryObject>().Find(id);
                                if (item != null)
                                {
                                    var cad = uofw.GetRepository<CorpProp.Entities.Estate.Cadastral>().Find(id);
                                    if (EstateHelper.CheckDuplInPC(uofw, cad, cad.CadastralNumber, oids, complexID))
                                        return Ok(new { error = String.Format(EstateHelper.DuplInPcError, item.CadastralNumbers) });

                                    item.PropertyComplexID = complexID;
                                    uofw.GetRepository<CorpProp.Entities.Estate.InventoryObject>().Update(item);
                                }
                                else
                                    return Ok(new { error = $"Ошибка добавления в ИК: объекта с идентификатором {id} не существует." });
                            }

                            count++;
                        }
                        uofw.SaveChanges();
                        mess = $"Объекты успешно добавлены в ИК{System.Environment.NewLine}Всего обработано: {count}";
                    }
                }
                else
                    return Ok(new { error = $"Ошибка добавления в ИК: не передан идентификатор комплекса." });
            }
            else
                return Ok(new { error = $"Выберите объекты для добавления в ИК!" });

            var res = new
            {
                message = mess
            };
            return Ok(res);
        }

        [HttpPost]
        [WebHttp.Route("addInComplexIO/{complexID}/{objectIds}")]
        public WebHttp.IHttpActionResult AddInComplexIO(int complexID, string objectIds)
        {
            string mess = "";
            int err = 0;
            int count = 0;

            if (String.IsNullOrEmpty(objectIds))
                return Ok(new { error = $"Ошибка при выборе объектов для добавления в ИК." });

            var oids = objectIds.Split(';');

            if (oids != null && oids.Length > 0)
            {
                if (complexID != 0)
                {
                    using (var uofw = CreateUnitOfWork())
                    {
                        foreach (var strId in oids)
                        {
                            int id = 0;
                            int.TryParse(strId, out id);

                            if (id != 0)
                            {
                                var item = uofw.GetRepository<InventoryObject>().Find(id);

                                if (item.ParentID != null)
                                {
                                    var parent = uofw.GetRepository<InventoryObject>().Find(item.ParentID);

                                    if (parent.IsPropertyComplex)
                                    {
                                        mess += string.IsNullOrEmpty(mess) ? $"Ошибка добавления в ИК:<br/>Объект имущества {item.Name} уже включен в ИК {parent.Name}<br/>" : $"Объект имущества {item.Name} уже включен в ИК {parent.Name}<br/>";
                                        err = 1;
                                    }
                                }

                                if (item != null)
                                {
                                    if (Type.Equals(item.GetType().BaseType, typeof(Cadastral)))
                                    {
                                        var cad = uofw.GetRepository<Cadastral>().Find(id);

                                        if (EstateHelper.CheckDuplInPC(uofw, cad, cad.CadastralNumber, oids, complexID, true))
                                            return Ok(new
                                            {
                                                error = String.Format(EstateHelper.DuplInPcError, item.CadastralNumbers)
                                            });
                                    }

                                    item.ParentID = complexID;
                                    uofw.GetRepository<InventoryObject>().Update(item);
                                }
                                else
                                    return Ok(new { error = $"Ошибка добавления в ИК: объекта с идентификатором {id} не существует." });
                            }

                            count++;
                        }

                        if (err == 1)
                            return Ok(new { error = mess });

                        uofw.SaveChanges();
                        mess = $"Объекты успешно добавлены в ИК</br>Всего обработано: {count}";
                    }
                }
                else
                    return Ok(new { error = $"Ошибка добавления в ИК: не передан идентификатор комплекса." });
            }
            else
                return Ok(new { error = $"Выберите объекты для добавления в ИК!" });

            var res = new
            {
                message = mess
            };

            return Ok(res);
        }

        /// <summary>
        /// Получает данные ОГи подразделения из профиля пользователя.
        /// </summary>
        /// <param name="id">ИД пользователя</param>
        /// <returns></returns>
        [HttpGet]
        [WebHttp.Route("getUserProfile/{id}")]
        public WebHttp.IHttpActionResult GetUserProfile(int? id)
        {
            SibUser sb = null;
            string societyName = "Общество группы";
            string societyDeptName = "Структурное подразделение";
            string societyIDEUP = "";
            using (var uow = CreateUnitOfWork())
            {
                sb = uow.GetRepository<SibUser>().Filter(x => x.UserID == id).FirstOrDefault();
                //при использовании sb.SocietyName выбрасывает
                //TODO
                if (sb != null && sb.SocietyID != null)
                {
                    int idd = sb.SocietyID.Value;
                    var soc = uow.GetRepository<Society>().Filter(x => x.ID == idd).FirstOrDefault();
                    societyName = soc?.ShortName;
                    societyIDEUP = soc?.IDEUP;
                }
                if (sb != null && sb.SocietyDeptID != null)
                {
                    int idsd = sb.SocietyDeptID.Value;
                    var socDept = uow.GetRepository<SocietyDept>().Filter(x => x.ID == idsd).FirstOrDefault();
                    societyDeptName = socDept?.Name;
                }
            }
            return Ok(
                    new
                    {
                        ID = sb?.ID ?? 0,
                        SocietyName = (sb != null && !String.IsNullOrEmpty(societyName)) ? societyName : "Общество группы",
                        SocietyId = sb?.SocietyID,
                        SocietyIDEUP = (sb != null && !String.IsNullOrEmpty(societyIDEUP)) ? societyIDEUP : null,
                        SocietyDeptName = (sb != null && !String.IsNullOrEmpty(societyDeptName)) ? societyDeptName : "Структурное подразделение",
                        SocietyDeptId = sb?.SocietyDeptID,
                        DeptName = (sb != null && !String.IsNullOrEmpty(sb.DeptName)) ? sb.DeptName : "Структурное подразделение"
                    });
        }

        /// <summary>
        /// Добавление ННА в перечень.
        /// </summary>
        /// <param name="ncaIds">Идентификаторы ННА.</param>
        /// <param name="listId">Идентификатор перечня.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("addNonCoreAsset/{ncaIds}/{listId}")]
        public WebHttp.IHttpActionResult AddNonCoreAsset(string ncaIds, int? listId)
        {
            if (String.IsNullOrEmpty(ncaIds) || listId == null || listId == 0)
                throw new Exception("Не указаны элементы или перечень ННА.");

            try
            {
                using (IUnitOfWork uofw = CreateUnitOfWork())
                {
                    List<int> srcIdsList = ncaIds.Split(',').Select(x => int.Parse(x)).ToList<int>();

                    NonCoreAssetList list = uofw.GetRepository<NonCoreAssetList>().Find(f => f.ID == listId);

                    if (list != null)
                    {
                        var linkRepo = uofw.GetRepository<NonCoreAssetAndList>();
                        foreach (int id in srcIdsList)
                        {
                            NonCoreAsset item = uofw.GetRepository<NonCoreAsset>().Find(f => f.ID == id);
                            NonCoreAssetAndList link = linkRepo.Find(f => f.ObjLeftId == item.ID && f.ObjRigthId == list.ID);

                            if (link == null)
                                link = new NonCoreAssetAndList();
                            else
                                continue;

                            link.ObjRigth = list;
                            link.ObjRigthId = list.ID;
                            link.ObjLeft = item;
                            link.ObjLeftId = item.ID;

                            linkRepo.Create(link);
                            uofw.SaveChanges();
                        }
                    }
                    else
                        throw new Exception("Не удалось получить перечень ННА.");
                }

                return Ok(new
                {
                    error = 0,
                    message = "Элементы добавлены."
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }
        }

        /// <summary>
        /// Проверка на наличие похожих записей ННА.
        /// </summary>
        /// <param name="ids">Идентификаторы ОИ.</param>
        /// <param name="typeCode">Код типа ННА.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("checkEstateInNCA/{ids}/{typeCode}")]
        public WebHttp.IHttpActionResult CheckEstateInNCA(string ids, string typeCode)
        {
            string result = "";
            try
            {
                if (String.IsNullOrEmpty(ids) || typeCode == null)
                    throw new Exception("Не указаны элементы или тип ННА.");

                using (IUnitOfWork uofw = CreateUnitOfWork())
                {
                    List<int> srcIdsList = ids.Split(',').Select(x => int.Parse(x)).ToList<int>();
                    NonCoreAssetType ncaType = uofw.GetRepository<NonCoreAssetType>().Find(f => f.Code == typeCode);
                    NonCoreAssetStatus status = uofw.GetRepository<NonCoreAssetStatus>().Find(f => f.Code == "01");
                    var ioRepo = uofw.GetRepository<InventoryObject>();

                    if (srcIdsList.Count == 0 || ncaType == null || status == null)
                        throw new Exception("Не удалось получить статус, тип или идентификаторы элементов.");

                    foreach (int id in srcIdsList)
                    {
                        List<int> IdsList = new List<int>();
                        int? pcId = ioRepo.Find(f => f.ID == id).PropertyComplexID;

                        if (pcId == null)
                            IdsList.Add(id);
                        else
                        {
                            IdsList = ioRepo.Filter(f => f.PropertyComplexID == pcId).Select(s => s.ID).ToList<int>();
                        }

                        result = this.CheckNonCoreAsset(uofw, IdsList, ncaType);

                        if (result.StartsWith("ERROR_"))
                            throw new Exception(result.Replace("ERROR_", ""));
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner(),
                    duplicates = false
                });
            }

            return Ok(new
            {
                error = 0,
                message = result,
                duplicates = !String.IsNullOrEmpty(result)
            });
        }

        /// <summary>
        /// Отнесение ИК к ННА.
        /// </summary>
        /// <param name="pcId">ИД ИК.</param>
        /// <param name="typeCode">Код типа ННА.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("createNonCoreAssetFromPC/{pcId}/{typeCode}")]
        public WebHttp.IHttpActionResult CreateNonCoreAssetFromPC(int? pcId, string typeCode)
        {
            if (pcId == null || pcId == 0 || typeCode == null)
                throw new Exception("Не указаны элементы или тип ННА.");

            try
            {
                using (IUnitOfWork uofw = CreateUnitOfWork())
                {
                    //PropertyComplex pc = uofw.GetRepository<PropertyComplex>().Find(f => f.ID == pcId);
                    NonCoreAssetType ncaType = uofw.GetRepository<NonCoreAssetType>().Find(f => f.Code == typeCode);
                    NonCoreAssetStatus status = uofw.GetRepository<NonCoreAssetStatus>().Find(f => f.Code == "01");

                    int[] IdsList = uofw.GetRepository<InventoryObject>().Filter(f => f.PropertyComplexID == pcId).Select(s => s.ID).ToArray<int>();

                    if (IdsList.Length == 0 || ncaType == null || status == null)
                        throw new Exception("Не удалось получить статус, тип или идентификаторы элементов.");

                    this.CreateNonCoreAsset(uofw, IdsList, ncaType, status);
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }

            return Ok(new
            {
                error = 0,
                message = "ННА создан(ы)."
            });
        }

        /// <summary>
        /// Отнесение ОИ к ННА
        /// </summary>
        /// <param name="Ids">Перечисление ид ОИ.</param>
        /// <param name="typeCode">Код типа ННА.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("createNonCoreAsset/{Ids}/{typeCode}")]
        public WebHttp.IHttpActionResult CreateNonCoreAsset(string Ids, string typeCode)
        {
            if (String.IsNullOrEmpty(Ids) || typeCode == null)
                throw new Exception("Не указаны элементы или тип ННА.");
            try
            {
                using (IUnitOfWork uofw = CreateUnitOfWork())
                {
                    List<int> srcIdsList = Ids.Split(',').Select(x => int.Parse(x)).ToList<int>();
                    NonCoreAssetType ncaType = uofw.GetRepository<NonCoreAssetType>().Find(f => f.Code == typeCode);
                    NonCoreAssetStatus status = uofw.GetRepository<NonCoreAssetStatus>().Find(f => f.Code == "01");
                    var ioRepo = uofw.GetRepository<InventoryObject>();

                    if (srcIdsList.Count == 0 || ncaType == null || status == null)
                        throw new Exception("Не удалось получить статус, тип или идентификаторы элементов.");

                    foreach (int id in srcIdsList)
                    {
                        List<int> IdsList = new List<int>();
                        int? pcId = ioRepo.Find(f => f.ID == id).PropertyComplexID;

                        if (pcId == null)
                            IdsList.Add(id);
                        else
                        {
                            IdsList = ioRepo.Filter(f => f.PropertyComplexID == pcId).Select(s => s.ID).ToList<int>();
                        }

                        this.CreateNonCoreAsset(uofw, IdsList.ToArray<int>(), ncaType, status);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }

            return Ok(new
            {
                error = 0,
                message = "ННА создан(ы)."
            });
        }

        /// <summary>
        /// Изменение статуса строк ННА
        /// </summary>
        /// <param name="itemsIds">Идентификаторы строк.</param>
        /// <param name="statusCode">Код статуса.</param>
        /// <param name="comment">Примечание к смене статуса.</param>
        /// <param name="isResetStatus">Признак сброса статуса.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("changeNCAStatus/{itemsIds}/{statusCode}")]
        public WebHttp.IHttpActionResult ChangeNCAStatus(
            string itemsIds
            , string statusCode
            , string comment = ""
            , bool isResetStatus = false
            )
        {
            try
            {
                if (String.IsNullOrEmpty(itemsIds) || (!isResetStatus && statusCode == null))
                    throw new Exception("Не указаны элементы или статус.");

                using (IUnitOfWork uofw = CreateUnitOfWork())
                {
                    List<int> srcItemsIds = itemsIds.Split(',').Select(x => int.Parse(x)).ToList<int>();
                    NonCoreAssetListItemState status = !isResetStatus ? uofw.GetRepository<NonCoreAssetListItemState>()
                        .Filter(f => !f.Hidden && f.Code == statusCode)
                        .FirstOrDefault() : null;

                    if (srcItemsIds.Count == 0 || (status == null && !isResetStatus))
                        throw new Exception("Ошибка при получении идентификаторов элементов или статуса");

                    var ncaAndListRepo = uofw.GetRepository<NonCoreAssetAndList>();
                    foreach (int id in srcItemsIds)
                    {
                        var item = ncaAndListRepo.Find(f => f.ID == id);

                        if (item == null)
                            continue;

                        item.NonCoreAssetListItemState = status;
                        item.NonCoreAssetListItemStateID = status?.ID;
                        item.NoticeCauk = (String.IsNullOrEmpty(item.NoticeCauk) && String.IsNullOrEmpty(comment)) ?
                            comment
                            : item.NoticeCauk + " " + comment;
                        ncaAndListRepo.Update(item);
                        uofw.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }

            return Ok(new
            {
                error = 0,
                message = "Статус изменен."
            });
        }

        /// <summary>
        /// Копирование строк ННА из прошлых периодов в текущий перечень
        /// </summary>
        /// <param name="ncaItemIds">Идентификаторы строк.</param>
        /// <param name="ncaListId">Перень в который будут скопированы строки.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("duplicateNCAItem/{ncaItemIds}/{ncaListId}")]
        public WebHttp.IHttpActionResult DuplicateNCAItem(string ncaItemIds, int ncaListId)
        {
            try
            {
                if (string.IsNullOrEmpty(ncaItemIds) || ncaListId == 0)
                    throw new Exception("Не указаны элементы или перечень.");

                List<int> srcItemsIds = ncaItemIds.Split(',').Select(x => int.Parse(x)).ToList<int>();

                if (srcItemsIds.Count == 0)
                    throw new Exception("Ошибка при получении идентификаторов элементов.");

                using (IUnitOfWork unitOfWork = CreateUnitOfWork())
                {
                    var ncaAndListRepo = unitOfWork.GetRepository<NonCoreAssetAndList>();
                    NonCoreAssetList nonCoreAssetList = unitOfWork.GetRepository<NonCoreAssetList>().Find(f => f.ID == ncaListId);
                    List<NonCoreAssetAndList> nonCoreAssetAndLists = ncaAndListRepo.Filter(f => srcItemsIds.Contains(f.ID)).ToList();

                    foreach (NonCoreAssetAndList nonCoreAssetAndList in nonCoreAssetAndLists)
                    {
                        ncaAndListRepo.Detach(nonCoreAssetAndList);
                        nonCoreAssetAndList.ID = 0;
                        nonCoreAssetAndList.ObjRigth = nonCoreAssetList;
                        nonCoreAssetAndList.ObjRigthId = nonCoreAssetList.ID;
                        nonCoreAssetAndList.NonCoreAssetListItemState = null;
                        nonCoreAssetAndList.NonCoreAssetListItemStateID = null;
                        ncaAndListRepo.Create(nonCoreAssetAndList);
                    }

                    unitOfWork.SaveChanges();
                }

                return Ok(new
                {
                    error = 0,
                    message = "Строки ННА созданы."
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }
        }

        /// <summary>
        /// Отнесение к ГГР
        /// </summary>
        /// <param name="complexID">ИД ИК.</param>
        /// <param name="stateID">ИД ГГР.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("createScheduleStateRecord/{complexID}/{stateID}")]
        public WebHttp.IHttpActionResult CreateScheduleStateRecord(int complexID, int stateID)
        {
            try
            {
                using (var uofw = CreateUnitOfWork())
                {
                    ScheduleStateRegistration ssr = uofw.GetRepository<ScheduleStateRegistration>().Find(f => f.ID == stateID);
                    PropertyComplex pc = uofw.GetRepository<PropertyComplex>().Find(f => f.ID == complexID);

                    if ((pc != null && pc.ID != 0) && (ssr != null && ssr.ID != 0))
                    {
                        List<int> ioIDColl = uofw.GetRepository<InventoryObject>().Filter(f => f.PropertyComplexID == pc.ID).Select(s => s.ID).ToList<int>();

                        if (ioIDColl.Count > 0)
                        {
                            List<AccountingObject> aoColl = uofw.GetRepository<AccountingObject>().Filter(f => f.EstateID != null && ioIDColl.Contains((int)f.EstateID)).ToList<AccountingObject>();

                            if (aoColl.Count > 0)
                            {
                                foreach (AccountingObject ao in aoColl)
                                {
                                    uofw.GetRepository<ScheduleStateRegistrationRecord>().Create(new ScheduleStateRegistrationRecord()
                                    {
                                        ID = 0,
                                        ScheduleStateRegistration = ssr,
                                        AccountingObject = ao,
                                        Owner = ao.Owner,
                                        SocietyName = ssr.Society?.Name,
                                        ObjectName = ao.Name,
                                        Location = ao.Address,
                                        InventoryNumber = ao.InventoryNumber,
                                        InServiceDate = ao.InServiceDate,
                                        SystemNumber = ao.ExternalID,
                                        InitialCost = ao.InitialCost,
                                        Year = ao.Year
                                    });
                                    uofw.SaveChanges();
                                }
                            }
                        }
                    }
                }

                return Ok(new
                {
                    error = 0,
                    message = "Ok"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }
        }

        /// <summary>
        /// Отнесение ОБУ к ГГР.
        /// </summary>
        /// <param name="itemsIds">Идентификаторы ОБУ.</param>
        /// <param name="ssrId">ИД ГГР.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("createScheduleStateRegistrationRecords/{itemsIds}/{ssrId}")]
        public WebHttp.IHttpActionResult CreateScheduleStateRegistrationRecords(string itemsIds, int? ssrId)
        {
            string resultMsg = "Все объекты добавлены.";
            int errorCount = 0;
            int addCount = 0;
            int notAddCount = 0;

            try
            {
                if (String.IsNullOrEmpty(itemsIds) || ssrId == null || ssrId == 0)
                    throw new Exception("Не указаны элементы или ГГР.");

                using (IUnitOfWork uofw = CreateUnitOfWork())
                {
                    List<int> srcIdsList = itemsIds.Split(',').Select(x => int.Parse(x)).ToList<int>();
                    ScheduleStateRegistration ssr = null;
                    int year = DateTime.Now.AddYears(1).Year;

                    ssr = uofw.GetRepository<ScheduleStateRegistration>().Find(f => f.ID == ssrId);

                    var aoRepo = uofw.GetRepository<AccountingObject>();
                    var recordRepo = uofw.GetRepository<ScheduleStateRegistrationRecord>();
                    var scheduleRepo = uofw.GetRepository<ScheduleStateRegistration>();

                    string existsObjectInvNumbers = "";
                    int existsObjectCount = 0;

                    if (ssr != null)
                    {
                        int[] ssrItemsIds = recordRepo.All().Where(w => w.ScheduleStateRegistrationID == ssr.ID && w.AccountingObjectID != null).Select(s => (int)s.AccountingObjectID).ToArray<int>();

                        int[] ssrOthers = scheduleRepo.All().Where(w => w.SocietyID == ssr.SocietyID).Select(s => (int)s.ID).ToArray<int>();
                        int[] ssrItemsIdothers = recordRepo.All().Where(w => w.ScheduleStateRegistrationID != null && ssrOthers.Contains(w.ScheduleStateRegistrationID.Value) && w.AccountingObjectID != null).Select(s => (int)s.AccountingObjectID).ToArray<int>();

                        foreach (int itemId in srcIdsList)
                        {
                            AccountingObject ao = aoRepo.Find(f => f.ID == itemId);

                            if (ao == null)
                            {
                                notAddCount++;
                                continue;
                            }

                            if (!ao.IsRealEstate)
                            {
                                resultMsg = errorCount == 0 ? "" : resultMsg;
                                resultMsg += $"<br />Объект с инвентарным номером {ao.InventoryNumber} не является недвижимостью <br />";
                                existsObjectInvNumbers += existsObjectCount >= 1 ? $", {ao.InventoryNumber}" : $"{ao.InventoryNumber}";
                                existsObjectCount++;
                                errorCount++;
                                notAddCount++;
                                continue;
                            }

                            if (ao.Year == ssr.Year || ssrItemsIds.Contains(ao.ID))
                            {
                                resultMsg = errorCount == 0 ? "" : resultMsg;
                                resultMsg += $"<br />Объект с инвентарным номером {ao.InventoryNumber} уже содержится в графике {ssr.Name}<br />";
                                existsObjectInvNumbers += existsObjectCount >= 1 ? $", {ao.InventoryNumber}" : $"{ao.InventoryNumber}";
                                existsObjectCount++;
                                errorCount++;
                                notAddCount++;
                                continue;
                            }

                            if ((ao.Year != null && ao.Year != 0) || ssrItemsIdothers.Contains(ao.ID))
                            {
                                resultMsg = errorCount == 0 ? "" : resultMsg;
                                resultMsg += $"<br />Объект с инвентарным номером {ao.InventoryNumber} уже содержится в другом графике<br />";
                                existsObjectInvNumbers += existsObjectCount >= 1 ? $", {ao.InventoryNumber}" : $"{ao.InventoryNumber}";
                                existsObjectCount++;
                                errorCount++;
                                notAddCount++;
                                continue;
                            }

                            ScheduleStateRegistrationRecord ssrRecord = new ScheduleStateRegistrationRecord()
                            {
                                SocietyName = ao.Owner?.ShortName,
                                Owner = ao.Owner,
                                ObjectName = ao.Name,
                                InventoryNumber = ao.InventoryNumber,
                                InServiceDate = ao.InServiceDate,
                                InitialCost = ao.InitialCost,
                                AccountingObject = ao,
                                ScheduleStateRegistration = ssr
                            };

                            ao.SSR = ssr;
                            aoRepo.Update(ao);
                            uofw.GetRepository<ScheduleStateRegistrationRecord>().Create(ssrRecord);
                            uofw.SaveChanges();
                            addCount++;
                        }

                        if (existsObjectCount > 0)
                        {
                            resultMsg +=
                                  $"<br /><hr>Из {srcIdsList.Count} объектов {existsObjectCount} объекты с инвентарными номерами {existsObjectInvNumbers} уже содержится/содержатся в графике.";
                            resultMsg += $"<br />(Обработано: {addCount}объект(ов))";
                            resultMsg += $"<br />(Отклонено: {notAddCount}объект(ов))";
                        }
                        else
                        {
                            resultMsg += $"<br />({addCount} объект(ов))";
                        }
                    }
                    else
                        throw new Exception("График гос. регистрации за текущий период для Вашего ОГ не найден.");
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }

            return Ok(new
            {
                error = errorCount > 0 ? 1 : 0,
                message = resultMsg
            });
        }

        /// <summary>
        /// Отнесение к ГР прекращения.
        /// </summary>
        /// <param name="itemsIds">Идентификаторы ОБУ.</param>
        /// <param name="sstId">Идентификатор пользователя или ГР.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("createScheduleStateTerminateRecords/{itemsIds}/{sstId}")]
        public WebHttp.IHttpActionResult CreateScheduleStateTerminateRecords(string itemsIds, int? sstId)
        {
            string resultMsg = "Все объекты добавлены.";
            int errorCount = 0;
            int addCount = 0;
            int notAddCount = 0;
            try
            {
                if (String.IsNullOrEmpty(itemsIds) || sstId == null || sstId == 0)
                    throw new Exception("Не указаны элементы или график прекращения.");

                using (IUnitOfWork uofw = CreateUnitOfWork())
                {
                    List<int> srcIdsList = itemsIds.Split(',').Select(x => int.Parse(x)).ToList<int>();
                    ScheduleStateTerminate sst = null;
                    int year = DateTime.Now.AddYears(1).Year;
                    SibUser currentUser = null;

                    sst = uofw.GetRepository<ScheduleStateTerminate>().Find(f => f.ID == sstId);

                    var aoRepo = uofw.GetRepository<AccountingObject>();
                    var recordRepo = uofw.GetRepository<ScheduleStateTerminateRecord>();
                    var terminateRepo = uofw.GetRepository<ScheduleStateTerminate>();

                    int[] ssrOthers = terminateRepo.All().Where(w => w.SocietyID == sst.SocietyID).Select(s => (int)s.ID).ToArray<int>();
                    int[] ssrItemsIdothers = recordRepo.All().Where(w => w.ScheduleStateTerminateID != null && ssrOthers.Contains(w.ScheduleStateTerminateID.Value) && w.AccountingObjectID != null).Select(s => (int)s.AccountingObjectID).ToArray<int>();

                    if (sst != null)
                    {
                        List<int?> sstItemsIds = recordRepo.All().Where(w => w.ScheduleStateTerminateID == sst.ID && w.AccountingObjectID != null).Select(s => s.AccountingObjectID).ToList<int?>();

                        string existsObjectInvNumbers = "";
                        int existsObjectCount = 0;

                        foreach (int itemId in srcIdsList)
                        {
                            AccountingObject ao = aoRepo.Find(f => f.ID == itemId);

                            if (ao == null)
                            {
                                notAddCount++;
                                continue;
                            }
                            if (!ao.IsRealEstate)
                            {
                                resultMsg = errorCount == 0 ? "" : resultMsg;
                                resultMsg += $"<br />Объект с инвентарным номером {ao.InventoryNumber} не является недвижимостью <br />";
                                existsObjectInvNumbers += existsObjectCount >= 1 ? $", {ao.InventoryNumber}" : $"{ao.InventoryNumber}";
                                existsObjectCount++;
                                errorCount++;
                                notAddCount++;
                                continue;
                            }

                            if (ao.Year == sst.Year || sstItemsIds.Contains((int)ao.ID))
                            {
                                resultMsg = errorCount == 0 ? "" : resultMsg;
                                resultMsg += $"<br />Объект с инвентарным номером {ao.InventoryNumber} уже содержится в графике {sst.Name}<br />";
                                existsObjectInvNumbers += existsObjectCount >= 1 ? $", {ao.InventoryNumber}" : $"{ao.InventoryNumber}";
                                existsObjectCount++;
                                errorCount++;
                                notAddCount++;
                                continue;
                            }

                            if ((ao.Year != null && ao.Year != 0) || ssrItemsIdothers.Contains((int)ao.ID))
                            {
                                resultMsg = errorCount == 0 ? "" : resultMsg;
                                resultMsg += $"<br />Объект с инвентарным номером {ao.InventoryNumber} уже содержится в другом графике<br />";
                                existsObjectInvNumbers += existsObjectCount >= 1 ? $", {ao.InventoryNumber}" : $"{ao.InventoryNumber}";
                                existsObjectCount++;
                                errorCount++;
                                notAddCount++;
                                continue;
                            }

                            ScheduleStateTerminateRecord sstRecord = new ScheduleStateTerminateRecord()
                            {
                                SocietyName = ao.Owner?.ShortName,
                                Owner = ao.Owner,
                                ObjectName = ao.Name,
                                InventoryNumber = ao.InventoryNumber,
                                InServiceDate = ao.InServiceDate,
                                InitialCost = ao.InitialCost,
                                AccountingObject = ao,
                                ScheduleStateTerminate = sst
                            };

                            ao.SSRTerminate = sst;
                            aoRepo.Update(ao);

                            uofw.GetRepository<ScheduleStateTerminateRecord>().Create(sstRecord);
                            uofw.SaveChanges();
                            addCount++;
                        }

                        if (existsObjectCount > 0)
                        {
                            resultMsg +=
                                $"<br /><hr>Из {srcIdsList.Count} объектов {existsObjectCount} объекты с инвентарными номерами {existsObjectInvNumbers} уже содержится/содержатся в графике.";
                            resultMsg += $"<br />(Обработано: {addCount}объект(ов))";
                            resultMsg += $"<br />(Отклонено: {notAddCount}объект(ов))";
                        }
                        else
                        {
                            resultMsg += $"<br />({addCount} объект(ов))";
                        }
                    }
                    else
                        throw new Exception("График прекращения регистрации за текущий период для Вашего ОГ не найден.");
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }

            return Ok(new
            {
                error = errorCount > 0 ? 1 : 0,
                message = resultMsg
            });
        }

        /// <summary>
        /// Проверяет обновление записи о праве модуля РР в АИС КС.
        /// </summary>
        /// <param name="ids">ИД-ы прав РР.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("checkUpdRightInCorpProp/{ids}")]
        public WebHttp.IHttpActionResult CheckUpdRightInCorpProp(string ids)
        {
            int err = 0;
            string mess = "";
            if (String.IsNullOrEmpty(ids))
                return Ok(new { error = $"Ошибка при выборе объектов для обновления в АИС КС." });
            var oids = ids.Split(',');

            if (oids != null && oids.Length > 0)
            {
                try
                {
                    using (var uow = CreateUnitOfWork())
                    {
                        var uowHist = CreateUnitOfWork();
                        MigrateHolder holder =
                        CorpProp.RosReestr.Migration.MigrateRights.StartMigrateRights(uow, uowHist, oids, this.SecurityUser?.ID, false);
                        if (holder.MigrateLogs.Where(x => x.MigrateState.Code == "103").Any())
                            err = 1;
                        mess = holder.GetCheckingReport();
                    }
                }
                catch (Exception ex)
                {
                    err = 1;
                    mess = ex.Message;
                }
            }
            else
                return Ok(new { error = $"Выберите объекты для обновления в АИС КС!" });

            var res = new
            {
                err = err,
                message = mess
            };
            return Ok(res);
        }

        /// <summary>
        /// Обновляет записи о праве модуля РР в АИС КС.
        /// </summary>
        /// <param name="ids">ИД-ы прав РР.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("updRightInCorpProp/{ids}")]
        public WebHttp.IHttpActionResult UpdRightInCorpProp(string ids)
        {
            int err = 0;
            string mess = "";
            if (String.IsNullOrEmpty(ids))
                return Ok(new { error = $"Ошибка при выборе объектов для обновления в АИС КС." });
            var oids = ids.Split(',');

            if (oids != null && oids.Length > 0)
            {
                try
                {
                    using (var uow = CreateUnitOfWork())
                    {
                        var uowHist = CreateUnitOfWork();
                        MigrateHolder holder =
                        CorpProp.RosReestr.Migration.MigrateRights.StartMigrateRights(uow, uowHist, oids, this.SecurityUser?.ID);
                        if (holder.MigrateLogs.Where(x => x.MigrateState.Code == "103").Any())
                            err = 1;
                        mess = holder.MigrateHistory.ResultText;
                    }
                }
                catch (Exception ex)
                {
                    err = 1;
                    mess = ex.Message;
                }
            }
            else
                return Ok(new { error = $"Выберите объекты для обновления в АИС КС!" });

            var res = new
            {
                err = err,
                message = mess
            };
            return Ok(res);
        }

        /// <summary>
        /// Проверяет на обновление объекты выписки на ОНИ модуля РР в АИС КС.
        /// </summary>
        /// <param name="ids">ИД выписки на ОНИ РР.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("checkUpdInCorpProp/{id}")]
        public WebHttp.IHttpActionResult CheckUpdInCorpProp(int id)
        {
            int err = 0;
            string mess = "Не удалось обновить объекты.";
            if (id == 0)
                return Ok(new { error = $"Ошибка при выборе объектов для обновления в реестре прав." });
            try
            {
                using (var uow = CreateUnitOfWork())
                {
                    var uowHis = CreateUnitOfWork();
                    MigrateHolder holder = CorpProp.RosReestr.Migration.MigrateExtract.CheckMigrateObjects(uow, uowHis, id, this.SecurityUser?.ID);
                    if (holder != null)
                    {
                        if (holder.MigrateLogs.Where(l => l.MigrateState.Code == "103").Any())
                            err = 1;
                        mess = mess = holder.GetCheckingReport();
                    }
                    else
                    {
                        err = 1;
                        mess = "Не удалось проанализировать обновление объектов.";
                    }
                }
            }
            catch (Exception ex)
            {
                err = 1;
                mess = ex.Message;
            }

            var res = new
            {
                err = err,
                message = mess
            };
            return Ok(res);
        }

        /// <summary>
        /// Обновляет объекты выписки на ОНИ модуля РР в АИС КС.
        /// </summary>
        /// <param name="ids">ИД выписки на ОНИ РР.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("updInCorpProp/{id}")]
        public WebHttp.IHttpActionResult UpdInCorpProp(int id)
        {
            int err = 0;
            string mess = "Не удалось обновить объекты.";
            if (id == 0)
                return Ok(new { error = $"Ошибка при выборе объектов для обновления в реестре прав." });
            try
            {
                using (var uow = CreateUnitOfWork())
                {
                    var uowHis = CreateUnitOfWork();
                    MigrateHolder holder = CorpProp.RosReestr.Migration.MigrateExtract.MigrateObjects(uow, uowHis, id, this.SecurityUser?.ID);
                    if (holder != null)
                    {
                        if (holder.MigrateLogs.Where(l => l.MigrateState.Code == "103").Any())
                            err = 1;
                        mess = holder.MigrateHistory.ResultText;
                    }
                    else
                    {
                        err = 1;
                        mess = "Не удалось обновить объекты.";
                    }
                    uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                err = 1;
                mess = ex.Message;
            }

            var res = new
            {
                err = err,
                message = mess
            };
            return Ok(res);
        }

        /// <summary>
        /// Создание шаблона проекта.
        /// </summary>
        /// <param name="projectId">ИД проекта.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("createProjectTemplate/{projectId}")]
        public WebHttp.IHttpActionResult CreateProjectTemplate(int? projectId)
        {
            try
            {
                if (projectId == null || projectId == 0)
                    throw new Exception("Сохраните проект.");

                using (IUnitOfWork uofw = CreateUnitOfWork())
                {
                    var projectRepo = uofw.GetRepository<SibProject>();
                    var taskRepo = uofw.GetRepository<SibTask>();
                    SibProject project = projectRepo.Find(f => f.ID == projectId);
                    SibProjectStatus projectStatus = uofw.GetRepository<SibProjectStatus>().Find(f => f.Code == "Draft");
                    List<SibTask> tasks = uofw.GetRepository<SibTask>().Filter(f => f.ProjectID == project.ID && !f.Hidden && !f.IsTemplate).ToList<SibTask>();

                    if (project != null)
                    {
                        projectRepo.Detach(project);
                        project.ID = 0;
                        project.IsTemplate = true;
                        project.ProjectNumber = null;

                        projectRepo.Create(project);
                        uofw.SaveChanges();

                        if (tasks.Count > 0)
                        {
                            SibTaskStatus taskStatus = uofw.GetRepository<SibTaskStatus>().Find(f => f.Code == "Draft");
                            SibProject projectTemplateAttach = projectRepo.Find(f => f.ID == project.ID);
                            foreach (SibTask task in tasks)
                            {
                                taskRepo.Detach(task);
                                task.ID = 0;
                                task.IsTemplate = true;
                                task.Number = null;
                                task.ProjectID = project.ID;
                                task.Project = project;
                                task.TaskParent = null;
                                task.TaskParentID = null;
                                task.SetParent(null);

                                taskRepo.Create(task);
                            }
                        }

                        uofw.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }

            return Ok(new
            {
                error = 0,
                message = "Шаблон успешно создан."
            });
        }

        /// <summary>
        /// Создание шаблона задачи.
        /// </summary>
        /// <param name="taskId">ИД задачи.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("createTaskTemplate/{taskId}")]
        public WebHttp.IHttpActionResult CreateTaskTemplate(int? taskId)
        {
            try
            {
                if (taskId == null || taskId == 0)
                    throw new Exception("Сохраните задачу.");

                using (IUnitOfWork uofw = CreateUnitOfWork())
                {
                    var taskRepo = uofw.GetRepository<SibTask>();
                    SibTask task = taskRepo.All().Single(f => f.ID == taskId);
                    SibTaskStatus taskStatus = uofw.GetRepository<SibTaskStatus>().Find(f => f.Code == "Draft");

                    if (task != null)
                    {
                        taskRepo.Detach(task);
                        task.ID = 0;
                        task.IsTemplate = true;
                        task.SibStatus = taskStatus;
                        task.Number = null;
                        task.InternalNumber = null;
                        task.TaskParent = null;
                        task.TaskParentID = null;
                        task.Project = null;
                        task.ProjectID = null;
                        task.SetParent(null);
                        uofw.IsModifiedEntity(task, BaseEntityState.Added);
                        taskRepo.Create(task);
                        uofw.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }

            return Ok(new
            {
                error = 0,
                message = "Шаблон успешно создан."
            });
        }

        /// <summary>
        /// Отправка уведомления исполнителю.
        /// </summary>
        /// <param name="taskId">ИД задачи.</param>
        /// <param name="userId">ИД исполнителя.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("sendTaskNotification/{taskId}/{userId}")]
        public WebHttp.IHttpActionResult SendTaskNotification(int? taskId, int? userId)
        {
            try
            {
                if (taskId == null || taskId == 0)
                    throw new Exception("Сохраните задачу.");

                if (userId == null || userId == 0)
                    throw new Exception("Ошибка определения пользователя");

                using (IUnitOfWork uofw = CreateUnitOfWork())
                {
                    SibTask task = uofw.GetRepository<SibTask>().Find(f => f.ID == taskId);

                    LinkBaseObject linkObj = GetLinkedObj(task as BaseObject);
                    if (task == null)
                        throw new Exception("Задача не найдена.");

                    if (task.Initiator == null || task.Initiator.User == null)
                        throw new Exception("Не задат инициатор или не найден пользователь.");

                    if (task.Responsible == null || task.Responsible.User == null)
                        throw new Exception("Не задат исполнитель или не найден пользователь.");

                    int uId = uofw.GetRepository<Base.Security.User>().Find(f => f.ID == task.Initiator.User.ID).ID;

                    if (uId != userId)
                        throw new Exception("Отправить уведомление может только инициатор.");

                    List<int> recipientsList = new List<int>()
                    {
                           task.Responsible.User.ID
                    };

                    string title = $"В задачу № {task.Number} внесены изменения";
                    string message = $"В задачу № {task.Number} внесены изменения";

                    _notificationService.CreateNotification(uofw, recipientsList.ToArray<int>(), linkObj, title, message);
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }

            return Ok(new
            {
                error = 0,
                message = "Уведомление отправлено."
            });
        }

        /// <summary>
        /// Импорт из ПХД
        /// </summary>
        /// <param name="fileCardIds">Идентификаторы FileCard</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("fileImport/{fileCardIds}")]
        public WebHttp.IHttpActionResult FileImport(string fileCardIds)
        {
            if (String.IsNullOrEmpty(fileCardIds))
                throw new Exception("Не выбраны файлы для загрузки.");
            int fail = 0;
            const string _errText = "Завершено с ошибками.";
            const string _goodText = "Импорт завершен.";
            string report = "";
            List<string> exportData = new List<string>();

            try
            {
                List<int> srcIdsList = fileCardIds.Split(',').Select(x => int.Parse(x)).ToList<int>();
                List<Guid> historyOids = new List<Guid>();
                using (IUnitOfWork historySession = CreateUnitOfWork())
                {
                    int count = 0;
                    string error = "";                    

                    using (ITransactionUnitOfWork uofw = CreateTransactionUnitOfWork())
                    {
                        foreach (int itemId in srcIdsList)
                        {
                            var fileCard = historySession.GetRepository<FileCardOne>().Find(f => f.ID == itemId);

                            if (fileCard == null)
                                throw new Exception("Не удалось получить данные о выбранном файле.");

                            FileData fileData = fileCard.FileData;
                            string filePath = _fileSystemService.GetFilePath(fileData.FileID);
                            if (fileData.Extension != "XML" && fileData.Extension != "XLS" && fileData.Extension != "XLSX")
                                return Ok(new
                                {
                                    error = 1,
                                    message = @"Разрешенные форматы файлов импорта: *.XLS, *.XLSX, *.XML"
                                });

                            bool isXML = fileData.Extension == "XML";
                            report += $"Имя файла: { fileData.FileName}. {System.Environment.NewLine}";
                            using (StreamReader stream = new StreamReader(filePath))
                            {
                                if (!isXML)
                                {
                                    var impHistory = ImportHelper.CreateImportHistory(historySession, fileCard.FileData.FileName, this.SecurityUser?.ID);
                                    historyOids.Add(impHistory.Oid);
                                    impHistory.FileCard = fileCard;

                                    using (IExcelDataReader reader = fileData.Extension == "XLSX" ?
                                        ExcelReaderFactory.CreateOpenXmlReader(stream.BaseStream) :
                                        ExcelReaderFactory.CreateBinaryReader(stream.BaseStream))
                                    {                                        
                                        var impHistoryService = _viewModelConfigService.Get("ImportHistory").GetService<IImportHistoryService>();
                                        impHistory.Mnemonic = ImportHelper.FindTypeName(reader);

                                        new CorpProp.ImportChecker().ParseFileNameDefult(impHistoryService, historySession, ref impHistory, false);
                                        int version = (impHistory.Consolidation != null && impHistory.Period != null) ? ImportHelper.GetVersion(historySession, impHistory.Consolidation?.Code, impHistory.Period.Value, impHistory.Mnemonic) : 0;
                                        impHistory.Version = ++version;
                                        impHistory.IsCorrection = version > 1;
                                       
                                        var _importStarter = new ImportStarter(_accessService, _importChecker, _logger);
                                        var checkImportVersionResult = _importStarter.CheckImport(_uiFasade, reader, uofw, historySession, stream, impHistory.FileName, impHistory);

                                        if (checkImportVersionResult.IsError)
                                        {
                                            impHistory.ImportErrorLogs.AddError(checkImportVersionResult.ErrorMessage);
                                        }
                                        else
                                        {
                                            int dataVersion = ImportHelper.FindDataVersionValue(reader);
                                            impHistory.DataVersion = dataVersion;

                                            _importStarter.Import(_uiFasade, uofw, historySession, reader, ref error, ref count, ref impHistory);
                                        }

                                        if (impHistory.ImportErrorLogs.Any())
                                        {
                                            impHistory.SetInvAndEUSILogs(reader);
                                            impHistory.ResultText = ImportHelper.GetFailedHistoryResultText(impHistory.ResultText, count);
                                            impHistory.IsSuccess = false;
                                            fail = 1;
                                            uofw.Rollback();
                                        }
                                        else
                                        {
                                            impHistory.ResultText = ImportHelper.GetSuccessHistoryResultText(impHistory.Mnemonic, impHistory.ResultText, count);
                                            impHistory.IsSuccess = true;
                                            uofw.Commit();
                                        }

                                        if (impHistory.FileCard != null)
                                        {
                                            impHistory.FileCard.Description += System.Environment.NewLine;
                                            impHistory.FileCard.Description += $"Импорт файла от {impHistory.ImportDateTime.ToString()}:";
                                            impHistory.FileCard.Description += $" {impHistory.ResultText}";
                                        }

                                        report += impHistory.ResultText;
                                        if (impHistory.ID == 0)
                                            historySession.GetRepository<ImportHistory>().Create(impHistory);
                                        else
                                            historySession.GetRepository<ImportHistory>().Update(impHistory);
                                    }
                                }
                                else
                                {
                                    var tx = "";
                                    var impHistory = ImportXml(uofw, historySession, stream, fileCard, fileData.FileName);
                                    if (impHistory != null)
                                    {
                                        historyOids.Add(impHistory.Oid);

                                        if (impHistory.ImportErrorLogs.Any())
                                        {
                                            tx = impHistory.ResultText.Contains(_errText) ? "" : _errText;
                                            tx += Environment.NewLine;
                                            tx += impHistory.ResultText;
                                            fail = 1;
                                            uofw.Rollback();
                                        }
                                        else
                                        {
                                            tx = impHistory.ResultText;
                                            uofw.Commit();
                                        }
                                    }
                                    else
                                    {
                                        tx = _errText;
                                        fail = 1;
                                    }
                                    report += tx + Environment.NewLine;
                                }
                            }

                            historySession.SaveChanges();
                            exportData = ExportResult(historySession, historyOids);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }
            if (exportData != null && exportData.Count > 0 && fail == 0)
            {
                return Ok(new
                {
                    error = fail,
                    message = report,
                    mimetype = "application/zip",
                    filename = "export.zip",
                    datas = exportData
                });
            }
            else
                return Ok(new
                {
                    error = fail,
                    message = report
                });
        }

        private List<string> ExportResult(IUnitOfWork uow, IList<Guid> historyOids)
        {
            var res = new List<string>();
            var list = uow.GetRepository<ImportObject>()
               .Filter(f => !f.Hidden && historyOids.Contains(f.ImportHistoryOid) && f.Type == TypeImportObject.CreateObject)
               .Include(inc => inc.Entity)
               .GroupBy(gr => gr.Entity.TypeName)
               .ToList();

            foreach (var gr in list)
            {
                var config = _viewModelConfigService.Get(gr.Key);

                if (config != null && config.ServiceType != null && config.Mnemonic != "EstateRegistration" &&
                    config.ServiceType.GetInterfaces().Contains(typeof(IExportToZip)))
                {
                    var service = config.GetService<IExportToZip>();
                    if (service != null)
                    {
                        var ids = gr.ToList().Select(s => s.Entity.ID).ToArray();
                        var val = service.ExportToZip(uow, ids);
                        if (!String.IsNullOrEmpty(val))
                            res.Add(val);
                    }
                }
            }

            return res;
        }

        public ImportHistory ImportXml(
              IUnitOfWork uofw
            , IUnitOfWork historySession
            , StreamReader stream
            , FileCardOne fileCard
            , string fileName
            )
        {
            ImportHistory importHistory = null;
            try
            {
                System.Xml.Linq.XDocument xml = System.Xml.Linq.XDocument.Load(stream.BaseStream);

                if (xml.Root.Name == "Файл")
                {
                    var conf = _uiFasade.GetViewModelConfig("Declaration");
                    var service = conf.GetService<IXmlImportEntity>();
                    service.CreateHolder(uofw, historySession, stream, fileCard);
                    var holder = service.Holder;
                    if (ImportHelper.CheckRepeatImport(uofw, stream.BaseStream))
                        holder.ImportHistory.ImportErrorLogs.AddError("Идентичный файл импортировался ранее.");
                    else
                        holder.Import();
                    importHistory = holder.ImportHistory;
                }
                else
                {
                    stream.BaseStream.Position = 0;
                    var holder = CorpProp.RosReestr.Helpers.ImportLoader.Import(fileCard,
                       uofw, historySession, stream.BaseStream, fileName, this.SecurityUser?.ID);
                    importHistory = holder.ImportHistory;
                }
            }
            catch (Exception ex)
            {
                importHistory.ImportErrorLogs.AddError(ex.ToStringWithInner());
            }
            return importHistory;
        }

        /// <summary>
        /// Получает праздничные дни.
        /// </summary>
        /// <param name="startDate">Дата начала поиска.</param>
        /// <param name="endDate">Дата окончания поиска.</param>
        /// <returns></returns>
        [HttpGet]
        [WebHttp.Route("getHolidays/{startDate}/{endDate}")]
        public WebHttp.IHttpActionResult GetHolidays(string startDate, string endDate)
        {
            List<DateTime> result = new List<DateTime>();
            try
            {
                using (IUnitOfWork uofw = CreateUnitOfWork())
                {
                    DateTime minDate = DateTime.MinValue;
                    DateTime maxDate = DateTime.MinValue;
                    if (!DateTime.TryParse(startDate, out minDate) || !DateTime.TryParse(endDate, out maxDate))
                        throw new Exception("Неверный формат даты.");

                    var holidays = uofw.GetRepository<HolidaysCalendar>().Filter(f => (f.DateFrom != null && f.DateTo != null) && (f.DateFrom.Value >= minDate && f.DateTo.Value <= maxDate)).ToList<HolidaysCalendar>();

                    if (holidays.Count > 0)
                    {
                        foreach (var holiday in holidays)
                        {
                            DateTime holidayStart = holiday.DateFrom.Value.Date;
                            DateTime holidayEnd = holiday.DateTo.Value.Date;
                            List<DateTime> holidayList = new List<DateTime>
                            {
                                holidayStart,
                                holidayEnd
                            };

                            var range = Enumerable.Range(0, (int)(holidayEnd - holidayStart).TotalDays + 1)
                                .Select(s => holidayStart.AddDays(s));

                            var missingDates = range.Except(holidayList);

                            foreach (var date in missingDates)
                            {
                                holidayList.Add(date);
                            }

                            result.AddRange(holidayList);
                        }
                    }
                }

                return Ok(new
                {
                    error = 0,
                    message = "",
                    holidays = result.OrderBy(o => o.Date)
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }
        }

        /// <summary>
        /// Получение мнемоник для уведомлений.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [WebHttp.Route("getNotificationMnemonics")]
        public WebHttp.IHttpActionResult GettNotificationMnemonics()
        {
            try
            {
                var mnemonics = _viewModelConfigService.GetAll()
                    .Where(w => w.ServiceType != null && w.ServiceType.GetInterfaces().Contains(typeof(ISibNotification)) && w.IsNotify);

                return Ok(
                    mnemonics.Select(s => new
                    {
                        ID = s.Mnemonic,
                        Text = $"{s.Mnemonic} : {s.Title ?? s.Name}"
                    })
                );
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = ex.ToStringWithInner()
                });
            }
        }

        /// <summary>
        /// Получение полей для уведомления (DateTime).
        /// </summary>
        /// <param name="mnemonic">Мнемоника.</param>
        /// <returns></returns>
        [HttpGet]
        [WebHttp.Route("getNotificationEditors/{mnemonic}")]
        public WebHttp.IHttpActionResult GetNotificationEditors(string mnemonic)
        {
            return Ok(_uiFasade.GetColumns(mnemonic).Where(w => w.PropertyType == typeof(DateTime) || w.PropertyType == typeof(DateTime?)).Select(x => new
            {
                ID = x.PropertyName,
                Text = x.Title
            }));
        }

        /// <summary>
        /// Удаление ННА.
        /// </summary>
        /// <param name="id">ИД ННА.</param>
        /// <returns></returns>
        [WebHttp.HttpDelete]
        [WebHttp.Route("removeNonCoreAsset/{id}")]
        public WebHttp.IHttpActionResult RemoveNonCoreAsset(int id)
        {
            try
            {
                using (IUnitOfWork uofw = CreateUnitOfWork())
                {
                    var ncaObj = uofw.GetRepository<NonCoreAsset>().Find(f => f.ID == id);

                    if (ncaObj == null)
                        throw new Exception("Объект не найден.");

                    string result = DeleteNonCoreAsset(uofw, ncaObj);

                    return Ok(new
                    {
                        error = 0,
                        message = result
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }
        }

        [HttpPost]
        [WebHttp.Route("createEstateAppraisal/{appraisalId}/{ids}")]
        public WebHttp.IHttpActionResult CreateEstateAppraisal(int appraisalId, string ids)
        {
            int count = 0;
            if (String.IsNullOrEmpty(ids))
                return Ok(new { error = $"Ошибка при выборе объектов." });

            try
            {
                List<int> srcIdsList = ids.Split(',').Select(x => int.Parse(x)).ToList<int>();
                using (IUnitOfWork unitOfWork = CreateUnitOfWork())
                {
                    var eaRepo = unitOfWork.GetRepository<EstateAppraisal>();
                    var appraisal = unitOfWork.GetRepository<Appraisal>().Find(f => f.ID == appraisalId);

                    foreach (int id in srcIdsList)
                    {
                        var ao = unitOfWork.GetRepository<AccountingObject>().Find(f => f.ID == id);
                        if (ao == null)
                            continue;
                        EstateAppraisal estateAppraisal = new EstateAppraisal()
                        {
                            Appraisal = appraisal,
                            AppraisalID = appraisal.ID,
                            AccountingObject = ao,
                            AccountingObjectID = ao.ID,
                            ShortDescriptionObjectAppraisal = ""
                        };

                        eaRepo.Create(estateAppraisal);

                        count++;
                    }
                    unitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }

            return Ok(new
            {
                error = 0,
                message = $"Создано {count} объектов оценки."
            });
        }

        /// <summary>
        /// Удаление ННА.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="nonCoreAssetObj">Объект ННА.</param>
        /// <returns></returns>
        private string DeleteNonCoreAsset(IUnitOfWork unitOfWork, NonCoreAsset nonCoreAssetObj)
        {
            try
            {
                var conf = _uiFasade.GetViewModelConfig(typeof(NonCoreAsset));
                var service = conf.GetService<CorpProp.Services.Asset.NonCoreAssetService>();
                var invObjRepo = unitOfWork.GetRepository<InventoryObject>();
                List<string> coName = new List<string>();
                List<int> inventoryObjectsIds = new List<int>();
                string pcName = "";
                int count = 0;

                if (nonCoreAssetObj.EstateObjectID != null && nonCoreAssetObj.EstateObjectID != 0)
                {
                    var inventoryObject = invObjRepo.Find(nonCoreAssetObj.EstateObjectID);

                    if (inventoryObject != null && inventoryObject.ParentID != null && unitOfWork.GetRepository<PropertyComplexIO>().FilterAsNoTracking(f => f.ID == inventoryObject.ParentID).Any())
                    {
                        pcName = unitOfWork.GetRepository<PropertyComplexIO>().Find(f => f.ID == inventoryObject.ParentID).Name;

                        inventoryObjectsIds.AddRange(invObjRepo.Filter(f => f.ParentID == inventoryObject.ParentID && !f.Hidden && !f.IsHistory).Select(s => s.ID).ToList<int>());
                    }

                    if (inventoryObject != null && inventoryObject.FakeID != null)
                    {
                        coName = unitOfWork.GetRepository<Right>().Filter(f => f.EstateID == inventoryObject.FakeID).Select(s => s.ObjectName).ToList();
                        inventoryObjectsIds.AddRange(unitOfWork.GetRepository<InventoryObject>().Filter(f => f.FakeID == inventoryObject.FakeID && !f.Hidden).Select(s => s.ID).ToList<int>());
                    }

                    if (inventoryObjectsIds.Count > 0)
                    {
                        inventoryObjectsIds = inventoryObjectsIds.Distinct().ToList();

                        foreach (var inventoryObjectId in inventoryObjectsIds)
                        {
                            var io = invObjRepo.Find(f => f.ID == inventoryObjectId);
                            io.IsNonCoreAsset = false;
                            invObjRepo.Update(io);

                            var ncaObjectsIds = service.GetAll(unitOfWork).Where(w => w.EstateObjectID == inventoryObjectId && !w.Hidden).Select(s => s.ID).ToList();

                            foreach (var id in ncaObjectsIds)
                            {
                                service.Delete(unitOfWork, service.Get(unitOfWork, id));
                                count++;
                            }
                        }
                    }
                }

                if (count > 0)
                    return $"Из реестра Объектов ННА исключены все объекты входящие в Имущественный комплекс {pcName} в количестве {count} шт.{System.Environment.NewLine}" +
                        $"Из реестра Объектов ННА исключены все объекты обладающие Правами {String.Join("; ", coName.ToArray())}, {count} шт.";
                else
                {
                    service.Delete(unitOfWork, nonCoreAssetObj);
                    return "Данные успешно удалены!";
                }
            }
            catch (Exception ex)
            {
                return ex.ToStringWithInner();
            }
        }

        /// <summary>
        /// Создание ННА.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="IdsList">Идентификаторы ОИ.</param>
        /// <param name="ncaType">Тип ННА.</param>
        /// <param name="ncaStatus">Статус ННА.</param>
        private void CreateNonCoreAsset(IUnitOfWork uofw, int[] IdsList, NonCoreAssetType ncaType, NonCoreAssetStatus ncaStatus)
        {
            CheckAccountingObjectInRent(uofw, IdsList);
            CheckEstateIsVehicle(uofw, IdsList);

            foreach (int id in IdsList)
            {
                //если есть ННА, пропускаем
                var exNca = uofw.GetRepository<NonCoreAsset>()
                    .Filter(x => !x.Hidden && x.EstateObjectID == id)
                    .FirstOrDefault();
                if (exNca != null) continue;

                var estateRepo = uofw.GetRepository<Estate>();
                Estate element = estateRepo.Find(f => f.ID == id);

                var calcElementRepo = uofw.GetRepository<EstateCalculatedField>();
                EstateCalculatedField calcElement = calcElementRepo.Find(f => f.ID == element.CalculateID);

                //Получаем наименование ИК
                string strPropertyComplexName = "";
                if (element != null)
                {
                    var estateInventory = uofw.GetRepository<InventoryObject>();
                    InventoryObject elementInventory = estateInventory.Find(f => f.ID == element.ID);

                    if (elementInventory != null)
                    {
                        var estateComplex = uofw.GetRepository<PropertyComplex>();
                        if (elementInventory.PropertyComplexID != null && elementInventory.PropertyComplexID != 0)
                        {
                            PropertyComplex elementComplex =
                                estateComplex.Find(f => f.ID == elementInventory.PropertyComplexID);
                            if (elementComplex != null)
                                strPropertyComplexName = elementComplex.Name;
                        }
                    }
                }
                //Получаем кадастровый номер
                string strCadastralNumber = "";
                if (element != null)
                {
                    var estateCadastral = uofw.GetRepository<Cadastral>();
                    Cadastral elementCadastral = estateCadastral.Find(f => f.ID == element.ID);

                    if (elementCadastral != null && !string.IsNullOrEmpty(elementCadastral.CadastralNumber))
                    {
                        strCadastralNumber = elementCadastral.CadastralNumber;
                    }
                }

                //if (element.IsNonCoreAsset)
                //    throw new Exception("Объект уже отнесен к ННА.");

                if (element != null && ncaType != null)
                {
                    var tAssetOwnerRepo = uofw.GetRepository<Society>();
                    var tAssetUserRepo = uofw.GetRepository<Society>();
                    var tAssetMainOwnerRepo = uofw.GetRepository<Society>();
                    Society tAssetOwner = null;
                    Society tAssetUser = null;
                    Society tAssetMainOwner = null;
                    if (calcElement != null)
                    {
                        tAssetOwner = tAssetOwnerRepo.Find(f => f.ID == calcElement.OwnerID);
                        tAssetUser = tAssetUserRepo.Find(f => f.ID == calcElement.WhoUseID);
                        tAssetMainOwner = tAssetMainOwnerRepo.Find(f => f.ID == calcElement.MainOwnerID);
                    }

                    NonCoreAsset nca = new NonCoreAsset
                    {
                        EstateObject = element,
                        NonCoreAssetName = element.Name,
                        NonCoreAssetNameComplex = strPropertyComplexName,
                        NameAsset = element.Name,
                        EstateObjectID = element.ID,
                        InventoryNumber = element.InventoryNumber,
                        CadastralNumber = strCadastralNumber,
                        NonCoreAssetType = ncaType,
                        NonCoreAssetTypeID = ncaType.ID,
                        NonCoreAssetStatus = ncaStatus,
                        NonCoreAssetStatusID = ncaStatus.ID,
                        AssetOwner = tAssetOwner,
                        AssetOwnerID = tAssetOwner?.ID,
                        AssetOwnerName = tAssetOwner?.ShortName,
                        AssetUser = tAssetUser,
                        AssetUserID = tAssetUser?.ID,
                        AssetMainOwner = tAssetMainOwner,
                        AssetMainOwnerID = tAssetMainOwner?.ID
                    };

                    element.IsNonCoreAsset = true;
                    uofw.GetRepository<NonCoreAsset>().Create(nca);
                    estateRepo.Update(element);
                    uofw.SaveChanges();
                }
                else
                    throw new Exception("Не найден элемент для отнесения или тип ННА.");
            }
        }

        /// <summary>
        /// Проверка на наличие похожих записей ННА.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="idsList">Список ИД ИО.</param>
        /// <param name="ncaType">Тип ННА.</param>
        /// <returns>Строка с найденными ННА.</returns>
        private string CheckNonCoreAsset(IUnitOfWork uofw, List<int> idsList, NonCoreAssetType ncaType)
        {
            string result = "";
            try
            {
                var ncaRepo = uofw.GetRepository<NonCoreAsset>();
                var ioRepo = uofw.GetRepository<InventoryObject>();
                var aoRepo = uofw.GetRepository<AccountingObject>();
                foreach (int id in idsList)
                {
                    var aoList = aoRepo.Filter(f => !f.Hidden && !f.IsHistory && f.EstateID == id).ToList();

                    if (aoList.Count > 0)
                    {
                        foreach (var ao in aoList)
                        {
                            //TODO: добвать проверку на кадастровый номер
                            var ncaList = ncaRepo.Filter(f =>
                            !f.Hidden && !f.IsHistory &&
                            (f.AssetOwnerID != ao.OwnerID
                            && (f.InventoryNumber != "" && f.InventoryNumber != null && f.InventoryNumber != ao.InventoryNumber)
                            && f.NonCoreAssetTypeID == ncaType.ID)
                            && ((f.EGRNNumber != "" && f.EGRNNumber != null && f.EGRNNumber == ao.RegNumber) ||
                            (f.NameAsset != "" && f.NameAsset != null && f.NameAsset == ao.Estate.Name)))
                            .ToList();

                            if (ncaList.Count > 0)
                            {
                                foreach (var nca in ncaList)
                                {
                                    result += $"{nca.NameAsset} {nca.InventoryNumber} {System.Environment.NewLine}";
                                }
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return $"ERROR_{ex.ToStringWithInner()}";
            }
        }

        /// <summary>
        /// Подготовка объекта для уведомления.
        /// </summary>
        /// <param name="obj">Объект который требуется привязать.</param>
        /// <returns>Объект готовый для привязки.</returns>
        private LinkBaseObject GetLinkedObj(BaseObject obj)
        {
            var ret = new LinkBaseObject(obj);
            return ret;
        }

        /// <summary>
        /// Удаление множества записей.
        /// </summary>
        /// <typeparam name="T">Тип объектов.</typeparam>
        /// <param name="mnemonic">Мнемоника.</param>
        /// <param name="ids">Идентификаторы объектов, указанный строкой, разделенные ;</param>
        /// <returns></returns>
        [WebHttp.HttpDelete]
        [WebHttp.Route("removeItems/{mnemonic}/{ids}")]
        [GenericAction("mnemonic")]
        public WebHttp.IHttpActionResult RemoveItems<T>(string mnemonic, string ids)
           where T : BaseObject
        {
            int error = 0;
            string message = "";

            try
            {
                if (mnemonic == "NonCoreAsset")
                    RemoveNonCoreAssets(ids);

                using (var uofw = CreateTransactionUnitOfWork())
                {
                    Delete<T>(uofw, ids);
                    uofw.Commit();
                }

                message = "Данные успешно удалены!";
            }
            catch (Exception e)
            {
                error = 1;
                message = $"Ошибка удаления записи: {e.ToStringWithInner()}";
            }

            return Ok(new
            {
                error = error,
                message = message
            });
        }

        private void Delete<T>(IUnitOfWork uofw, string ids)
           where T : BaseObject
        {
            if (String.IsNullOrEmpty(ids)) return;

            var serv = GetBaseObjectService<T>();

            foreach (var item in ids.Split(';'))
            {
                int id = 0;
                int.TryParse(item, out id);
                if (id != 0)
                {
                    var obj = serv.Get(uofw, id);

                    if (obj == null) return;

                    serv.Delete(uofw, obj);
                }
            }
        }

        public WebHttp.IHttpActionResult RemoveNonCoreAssets(string ids)
        {
            if (String.IsNullOrEmpty(ids))
                return Ok(new
                {
                    error = 1,
                    message = "Выберите данные для удаления!"
                });

            string result = "Данные успешно удалены!";
            try
            {
                using (IUnitOfWork uofw = CreateUnitOfWork())
                {
                    foreach (var item in ids.Split(';'))
                    {
                        int id = 0;
                        int.TryParse(item, out id);
                        if (id != 0)
                        {
                            var ncaObj = uofw.GetRepository<NonCoreAsset>().Find(f => f.ID == id);
                            if (ncaObj == null)
                                throw new Exception("Объект не найден.");

                            result = DeleteNonCoreAsset(uofw, ncaObj);
                        }
                    }
                }
                return Ok(new
                {
                    error = 0,
                    message = result
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner()
                });
            }
        }

        [WebHttp.HttpGet]
        [WebHttp.Route("getAuditInfo/{mnemonic}/{id}")]
        [GenericAction("mnemonic")]
        public WebHttp.IHttpActionResult GetAuditInfo<T>(string mnemonic, int id)
           where T : BaseObject
        {
            try
            {
                var res = new List<object>();
                using (IUnitOfWork uofw = CreateUnitOfWork())
                {
                    var tName = typeof(T).GetTypeName();
                    res = uofw.GetRepository<DiffItem>()
                        .Filter(f => !f.Hidden && f.Parent != null
                        && f.Parent.Entity.TypeName == tName
                        && f.Parent.Entity.ID == id)
                       .Include(inc => inc.Parent)
                       .Include(inc => inc.Parent.User)
                       .OrderBy(x => x.Member)
                       .ThenByDescending(x => x.Parent.Date)
                       .Select(s => new
                       {
                           Property = s.Member,
                           Date = s.Parent.Date,
                           OldValue = s.OldValue,
                           NewValue = s.NewValue,
                           UserName = s.Parent.User.FullName
                       }
                        )
                        .ToList<object>();
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Ok(new List<object>());
            }
        }

        /// <summary>
        /// Отменяет результаты импорта.
        /// </summary>
        /// <param name="id">ИД истории импорта.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("cancelImport/{id}")]
        public WebHttp.IHttpActionResult CancelImport(int id)
        {
            int err = 0;
            string mess = "Не удалось отменить импорт.";
            if (id == 0)
                return Ok(new { error = $"Ошибка при выборе истории импорта." });
            try
            {
                using (var uow = CreateUnitOfWork())
                {
                    var history = uow.GetRepository<ImportHistory>().Find(id);

                    var conf = _viewModelConfigService.Get(history.Mnemonic);
                    var serv = conf.GetService<IExcelImportEntity>();
                    if (serv != null)
                    {
                        serv.CancelImport(uow, ref history);
                        mess = history.ResultText;
                    }
                    uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                err = 1;
                mess = ex.Message;
            }

            var res = new
            {
                err = err,
                message = mess
            };
            return Ok(res);
        }

        [WebHttp.HttpGet]
        [WebHttp.Route("getIdByOid/{mnemonic}/{oid}")]
        [GenericAction("mnemonic")]
        public WebHttp.IHttpActionResult GetIdByOid<T>(string mnemonic, Guid oid) where T : BaseObject
        {
            try
            {
                if (!typeof(ITypeObject).IsAssignableFrom(typeof(T)))
                    throw new Exception();

                using (IUnitOfWork uofw = CreateUnitOfWork())
                {
                    var service = GetConfig().GetService<IBaseObjectService<T>>();
                    var id = service.GetAll(uofw, false).Where($"it.{nameof(ITypeObject.Oid)} == @0", oid).FirstOrDefault()?.ID;
                    return Ok(id);
                }
            }
            catch (Exception e)
            {
                return Ok(-1);
            }
        }

        [HttpPost]
        [WebHttp.Route("addNCAPreviousPeriod/{currentID}/{id}")]
        public WebHttp.IHttpActionResult AddNCAPreviousPeriod(int currentID, int id)
        {
            int err = 0;
            string mess = "Не удалось выполнить операцию.";
            if (id == 0)
                return Ok(new { error = $"Ошибка при выборе элемента." });
            try
            {
                using (var uow = CreateUnitOfWork())
                {
                    var currEUP = AppContext.SecurityUser.GetUserIDEUP(uow);
                    var planDate = DateTime.ParseExact(("01.01." + DateTime.Now.Year.ToString()), "dd.MM.yyyy", null);

                    var sales = uow.GetRepository<NonCoreAssetSale>()
                        .Filter(ff => !ff.Hidden).DefaultIfEmpty();

                    var q = uow.GetRepository<NonCoreAssetAndList>()
                    .Filter(f => !f.Hidden && f.ObjLeft != null
                    && !f.ObjLeft.Hidden
                    && f.ObjLeft.ForecastPeriod > planDate
                    && f.ObjLeft.AssetOwner != null
                    && f.ObjLeft.AssetOwner.IDEUP == currEUP
                    && f.ObjRigthId == id)
                    .Where(w => !sales.Select(sale => sale.NonCoreAssetID).Contains(w.ObjLeftId));

                    var rows = q.ToList();
                    var count = 0;
                    foreach (var item in rows)
                    {
                        var clone = uow.GetRepository<NonCoreAssetAndList>().GetOriginal(item.ID);
                        clone.ID = 0;
                        clone.ObjRigth = null;
                        clone.ObjRigthId = currentID;
                        clone.IsNCAPreviousPeriod = true;
                        clone.Oid = Guid.NewGuid();
                        clone.CreateDate = DateTime.Now;
                        uow.GetRepository<NonCoreAssetAndList>().Create(clone);
                        count++;
                    }
                    uow.SaveChanges();
                    mess = $"Добавлено {count} ННА предыдущих периодов";
                }
            }
            catch (Exception ex)
            {
                err = 1;
                mess = ex.Message;
            }

            var res = new
            {
                err = err,
                message = mess
            };
            return Ok(res);
        }

        /// <summary>
        /// Переопределяет ОГ для перечня ННА с созданием дубля перечня, его строк и объектов ННА.
        /// </summary>
        /// <param name="currentID">Текущий ИД перечня ННА.</param>
        /// <param name="id">ИД выбранного ОГ.</param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("ncaChangeOG/{currentID}/{id}")]
        public WebHttp.IHttpActionResult NCAChangeOG(int currentID, int id)
        {
            int err = 0;
            string mess = "Не удалось выполнить операцию.";
            if (id == 0)
                return Ok(new { error = $"Ошибка при выборе элемента." });
            try
            {
                using (var uow = CreateUnitOfWork())
                {
                    var nnaListService = _viewModelConfigService.Get(nameof(NonCoreAssetList))
                        .GetService<CorpProp.Services.Asset.NonCoreAssetListService>();
                    var og = uow.GetRepository<Society>().Find(id);
                    nnaListService.ChangeOG(uow, currentID, og?.ID);
                    mess = $"Для Общества группы {og?.Name} успешно созданы дубликаты Перечня ННА, его строк и объектов ННА.";
                }
            }
            catch (Exception ex)
            {
                err = 1;
                mess = ex.Message;
            }

            var res = new
            {
                err = err,
                message = mess
            };
            return Ok(res);
        }

        [HttpGet]
        [WebHttp.Route("getByDate/{mnemonic}/{id}/{date}")]
        [GenericAction("mnemonic")]
        public WebHttp.IHttpActionResult GetByDate<T>(string mnemonic, int id, string date = null)
          where T : BaseObject
        {
            try
            {
                using (var uofw = CreateTransactionUnitOfWork())
                {
                    var dict = GetObjByDate<T>(uofw, id, date);
                    var mes = dict.Values.First();
                    var err = (!String.IsNullOrEmpty(mes)) ? 1 : 0;
                    return Ok(new
                    {
                        model = dict.Keys.First(),
                        access = GetObjAccess<T>(uofw, id),
                        byDate = date,
                        error = err,
                        message = dict.Values.First()
                    });
                }
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message });
            }
        }

        private Dictionary<object, string> GetObjByDate<T>(IUnitOfWork uofw, int id, string date)
          where T : BaseObject
        {
            Dictionary<object, string> dict = new Dictionary<object, string>();
            DateTime? dt = date.GetDate();
            var config = GetConfig();
            object model = null;
            Base.Service.Crud.IBaseObjectCrudService serv = GetBaseObjectService<T>();

            if (typeof(T).GetInterfaces().Contains(typeof(IArchiveObject)) &&
                this.ViewModelConfigService.GetAll()
                    .Where(f => Type.Equals(f.TypeEntity, typeof(T)) && f.ServiceType != null
                        && f.ServiceType.GetInterfaces().Contains(typeof(IAllObjectsService)))
                    .Any())
            {
                serv = this.ViewModelConfigService.GetAll()
                        .FirstOrDefault(f => Type.Equals(f.TypeEntity, typeof(T)) && f.ServiceType != null
                            && f.ServiceType.GetInterfaces().Contains(typeof(IAllObjectsService)))
                        .GetService<IAllObjectsService>();
            }

            if (serv is IHistoryService<T>)
            {
                //Доработать с учетом доступа к экземпляру
                var newID = ((IHistoryService<T>)serv).GetObjIDByDate(uofw, id, dt);
                if (newID == null || newID == 0)
                {
                    var closeDate = ((IHistoryService<T>)serv).GetMinDate(uofw, id);
                    var closeDateStr = (closeDate != null) ? closeDate.Value.ToString("dd.MM.yyyy") : "НЕТ ДАННЫХ";
                    dict.Add(1, $"На выбранную дату <{date}> нет данных. Ближайшая дата, на которую есть данные:{closeDateStr}.");
                    return dict;
                }
                else
                    model = config.DetailView.GetData(uofw, serv, newID.Value);
            }
            else
            {
                model = config.DetailView.GetData(uofw, serv, id);
            }
            dict.Add(model, "");
            return dict;
        }

        private object GetObjAccess<T>(IUnitOfWork uofw, int id)
        {
            if (typeof(IAccessibleObject).IsAssignableFrom(typeof(T)))
            {
                var access = _security_service.GetAccessType(uofw, typeof(T), id);

                return new
                {
                    Update = access.HasFlag(AccessType.Update),
                    Delete = access.HasFlag(AccessType.Delete)
                };
            }

            return new
            {
                Update = true,
                Delete = true,
            };
        }

        /// <summary>
        /// Уведомление о результате импорта.
        /// </summary>
        /// <param name="mnemonic"></param>
        /// <returns></returns>
        [HttpPost]
        [WebHttp.Route("notifyOfImport")]
        public WebHttp.IHttpActionResult NotifyOfImport(int[] ids)
        {
            int err = 0;
            string mess = "Уведомления отправлены";
            try
            {
                using (var uow = CreateTransactionUnitOfWork())
                {
                    var conf = _viewModelConfigService.Get(nameof(ImportHistory));
                    var serv = conf.GetService<IBaseObjectService<ImportHistory>>();
                    if (serv != null)
                    {
                        var items = serv.GetAll(uow)
                            .Include(s => s.ImportErrorLogs)
                            .Where(w => ids.Contains(w.ID))
                            .ToList()
                            .GroupBy(gr => gr.Mnemonic);
                        if (items.Count() == 0)
                            mess = "Новых уведомлений для отправки нет.";
                        else
                        {
                            var sent = 0;
                            var noSent = 0;
                            foreach (IGrouping<string, ImportHistory> group in items)
                            {
                                _emailService.SetNotificationStrategyByMnemonic(group.Key);

                                _emailService.SendImportNotice(uow, group.ToList<ImportHistory>(), ref sent, ref noSent);
                            }

                            mess = $"Всего отправлено: {sent} уведомлений.";
                            if (noSent > 0)
                            {
                                err = 1;
                                mess += System.Environment.NewLine;
                                mess += $"Не удалось отправить: {noSent} уведомлений.";
                            }
                        }
                    }

                    uow.SaveChanges();
                    uow.Commit();
                }
            }
            catch (Exception ex)
            {
                err = 1;
                mess = ex.Message;
            }

            var res = new
            {
                err = err,
                message = mess
            };
            return Ok(res);
        }

        [HttpGet]
        [WebHttp.Route("warmUp")]
        public WebHttp.IHttpActionResult WarmUp()
        {
            using (IUnitOfWork uow = CreateUnitOfWork())
            {
                var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic);

                List<Type> types = new List<Type>();
                foreach (var item in assemblys)
                {
                    var ass = GetAssemblyTypes(item)
                        .Where(w => !w.IsAbstract && !w.IsInterface && w.IsSubclassOf(typeof(Base.BaseObject)));
                    if (ass.Any())
                        types.AddRange(ass);
                }

                foreach (var tt in types)
                {
                    try
                    {
                        MethodInfo methodUow = uow.GetType().GetMethod("GetRepository");
                        MethodInfo genericUow = methodUow.MakeGenericMethod(tt);
                        var reposit = genericUow.Invoke(uow, null);
                        object[] paramss = new object[] { new object[] { 0 } };
                        MethodInfo method = reposit.GetType().GetMethod("Find", new Type[] { typeof(object[]) });
                        method.Invoke(reposit, paramss);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            return Ok(new
            {
                error = 0,
                message = "OK"
            });
        }

        /// <summary>
        /// Проверка ОБУ на нахождени в аренде.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="estateIds">ИД ОИ.</param>
        private static void CheckAccountingObjectInRent(IUnitOfWork unitOfWork, int[] estateIds)
        {
            string names = "";
            int accountNumber = 0;
            int? statusId = unitOfWork.GetRepository<AccountingStatus>().Find(f => f.Code == "052")?.ID;
            if (statusId == null)
                return;

            //Сделать исключение - когда Балансодержатель IDEUP не равен 1.
            if (!unitOfWork.GetRepository<AccountingObject>().Filter(f =>
                    !f.Hidden
                    && !f.IsHistory
                    && f.EstateID != null
                    && estateIds.Contains((int)f.EstateID)
                    && f.AccountingStatusID == statusId
                    && f.Estate != null
                    && f.Estate.Calculate != null
                    && f.Estate.Calculate.Owner != null
                    && f.Estate.Calculate.Owner.IDEUP != "1"
                    ).Any())
                return;

            List<AccountingObject> accountingObjects =
                unitOfWork.GetRepository<AccountingObject>().Filter(f =>
                    f.EstateID != null && estateIds.Contains((int)f.EstateID) &&
                    f.AccountingStatusID == statusId && !f.Hidden).ToList();

            if (accountingObjects.Count() > 0)
            {
                foreach (var ao in accountingObjects)
                {
                    if (ao.AccountNumber == null || !int.TryParse(ao.AccountNumber, out accountNumber))
                        continue;

                    if (int.TryParse(ao.AccountNumber, out accountNumber) && accountNumber == 1)
                        names += names.Length > 0 ? $"<br/>{ao.Name}" : ao.Name;
                }
            }

            if (!string.IsNullOrEmpty(names))
                throw new Exception($"Следующие ОС находятся в аренде:<br/>{names}");
        }

        private static void CheckEstateIsVehicle(IUnitOfWork unitOfWork, int[] estateIds)
        {
            string names = "";

            List<Vehicle> vehicles = unitOfWork.GetRepository<Vehicle>().Filter(f => estateIds.Contains(f.ID) && !f.Hidden).ToList();

            if (vehicles.Count > 0)
            {
                foreach (var veh in vehicles)
                {
                    names += names.Length > 0 ? $"<br/>{veh.Name}" : veh.Name;
                }
            }

            if (!string.IsNullOrEmpty(names))
                throw new Exception($"Следующие ОИ являются ТС:<br/>{names}");
        }

        private static IList<Type> GetAssemblyTypes(System.Reflection.Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (System.Reflection.ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null).ToList();
            }
        }

        /// <summary>
        /// Возвращает перечень статусов строки ННА.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [WebHttp.Route("getNNARowStates")]
        public WebHttp.IHttpActionResult GetNNARowStates()
        {
            var list = new List<NonCoreAssetListItemState>();
            using (var uow = CreateUnitOfWork())
            {
                list = (Base.Ambient.AppContext.SecurityUser.IsFromCauk(uow)) ?

                    uow.GetRepository<NonCoreAssetListItemState>()
                    .Filter(f => !f.Hidden && (f.Code == "104" || f.Code == "108" || f.Code == "109"))
                    .ToList()
                    :
                    uow.GetRepository<NonCoreAssetListItemState>()
                    .Filter(f => !f.Hidden && (f.Code == "104" || f.Code == "108"))
                    .ToList();
            }
            return Ok(list);
        }

        /// <summary>
        /// Отправляет выбранные истории импорта в архив.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mnemonic"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [WebHttp.HttpPost]
        [WebHttp.Route("sendToArhive/{mnemonic}/{ids}")]
        [GenericAction("mnemonic")]
        public WebHttp.IHttpActionResult SendToArhive<T>(string mnemonic, string ids)
          where T : BaseObject
        {
            int error = 0;
            string message = "";

            try
            {
                if (String.IsNullOrEmpty(ids))
                    return Ok(new
                    {
                        error = 1,
                        message = "Выберите элементы для перемещения в архив."
                    });

                using (var uofw = CreateTransactionUnitOfWork())
                {
                    var state = uofw.GetRepository<ImportHistoryState>()
                                .Filter(f => f.Code == "Arhive").FirstOrDefault();
                    if (state == null) return Ok(new
                    {
                        error = 1,
                        message = "Не найден статус <Архив>"
                    });
                    foreach (var item in ids.Split(';'))
                    {
                        int id = 0;
                        int.TryParse(item, out id);
                        if (id != 0)
                        {
                            var obj = uofw.GetRepository<ImportHistory>()
                                .Filter(f => f.ID == id).FirstOrDefault();
                            if (obj != null)
                            {
                                obj.ImportHistoryState = state;
                                obj.ImportHistoryStateID = state.ID;
                            }
                        }
                    }
                    uofw.SaveChanges();
                    uofw.Commit();
                }

                message = "Записи успешно пермещены в архив!";
            }
            catch (Exception e)
            {
                error = 1;
                message = $"Ошибка перемещения записи в архив: {e.ToStringWithInner()}";
            }

            return Ok(new
            {
                error = error,
                message = message
            });
        }
    }
}
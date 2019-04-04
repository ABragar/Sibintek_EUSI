using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Base;
using Base.DAL;
using Base.Service.Log;
using Base.UI;
using Base.Utils.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Base;
using CorpProp.Entities.Estate;
using CorpProp.Entities.ManyToMany;
using EUSI.Entities.Models;
using WebApi.Attributes;
using WebApi.Controllers;
using WebApi.Models.Crud;

namespace EUSI.WebApi.Controllers
{
    [CheckSecurityUser]
    [RoutePrefix("eusi/archive")]
    public class ArchiveController : BaseApiController
    {
        private IUnitOfWork _unitOfWork;
        private const string OkResMessage = "Данные успешно сохранены";
        private readonly ILogService _logger;

        private IUnitOfWork UnitOfWork => _unitOfWork ?? (_unitOfWork = CreateUnitOfWork());

        public ArchiveController(IViewModelConfigService viewModelConfigService,
            IUnitOfWorkFactory unitOfWorkFactory, ILogService logger)
            : base(viewModelConfigService, unitOfWorkFactory, logger)
        {
            _logger = logger;
        }


        [HttpPost]
        [Route("sendEstateToArchive/{mnemonic}")]
        [GenericAction("mnemonic")]
        public IHttpActionResult SendEstateToArchive<T>(string mnemonic, [FromBody] SaveModel<T> save_model)
            where T : BaseObject
        {
            var sm = save_model as SaveModel<SetCommentModel>;
            if (String.IsNullOrWhiteSpace(sm.model.EntityIds))
            {
                return Ok(new
                {
                    error = 1,
                    message = "Объекты не выбраны.",
                });
            }
            try
            {
                var idsArray = sm.model.EntityIds.Split(';').Select(int.Parse).ToArray();               
                // ИК нельзя помещать в архив
                var iks = GetIKIds(idsArray);
                if (iks.Any())
                {
                    idsArray = idsArray.Except(iks).ToArray();
                }
                if (!idsArray.Any())
                {
                    return Ok(new
                    {
                        error = 1,
                        message = "Нет объектов для пометки на удаление.",
                    });
                }

                var selectedItems = GetSelectedItems<Estate>(idsArray);
                selectedItems.ForEach(e => {

                    e.IsArchived = true;

                    if (!String.IsNullOrWhiteSpace(sm.model.Comment))
                        e.Comment = sm.model.Comment;
                });

                var aoRepo = UnitOfWork.GetRepository<AccountingObject>();
                var accountingObjects = aoRepo.Filter(
                    ao => ao.EstateID.HasValue
                          && idsArray.Contains(ao.EstateID.Value)
                          && !ao.Hidden);

                foreach (var ao in accountingObjects)
                {
                    ao.IsArchived = true;
                }

                UnitOfWork.SaveChanges();

                return Ok(new
                {
                    error = 0,
                    message = OkResMessage
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner(),
                });
            }
        }
        [HttpPost]
        [Route("returnEstateFromArchive/{mnemonic}")]
        public IHttpActionResult ReturnEstateFromArchive(string mnemonic, string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return Ok(new
                {
                    error = 1,
                    message = "Объекты не выбраны.",
                });
            try
            {
                var idsArray = ids.Split(';').Select(int.Parse).ToArray();
                var selectedItems = GetSelectedItems<Estate>(idsArray);
                selectedItems.ForEach(e => e.IsArchived = false);

                UnitOfWork.SaveChanges();

                return Ok(new
                {
                    error = 0,
                    message = OkResMessage,
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner(),
                });
            }
        }

        [HttpPost]
        [Route("sendObuToArchive/{mnemonic}")]
        [GenericAction("mnemonic")]
        public IHttpActionResult SendObuToArchive<T>(string mnemonic, [FromBody] SaveModel<T> save_model)
        {
            var sm = save_model as SaveModel<SetCommentModel>;
            if (String.IsNullOrWhiteSpace(sm.model.EntityIds))
            {
                return Ok(new
                {
                    error = 1,
                    message = "Объекты не выбраны.",
                });
            }
            try
            {
                var idsArray = sm.model.EntityIds.Split(';').Select(int.Parse).ToArray();
                var selectedItems = GetSelectedItems<AccountingObject>(idsArray);
                selectedItems.ForEach(e => {

                    e.IsArchived = true;

                    if (!String.IsNullOrWhiteSpace(sm.model.Comment))
                        e.Comment = sm.model.Comment;
                });

                UnitOfWork.SaveChanges();

                return Ok(new
                {
                    error = 0,
                    message = OkResMessage,
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner(),
                });
            }
        }

        [HttpPost]
        [Route("returnObuFromArchive/{mnemonic}")]
        public IHttpActionResult ReturnObuFromArchive(string mnemonic, string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return Ok(new
                {
                    error = 1,
                    message = "Объекты не выбраны.",
                });
            try
            {
                var idsArray = ids.Split(';').Select(int.Parse).ToArray();
                var selectedItems = GetSelectedItems<AccountingObject>(idsArray);
                selectedItems.ForEach(e => e.IsArchived = false);
                UnitOfWork.SaveChanges();
                return Ok(new
                {
                    error = 0,
                    message = OkResMessage,
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    error = 1,
                    message = ex.ToStringWithInner(),
                });
            }
        }

        private List<T> GetSelectedItems<T>(int[] ids)
            where T : TypeObject
        {
            var repo = UnitOfWork.GetRepository<T>();
            return repo.Filter(f => ids.Contains(f.ID) && !f.Hidden).ToList();
        }

        private List<int> GetIKIds(int[] ids)           
        {
            
            return UnitOfWork.GetRepository<PropertyComplexIO>()
            .FilterAsNoTracking(f => ids.Contains(f.ID) && !f.Hidden)
            .Select(s => s.ID)
            .ToList();
        }
    }
}

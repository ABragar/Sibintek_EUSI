using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Base.BusinessProcesses.Services.Abstract;
using Base.DAL;
using Base.Security;
using Base.Service.Log;
using Base.UI;
using CorpProp.Entities.Security;
using WebApi.Attributes;
using WebApi.Controllers;

namespace EUSI.WebApi.Controllers
{
    /// <summary>
    /// Блокировка записей.
    /// </summary>
    [CheckSecurityUser]
    [RoutePrefix("eusi/recordBlocker")]
    class RecordBlockController : BaseApiController
    {
        private readonly ILogService _logger;

        private static List<BlockerEntityModel> BlockedRecords = new List<BlockerEntityModel>();
        private ISecurityUser _securityUser = Base.Ambient.AppContext.SecurityUser;

        public RecordBlockController(IViewModelConfigService viewModelConfigService, IUnitOfWorkFactory unitOfWorkFactory, ILogService logger) : base(viewModelConfigService, unitOfWorkFactory, logger) { _logger = logger; }

        /// <summary>
        /// Проверка и установка блокировки на запись.
        /// </summary>
        /// <param name="entityId">ИД записи.</param>
        /// <param name="entityMnemonic">Мнемоника.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("checkRecordBlock/{entityId}/{entityMnemonic}")]
        public IHttpActionResult CheckBlock(int entityId, string entityMnemonic)
        {
            try
            {
                bool isRecordBlocked = false;
                string userName = "";
                if (string.IsNullOrEmpty(entityMnemonic))
                {
                    _logger.Log("RecordBlockController.CheckBlock: Параметр \"entityMnemonic\" не имеет значение.");
                    entityMnemonic = "";
                }
                else
                {
                    if (BlockedRecords == null || BlockedRecords.Count < 0)
                        BlockedRecords = new List<BlockerEntityModel>();

                    BlockerEntityModel record = BlockedRecords.FirstOrDefault(f => f.BlockedEntityId == entityId
                        && f.BlockedEntityMnemonic.ToLower() == entityMnemonic.ToLower());

                    bool isSameUser = record?.UserId == _securityUser.ID;
                    isRecordBlocked = !isSameUser && record != null;

                    if (!isRecordBlocked && !isSameUser && !string.IsNullOrWhiteSpace(entityMnemonic))
                    {
                        BlockedRecords.Add(new BlockerEntityModel
                        {
                            BlockedEntityId = entityId,
                            BlockedEntityMnemonic = entityMnemonic,
                            UserId = _securityUser.ID
                        });

                        BlockedRecords.Distinct();
                    }
                    else
                    {
                        using (IUnitOfWork unitOfWork = CreateUnitOfWork())
                        {
                            if (record != null)
                            {
                                var userID = record?.UserId;
                                SibUser user = unitOfWork.GetRepository<SibUser>()
                                    .FilterAsNoTracking(f => f.User != null && f.UserID == record.UserId)
                                    .FirstOrDefault();
                                userName = user?.FullName;
                            }
                        }
                    }
                }

                return Ok(new
                {
                    isBlocked = isRecordBlocked,
                    userFullName = userName
                });
            }
            catch (Exception e)
            {
                _logger.Log(e);
                //if (BlockedRecords == null || BlockedRecords.Count < 0)
                    BlockedRecords = new List<BlockerEntityModel>();
                return Ok(new { error = e.Message });
            }
        }

        /// <summary>
        /// Снятие блокировки с записи.
        /// </summary>
        /// <param name="entityId">ИД записи.</param>
        /// <param name="entityMnemonic">Мнемоника.</param>
        [HttpPost]
        [Route("removeRecordBlock/{entityId}/{entityMnemonic}")]
        public void RemoveLock(int entityId, string entityMnemonic)
        {
            try
            {
                if (string.IsNullOrEmpty(entityMnemonic))
                {
                    _logger.Log("RecordBlockController.RemoveLock: Параметр \"entityMnemonic\" не имеет значение.");
                    entityMnemonic = "";
                }
                else
                {
                    if (BlockedRecords == null || BlockedRecords.Count < 0)
                        BlockedRecords = new List<BlockerEntityModel>();
                    BlockerEntityModel blockerEntity = BlockedRecords.FirstOrDefault(f => f.BlockedEntityId == entityId
                    && f.BlockedEntityMnemonic.ToLower() == entityMnemonic.ToLower()
                    && f.UserId == _securityUser.ID);

                    if (blockerEntity != null && BlockedRecords.Count > 0)
                    {
                        if (BlockedRecords.Contains(blockerEntity))
                            BlockedRecords.Remove(blockerEntity);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                //if (BlockedRecords.Count < 0)
                    BlockedRecords = new List<BlockerEntityModel>();
            }
        }


        internal class BlockerEntityModel
        {
            public int BlockedEntityId { get; set; }

            public string BlockedEntityMnemonic { get; set; }

            public int UserId { get; set; }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Base.DAL;
using CorpProp.Entities.Access;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class SibPermissionController : BaseController
    {
        public SibPermissionController(IBaseControllerServiceFacade serviceFacade) : base(serviceFacade)
        {
        }

        public ActionResult GetSibPermissions()
        {
            return PartialView("SibPermission");
        }

        public enum AccessEnum
        {
            Access,
            Deny
        }

        public class PermissionsParams
        {
            public List<int> ObjectIds { get; set; }
            public List<int> SocietyIds { get; set; }
            public string Mnemonic { get; set; }
            public AccessEnum Access { get; set; }
        }

        [HttpPost]
        public ActionResult SetPermission(PermissionsParams param)
        {
            using (var uow = CreateUnitOfWork())
            {
                foreach (var objectId in param.ObjectIds)
                {
                    foreach (var societyId in param.SocietyIds)
                    {
                        var permissions = uow.GetRepository<SibPermission>()
                                            .Filter(
                                                    sibPermission =>
                                                        sibPermission.SocietyID == societyId 
                                                        && sibPermission.ObjectMnemonic == param.Mnemonic 
                                                        && sibPermission.ObjectId == objectId);
                        var permission = permissions.FirstOrDefault();
                        if (permission == null)
                            CreatePermission(uow, param.Access, param.Mnemonic, objectId, societyId);
                        else
                            UpdatePermission(uow, param.Access, permission, param.Mnemonic, objectId, societyId);
                    }
                }
                uow.SaveChanges();
            }
            return new JsonNetResult(null);
        }

        private void UpdatePermission(IUnitOfWork uow, AccessEnum access, SibPermission permission, string mnemonic, int objectId, int societyId)
        {
            if (access == AccessEnum.Deny)
            {
                uow.GetRepository<SibPermission>().Delete(permission);
            }
            else
            {
                permission.SocietyID = societyId;
                permission.ObjectMnemonic = mnemonic;
                permission.ObjectId = objectId;
                uow.GetRepository<SibPermission>().Update(permission);
            }
        }

        private void CreatePermission(IUnitOfWork uow, AccessEnum access, string mnemonic, int objectId, int societyId)
        {
            if (access == AccessEnum.Deny)
                return;
            var permission = new SibPermission()
            {
                SocietyID = societyId,
                ObjectMnemonic = mnemonic,
                ObjectId = objectId
            };
            uow.GetRepository<SibPermission>().Create(permission);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Base.DAL;
using Base.Extensions;
using Base.Security;
using Base.Security.Service;
using CorpProp.Entities.Access;
using WebUI.Helpers;

namespace WebUI.Controllers
{
    public class RolesController: BaseController
    {
        private readonly IRoleService _roleService;

        public RolesController(IBaseControllerServiceFacade serviceFacade, IRoleService roleService) : base(serviceFacade)
        {
            _roleService = roleService;
        }

        public ActionResult GetPermissionsToolbar()
        {
            return PartialView("RolesCustomToolbar");
        }

        public class PermissionsParams
        {
            public List<int> SrcObjectIds { get; set; }
            public List<int> DestObjectIds { get; set; }
        }

        [HttpPost]
        public ActionResult SetPermissionAddFrom(PermissionsParams param)
        {
            if (param != null)
            {
                if (param.SrcObjectIds == null)
                    throw new ArgumentException(@"Исходные роли отсутствуют", nameof(param.SrcObjectIds));
                if (param.DestObjectIds == null)
                    throw new ArgumentException(@"Роли назначения отсутствуют", nameof(param.DestObjectIds));
            }
            else
            {
                throw new ArgumentException(@"Параметр обязателен", nameof(param));
            }


            using (var uow = CreateUnitOfWork())
            {
                var srcRoles = _roleService.GetAll(uow).Where(role => param.SrcObjectIds.Contains(role.ID));
                var destRoles = _roleService.GetAll(uow).Where(role => param.DestObjectIds.Contains(role.ID));

                var permissionsRepo = uow.GetRepository<Permission>();

                var permissions = permissionsRepo.All();

                var srcPermissions = from permission in permissions
                                     join srcRole in srcRoles on permission.RoleID equals srcRole.ID
                                     select permission;
                var srcPermissionsDistinct = srcPermissions.Distinct();

                var srcPermissionsList = srcPermissionsDistinct.ToList();

                try
                {
                    destRoles.ForEach(
                                      role =>
                                      {
                                          srcPermissionsList.ForEach(
                                                                     permission =>
                                                                     {
                                                                         var permissionClone = permission;
                                                                         permissionClone.RoleID = role.ID;
                                                                         permissionsRepo.Create(permissionClone);
                                                                     });
                                      });

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                uow.SaveChanges();

            }

            return new JsonNetResult(null);
        }
        
    }
}
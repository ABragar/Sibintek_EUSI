using System;
using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Security;
using Base.Security.Service;
using Base.Service.Log;
using Base.UI;
using Common.Data.BaseImport.Exceptions;
using Common.Data.BaseImport.Projections;
using Common.Data.Services.Abstract;

namespace Common.Data.Services.Concrete
{
    public class RolesBaseImportService : BaseImportService, IRolesBaseImportService
    {
        private readonly IViewModelConfigService _viewModelConfigService;
        private readonly IRoleService _roleService;
        private bool _fullImport = true;
        private Dictionary<string, Role> _registryRoles;
        public RolesBaseImportService(IViewModelConfigService viewModelConfigService, IRoleService roleService,
            IUnitOfWorkFactory unitOfWorkFactory, ILogService logService) : base(unitOfWorkFactory, logService)
        {
            _viewModelConfigService = viewModelConfigService;
            _roleService = roleService;
        }
        public override void Import(string pathFile)
        {
            try
            {
                _registryRoles = new Dictionary<string, Role>();
                ExecuteProjection<RoleImportProjection>(CreateRole, pathFile, "Лист1");
                ExecuteProjection<PermissionImportProjection>(AddPermissions, pathFile, "Пример");

                //Update role Permissions
                foreach (var role in _registryRoles.Select(x => x.Value).ToList())
                {
                    using (var unitOfWork = _unitOfWorkFactory.Create())
                    {
                        var roleToUpdate = _roleService.GetAll(unitOfWork).FirstOrDefault(x => x.Name == role.Name);

                        if (roleToUpdate != null)
                        {
                            roleToUpdate.Permissions = role.Permissions;

                            try
                            {
                                _roleService.Update(unitOfWork, roleToUpdate);
                            }
                            catch (Exception ex)
                            {
                                _logService.Log(ex, $"roleName: {roleToUpdate.Name}");
                            }

                        }
                        else
                        {
                            _logService.Log($"roleName: {role.Name} not found");
                        }
                    }
                }
                if (!_fullImport)
                    throw new FailImportException();
            }
            catch (FailImportException e)
            {
                throw new FailImportException("Произошла одна или несколько ошибок при обработке записей");
            }
            catch (Exception e)
            {
                _logService.Log($"Ошибка во время импорта: {e.Message}");
                throw new FailImportException("Произошел сбой во время импорта");
            }

        }

        private void CreateRole(RoleImportProjection roleProjection)
        {
            try
            {
                if (roleProjection.ID == null) return;
                using (var unitOfWork = _unitOfWorkFactory.Create())
                {
                    Role role = _roleService.GetAll(unitOfWork).SingleOrDefault(x => x.Name == roleProjection.Name);
                    if (role != null)
                        _roleService.Delete(unitOfWork, role);
                    role = _roleService.Create(unitOfWork, new Role() { Name = roleProjection.Name });
                    if (_registryRoles.ContainsKey(roleProjection.ID))
                        throw new Exception("В excel файле определено несколько одинаковых ID роли");
                    else
                        _registryRoles.Add(roleProjection.ID, role);
                }
            }
            catch (Exception e)
            {
                _fullImport = false;
                throw new Exception(e.Message);
            }
        }

        private void AddPermissions(PermissionImportProjection projection)
        {
            try
            {
                Type type = FindType(projection.Mnemonic);
                if (type == null)
                    return;
                Permission permission = new Permission(type)
                {
                    AllowRead = projection.Read == "да",
                    AllowWrite = projection.Write == "да",
                    AllowCreate = projection.Create == "да",
                    AllowDelete = projection.Delete == "да"
                };
                _registryRoles[projection.RoleId].Permissions.Add(permission);
            }
            catch (Exception e)
            {
                _fullImport = false;
                throw new Exception(e.Message);
            }
        }

        private Type FindType(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            return _viewModelConfigService.Get(name)?.TypeEntity ?? Type.GetType(name, false);
        }
    }
}

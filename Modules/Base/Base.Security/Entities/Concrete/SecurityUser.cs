using System;
using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Enums;
using Base.Security.Entities.Concrete;
using Base.Security.Service.Abstract;

namespace Base.Security
{
    public class SecurityUser : ISecurityUser
    {
        private readonly IEnumerable<string> _roles;
        private readonly IEnumerable<string> _roleCodes;
        private readonly IEnumerable<int> _roleIds;
        private readonly IReadOnlyList<SystemRole> _sysroles;
        private readonly IDictionary<string, List<TypePermission>> _permissions;

        private readonly IDictionary<string, HashSet<string>> _propertiesCannotRead;
        private readonly IDictionary<string, HashSet<string>> _propertiesCannotWrite;


        public SecurityUser(IUnitOfWork uow, User user, string login)
        {
            
        Login = login;

            _permissions = new Dictionary<string, List<TypePermission>>();
            _propertiesCannotRead = new Dictionary<string, HashSet<string>>();
            _propertiesCannotWrite = new Dictionary<string, HashSet<string>>();

            if (user == null) return;

            ID = user.ID;

            if (user.BaseProfileID != null)
            {
                ProfileInfo = uow.GetRepository<BaseProfile>()
                    .All()
                    .Where(x => x.ID == user.BaseProfileID.Value)
                    .Select(x => new UserProfileInfo()
                {
                    ID = x.ID,
                    ImageGuid = x.Image != null ? x.Image.FileID : Guid.Empty,
                    FullName = x.FullName,
                    IsEmpty = x.IsEmpty,
                    Mnemonic = x.ExtraID
                }).Single();

                ProfileInfo.Email =
                    uow.GetRepository<ProfileEmail>()
                        .All()
                        .Where(x => x.BaseProfileID == user.BaseProfileID.Value)
                        .Where(x => x.IsPrimary)
                        .Select(x => x.Email)
                        .FirstOrDefault();
            }

            var cat = uow.GetRepository<UserCategory>().Find(user.CategoryID);

            if(cat == null) return;

            CategoryInfo = new UserCategoryInfo()
            {
                ID = user.CategoryID,
                Name = cat.Name,
                SysAllParents = cat.sys_all_parents
            };

            var roles = GetCategoryRoles(uow, cat);

            _roles = roles.Select(m => m.Name.ToUpper()).ToList();
            _roleCodes = roles.Select(m => m.Code?.ToLower()).ToList();
            _roleIds = roles.Select(m => m.ID).ToList();

            _sysroles = roles.Where(x => x.SystemRole != null).Select(m => m.SystemRole).ToList().AsReadOnly();

            var permissions =
                roles.SelectMany(m => m.Permissions)
                    .Where(x => x.FullName != null)
                    .OrderBy(m => m.FullName)
                    .ToList();

            permissions.ForEach(perm =>
            {
                if (!_permissions.ContainsKey(perm.FullName))
                {
                    _permissions.Add(perm.FullName, new List<TypePermission>());
                }

                var listPerm = _permissions[perm.FullName];

                if (perm.AllowCreate && !listPerm.Contains(TypePermission.Create))
                {
                    listPerm.Add(TypePermission.Create);
                }
                if (perm.AllowDelete && !listPerm.Contains(TypePermission.Delete))
                {
                    listPerm.Add(TypePermission.Delete);
                }
                if (perm.AllowNavigate && !listPerm.Contains(TypePermission.Navigate))
                {
                    listPerm.Add(TypePermission.Navigate);
                }
                if (perm.AllowRead && !listPerm.Contains(TypePermission.Read))
                {
                    listPerm.Add(TypePermission.Read);
                }
                if (perm.AllowWrite && !listPerm.Contains(TypePermission.Write))
                {
                    listPerm.Add(TypePermission.Write);
                }


                foreach (var propertyPermission in perm.PropertyPermissions.Where(x => !x.AllowRead))
                {
                    if (!_propertiesCannotRead.ContainsKey(perm.FullName))
                    {
                        _propertiesCannotRead.Add(perm.FullName, new HashSet<string>());
                    }

                    var hash = _propertiesCannotRead[perm.FullName];

                    if (!hash.Contains(propertyPermission.PropertyName))
                    {
                        hash.Add(propertyPermission.PropertyName);
                    }
                }

                foreach (var propertyPermission in perm.PropertyPermissions.Where(x => !x.AllowWrite))
                {
                    if (!_propertiesCannotWrite.ContainsKey(perm.FullName))
                    {
                        _propertiesCannotWrite.Add(perm.FullName, new HashSet<string>());
                    }

                    var hash = _propertiesCannotWrite[perm.FullName];

                    if (!hash.Contains(propertyPermission.PropertyName))
                    {
                        hash.Add(propertyPermission.PropertyName);
                    }
                }
            });

            IsAdmin = _sysroles.Any(m => m == SystemRole.Admin);
        }

        public string Login { get; }
        public int ID { get; }
        public bool IsAdmin { get; }
        public UserProfileInfo ProfileInfo { get; }

        public bool PropertyCanRead(Type type, string propertyName)
        {
            if (!IsPermission(type, TypePermission.Read)) return false;

            var strtype = type.GetTypeName();

            return !_propertiesCannotRead.ContainsKey(strtype) || !_propertiesCannotRead[strtype].Contains(propertyName);
        }

        public bool PropertyCanWrite(Type type, string propertyName)
        {
            if (!IsPermission(type, TypePermission.Read)) return false;

            var strtype = type.GetTypeName();

            return !_propertiesCannotWrite.ContainsKey(strtype) || !_propertiesCannotWrite[strtype].Contains(propertyName);
        }

        public string GetKey()
        {
            return ID.ToString();
        }

        public UserCategoryInfo CategoryInfo { get; }

        public bool IsPermission(Type type, TypePermission typePermission)
        {
            if (IsAdmin) return true;

            if (type == null || typePermission == 0)
                throw new ArgumentNullException(nameof(type));

            if (_permissions.ContainsKey(type.GetTypeName()) && _permissions[type.GetTypeName()].Aggregate((r, p) => r|p).HasFlag(typePermission))
                return true;
            return _permissions.ContainsKey(type.FullName) && _permissions[type.FullName].Aggregate((r, p) => r | p).HasFlag(typePermission);
        }

        public bool IsPermission<T>(TypePermission typePermission)
        {
            return IsPermission(typeof(T), typePermission);
        }


        public bool IsRole(string role)
        {
            if (_roles != null && !string.IsNullOrEmpty(role))
            {
                return _roles.Contains(role.ToUpper());
            }

            return false;
        }

        public bool IsRole(int roleID)
        {
            return _roleIds != null && _roleIds.Contains(roleID);
        }

        public bool IsSysRole(SystemRole sysrole)
        {
            return _roles != null && _sysroles.Any(x => x == sysrole);
        }

        private ICollection<Role> GetCategoryRoles(IUnitOfWork uow, UserCategory category)
        {
            if (category == null)
                throw new NullReferenceException("User category is null");

            var roles = category.Roles;

            if (category.ParentID != null)
            {
                var parents = category.sys_all_parents.Split(HCategory.Seperator).Select(HCategory.IdToInt);
                var r = uow.GetRepository<UserCategory>().All().Where(x => parents.Contains(x.ID)).SelectMany(x => x.Roles).Distinct().ToList();
                roles = roles.Union(r).Distinct().ToList();
            }

            return roles;
        }


        //sib
        public bool IsRoleCode(string roleCode)
        {
            if (_roleCodes != null && !String.IsNullOrEmpty(roleCode))
            {
                return _roleCodes.Contains(roleCode.ToLower());
            }

            return false;
        }
        //end sib

    }

    public class SystemUser : ISecurityUser
    {
        public string Login => null;
        public int ID => 0;
        public bool IsAdmin => true;
        public UserProfileInfo ProfileInfo => new UserProfileInfo() { FullName = "SystemUser" };
        public UserCategoryInfo CategoryInfo => null;

        public bool IsPermission(Type type, TypePermission typePermission)
        {
            return true;
        }

        public bool IsPermission<T>(TypePermission typePermission)
        {
            return true;
        }

        public bool IsRole(string role)
        {
            throw new NotImplementedException();
        }

        public bool IsRole(int roleID)
        {
            throw new NotImplementedException();
        }

        public bool IsSysRole(SystemRole sysrole)
        {
            throw new NotImplementedException();
        }

        public bool PropertyCanRead(Type type, string propertyName)
        {
            return true;
        }

        public bool PropertyCanWrite(Type type, string propertyName)
        {
            return true;
        }

        public string GetKey()
        {
            throw new NotImplementedException();
        }

        //sib
        public bool IsRoleCode(string roleCode)
        {
            throw new NotImplementedException();
        }
        //end sib
    }
}

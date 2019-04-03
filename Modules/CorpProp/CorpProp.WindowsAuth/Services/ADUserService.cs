using Base.DAL;
using Base.Security;
using Base.Security.Service;
using Base.Service;
using Base.Service.Log;
using Base.Settings;
using CorpProp.Entities.Security;
using CorpProp.Entities.Settings;
using CorpProp.Extentions;
using CorpProp.WindowsAuth.Entities;
using CorpProp.WindowsAuth.Extentions;
using CorpProp.WindowsAuth.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.WindowsAuth.Services
{
    public interface IADUserService : IBaseObjectService<ADUser>
    {
        Task<ADUser> GetByLoginAsync(IUnitOfWork unitOfWork, string adLogin);
        User BuildUserFromADUser(ADUser adUser);
        Task<User> CreateUserFromADUserAsync(IUnitOfWork unitOfWork, string adLogin);
    }

    public class ADUserService : BaseObjectService<ADUser>, IADUserService
    {
        private readonly ILogService _logger;
        public ADUserService(IBaseObjectServiceFacade facade, ILogService logger) : base(facade)
        {
            _logger = logger;
        }

        public override IQueryable<ADUser> GetAll(IUnitOfWork unit_of_work, bool? hidden)
        {
            var users = new List<ADUser>();
            var ctx = new PrincipalContext(ContextType.Domain);
            try
            {
                foreach (var groupName in ADSettingsHelper.GetAllADUsersGroups())
                {
                    using (var group = GroupPrincipal.FindByIdentity(ctx, groupName))
                    {
                        users.AddRange(group.GetMembers()
                            .Where(x => x is UserPrincipal)
                            .Select(x => MapUserPrincipleToAdUser(x as UserPrincipal))
                            .ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
            finally
            {
                ctx.Dispose();
            }
            return users.AsQueryable();
        }

        public Task<ADUser> GetByLoginAsync(IUnitOfWork unitOfWork, string samAccountName)
        {
            if (string.IsNullOrEmpty(samAccountName))
            {
                throw new ArgumentException("SamAccountName is null or empty.");
            }
            return Task.Run(() =>
            {
                var ctx = new PrincipalContext(ContextType.Domain);
                ADUser adUser = null;
                try
                {
                    var userPrinciple = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, samAccountName);
                    if (userPrinciple != null)
                    {
                        adUser = MapUserPrincipleToAdUser(userPrinciple);
                    }
                    _logger.Log($"User with SamAccountName:{samAccountName} does't exist in domain service.");
                }
                catch (Exception ex)
                {
                    _logger.Log(ex);
                }
                finally
                {
                    ctx.Dispose();
                }
                return adUser;
            });
        }

        public async Task<User> CreateUserFromADUserAsync(IUnitOfWork unitOfWork, string adLogin)
        {
            var adUser = await GetByLoginAsync(unitOfWork, adLogin);
            if (adUser != null)
            {
                var user = BuildUserFromADUser(adUser);
                return user;
            }
            return null;
        }

        public User BuildUserFromADUser(ADUser adUser)
        {
            if (adUser == null)
            {
                throw new NullReferenceException("ADUser is null.");
            }
            return new User
            {
                SysName = adUser.Login,
                Profile = new SibUser()
                {
                    LastName = adUser.LastName,
                    FirstName = adUser.FirstName,
                    MiddleName = adUser.MiddleName,
                    DeptName = adUser.Department,
                    PostName = adUser.JobTitle,
                    Email = adUser.Email,
                    Phone = adUser.Phone,
                    Mobile = adUser.Mobile,
                    Description = adUser.Description
                }
            };
        }

        private static ADUser MapUserPrincipleToAdUser(UserPrincipal userPrincipal)
        {
            var firstName = userPrincipal.GivenName;
            var middleName = userPrincipal.MiddleName;
            if (!string.IsNullOrEmpty(firstName))
            {
                var indexOfspace = firstName.IndexOf(" ");
                if (indexOfspace > 0)
                {
                    middleName = firstName.Substring(indexOfspace + 1);
                    firstName = firstName.Substring(0, indexOfspace);
                }
            }

            return new ADUser()
            {
                ID = userPrincipal.Sid.GetHashCode(),
                Login = userPrincipal.SamAccountName,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = userPrincipal.Surname,
                Email = userPrincipal.EmailAddress,
                Phone = userPrincipal.VoiceTelephoneNumber,
                Mobile = userPrincipal.GetMobilePhone(),
                Department = userPrincipal.GetDepartment(),
                JobTitle = userPrincipal.GetJobTitle(),
                Description = userPrincipal.Description
            };
        }

        #region Not Emplemented
        public override void ChangeSortOrder(IUnitOfWork unitOfWork, ADUser obj, int posId)
        {
            throw new NotImplementedException();
        }

        public override ADUser Create(IUnitOfWork unitOfWork, ADUser obj)
        {
            throw new NotImplementedException();
        }

        public override IReadOnlyCollection<ADUser> CreateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<ADUser> collection)
        {
            throw new NotImplementedException();
        }

        public override void Delete(IUnitOfWork unitOfWork, ADUser obj)
        {
            throw new NotImplementedException();
        }

        public override void DeleteCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<ADUser> collection)
        {
            throw new NotImplementedException();
        }

        public override ADUser Update(IUnitOfWork unitOfWork, ADUser obj)
        {
            throw new NotImplementedException();
        }

        public override IReadOnlyCollection<ADUser> UpdateCollection(IUnitOfWork unitOfWork, IReadOnlyCollection<ADUser> collection)
        {
            throw new NotImplementedException();
        }

        ADUser IBaseObjectService<ADUser>.CreateDefault(IUnitOfWork unitOfWork)
        {
            throw new NotImplementedException();
        }

        ADUser IBaseObjectService<ADUser>.Get(IUnitOfWork unitOfWork, int id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

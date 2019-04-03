using System;
using System.Threading.Tasks;
using Base.Access.Entities;
using Base.DAL;
using Base.Enums;
using Base.Security;
using Base.Service;
using AppContext = Base.Ambient.AppContext;

namespace Base.Access.Service
{
    public class AccessService: IAccessService, IAccessEntryService, IAccessEntryFactory
    {
        private readonly IAccessErrorDescriber _accessErrorDescriber;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public AccessService(IAccessErrorDescriber accessErrorDescriber, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _accessErrorDescriber = accessErrorDescriber;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        private static bool IsSystemUnitOfWork(IUnitOfWork unitOfWork)
        {
            return unitOfWork is ISystemUnitOfWork;
        }

        public void ThrowIfAccessDenied(IUnitOfWork unitOfWork, Type type, TypePermission typePermission)
        {
            if (IsSystemUnitOfWork(unitOfWork)) return;

            if (!AppContext.SecurityUser.IsPermission(type, typePermission))
                throw new AccessDeniedException(_accessErrorDescriber.GetAccessDenied(type));
        }

        public Task<AccessEntry> GetAccessEntry(Type type, int? objId)
        {
            using (var uofw = _unitOfWorkFactory.CreateSystem())
            {
                return null;
            }
        }

        public AccessEntry CreateAccessEntry(Type type)
        {
            var access = new AccessEntry()
            {
                   
            };

            return access;
        }
    }
}

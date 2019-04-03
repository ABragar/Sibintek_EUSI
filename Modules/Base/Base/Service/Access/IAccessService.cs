using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Enums;

namespace Base.Service
{
    public interface IAccessService
    {
        void ThrowIfAccessDenied(IUnitOfWork unitOfWork, Type type, TypePermission typePermission);
    }
}

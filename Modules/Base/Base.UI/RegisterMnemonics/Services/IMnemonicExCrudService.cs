using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Service;
using Base.UI.RegisterMnemonics.Entities;
using Base.UI.ViewModal;

namespace Base.UI.RegisterMnemonics.Services
{
    public interface IMnemonicExCrudService<T> : IBaseObjectService<T>, IMnemonicExQueryService where T : MnemonicEx
    {
    }

    public interface IMnemonicExQueryService
    {
        IQueryable<MnemonicEx> GetAllMnemonicEx(IUnitOfWork unitOfWork);
    }
}
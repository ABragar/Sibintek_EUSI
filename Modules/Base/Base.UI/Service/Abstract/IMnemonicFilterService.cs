using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Service;
using Base.UI.Filter;

namespace Base.UI.Service.Abstract
{
    public interface IMnemonicFilterService<T> : IBaseObjectService<T> where T : MnemonicFilter
    {
        Task<IQueryable<TObject>> AddMnemonicFilter<TObject>(IUnitOfWork uofw, IQueryable<TObject> q, string mnemonic,
            int mnemonicFilterId) where TObject : IBaseObject;

        Task<IQueryable> AddMnemonicFilter(IUnitOfWork uofw, IQueryable q, string mnemonic,
            int mnemonicFilterId);
    }
}

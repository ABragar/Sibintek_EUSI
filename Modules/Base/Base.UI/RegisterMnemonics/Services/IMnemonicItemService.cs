using Base.Service;
using Base.UI.RegisterMnemonics.Entities;

namespace Base.UI.RegisterMnemonics.Services
{
    public interface IMnemonicItemService<T>: IBaseObjectService<T> where T: MnemonicItem
    {

    }
}

using System;
using Base.Service;

namespace Base.UI.Service
{
    public interface IUiEnumService : IBaseObjectService<UiEnum>
    {
        void Sync();
        UiEnum GetEnum(Type type);
    }
}

using System.Threading.Tasks;
using Base.DAL;
using Base.Events;

namespace Base.UI.Service
{
    public interface IPresetService<T> :
        IEventBusHandler<IChangeObjectEvent<IUserCategory>>,
        IEventBusHandler<IChangeObjectEvent<PresetRegistor>>
        where T : Preset
    {
        Task<T> GetAsync(string ownerName);
        Task<T> GetUserCategoryPresetAsync(string ownerName);
        Task<T> SaveAsync(T preset);
        Task DeleteAsync(Preset preset);
        T GetDefaultPreset(string ownerName);
        void PresetClear();
        void DefaultPresetClear(string ownerName);

        //sib
        void Delete(PresetRegistor pr);
        //end sib
    }
}

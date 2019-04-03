using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Service;

namespace Base.UI.Service
{
    public class PresetRegistorService : BaseObjectService<PresetRegistor>, IPresetRegistorService
    {
        private readonly IServiceLocator _locator;

        public PresetRegistorService(IBaseObjectServiceFacade facade
            , IServiceLocator locator) : base(facade) 
        {
            _locator = locator;
        } 

        protected override IObjectSaver<PresetRegistor> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<PresetRegistor> objectSaver)
        {
            var preset = objectSaver.Src.Preset;
            preset.For = objectSaver.Src.For;
            objectSaver.Dest.Preset = preset;
            //objectSaver.Dest.Preset.For = objectSaver.Src.For;
            return base.GetForSave(unitOfWork, objectSaver);
        }

        public override void Delete(IUnitOfWork unitOfWork, PresetRegistor obj)
        {           
            if (obj.Preset != null && obj.UserID != null)
            {
                
                MethodInfo method = _locator.GetType().GetMethod("GetService", new Type[0]);
                Type genericType = typeof(IPresetService<>);
                Type constructed = genericType.MakeGenericType(obj.Preset.GetType());
                MethodInfo generic = method.MakeGenericMethod(constructed);
                var prService = generic.Invoke(_locator, null);
               
                if (prService != null)
                {
                    object[] paramss = new object[] { obj };
                    MethodInfo methodDel = prService.GetType().GetMethod("Delete");
                    methodDel.Invoke(prService, paramss);
                }                             
            }
            else
                base.Delete(unitOfWork, obj);
        }
    }
}

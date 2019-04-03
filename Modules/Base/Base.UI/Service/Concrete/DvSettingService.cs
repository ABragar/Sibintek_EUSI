using System;
using System.Collections.Generic;
using System.Linq;
using Base.DAL;
using Base.Service;
using Base.UI.DetailViewSetting;
using Base.UI.ViewModal;

namespace Base.UI.Service
{
    public class DvSettingService<T> : BaseCategoryService<T>, IDvSettingService<T> where T : DvSetting, new()
    {
        private readonly IViewModelConfigService _configService;

        public DvSettingService(IBaseObjectServiceFacade facade, IViewModelConfigService configService, IAutoMapperCloner cloner) : base(facade)
        {
            _configService = configService;
        }

        public override void ChangePosition(IUnitOfWork unitOfWork, T obj, int? posChangeID, string typePosChange)
        {
            throw new Exception("Нельзя изменить структуру");
        }

        protected virtual bool CheckTypes(IUnitOfWork uow, T obj)
        {
            var dvsm = obj as DvSettingForMnemonic;
            if (dvsm != null)
            {
                string objecttype = _configService.Get(dvsm.Mnemonic)?.Entity;

                if (objecttype == null)
                    throw new Exception("Объект не прошел валидацию");

                var type = Type.GetType(objecttype);

                if (type == null)
                    throw new Exception("Объект не прошел валидацию");

                var props = type.GetProperties().Select(x => x.Name);

                if (!dvsm.Fields.All(x => props.Contains(x.FieldName)))
                {
                    throw new Exception("Объект не прошел валидацию");
                }
            }

            return true;
        }

        protected override IObjectSaver<T> GetForSave(IUnitOfWork unitOfWork, IObjectSaver<T> objectSaver)
        {
            var obj = objectSaver.Src;

            if (CheckTypes(unitOfWork, obj))
            {

                return base.GetForSave(unitOfWork, objectSaver)
                    .SaveOneToMany(x => x.Fields,
                        x => x
                            //.SaveOneToMany(o => o.EnableRoles, e => e.SaveOneObject(r => r.Object))
                            //.SaveOneToMany(o => o.HiddenRoles, e => e.SaveOneObject(r => r.Object))
                            //.SaveOneToMany(o => o.ReadOnlyRoles, e => e.SaveOneObject(r => r.Object))
                            //.SaveOneToMany(o => o.VisibleRoles, e => e.SaveOneObject(r => r.Object))

                            .SaveOneToMany(o => o.BEnableRules, e => e.SaveOneObject(r => r.Object))
                            .SaveOneToMany(o => o.BHiddenRules, e => e.SaveOneObject(r => r.Object))
                            .SaveOneToMany(o => o.BVisibleRules, e => e.SaveOneObject(r => r.Object))
                            .SaveOneToMany(o => o.BReadOnlyRules, e => e.SaveOneObject(r => r.Object))
                            .SaveOneToMany(o => o.ReadOnlyRules)
                            .SaveOneToMany(o => o.EnableRules)
                            .SaveOneToMany(o => o.VisibleRules)
                            .SaveOneToMany(o => o.HiddenRules));
            }
            else
            {
                throw new Exception("Настройка не прошла валидацию");
            }
        }

        public virtual IQueryable<T> GetDvSettings(IUnitOfWork uow, string objectType)
        {
            throw new NotImplementedException();
        }

        public virtual ICollection<DvSettingValidationResult> ValidateSetting(DvSettingForType setting)
        {
            throw new NotImplementedException();
        }
    }

    public class DvSettingValidationResult
    {
        public string Field { get; set; }
        public string Description { get; set; }
        public ErrorLevel ErrorLevel { get; set; }
    }

    public enum ErrorLevel
    {
        Error = 0,
        Warning = 1,
    }
}

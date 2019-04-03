using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;
using Base.Service;
using Base.UI.DetailViewSetting;

namespace Base.UI.Service
{
    public class DvSettingsForTypeService : DvSettingService<DvSettingForType>
    {
        public DvSettingsForTypeService(IBaseObjectServiceFacade facade, IViewModelConfigService configService, IAutoMapperCloner cloner) : base(facade, configService, cloner)
        {
        }

        protected override bool CheckTypes(IUnitOfWork uow, DvSettingForType obj)
        {
            var type = Type.GetType(obj.ObjectType);

            if (type == null)
                throw new Exception("Объект не прошел валидацию");

            var props = type.GetProperties().Select(x => x.Name);

            if (!obj.Fields.All(x => props.Contains(x.FieldName)))
            {
                throw new Exception("Объект не прошел валидацию");
            }

            if (obj.ParentID != null)
            {
                var parent = Get(uow, obj.ParentID.Value);
                var parentType = Type.GetType(parent.ObjectType);
                if (parentType == null)
                    throw new Exception("Объект не прошел валидацию");

                if (!parentType.IsAssignableFrom(type))
                {
                    throw new Exception("Объект не является наследником");
                }

            }

            return true;
        }

        public override IQueryable<DvSettingForType> GetDvSettings(IUnitOfWork uow, string objectType)
        {
            var dvSettings = GetAll(uow);
            if (!string.IsNullOrEmpty(objectType))
            {
                dvSettings = dvSettings.Where(x => x.ObjectType == objectType);

                var parents = dvSettings
                    .Where(x => x.sys_all_parents != null && !x.Hidden)
                    .Select(x => x.sys_all_parents)
                    .ToList()
                    .Select(x => x.Split(HCategory.Seperator).Select(HCategory.IdToInt).ToList())
                    .SelectMany(x => x).Distinct();

                var p = GetAll(uow).Where(x => parents.Contains(x.ID));
                dvSettings = dvSettings.Union(p);
            }
            return dvSettings;
        }

        public override ICollection<DvSettingValidationResult> ValidateSetting(DvSettingForType setting)
        {
            var result = new List<DvSettingValidationResult>();

            var type = Type.GetType(setting.ObjectType);

            if (type == null)
            {
                result.Add(new DvSettingValidationResult { Description = "Тип не найден", Field = "Объект" });

                return result;
            }

            var props = type.GetProperties().Select(x => x.Name).ToList();

            result.AddRange(from prop in setting.Fields.Select(z => z.FieldName)
                            where !props.Contains(prop)
                            select new DvSettingValidationResult
                            {
                                Description = "Свойство отсутствует в типе",
                                Field = prop
                            });

            result.AddRange(from requiredField in setting.Fields.Where(x => x.Required)
                            where !requiredField.Enable || !requiredField.Visible
                            select new DvSettingValidationResult
                            {
                                Description = "Обязательно но не доступно/видимо",
                                Field = requiredField.FieldName,
                                ErrorLevel = ErrorLevel.Warning
                            });

            result.AddRange(from duplicate in setting.Fields.GroupBy(x => x.FieldName)
                            where duplicate.Count() > 1
                            select new DvSettingValidationResult()
                            {
                                Field = duplicate.Key,
                                Description = "Настройки заданы несолько раз",
                                ErrorLevel = ErrorLevel.Warning
                            });

            


            return result;
        }
    }
}

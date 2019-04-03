using Base.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.Settings;

namespace Base.Audit.Entities
{
    [Serializable]
    public class AuditSetting: SettingItem
    {
        [DetailView("Максимальное кол-во записей в журнале")]
        public int MaxRecordCount { get; set; }

        [DetailView("Регистрировать вход/выход в систему")]
        public bool RegisterLogIn { get; set; }

        [DetailView(TabName = "Сущности")]
        [PropertyDataType("AuditListEntities")]
        public virtual ICollection<AuditSettingEntity> Entities { get; set; }

        private Dictionary<string, AuditSettingEntity> _entities; 

        public bool IsAudit(Type type)
        {
            if(type == null)
                throw new ArgumentNullException(nameof(type));

            if (Entities != null && _entities == null)
                _entities = Entities.ToDictionary(x => x.FullName, x => x);

            return _entities?.ContainsKey(type.GetTypeName()) ?? false;
        }
    }
    
    [Serializable]
    public class AuditSettingEntity: BaseObject
    {
        [DetailView(Name = "Объект"), ListView]
        public string FullName { get; set; }
    }
}

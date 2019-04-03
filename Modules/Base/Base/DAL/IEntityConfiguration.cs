using System;
using System.Collections.Generic;

namespace Base.DAL
{
    public interface IEntityConfiguration
    {
        EntityConfigurationItem<T> Get<T>() where T : BaseObject;
        IEnumerable<EntityConfigurationItem> GetContextConfig(Type contextType);
        IEnumerable<EntityConfigurationItem> GetContextConfig(IBaseContext context);
        IEnumerable<EntityConfigurationItem> GetParents<T>() where T : BaseObject;
    }
}

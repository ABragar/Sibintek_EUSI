using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;

namespace Base.SysRegistry
{
    public class SystemRegistryService: ISystemRegistryService
    {
        public string Get(IUnitOfWork uofw, string key)
        {
            return uofw.GetRepository<SystemRegistryItem>().All().Where(x => x.Key == key).Select(x => x.Value).SingleOrDefault();
        }

        public void AddOrUpdate(IUnitOfWork uofw, string key, Func<string> addValueFactory, Func<string, string> updateValueFactory)
        {
            var rep = uofw.GetRepository<SystemRegistryItem>();

            var val = rep.All().SingleOrDefault(x => x.Key == key);

            if (val == null)
            {
                rep.Create(new SystemRegistryItem()
                {
                    Key = key,
                    Value = addValueFactory()
                });
            }
            else
            {
                val.Value = updateValueFactory(val.Value);
                rep.Update(val);
            }

            uofw.SaveChanges();
        }
    }
}

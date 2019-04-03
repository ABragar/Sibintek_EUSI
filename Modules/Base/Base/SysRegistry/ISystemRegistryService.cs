using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.DAL;

namespace Base.SysRegistry
{
    public interface ISystemRegistryService
    {
        string Get(IUnitOfWork uofw, string key);
        void AddOrUpdate(IUnitOfWork uofw, string key, Func<string> addValueFactory, Func<string, string> updateValueFactory);
    }
}

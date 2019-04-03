using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Access.Entities;

namespace Base.Access.Service
{
    public interface IAccessEntryService
    {
        Task<AccessEntry> GetAccessEntry(Type type, int? objId);
    }

    public interface IAccessEntryFactory
    {
        AccessEntry CreateAccessEntry(Type type);
    }
}

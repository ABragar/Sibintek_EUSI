using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using Base.ComplexKeyObjects;
using Base.Service;

namespace Base.DAL.EF
{
    public class EFContextEntityTypeResolver<TContext>: IEntityTypeResolver 
        where  TContext: DbContext
    {
        private readonly IServiceFactory<DbContext> _factory;

        public EFContextEntityTypeResolver(IServiceFactory<TContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Type> GetEntityTypes()
        {
            using (var context = _factory.GetService())
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                IObjectContextAdapter a = context;
                var t = a.ObjectContext.MetadataWorkspace.GetItems<EntityType>(DataSpace.OSpace)
                    .Select(c => c.FullName);

                

                var tt = t.SelectMany(x=>assemblies.Select(ass=>ass.GetType(x))).Where(x=>x!=null);

                return tt;
            }
        }

    }
}
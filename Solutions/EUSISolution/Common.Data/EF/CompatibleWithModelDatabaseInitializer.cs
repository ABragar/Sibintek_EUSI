using System;
using System.Data.Entity;

namespace Common.Data.EF
{
    public class CompatibleWithModelDatabaseInitializer<TContext> : IDatabaseInitializer<TContext>
        where TContext : DbContext
    {
        public void InitializeDatabase(TContext context)
        {
            if (!context.Database.CompatibleWithModel(true))
                throw new InvalidOperationException();
        }
    }
}
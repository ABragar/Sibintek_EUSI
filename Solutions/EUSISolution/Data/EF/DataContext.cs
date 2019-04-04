using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using Base.DAL;
using Base.DAL.EF;

namespace Data.EF
{
    public sealed class ConfigurationCaching : DbConfiguration
    {
        public ConfigurationCaching() : base()
        {
            var path = Path.GetDirectoryName(this.GetType().Assembly.Location);
            SetModelStore(new DefaultDbModelStore(path));
        }
    }

    [DbConfigurationType(typeof(ConfigurationCaching))]
    public class DataContext : EFContext
    {
        public DataContext(IEntityConfiguration entityConfiguration)
            : base(entityConfiguration)
        {
            this.Database.CommandTimeout = 180;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Base.DAL;
using Base.DAL.EF;

namespace Data.EF
{
    public class DataContext : EFContext
    {
        public DataContext(IEntityConfiguration entityConfiguration)
            : base(entityConfiguration)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
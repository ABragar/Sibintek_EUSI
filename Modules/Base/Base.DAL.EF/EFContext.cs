using Base.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Base.Attributes;
using IndexAttribute = Base.Attributes.IndexAttribute;

namespace Base.DAL.EF
{
    public abstract class EFContext : DbContext, IBaseContext
    {
        protected readonly IEntityConfiguration _entityConfiguration;

        private DbContextTransaction _transaction = null;
        public bool WriteLog { get; set; } = false;

        protected EFContext(IEntityConfiguration entityConfiguration)
        {
            _entityConfiguration = entityConfiguration;
#if(DEBUG)
            if (WriteLog) this.Database.Log = s => Log(s);
#endif
        }

        int IBaseContext.SaveChanges()
        {

            try
            {
                return this.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new UpdateConcurrencyException("Ошибка обновления записи, были внеcены изменения до вас, обновите модель");
            }
            catch (DbEntityValidationException e)
            {
                string error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    error = String.Format("У сущности \"{0}\" в состоянии \"{1}\" найдены следующие ошибки:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        error += String.Format("- Свойство: \"{0}\", Ошибка: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw new DbEntityValidationException(error);
            }

        }

        async Task<int> IBaseContext.SaveChangesAsync()
        {
            try
            {
                return await this.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new UpdateConcurrencyException("Ошибка обновления записи, были внеcены изменения до вас, обновите модель");
            }

        }

        public bool Transaction
        {
            get { return _transaction != null; }
        }

        public IDisposable BeginTransaction()
        {
            return (_transaction =
                this.Database.BeginTransaction());
        }

        public void Commit()
        {
            if (!Transaction) return;

            _transaction.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            if (!Transaction) return;

            _transaction.Rollback();
            _transaction = null;
        }

        void IBaseContext.ChangeState<TEntity>(TEntity entity, BaseEntityState entityState)
        {
            switch (entityState)
            {
                case BaseEntityState.Added:
                    this.Entry<TEntity>(entity).State = EntityState.Added;
                    break;
                case BaseEntityState.Deleted:
                    this.Entry<TEntity>(entity).State = EntityState.Deleted;
                    break;
                case BaseEntityState.Detached:
                    this.Entry<TEntity>(entity).State = EntityState.Detached;
                    break;
                case BaseEntityState.Modified:
                    this.Entry<TEntity>(entity).State = EntityState.Modified;
                    break;
                case BaseEntityState.Unchanged:
                    this.Entry<TEntity>(entity).State = EntityState.Unchanged;
                    break;
                default: break;
            }
        }

        public Dictionary<TEntity, BaseEntityState> GetModifiedEntities<TEntity>(bool recursive = true) where TEntity : class
        {
            IEnumerable<DbEntityEntry> q = this.ChangeTracker.Entries();


            //TODO WTF???
            if (recursive)
            {
                q = q.Where(e => typeof(TEntity).IsAssignableFrom(e.Entity.GetType()));
            }
            else
            {
                q = q.Where(e => e.Entity is TEntity);
            }

            return q.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToDictionary(e => (TEntity)e.Entity, e =>
                {
                    switch (e.State)
                    {
                        case EntityState.Added: return BaseEntityState.Added;
                        case EntityState.Deleted: return BaseEntityState.Deleted;
                        default: return BaseEntityState.Modified;
                    }
                });
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var config in _entityConfiguration.GetContextConfig(this))
            {
                var m = this.GetType().GetMethod(nameof(ConfigureEntity), BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(new Type[] { config.EntityType });

                m.Invoke(this, new object[] { modelBuilder, config });
            }


            modelBuilder.Properties().Configure(x =>
            {

            });

            modelBuilder.Conventions.Remove<IndexAttributeConvention>();
            modelBuilder.Conventions.Add(new NonPublicColumnAttributeConvention());
            modelBuilder.Conventions.Add(new CustomIndexAttributeConvention());
            modelBuilder.Conventions.Add(new DecimalPrecisionAttributeConvention());

        }


        protected void ConfigureEntity<TEntity>(DbModelBuilder modelBuilder, EntityConfigurationItem config) where TEntity : class
        {
            var entityConfig = modelBuilder.Entity<TEntity>();

            string tableName = config.Name ?? config.EntityType.Name;

            string scheme = config.EntityType.Namespace.Replace("Base.", "").Replace(".Entities", "");

            entityConfig.ToTable(tableName, scheme);
        }


        public class CustomIndexAttributeConvention : Convention
        {
            /// <summary>
            /// Constructs a new instance of the convention.
            /// 
            /// </summary>
            public CustomIndexAttributeConvention()
            {


                AddAnnotation<IndexAttribute>(Convert);

                AddAnnotation<System.ComponentModel.DataAnnotations.Schema.IndexAttribute>(x=>x);

            }

            private void AddAnnotation<TAttribute>(
                Func<TAttribute, System.ComponentModel.DataAnnotations.Schema.IndexAttribute> convert_func)
                where TAttribute: Attribute
            {
                Properties().Having(x => x.GetCustomAttributes<TAttribute>().ToArray()).Configure(
                    (x, a) =>
                    {
                        if (a.Any())
                        {
                            x.HasColumnAnnotation("Index", new IndexAnnotation(a.Select(convert_func)));
                        }

                    });

            }

            private static System.ComponentModel.DataAnnotations.Schema.IndexAttribute Convert(IndexAttribute a)
            {
                return new System.ComponentModel.DataAnnotations.Schema.IndexAttribute(a.Name, a.Order) { IsUnique = a is UniqueIndexAttribute };
            }
        }

        public sealed class NonPublicColumnAttributeConvention : Convention
        {
            public NonPublicColumnAttributeConvention()
            {
                Types().Having(NonPublicProperties)
                       .Configure((config, properties) =>
                       {
                           foreach (var prop in properties)
                           {
                               config.Property(prop);
                           }
                       });
            }

            private IEnumerable<PropertyInfo> NonPublicProperties(Type type)
            {
                var matchingProperties = type.GetProperties(BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance)
                                             .Where(propInfo => propInfo.GetCustomAttributes(typeof(ColumnAttribute), true).Length > 0)
                                             .ToArray();
                return matchingProperties.Length == 0 ? null : matchingProperties;
            }
        }


        public bool IsModifiedEntity<TEntity>(TEntity entity, BaseEntityState modif) where TEntity : BaseObject
        {
            switch (modif)
            {
                case BaseEntityState.Added:
                    return this.ChangeTracker.Entries<TEntity>().Any(x => x.State == EntityState.Added && (x.Entity == entity || (entity.ID != 0 && x.Entity.ID == entity.ID)));
                case BaseEntityState.Modified:
                    return this.ChangeTracker.Entries<TEntity>().Any(x => x.State == EntityState.Modified && (x.Entity == entity || (entity.ID != 0 && x.Entity.ID == entity.ID)));
                case BaseEntityState.Deleted:
                    return this.ChangeTracker.Entries<TEntity>().Any(x => x.State == EntityState.Deleted && (x.Entity == entity || (entity.ID != 0 && x.Entity.ID == entity.ID)));
            }
            return false;
        }

        private void Log(string msg)
        {
            using (var stream = new StreamWriter("D:\\log.txt", true))
            {
                stream.WriteLine(msg);
            }
        }

        // sib
        // некое подобие SavingChanges
        // без вызова события и подписывания/отписывания

        /// <summary>
        /// Возвращает записи, отслеживаемые в конексте со статусом "Добавлено" или "Изменено".
        /// </summary>
        /// <returns>Список записей БД.</returns>
        public List<DbEntityEntry> GetModifiedEntries()
        {
            List<DbEntityEntry> modifiedChanges = new List<DbEntityEntry>();
            if (ChangeTracker != null && ChangeTracker.Entries() != null)
                modifiedChanges = this.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added).ToList();
            return modifiedChanges;
        }

        /// <summary>
        /// Метод, вызываемый перед вызовом сохранения изменений контекста.
        /// </summary>
        /// <remarks>
        /// Вызывается при сохранении сессии UnitOfWork.SaveChanges()
        /// </remarks>
        public void OnSavingChanges(IUnitOfWork uow)
        {
            List<DbEntityEntry> entries = GetModifiedEntries();
            foreach (var entry in entries)
            {
               
                var bo = entry.Entity as IBaseObject;
                if (bo == null) continue;

                //от каждой записи вызываем OnSaving... если есть
                MethodInfo method = entry.Entity.GetType().GetMethod("OnSaving");
                if (method == null) continue; 
                object[] paramss = new object[] { uow, entry  };
                method.Invoke(entry.Entity, paramss);
            }
        }

        /// <summary>
        /// Перечитывание записи из БД.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">Сущность, которая будет перечитана из БД.</param>
        /// <param name="collection">Св-во типа коллекция, которое будет также перечитано и загружено из БД.</param>
        public void ReloadEntity<TEntity>(TEntity entity, string collection = null) where TEntity : class
        {
            var obj = this.Entry(entity);
            obj.Reload();
            if (!String.IsNullOrWhiteSpace(collection))
                obj.Collection(collection).Load();
        }

        //end sib

    }
}

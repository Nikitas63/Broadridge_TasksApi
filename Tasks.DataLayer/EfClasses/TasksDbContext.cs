using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tasks.DataLayer.EfClasses.EntityConfigurations;
using Tasks.DataLayer.Interfaces;
using Tasks.DataLayer.Models;

namespace Tasks.DataLayer.EfClasses
{
    /// <summary>
    /// Represents tasks database context.
    /// </summary>
    public class TasksDbContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "dbo";

        private const string _updatedDateProperty = "UpdatedDate";
        protected const string _isDeletedProperty = "IsDeleted";

        public TasksDbContext(DbContextOptions options)
            : base(options)
        {
        }

        #region DbSets

        public DbSet<TaskModel> Tasks { get; set; }

        #endregion

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigureCommonEntityProperties(builder);
            ApplyConfiguration(builder);
        }

        private static void ConfigureSoftDelete<T>(ModelBuilder builder)
            where T : class, ISoftDeletable
        {
                builder.Entity<T>()
                    .Property<bool>(_isDeletedProperty)
                    .IsRequired()
                    .HasDefaultValue(false);

                builder.Entity<T>()
                    .HasQueryFilter(t => !EF.Property<bool>(t, _isDeletedProperty));
        }

        private static void ConfigureTimeTracking<T>(ModelBuilder builder)
            where T : class, ITimeTrackable
        {
            builder.Entity<T>()
                .Property(t => t.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("sysdatetimeoffset()")
                .HasColumnType("datetimeoffset(7)");

            builder.Entity<T>()
                .Property(t => t.UpdatedDate)
                .IsRequired()
                .HasDefaultValueSql("sysdatetimeoffset()")
                .HasColumnType("datetimeoffset(7)")
                .IsConcurrencyToken();
        }

        private void ApplyConfiguration(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TaskModelEntityTypeConfiguration());
        }

        private void ConfigureCommonEntityProperties(ModelBuilder builder)
        {
            var configureSoftDeleteMethodInfo = typeof(TasksDbContext)
                .GetMethod(
                    nameof(ConfigureSoftDelete),
                    BindingFlags.NonPublic | BindingFlags.Static);
            var configureTimeTrackingMethodInfo = typeof(TasksDbContext)
                .GetMethod(
                    nameof(ConfigureTimeTracking),
                    BindingFlags.NonPublic | BindingFlags.Static);

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entity.ClrType))
                {
                    configureSoftDeleteMethodInfo.MakeGenericMethod(entity.ClrType)
                        .Invoke(null, new object[] { builder });
                }

                if (typeof(ITimeTrackable).IsAssignableFrom(entity.ClrType))
                {
                    configureTimeTrackingMethodInfo.MakeGenericMethod(entity.ClrType)
                        .Invoke(null, new object[] { builder });
                }
            }
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in this.ChangeTracker.Entries<ISoftDeletable>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues[_isDeletedProperty] = false;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues[_isDeletedProperty] = true;
                        break;
                }
            }

            var currentDateTime = DateTimeOffset.Now;
            foreach (var entry in this.ChangeTracker.Entries<ITimeTrackable>())
            {
                if (entry.State == EntityState.Deleted || entry.State == EntityState.Modified)
                {
                    entry.CurrentValues[_updatedDateProperty] = currentDateTime;
                }
            }
        }
    }
}

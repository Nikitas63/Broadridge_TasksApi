using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tasks.DataLayer.Models;
using Tasks.DataLayer.Models.Enums;

namespace Tasks.DataLayer.EfClasses.EntityConfigurations
{
    public class TaskModelEntityTypeConfiguration : IEntityTypeConfiguration<TaskModel>
    {
        public void Configure(EntityTypeBuilder<TaskModel> builder)
        {
            builder.ToTable(
                "TaskModel",
                TasksDbContext.DEFAULT_SCHEMA);

            builder.Property(v => v.Id)
                .HasDefaultValueSql("newsequentialid()");

            builder.HasKey(v => v.Id)
                .ForSqlServerIsClustered();

            builder.Property(q => q.Name)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(q => q.Description)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(q => q.Priority)
                .IsRequired();

            builder.Property(q => q.TimeToComplete)
                .IsRequired();

            builder.Property(f => f.Status)
                .IsRequired()
                .HasConversion(new EnumToNumberConverter<TaskModelStatus, int>());
        }
    }
}

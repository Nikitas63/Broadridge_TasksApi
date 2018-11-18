using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tasks.DataLayer.EfClasses
{
    /// <summary>
    /// Tasks context factory (for migration purposes).
    /// </summary>
    public class TasksDbContextDesignFactory : IDesignTimeDbContextFactory<TasksDbContext>
    {
        public TasksDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TasksDbContext>()
                .UseSqlServer("Server=.\\SQLEXPRESS;Initial Catalog=TasksDb;Integrated Security=true");

            return new TasksDbContext(optionsBuilder.Options);
        }
    }
}

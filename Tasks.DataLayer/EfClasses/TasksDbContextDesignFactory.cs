using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tasks.DataLayer.EfClasses
{
    public class EFormsContextDesignFactory : IDesignTimeDbContextFactory<TasksDbContext>
    {
        public TasksDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TasksDbContext>()
                .UseSqlServer("Server=.\\SQLEXPRESS;Initial Catalog=TasksDb;Integrated Security=true");

            return new TasksDbContext(optionsBuilder.Options);
        }
    }
}

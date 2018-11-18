using System;
using System.IO;
using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tasks.DataLayer.EfClasses;
using TasksApi.Filters;

namespace TasksApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, ILogger<Startup> logger)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var appVersion = typeof(Program).Assembly.GetName().Version;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR();

            services.AddMvc(options =>
            {
                options.Filters.Add<HttpGlobalExceptionFilter>();
                options.Filters.Add<ValidateModelFilter>();
            })
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<AssemblyMarker>();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<TasksDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TasksDb"));
            });

            var context = services.BuildServiceProvider()
                .GetService<TasksDbContext>();
            context.Database.Migrate();

            services.AddSwaggerGen(c =>
            {
                var swaggerInfo = new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "0.1.0",
                    Title = "Tasks API",
                    Description = "Tasks API web methods."
                };

                c.SwaggerDoc("v1", swaggerInfo);

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            app.UseMvc();
        }
    }
}

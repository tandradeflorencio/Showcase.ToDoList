using Serilog;
using Showcase.ToDoList.Application.Services;
using Showcase.ToDoList.Application.Services.Interfaces;
using Showcase.ToDoList.Infrastructure.Repositories;
using Showcase.ToDoList.Infrastructure.Repositories.Interfaces;

namespace Showcase.ToDoList.Configurations
{
    internal static class DependencyInjectionConfiguration
    {
        public static IServiceCollection ConfigureDependencies(this IServiceCollection services)
        {
            services.AddScoped<ITodoRepository, TodoRepository>();

            services.AddScoped<IToDoService, ToDoService>();

            services.AddScoped<IUnitOfWork>(factory => factory.GetRequiredService<ApplicationDbContext>());

            return services;
        }

        public static void ConfigureLogs(this IServiceCollection services, ConfigurationManager configuration)
        {
            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .Enrich.WithProperty("Showcase.ToDoList", "Showcase.ToDoList")
                            .CreateLogger();

            services.AddSingleton(Log.Logger);
        }
    }
}
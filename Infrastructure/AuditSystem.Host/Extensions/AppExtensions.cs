using AuditSystem.DataAccess;

namespace AuditSystem.Host.Extensions
{
    public static class AppExtensions
    {
        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;
            var isProduction = app.ApplicationServices.GetRequiredService<IHostEnvironment>().IsProduction();

            DatabaseInitializer.InitializeDatabase(serviceProvider, isProduction);
        }
    }
}
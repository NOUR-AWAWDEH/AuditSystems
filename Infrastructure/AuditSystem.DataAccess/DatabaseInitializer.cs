using AuditSystem.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuditSystem.DataAccess;

public class DatabaseInitializer
{
    public static void InitializeDatabase(IServiceProvider serviceProvider, bool isProduction)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (!isProduction)
            {
                context.Database.Migrate(); // Auto-migrate in non-production
            }
        }
    }
}
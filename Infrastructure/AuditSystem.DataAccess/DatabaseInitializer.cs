using AuditSystem.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuditSystem.DataAccess;

public class DatabaseInitializer
{
    public static void InitializeDatabase(IServiceProvider serviceProvider)
    {
        var context  = serviceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }    
}

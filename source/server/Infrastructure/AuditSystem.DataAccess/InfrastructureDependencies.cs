using AuditSystem.Contract.Interfaces.Contexts;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.DataAccess.Constants;
using AuditSystem.DataAccess.Contexts;
using AuditSystem.DataAccess.Repositories;
using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Auth;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Processes;
using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.Domain.Entities.SupportingDocs;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuditSystem.DataAccess;

public static class InfrastructureDependencies
{
    public static IServiceCollection AddDataAccess(
        this IServiceCollection services,
        IConfiguration configuration,
        IHealthChecksBuilder healthChecksBuilder,
        bool isProduction)
    {
        // Register DbContext, repositories, and identity
        services
            .AddDbContext(configuration, isProduction)
            .AddRepositories()
            .AddIdentity();

        // Add health check
        healthChecksBuilder.AddDbContextHealthCheck();

        // Register the generic context
        services.AddScoped<IContext, EntityContext>();

        return services;
    }


    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration, bool isProduction)
    {
        // Using Sql Server

        //var connectionString = configuration[ConfigurationKeys.DefaultConnectionString] ?? throw new ArgumentNullException("DB_CONNECTION_STRING", "Database connection string is missing in configuration");
        //return services.AddDbContext<ApplicationDbContext>(options =>
        //    options.UseSqlServer(connectionString)
        //           .EnableSensitiveDataLogging(false)
        //           .LogTo(Console.WriteLine, LogLevel.Information)
        //           .ConfigureWarnings(warnings => warnings.Ignore(
        //                Microsoft.EntityFrameworkCore.Diagnostics.SqlServerEventId.SavepointsDisabledBecauseOfMARS)));

        // Using PostgreSQL

        var connectionString = configuration[ConfigurationKeys.DefaultConnectionString]
                ?? throw new ArgumentNullException("DB_CONNECTION_STRING", "Database connection string is missing in configuration");

        return services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString, 
            npgsqlOptions => npgsqlOptions.MigrationsAssembly("AuditSystem.DataAccess"))
                .EnableSensitiveDataLogging(false)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .ConfigureWarnings(warnings => warnings.Ignore(
                    Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.BoolWithDefaultWarning))
        );
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
    services
        //Audits
        .AddRepository<Guid, AuditProgram>()
        .AddRepository<Guid, Objective>()

        //Common
        .AddRepository<Guid, Rate>()

        //Organization
        .AddRepository<Guid, SubLocation>()
        .AddRepository<Guid, SubDepartment>()
        .AddRepository<Guid, Location>()
        .AddRepository<Guid, Department>()
        .AddRepository<Guid, Company>()
        //Processes
        .AddRepository<Guid, SubProcess>()
        .AddRepository<Guid, AuditProcess>()

        //RiskControls
        .AddRepository<Guid, RiskControlMatrix>()

        //Risks
        .AddRepository<Guid, Risk>()
        .AddRepository<Guid, ExitControl>()

        //SupportingDocs
        .AddRepository<Guid, SupportingDoc>()
        //Auth
        .AddRepository<Guid, RefreshToken>();

    private static IServiceCollection AddRepository<TId, TEntity>(this IServiceCollection services)
        where TId : IComparable<TId>
        where TEntity : Entity<TId>
    {
        services.AddScoped<IRepository<TId, TEntity>, EntityRepository<TId, TEntity>>();
        return services;
    }

    private static IHealthChecksBuilder AddDbContextHealthCheck(this IHealthChecksBuilder healthChecksBuilder) =>
        healthChecksBuilder.AddDbContextCheck<ApplicationDbContext>(
            name: "ApplicationDbContext-HealthCheck",
            tags: new[] { "db", "sqlserver" });
}
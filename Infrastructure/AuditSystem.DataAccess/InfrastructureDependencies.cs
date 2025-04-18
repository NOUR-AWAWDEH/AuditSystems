using AuditSystem.Contract.Interfaces.Contexts;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.DataAccess.Constants;
using AuditSystem.DataAccess.Contexts;
using AuditSystem.DataAccess.Repositories;
using AuditSystem.Domain.Entities;
using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Auth;
using AuditSystem.Domain.Entities.Checklists;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Compliance;
using AuditSystem.Domain.Entities.Jobs;
using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Processes;
using AuditSystem.Domain.Entities.Reports;
using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.Domain.Entities.Skills;
using AuditSystem.Domain.Entities.SupportingDocs;
using AuditSystem.Domain.Entities.Tasks;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuditSystem.DataAccess;

public static class InfrastructureDependencies
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services,
        IConfiguration configuration,
        IHealthChecksBuilder healthChecksBuilder, bool isProduction)
    {
        services
            .AddDbContext(configuration, isProduction)
            .AddRepositories()
            .AddIdentity();

        healthChecksBuilder.AddDbContextHealthCheck();
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

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration, bool isProduction) =>
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(ConfigurationKeys.DefaultConnectionString))
                   .EnableSensitiveDataLogging(!isProduction)  // Disable sensitive data logging in production
                   .LogTo(Console.WriteLine, LogLevel.Information));  // Optional query logging for debugging

    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
    services
        //Audits
        .AddRepository<Guid, AuditDomain>()
        .AddRepository<Guid, AuditEngagement>()
        .AddRepository<Guid, AuditPlanSummary>()
        .AddRepository<Guid, AuditUniverseObjective>()
        .AddRepository<Guid, AuditUniverse>()
        .AddRepository<Guid, BusinessObjective>()
        .AddRepository<Guid, BusinessObjective>()
        .AddRepository<Guid, SpecialProject>()
        .AddRepository<Guid, Objective>()
        //Checklists
        .AddRepository<Guid, Checklist>()
        .AddRepository<Guid, Remark>()
        //Common
        .AddRepository<Guid, Rating>()
        //Compliance
        .AddRepository<Guid, ComplianceChecklist>()
        //Jobs
        .AddRepository<Guid, JobScheduling>()
        .AddRepository<Guid, JobPrioritization>()
        .AddRepository<Guid, AuditJob>()
        //Organisation
        .AddRepository<Guid, SubLocation>()
        .AddRepository<Guid, SubDepartment>()
        .AddRepository<Guid, Location>()
        .AddRepository<Guid, Department>()
        .AddRepository<Guid, Company>()
        //Processes
        .AddRepository<Guid, SubProcess>()
        .AddRepository<Guid, AuditProcess>()
        //Reports
        .AddRepository<Guid, PlanningReport>()
        .AddRepository<Guid, JobTimeAllocationReport>()
        .AddRepository<Guid, InternalAuditConsolidationReport>()
        .AddRepository<Guid, AuditPlanSummaryReport>()
        .AddRepository<Guid, AuditExceptionReport>()
        .AddRepository<Guid, ReportingFollowUp>()
        //RiskControls
        .AddRepository<Guid, RiskControlMatrix>()
        .AddRepository<Guid, RiskControls>()
        .AddRepository<Guid, RiskProgram>()
        //Risks
        .AddRepository<Guid, Risk>()
        .AddRepository<Guid, RiskAssessment>()
        .AddRepository<Guid, RiskFactor>()
        .AddRepository<Guid, SpecificRiskFactor>()
        //SupportingDocs
        .AddRepository<Guid, SupportingDoc>()
        //Tasks
        .AddRepository<Guid, TaskManagement>()
        //Skills
        .AddRepository<Guid, Skill>()
        .AddRepository<Guid, SkillSet>()
        .AddRepository<Guid, SkillCategory>()
        //Users
        .AddRepository<Guid, UserDesignation>()
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
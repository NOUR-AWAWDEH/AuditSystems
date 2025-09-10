using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Auth;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Processes;
using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.Domain.Entities.SupportingDocs;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.DataAccess.Contexts;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Common
    public required DbSet<Rate> Rates { get; set; }

    // Organization
    public required DbSet<Company> Companies { get; set; }
    public required DbSet<Location> Locations { get; set; }
    public required DbSet<SubLocation> SubLocations { get; set; }
    public required DbSet<Department> Departments { get; set; }
    public required DbSet<SubDepartment> SubDepartments { get; set; }

    // Process
    public required DbSet<AuditProcess> AuditProcesses { get; set; }
    public required DbSet<SubProcess> SubProcesses { get; set; }

    // Risk Controls
    public required DbSet<RiskControlMatrix> RiskControlMatrices { get; set; }
    public required DbSet<Objective> Objectives { get; set; }
    public required DbSet<Risk> Risks { get; set; }
    public required DbSet<ExitControl> ExitControls { get; set; }
    public required DbSet<AuditProgram> AuditPrograms { get; set; }

    // Supporting Docs
    public required DbSet<SupportingDoc> SupportingDocs { get; set; }

    public new required DbSet<Role> Roles { get; set; }
    // Auth
    public required DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
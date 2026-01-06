using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Auth;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Observations;
using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Processes;
using AuditSystem.Domain.Entities.Recommendations;
using AuditSystem.Domain.Entities.Reports;
using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.Domain.Entities.SupportingDocs;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.DataAccess.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, Role, Guid>(options)
{

    // -------------------
    // Common
    // -------------------
    public virtual DbSet<Rate> Rates { get; set; } = null!;

    // -------------------
    // Organization
    // -------------------
    public virtual DbSet<Company> Companies { get; set; } = null!;
    public virtual DbSet<Location> Locations { get; set; } = null!;
    public virtual DbSet<Department> Departments { get; set; } = null!;
    public virtual DbSet<SubDepartment> SubDepartments { get; set; } = null!;
    public virtual DbSet<JobTitle> JobTitles { get; set; } = null!;

    // -------------------
    // Process
    // -------------------
    public virtual DbSet<AuditProcess> AuditProcesses { get; set; } = null!;
    public virtual DbSet<SubProcess> SubProcesses { get; set; } = null!;

    // -------------------
    // Risk Controls
    // -------------------
    public virtual DbSet<RiskControlMatrix> RiskControlMatrices { get; set; } = null!;
    public virtual DbSet<Objective> Objectives { get; set; } = null!;
    public virtual DbSet<Risk> Risks { get; set; } = null!;
    public virtual DbSet<ExitControl> ExitControls { get; set; } = null!;
    public virtual DbSet<ControlDesign> ControlDesigns { get; set; } = null!;
    public virtual DbSet<AuditProgram> AuditPrograms { get; set; } = null!;
    public virtual DbSet<Observation> Observations { get; set; } = null!;
    public virtual DbSet<Recommendation> Recommendations { get; set; } = null!;

    // -------------------
    // Supporting Docs
    // -------------------
    public virtual DbSet<SupportingDoc> SupportingDocs { get; set; } = null!;

    // -------------------
    // User
    // -------------------
    public virtual DbSet<AppUser> AppUsers { get; set; } = null!;
    public virtual DbSet<CompanyUser> CompanyUsers { get; set; } = null!;
    public virtual DbSet<UserSessionInfo> UserSessions { get; set; } = null!;

    // -------------------
    // Reports
    // -------------------
    public virtual DbSet<DriftReport> DriftReports { get; set; } = null!;

    // -------------------
    // Auth
    // -------------------
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

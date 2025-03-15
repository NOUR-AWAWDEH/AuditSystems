using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.CheckLists;
using AuditSystem.Domain.Entities.Compliance;
using AuditSystem.Domain.Entities.Jobs;
using AuditSystem.Domain.Entities.Organisation;
using AuditSystem.Domain.Entities.Reporting;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.Domain.Entities.Tasks;
using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Domain.Entities.Processes;
using AuditSystem.Domain.Entities.SupportingDocs;
using AuditSystem.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AuditSystem.Domain.Entities;

namespace AuditSystem.DataAccess.Contexts;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Common
    public required DbSet<Rating> Ratings { get; set; }

    // Audit
    public required DbSet<AuditEngagement> AuditEngagements { get; set; }
    public required DbSet<AuditPlanSummary> AuditPlanSummaries { get; set; }
    public required DbSet<AuditUniverse> AuditUniverses { get; set; }
    public required DbSet<AuditUniverseObjective> AuditUniverseObjectives { get; set; }
    public required DbSet<BusinessObjective> BusinessObjectives { get; set; }
    public required DbSet<AuditDomain> Domains { get; set; }
    public required DbSet<Objective> Objectives { get; set; }

    // Checklists
    public required DbSet<Checklist> Checklists { get; set; }
    public required DbSet<ChecklistManagement> ChecklistManagements { get; set; }
    public required DbSet<Remark> Remarks { get; set; }

    // Compliance
    public required DbSet<ComplianceAuditLink> ComplianceAuditLinks { get; set; }
    public required DbSet<ComplianceChecklist> ComplianceChecklists { get; set; }

    // Jobs
    public required DbSet<AuditJob> AuditJobs { get; set; }
    public required DbSet<JobPrioritization> JobPrioritizations { get; set; }
    public required DbSet<JobScheduling> JobSchedulings { get; set; }

    // Organisation
    public required DbSet<Company> Companies { get; set; }
    public required DbSet<Department> Departments { get; set; }
    public required DbSet<Location> Locations { get; set; }
    public required DbSet<SubDepartment> SubDepartments { get; set; }
    public required DbSet<SubLocation> SubLocations { get; set; }

    // Process
    public required DbSet<Process> Processes { get; set; }
    public required DbSet<SubProcess> SubProcesses { get; set; }

    // Reporting
    public required DbSet<ReportingFollowUp> ReportingFollowUps { get; set; }
    public required DbSet<PlanningReport> PlanningReports { get; set; }
    public required DbSet<JobTimeAllocationReport> JobTimeAllocationReports { get; set; }
    public required DbSet<InternalAuditConsolidationReport> InternalAuditConsolidationReports { get; set; }
    public required DbSet<AuditPlanSummaryReport> AuditPlanSummaryReports { get; set; }
    public required DbSet<AuditExceptionReport> AuditExceptionReports { get; set; }

    // Risk Controls
    public required DbSet<RiskControl> RiskControls { get; set; }
    public required DbSet<RiskControlMatrix> RiskControlMatrices { get; set; }
    public required DbSet<RiskProgram> RiskPrograms { get; set; }

    // Risks
    public required DbSet<Risk> Risks { get; set; }
    public required DbSet<RiskAssessment> RiskAssessments { get; set; }
    public required DbSet<RiskFactor> RiskFactors { get; set; }
    public required DbSet<SpecificRiskFactor> SpecificRiskFactors { get; set; }

    // Supporting Docs
    public required DbSet<SupportingDoc> SupportingDocs { get; set; }

    // Tasks
    public required DbSet<TaskManagement> TaskManagements { get; set; }

    // Users (Identity handles Users and Roles, so no need for UserRole)
    public required DbSet<Role> Roles { get; set; }
    public required DbSet<UserManagement> UserManagements { get; set; }
    public required DbSet<AuditorSettings> AuditorSettings { get; set; }
    public required DbSet<Skill> Skills { get; set; }
    public required DbSet<SkillSet> SkillSets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Seed Identity Roles with static GUIDs
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                Name = Role.Admin,
                NormalizedName = Role.Admin.ToUpper()
            },
            new Role
            {
                Id = new Guid("b2c3d4e5-f678-90ab-cdef-1234567890ab"),
                Name = Role.Auditor,
                NormalizedName = Role.Auditor.ToUpper()
            },
            new Role
            {
                Id = new Guid("c3d4e5f6-7890-abcd-ef12-34567890abcd"),
                Name = Role.User,
                NormalizedName = Role.User.ToUpper()
            });
    }
}
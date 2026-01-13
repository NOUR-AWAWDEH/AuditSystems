using AuditSystem.Domain.Entities.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class DriftReportConfiguration : IEntityTypeConfiguration<DriftReport>
{
    public void Configure(EntityTypeBuilder<DriftReport> builder)
    {
        builder.ToTable("DriftReports");

        builder.HasKey(dr => dr.Id);

        // Map the CreatedBy relationship to the CreatedByUserId property
        // This prevents EF Core from creating a shadow property "CreatedById"
        builder.HasOne(dr => dr.CreatedBy)
               .WithMany()
               .HasForeignKey(dr => dr.CreatedByUserId)
               .OnDelete(DeleteBehavior.Restrict);

        // Ensure other IDs are required
        builder.Property(dr => dr.CompanyId).IsRequired();
        builder.Property(dr => dr.LocationId).IsRequired();
        builder.Property(dr => dr.DepartmentId).IsRequired();
        builder.Property(dr => dr.SubDepartmentId).IsRequired();
        builder.Property(dr => dr.ObjectiveId).IsRequired();
        builder.Property(dr => dr.AuditProgramId).IsRequired();
    }
}

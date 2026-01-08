using AuditSystem.Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class AuditProgramConfiguration : IEntityTypeConfiguration<AuditProgram>
{
       public void Configure(EntityTypeBuilder<AuditProgram> builder)
       {
              builder.ToTable("AuditPrograms");

              // Primary Key
              builder.HasKey(ap => ap.Id);

              // Properties
              builder.Property(ap => ap.Description)
                     .IsRequired()
                     .HasMaxLength(500);

              builder.Property(ap => ap.ExitControlId)
                     .IsRequired();

              builder.Property(ap => ap.ProcessOwnerId)
                     .IsRequired();

              builder.Property(ap => ap.ControlDesignId)
                     .IsRequired(false);

              // Relationships

              // ExitControl (required)
              builder.HasOne(ap => ap.ExitControl)
                     .WithMany(ec => ec.AuditPrograms)
                     .HasForeignKey(ap => ap.ExitControlId)
                     .OnDelete(DeleteBehavior.Restrict);


              // ControlDesign (optional)
              builder.HasOne(ap => ap.ControlDesign)
                     .WithMany(cd => cd.AuditPrograms)
                     .HasForeignKey(ap => ap.ControlDesignId)
                     .OnDelete(DeleteBehavior.Restrict);

              // Observations (collection)
              builder.HasMany(ap => ap.Observations)
                     .WithOne(o => o.AuditProgram)
                     .HasForeignKey(o => o.AuditProgramId)
                     .OnDelete(DeleteBehavior.Restrict);


              builder.HasOne(e => e.CreatedBy)
                 .WithMany()
                 .HasForeignKey(e => e.CreatedByUserId)
                 .OnDelete(DeleteBehavior.Restrict);


              // Indexes
              builder.HasIndex(ap => ap.ExitControlId);
              builder.HasIndex(ap => ap.ProcessOwnerId);
              builder.HasIndex(ap => ap.ControlDesignId);
              builder.HasIndex(ap => ap.Description);
       }
}

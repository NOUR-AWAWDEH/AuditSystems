using AuditSystem.Domain.Entities.Checklists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class ChecklistConfiguration : IEntityTypeConfiguration<Checklist>
{
    public void Configure(EntityTypeBuilder<Checklist> builder)
    {
        builder.Property(c => c.Area).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Particulars).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Observation).IsRequired().HasMaxLength(500);
        builder.Property(c => c.ChecklistCollectionId).IsRequired();

        builder.HasOne(c => c.ChecklistCollection)
            .WithMany(cc => cc.Checklists)
            .HasForeignKey(c => c.ChecklistCollectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

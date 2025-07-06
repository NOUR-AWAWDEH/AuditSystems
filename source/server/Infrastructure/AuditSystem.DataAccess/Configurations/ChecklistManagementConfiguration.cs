using AuditSystem.Domain.Entities.Checklists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class ChecklistManagementConfiguration : IEntityTypeConfiguration<ChecklistCollection>
{
    public void Configure(EntityTypeBuilder<ChecklistCollection> builder)
    {
        builder.HasMany(clm => clm.Checklists)
               // Each Checklist has one reference to its ChecklistCollection.
               .WithOne(c => c.ChecklistCollection)
               .HasForeignKey(c => c.ChecklistCollectionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

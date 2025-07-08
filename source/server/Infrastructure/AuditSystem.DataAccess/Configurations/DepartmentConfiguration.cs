using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditSystem.DataAccess.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department> 
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Department");

        builder.Property(d => d.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.SubLocationId)
            .IsRequired();

        // RelationShips
        builder.HasOne(d => d.SubLocation)
            .WithMany(sl => sl.Departments)
            .HasForeignKey(d => d.SubLocationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

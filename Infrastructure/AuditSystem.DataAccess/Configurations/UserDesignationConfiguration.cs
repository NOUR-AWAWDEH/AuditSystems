using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AuditSystem.Domain.Entities.Users;

public class UserDesignationConfiguration : IEntityTypeConfiguration<UserDesignation>
{
    public void Configure(EntityTypeBuilder<UserDesignation> builder)
    {
        builder.ToTable("UserDesignations");

        builder.HasKey(ud => ud.Id);

        builder.Property(ud => ud.Designation)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ud => ud.Level)
            .IsRequired()
            .HasMaxLength(100); // Optional: adjust max length as needed

        builder.Property(ud => ud.IsActive)
            .IsRequired();

        builder.Property(ud => ud.Description)
            .HasMaxLength(500); // Optional: you can omit if no strict length needed

        builder.Property(ud => ud.UserId)
            .IsRequired();

        builder.HasOne(ud => ud.User)
            .WithMany()
            .HasForeignKey(ud => ud.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete
    }
}

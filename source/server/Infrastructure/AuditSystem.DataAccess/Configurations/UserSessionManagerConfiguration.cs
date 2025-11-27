using AuditSystem.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace AuditSystem.DataAccess.Configurations;

public class UserSessionInfoConfiguration : IEntityTypeConfiguration<UserSessionInfo>
{
       public void Configure(EntityTypeBuilder<UserSessionInfo> builder)
       {
              builder.ToTable("UserSessions");

              builder.HasKey(usi => usi.Id);

              // Relationship to AppUser
              builder.HasOne(usi => usi.AppUser)
                     .WithMany(au => au.UserSessions)
                     .HasForeignKey(usi => usi.AppUserId)
                     .OnDelete(DeleteBehavior.Cascade);

              // Default values
              builder.Property(usi => usi.TotalSessionTime)
                     .IsRequired()
                     .HasDefaultValue(TimeSpan.Zero);

              builder.Property(usi => usi.IsActive)
                     .IsRequired()
                     .HasDefaultValue(false);

              builder.Property(usi => usi.CurrentSessionStart)
                     .IsRequired(false);

              builder.Property(usi => usi.LastLogin)
                     .IsRequired(false);

       }
}

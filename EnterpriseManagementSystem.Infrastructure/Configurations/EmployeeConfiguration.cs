using EnterpriseManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnterpriseManagementSystem.Infrastructure.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        
        builder.ToTable("Employees", "hr");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .HasMaxLength(255);

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(e => e.Department)
            .HasMaxLength(100);

        builder.Property(e => e.Position)
            .HasMaxLength(100);

        builder.Property(e => e.IdentityUserId)
            .HasColumnName("IdentityUserId");

        // Note: The FK constraint to auth.AspNetUsers.Id must be created manually in the migration
        // This is because we cannot reference Identity types from Business module (Clean Architecture)
        // See MIGRATION_GUIDE.md for instructions on adding the FK constraint

        builder.HasIndex(e => e.IdentityUserId)
            .HasDatabaseName("IX_Employees_IdentityUserId");

        builder.HasIndex(e => e.Email)
            .HasDatabaseName("IX_Employees_Email");
    }
}


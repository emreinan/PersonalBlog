using App.Data.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).IsRequired().HasMaxLength(50);
        builder.Property(r => r.CreatedAt).IsRequired();
        builder.HasIndex(r => r.Name).IsUnique();

        new RoleSeed().Configure(builder);
    }
}
internal class RoleSeed : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(
            new Role
            {
                Id = 1,
                Name = "Admin",
                CreatedAt = DateTime.Now
            },
            new Role
            {
                Id = 2,
                Name = "Commenter",
                CreatedAt = DateTime.Now
            }
        );
    }
}
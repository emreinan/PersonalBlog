using App.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class RoleConfiguration : BaseEntityConfiguration<Role, int>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);
        builder.Property(r => r.Name).IsRequired().HasMaxLength(50);
        
        builder.HasIndex(r => r.Name).IsUnique();
    }
}
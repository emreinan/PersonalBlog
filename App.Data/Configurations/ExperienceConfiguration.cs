using App.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class ExperienceConfiguration : BaseEntityConfiguration<Experience, int>
{
    public override void Configure(EntityTypeBuilder<Experience> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Company).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Description).HasMaxLength(1000);
    }
}

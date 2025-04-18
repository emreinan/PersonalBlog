using App.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class EducationConfiguration : BaseEntityConfiguration<Education,int>
{
    public override void Configure(EntityTypeBuilder<Education> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.School).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Degree).IsRequired().HasMaxLength(100);
        builder.Property(e => e.FieldOfStudy).IsRequired().HasMaxLength(100);
    }
}

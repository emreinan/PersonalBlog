using App.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class AboutMeConfiguration : BaseEntityConfiguration<AboutMe, int>
{
    public override void Configure(EntityTypeBuilder<AboutMe> builder)
    {
        base.Configure(builder);
        builder.Property(a => a.Introduciton).IsRequired().HasMaxLength(1000);
        builder.Property(a => a.ImageUrl1).HasMaxLength(255);
        builder.Property(a => a.ImageUrl2).HasMaxLength(255);
    }
}

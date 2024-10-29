using App.Data.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class AboutMeConfiguration : IEntityTypeConfiguration<AboutMe>
{
    public void Configure(EntityTypeBuilder<AboutMe> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Introduciton).IsRequired().HasMaxLength(1000);
        builder.Property(a => a.ImageUrl1).HasMaxLength(255);
        builder.Property(a => a.ImageUrl2).HasMaxLength(255);

        new AboutMeSeed().Configure(builder);
    }
}
internal class AboutMeSeed : IEntityTypeConfiguration<AboutMe>
{
    public void Configure(EntityTypeBuilder<AboutMe> builder)
    {
        builder.HasData(
            new AboutMe
            {
                Id = 1,
                Introduciton = "Hello, I'm a software developer.",
                ImageUrl1 = "https://localhost:7207/api/File/GetByUrl?FileUrl=about.jpg",
                ImageUrl2 = "",
                Cv = "Emre-Inan-Cv-Eng.pdf",
                Title = "Junior Backend Developer"
            }
        );
    }
}
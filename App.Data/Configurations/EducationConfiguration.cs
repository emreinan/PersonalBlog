using App.Data.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.School).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Degree).IsRequired().HasMaxLength(100);
        builder.Property(e => e.FieldOfStudy).IsRequired().HasMaxLength(100);
        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(cm => cm.CreatedAt).IsRequired();

        new EducationSeed().Configure(builder);
    }
}
internal class EducationSeed : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.HasData(
            new Education
            {
                Id = 1,
                School = "Tech University",
                Degree = "Bachelor of Science",
                FieldOfStudy = "Computer Science",
                StartDate = new DateTime(2015, 9, 1),
                EndDate = new DateTime(2019, 6, 30)
            },
            new Education
            {
                Id = 2,
                School = "Online Coding Academy",
                Degree = "Certificate",
                FieldOfStudy = "Web Development",
                StartDate = new DateTime(2020, 1, 15),
                EndDate = new DateTime(2020, 5, 15)
            },
            new Education
            {
                Id = 3,
                School = "Digital Marketing Institute",
                Degree = "Diploma",
                FieldOfStudy = "Digital Marketing",
                StartDate = new DateTime(2021, 3, 1),
                EndDate = new DateTime(2021, 9, 1)
            }
        );
    }
}

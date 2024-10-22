using App.Data.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Company).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Description).HasMaxLength(1000);
        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(cm => cm.CreatedAt).IsRequired();

        new ExperienceSeed().Configure(builder);
    }
}
internal class ExperienceSeed : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.HasData(
            new Experience
            {
                Id = 1,
                Title = "Software Engineer",
                Company = "Tech Innovations Inc.",
                StartDate = new DateTime(2020, 1, 15),
                EndDate = new DateTime(2022, 6, 30),
                Description = "Developed and maintained web applications using ASP.NET Core."
            },
            new Experience
            {
                Id = 2,
                Title = "Frontend Developer",
                Company = "Creative Solutions Ltd.",
                StartDate = new DateTime(2018, 3, 1),
                EndDate = new DateTime(2020, 1, 14),
                Description = "Designed user interfaces and implemented responsive layouts."
            },
            new Experience
            {
                Id = 3,
                Title = "Backend Developer",
                Company = "Global Tech Systems",
                StartDate = new DateTime(2017, 5, 10),
                EndDate = null, // Currently working
                Description = "Focused on server-side application logic and database management."
            },
            new Experience
            {
                Id = 4,
                Title = "Intern",
                Company = "Future Coders Academy",
                StartDate = new DateTime(2016, 6, 1),
                EndDate = new DateTime(2016, 9, 1),
                Description = "Assisted in developing small-scale projects and learning programming practices."
            },
            new Experience
            {
                Id = 5,
                Title = "Project Manager",
                Company = "Innovatech Group",
                StartDate = new DateTime(2022, 7, 1),
                EndDate = null, // Currently working
                Description = "Managing project timelines and collaborating with development teams."
            }
        );
    }
}

using App.Data.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace App.Data.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(1000);
        builder.Property(p => p.ImageUrl).HasMaxLength(255);
        builder.Property(p => p.IsActive).IsRequired();
        builder.Property(cm => cm.CreatedAt).IsRequired();
        

        new ProjectSeed().Configure(builder);
    }
}

internal class ProjectSeed : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasData(
            new Project
            {
                Id = 1,
                Title = "Web Development",
                Description = "Creating responsive and dynamic websites.",
                ImageUrl = "https://picsum.photos/seed/webdev/200/300"
            },
            new Project
            {
                Id = 2,
                Title = "Mobile App Development",
                Description = "Building mobile applications for iOS and Android.",
                ImageUrl = "https://picsum.photos/seed/mobileapp/200/300"
            },
            new Project
            {
                Id = 3,
                Title = "Machine Learning",
                Description = "Developing machine learning models for data analysis.",
                ImageUrl = "https://picsum.photos/seed/machinelearning/200/300"
            },
            new Project
            {
                Id = 4,
                Title = "Cloud Computing",
                Description = "Utilizing cloud services for scalable applications.",
                ImageUrl = "https://picsum.photos/seed/cloudcomputing/200/300"
            },
            new Project
            {
                Id = 5,
                Title = "Digital Marketing",
                Description = "Strategies for promoting products online.",
                ImageUrl = "https://picsum.photos/seed/digitalmarketing/200/300"
            },
            new Project
            {
                Id = 6,
                Title = "Cybersecurity",
                Description = "Protecting systems and networks from cyber threats.",
                ImageUrl = "https://picsum.photos/seed/cybersecurity/200/300"
            }
        );
    }
}

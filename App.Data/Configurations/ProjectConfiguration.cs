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
                ImageUrl = "https://localhost:7207/images/project-3.jpg"
            },
            new Project
            {
                Id = 2,
                Title = "Mobile App Development",
                Description = "Building mobile applications for iOS and Android.",
                ImageUrl = "https://localhost:7207/images/project-4.jpg"
            },
            new Project
            {
                Id = 3,
                Title = "Machine Learning",
                Description = "Developing machine learning models for data analysis.",
                ImageUrl = "https://localhost:7207/images/project-5.jpg"
            },
            new Project
            {
                Id = 4,
                Title = "Cloud Computing",
                Description = "Utilizing cloud services for scalable applications.",
                ImageUrl = "https://localhost:7207/images/project-1.jpg"
            },
            new Project
            {
                Id = 5,
                Title = "Digital Marketing",
                Description = "Strategies for promoting products online.",
                ImageUrl = "https://localhost:7207/images/project-2.jpg"
            },
            new Project
            {
                Id = 6,
                Title = "Cybersecurity",
                Description = "Protecting systems and networks from cyber threats.",
                ImageUrl = "https://localhost:7207/images/project-6.jpg"
            }
        );
    }
}

using App.Data.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.HasKey(bp => bp.Id);
        builder.Property(bp => bp.Title).IsRequired().HasMaxLength(100);
        builder.Property(bp => bp.Content).IsRequired();
        builder.Property(bp => bp.CreatedAt).IsRequired();

        builder.HasMany(bp => bp.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        new BlogPostSeed().Configure(builder);
    }
}
internal class BlogPostSeed : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.HasData(
            new BlogPost
            {
                Id = new Guid("24900544-58fc-4be7-9ab1-18f088510da4"),
                AuthorId = new Guid("7c117612-fb38-48c9-9908-97b433a3f92b"),
                Title = "Introduction to ASP.NET Core",
                Content = "ASP.NET Core is a cross-platform, high-performance framework for building modern, cloud-based, internet-connected applications. It allows developers to create web apps, services, and APIs with ease.",
                ImageUrl = "https://localhost:7207/images/image_1.jpg",
                CreatedAt = DateTime.Now,
            },
            new BlogPost
            {
                Id = new Guid("7aa98865-285c-4c67-a96a-8fcd30855234"),
                AuthorId = new Guid("7c117612-fb38-48c9-9908-97b433a3f92b"),
                Title = "Understanding Entity Framework Core",
                Content = "Entity Framework Core is a lightweight and extensible version of Entity Framework. It is an open-source object-relational mapper for .NET applications.",
                ImageUrl = "https://localhost:7207/images/image_2.jpg",
                CreatedAt = DateTime.Now,
            },
            new BlogPost
            {
                Id = new Guid("b860336c-64af-497d-9bbd-ed95fd752e9f"),
                AuthorId = new Guid("7c117612-fb38-48c9-9908-97b433a3f92b"),
                Title = "Building RESTful APIs with ASP.NET Core",
                Content = "In this blog post, we will explore how to build RESTful APIs using ASP.NET Core. We will cover routing, controllers, and data handling.",
                ImageUrl = "https://localhost:7207/images/image_3.jpg",
                CreatedAt = DateTime.Now,
            }
        );
    }
}
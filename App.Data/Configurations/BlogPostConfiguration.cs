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

 
    }
}
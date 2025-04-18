using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class BlogPostConfiguration : BaseEntityConfiguration<BlogPost, Guid>
{
    public override void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        base.Configure(builder);
        builder.Property(bp => bp.Title).IsRequired().HasMaxLength(100);
        builder.Property(bp => bp.Content).IsRequired();

        builder.HasMany(bp => bp.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
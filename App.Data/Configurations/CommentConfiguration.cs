using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class CommentConfiguration : BaseEntityConfiguration<Comment, int>
{
    public override void Configure(EntityTypeBuilder<Comment> builder)
    {
        base.Configure(builder);
        builder.Property(c => c.Content).IsRequired().HasMaxLength(1000);
        builder.Property(c => c.IsApproved).IsRequired();
    }
}
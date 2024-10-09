﻿using App.Data.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Content).IsRequired().HasMaxLength(1000);
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.IsApproved).IsRequired();

        builder.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId);

        builder.HasOne(c => c.Post)
            .WithMany(bp => bp.Comments)
            .HasForeignKey(c => c.PostId);
    }
}
using App.Data.Entities.Auth;
using App.Shared.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Configurations;

public class UserConfiguraiton : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.UserName).IsRequired().HasMaxLength(50);
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.PasswordSalt).IsRequired();
        builder.Property(u => u.CreatedAt).IsRequired();

        builder.HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Experiences)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.Educations)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.Projects)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.BlogPosts)
            .WithOne(bp => bp.User)
            .HasForeignKey(bp => bp.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        new UserSeed().Configure(builder);
    }
}

internal class UserSeed : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        HashingHelper.CreatePasswordHash("1234", out byte[] passwordHash, out byte[] passwordSalt);

        builder.HasData(
            new User
            {
                Id = Guid.NewGuid(),
                UserName = "admin",
                Email = "admin@mail.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RoleId = 1,
                CreatedAt = DateTime.Now,
                IsActive =true
            },
            new User
            {
            Id = Guid.NewGuid(),
            UserName = "commenter",
            Email = "commenter@mail.com",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            RoleId = 2,
            CreatedAt = DateTime.Now,
            IsActive = true    
            }
        );
    }
}

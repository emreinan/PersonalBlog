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
               Id = new Guid("7c117612-fb38-48c9-9908-97b433a3f92b"),
               UserName = "Admin",
               Email = "admin@mail.com",
               PasswordHash = passwordHash,
               PasswordSalt = passwordSalt,
               RoleId = 1,
               CreatedAt = DateTime.Now,
               IsActive = true,
               ProfilePhotoUrl = "Dosya_000.jpeg"
           },
            new User
            {
                Id = new Guid("a8647b9a-fbf3-4867-ba8f-5fc8239bff12"),
                UserName = "John",
                Email = "commenter@mail.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RoleId = 2,
                CreatedAt = DateTime.Now,
                IsActive = true
            },
            new User
            {
                Id = new Guid("44063c52-807a-40bf-a65c-a06d5ac3ee26"),
                UserName = "Alice",
                Email = "commenter2@mail.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RoleId = 2,
                CreatedAt = DateTime.Now,
                IsActive = true
            }
            );
    }
}

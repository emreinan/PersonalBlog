using App.Data.Configurations;
using App.Data.Entities.Auth;
using App.Data.Entities.Data;
using App.Shared.Security;
using Microsoft.EntityFrameworkCore;


namespace App.Data.Contexts;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguraiton());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());

        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var userId3 = Guid.NewGuid();
        var blogId1 = Guid.NewGuid();
        var blogId2 = Guid.NewGuid();
        var blogId3 = Guid.NewGuid();

        HashingHelper.CreatePasswordHash("1234", out byte[] passwordHash, out byte[] passwordSalt);


        modelBuilder.Entity<User>().HasData(
             new User
             {
                 Id = Guid.NewGuid(),
                 UserName = "admin",
                 Email = "admin@mail.com",
                 PasswordHash = passwordHash,
                 PasswordSalt = passwordSalt,
                 RoleId = 1,
                 CreatedAt = DateTime.Now,
                 IsActive = true
             },
            new User
            {
                Id = Guid.NewGuid(),
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
                Id = Guid.NewGuid(),
                UserName = "Alice",
                Email = "commenter2@mail.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RoleId = 2,
                CreatedAt = DateTime.Now,
                IsActive = true
            }
            );

        modelBuilder.Entity<BlogPost>().HasData(
            new BlogPost
            {
                Id = blogId1,
                Title = "Introduction to ASP.NET Core",
                Content = "ASP.NET Core is a cross-platform, high-performance framework for building modern, cloud-based, internet-connected applications. It allows developers to create web apps, services, and APIs with ease.",
                ImageUrl = "https://picsum.photos/seed/aspnetcore/400/300",
                CreatedAt = DateTime.Now,
            },
            new BlogPost
            {
                Id = blogId2,
                Title = "Understanding Entity Framework Core",
                Content = "Entity Framework Core is a lightweight and extensible version of Entity Framework. It is an open-source object-relational mapper for .NET applications.",
                ImageUrl = "https://picsum.photos/seed/entityframework/400/300",
                CreatedAt = DateTime.Now,
            },
            new BlogPost
            {
                Id = blogId3,
                Title = "Building RESTful APIs with ASP.NET Core",
                Content = "In this blog post, we will explore how to build RESTful APIs using ASP.NET Core. We will cover routing, controllers, and data handling.",
                ImageUrl = "https://picsum.photos/seed/restfulapi/400/300",
                CreatedAt = DateTime.Now,
            }
            );

        modelBuilder.Entity<Comment>().HasData(
            new Comment
            {
                Id = 1,
                Content = "Great post!",
                CreatedAt = DateTime.Now,
                IsApproved = true,
                PostId = blogId1,
                UserId = userId2
            },
            new Comment
            {
                Id = 2,
                Content = "Thanks for sharing!",
                CreatedAt = DateTime.Now,
                IsApproved = true,
                PostId = blogId2,
                UserId = userId3
            },
            new Comment
            {
                Id = 3,
                Content = "Awesome!",
                CreatedAt = DateTime.Now,
                IsApproved = true,
                PostId = blogId3,
                UserId = userId2
            },
            new Comment
            {
                Id = 4,
                Content = "Nice work!",
                CreatedAt = DateTime.Now,
                IsApproved = true,
                PostId = blogId1,
                UserId = userId3
            },
            new Comment
            {
                Id = 5,
                Content = "Keep it up!",
                CreatedAt = DateTime.Now,
                IsApproved = true,
                PostId = blogId2,
                UserId = userId2
            }
            );
    }
}

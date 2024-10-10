using App.Data.Configurations;
using App.Data.Entities.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Contexts;

public class DataDbContext : DbContext
{
    public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
    {
    }
    public DbSet<AboutMe> AboutMes { get; set; }
    public DbSet<Experience> Experiences { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }
    public DbSet<PersonalInfo> PersonalInfos { get; set; }
    public DbSet<Project> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AboutMeConfiguration());
        modelBuilder.ApplyConfiguration(new ExperienceConfiguration());
        modelBuilder.ApplyConfiguration(new EducationConfiguration());
        modelBuilder.ApplyConfiguration(new BlogPostConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new ContactMessageConfiguration());
        modelBuilder.ApplyConfiguration(new PersonalInfoConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());

    }
}

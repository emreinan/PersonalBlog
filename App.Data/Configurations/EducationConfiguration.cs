using App.Data.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.School).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Degree).IsRequired().HasMaxLength(100);
        builder.Property(e => e.FieldOfStudy).IsRequired().HasMaxLength(100);
        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(cm => cm.CreatedAt).IsRequired();


        builder.HasOne(e => e.User)
            .WithMany(u => u.Educations)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
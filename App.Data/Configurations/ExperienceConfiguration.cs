using App.Data.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Company).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Description).HasMaxLength(1000);
        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(cm => cm.CreatedAt).IsRequired();

        builder.HasOne(e => e.User)
            .WithMany(u => u.Experiences)
            .HasForeignKey(e => e.UserId);
    }
}
using App.Data.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
{
    public void Configure(EntityTypeBuilder<ContactMessage> builder)
    {
        builder.HasKey(cm => cm.Id);
        builder.Property(cm => cm.Name).IsRequired().HasMaxLength(50);
        builder.Property(cm => cm.Email).IsRequired().HasMaxLength(100);
        builder.Property(cm => cm.Subject).IsRequired().HasMaxLength(100);
        builder.Property(cm => cm.Message).IsRequired().HasMaxLength(1000);
        builder.Property(cm => cm.CreatedAt).IsRequired();
    }
}
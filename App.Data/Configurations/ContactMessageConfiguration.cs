using App.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class ContactMessageConfiguration : BaseEntityConfiguration<ContactMessage, int>
{
    public override void Configure(EntityTypeBuilder<ContactMessage> builder)
    {
        base.Configure(builder);

        builder.Property(cm => cm.Name).IsRequired().HasMaxLength(50);
        builder.Property(cm => cm.Email).IsRequired().HasMaxLength(100);
        builder.Property(cm => cm.Subject).IsRequired().HasMaxLength(100);
        builder.Property(cm => cm.Message).IsRequired().HasMaxLength(1000);
    }
}
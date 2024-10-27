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

        new ContactMessageSeed().Configure(builder);
    }
}

internal class ContactMessageSeed : IEntityTypeConfiguration<ContactMessage>
{
    public void Configure(EntityTypeBuilder<ContactMessage> builder)
    {
        builder.HasData(
            new ContactMessage
            {
                Id = 1,
                Name = "John Doe",
                Email = "user@mail.com",
                Subject = "Hello",
                Message = "Hello, how are you?",
                CreatedAt = DateTime.Now
            },
            new ContactMessage
            {
                Id = 2,
                Name = "Jane Doe",
                Email = "user1@mail.com",
                Subject = "Hello",
                Message = "Hello, how are you?",
                CreatedAt = DateTime.Now
            },
            new ContactMessage
            {
                Id = 3,
                Name = "Emre İnan",
                Email = "emreinannn@gmail.com",
                Subject = "Selam",
                Message = "Lütfen mesajımı oku ve mail gönder!",
                CreatedAt = DateTime.Now
            }
            );
    }
}

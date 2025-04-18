using App.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class PersonalInfoConfiguration : BaseEntityConfiguration<PersonalInfo, int>
{
    public override void Configure(EntityTypeBuilder<PersonalInfo> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
        builder.Property(p => p.PhoneNumber).HasMaxLength(20);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(150);
        builder.Property(p => p.BirthDate).IsRequired();
        builder.Property(p => p.Address).HasMaxLength(1000);
    }
}
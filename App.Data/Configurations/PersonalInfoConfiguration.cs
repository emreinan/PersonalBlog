﻿using App.Data.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Configurations;

public class PersonalInfoConfiguration : IEntityTypeConfiguration<PersonalInfo>
{
    public void Configure(EntityTypeBuilder<PersonalInfo> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
        builder.Property(p => p.PhoneNumber).HasMaxLength(20);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(150);
        builder.Property(p => p.BirthDate).IsRequired();
        builder.Property(p => p.Address).HasMaxLength(1000);

        new PersonalInfoSeed().Configure(builder);
    }
}
internal class PersonalInfoSeed : IEntityTypeConfiguration<PersonalInfo>
{
    public void Configure(EntityTypeBuilder<PersonalInfo> builder)
    {
        builder.HasData(new PersonalInfo
        {
            Id = 1,
            FirstName = "Emre",
            LastName = "İnan",
            PhoneNumber = "+90 553 238 2222",
            Email = "emreinannn@gmail.com",
            BirthDate = new DateTime(1993, 8, 9),
            Address = "İstanbul TR"
        });
    }
}
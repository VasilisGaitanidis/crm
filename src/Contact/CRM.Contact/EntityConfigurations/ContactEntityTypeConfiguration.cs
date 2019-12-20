using System;
using CRM.Contact.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CRM.Contact.EntityConfigurations
{
    public class ContactEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Contact>
    {
        public void Configure(EntityTypeBuilder<Domain.Contact> builder)
        {
            builder.ToTable("contact", ContactContext.DEFAULT_SCHEMA);
            builder.HasKey(x => x.ContactId);
            builder.Property(p=> p.ContactType)
                .HasConversion(new EnumToStringConverter<ContactType>());
        }
    }
}
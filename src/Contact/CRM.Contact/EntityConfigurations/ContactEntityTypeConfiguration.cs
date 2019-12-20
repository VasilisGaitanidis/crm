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
            builder.Property(p => p.ContactType)
                .HasConversion(new EnumToStringConverter<ContactType>());
            builder.OwnsOne(
                o => o.MailingAddress,
                ma =>
                {
                    ma.Property(x => x.Street).HasColumnName("mailing_street");
                    ma.Property(x => x.City).HasColumnName("mailing_city");
                    ma.Property(x => x.State).HasColumnName("mailing_state");
                    ma.Property(x => x.ZipCode).HasColumnName("mailing_zipcode");
                    ma.Property(x => x.Country).HasColumnName("mailing_country");
                }
            );
            builder.OwnsOne(
                o => o.ContactInfo,
                ma =>
                {
                    ma.Property(x => x.Email).HasColumnName("email");
                    ma.Property(x => x.Mobile).HasColumnName("mobile");
                    ma.Property(x => x.WorkPhone).HasColumnName("work_phone");
                    ma.Property(x => x.HomePhone).HasColumnName("home_phone");
                }
            );
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Core;

public class ContactConfiguration : IEntityTypeConfiguration<Model.Core.Models.Contact>
{
    public void Configure(EntityTypeBuilder<Model.Core.Models.Contact> builder)
    {
        builder.HasKey(contact => contact.Id);
        builder.Property(contact => contact.Id)
            .ValueGeneratedOnAdd();   
        builder.Property(contact => contact.Name).IsRequired();
        builder.Property(contact => contact.MobilePhone).IsRequired();
    }
}
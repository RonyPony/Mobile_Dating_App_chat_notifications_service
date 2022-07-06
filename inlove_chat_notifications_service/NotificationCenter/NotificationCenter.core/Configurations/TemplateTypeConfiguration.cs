using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationCenter.Core.Domain;

namespace NotificationCenter.Core.Configurations
{
    /// <summary>
    /// Represents a <see cref="TemplateType"/> configuration
    /// </summary>
    internal class TemplateTypeConfiguration : IEntityTypeConfiguration<TemplateType>
    {
        ///<inheritdoc cref="IEntityTypeConfiguration{TemplateType}"/>
        public void Configure(EntityTypeBuilder<TemplateType> builder)
        {
            builder.HasKey(prop => prop.Id);
            builder.Property(prop => prop.Id).ValueGeneratedOnAdd();
            builder.Property(prop => prop.ReadableId).IsRequired();
            builder.HasIndex(prop => prop.ReadableId).IsUnique();
            builder.Property(prop => prop.Name).IsRequired();
            builder.Property(prop => prop.Name).HasMaxLength(64);
            builder.Property(prop => prop.Description).IsRequired();
            builder.Property(prop => prop.Description).HasMaxLength(1024);
            builder.HasMany(prop => prop.Data).WithOne(prop => prop.Template);
        }
    }
}

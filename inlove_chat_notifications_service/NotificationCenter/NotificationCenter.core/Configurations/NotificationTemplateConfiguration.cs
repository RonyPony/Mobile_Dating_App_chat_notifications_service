using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationCenter.Core.Domain;

namespace NotificationCenter.Core.Configurations
{
    /// <summary>
    /// Represents a <see cref="NotificationTemplate"/> configuration
    /// </summary>
    internal sealed class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
    {
        ///<inheritdoc cref="IEntityTypeConfiguration{NotificationTemplate}"/>
        public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
        {
            builder.HasKey(prop => prop.Id);
            builder.Property(prop => prop.Id).ValueGeneratedOnAdd();
            builder.Property(prop => prop.Title).IsRequired();
            builder.Property(prop => prop.Body).IsRequired();
        }
    }
}

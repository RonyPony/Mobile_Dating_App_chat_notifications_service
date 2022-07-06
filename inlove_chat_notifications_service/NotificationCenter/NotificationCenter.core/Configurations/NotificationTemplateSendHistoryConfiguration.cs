using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationCenter.Core.Domain;

namespace NotificationCenter.Core.Configurations
{
    /// <summary>
    /// Represents a <see cref="NotificationTemplateSendHistory"/> configuration
    /// </summary>
    internal sealed class NotificationTemplateSendHistoryConfiguration : IEntityTypeConfiguration<NotificationTemplateSendHistory>
    {
        ///<inheritdoc cref="IEntityTypeConfiguration{NotificationTemplate}"/>
        public void Configure(EntityTypeBuilder<NotificationTemplateSendHistory> builder)
        {
            builder.HasKey(prop => prop.Id);
            builder.Property(prop => prop.Id).ValueGeneratedOnAdd();

            builder.HasOne(prop => prop.TokenUser)
                .WithMany(user => user.NotificationTemplate)
                .HasForeignKey(prop => prop.TokenUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(prop => prop.NotificationTemplate)
                .WithMany(template => template.History)
                .HasForeignKey(prop => prop.NotificationTemplateId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

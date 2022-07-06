using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationCenter.Core.Domain;

namespace NotificationCenter.Core.Configurations
{
    /// <summary>
    /// Represents the <see cref="Device"/> entity configuration
    /// </summary>
    internal sealed class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        ///<inheritdoc cref="IEntityTypeConfiguration{Device}"/>
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.HasKey(prop => prop.Id);
            builder.Property(prop => prop.Id).ValueGeneratedOnAdd();
            builder.HasIndex(prop => prop.FcmToken).IsUnique();
            builder.HasOne(prop => prop.App)
                .WithMany(a => a.Devices).HasForeignKey(prop => prop.AppId);
            builder.HasIndex(prop => new { prop.TokenUserId, prop.DeviceIdentifier })
                .IsUnique();
            builder.HasOne(prop => prop.TokenUser)
                .WithMany(u => u.Devices).HasForeignKey(prop => prop.TokenUserId);
        }
    }
}

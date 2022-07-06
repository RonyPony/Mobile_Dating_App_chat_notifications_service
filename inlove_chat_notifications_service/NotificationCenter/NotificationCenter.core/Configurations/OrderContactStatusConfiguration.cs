using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationCenter.Core.Domain;
using System;

namespace NotificationCenter.Core.Configurations
{
    /// <summary>
    /// Represents a <see cref="OrderContactStatus"/> configuration
    /// </summary>
    internal class OrderContactStatusConfiguration : IEntityTypeConfiguration<OrderContactStatus>
    {
        ///<inheritdoc cref="IEntityTypeConfiguration{OrderContactStatus}"/>
        public void Configure(EntityTypeBuilder<OrderContactStatus> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}

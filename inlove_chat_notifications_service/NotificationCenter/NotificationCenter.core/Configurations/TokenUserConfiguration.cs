using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationCenter.Core.Domain;

namespace NotificationCenter.Core.Configurations
{
    /// <summary>
    /// Represents the Token User configuration
    /// </summary>
    internal sealed class TokenUserConfiguration : IEntityTypeConfiguration<TokenUser> 
    {
        /// <inheritdoc cref="IEntityTypeConfiguration{TokenUser}"/>
        public void Configure(EntityTypeBuilder<TokenUser> builder)
        {
            builder.HasKey(prop => prop.Id);
            builder.Property(prop => prop.Id).ValueGeneratedOnAdd();
            builder.HasIndex(prop => prop.UserId).IsUnique();
        }
    }
}
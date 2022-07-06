using Microsoft.EntityFrameworkCore;
using NotificationCenter.Core.Configurations;
using NotificationCenter.Core.Domain;

namespace NotificationCenter.Core
{
    /// <summary>
    /// Represents the DbContext for the notification center
    /// </summary>
    public class NotificationCenterContext : DbContext
    {
        ///<summary>
        /// Builds a new instance of notification center
        /// </summary>
        /// <param name="options">The options to be passed to this <see cref="NotificationCenterContext"/>.</param>
        public NotificationCenterContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TokenUser> TokenUsers { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<App> Apps { get; set; }
        public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
        public DbSet<TemplateType> NotificationTemplateTypes { get; set; }
        public DbSet<NotificationTemplateSendHistory> NotificationTemplateHistory { get; set; }
        public DbSet<OrderContactStatus> OrderContactStatuses { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ClientAndRoom> ClientAndRoom { get; set; }
        public DbSet<ChatRoomMessages> RoomMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new DeviceConfiguration());
            modelBuilder.ApplyConfiguration(new TemplateTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationTemplateConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationTemplateSendHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new TokenUserConfiguration());
            modelBuilder.ApplyConfiguration(new AppConfiguration());
            modelBuilder.Entity<ClientAndRoom>(build =>
            {
                build.HasOne(cr => cr.Client)
                .WithMany(c => c.ClientAndRooms)
                .HasForeignKey(cr => cr.ClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

                build.HasOne(cr => cr.Room)
                .WithMany(r => r.ClientAndRooms)
                .HasForeignKey(cr => cr.RoomId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            });
        }
    }
}

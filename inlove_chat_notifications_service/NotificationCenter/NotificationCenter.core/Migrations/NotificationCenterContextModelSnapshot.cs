﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NotificationCenter.Core;

namespace NotificationCenter.Core.Migrations
{
    [DbContext(typeof(NotificationCenterContext))]
    partial class NotificationCenterContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NotificationCenter.Core.Domain.App", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AppName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PackageNames")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AppName")
                        .IsUnique();

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("NotificationCenter.Core.Domain.ChatRoomMessages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("MessageContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RoomMessages");
                });

            modelBuilder.Entity("NotificationCenter.Core.Domain.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AppType")
                        .HasColumnType("int");

                    b.Property<string>("ConnectionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsConnected")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("NotificationCenter.Core.Domain.ClientAndRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("RoomId");

                    b.ToTable("ClientAndRoom");
                });

            modelBuilder.Entity("NotificationCenter.Core.Domain.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ApnsToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("AppId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DeviceIdentifier")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FcmToken")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("TokenUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.HasIndex("FcmToken")
                        .IsUnique()
                        .HasFilter("[FcmToken] IS NOT NULL");

                    b.HasIndex("TokenUserId", "DeviceIdentifier")
                        .IsUnique()
                        .HasFilter("[DeviceIdentifier] IS NOT NULL");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("NotificationCenter.Core.Domain.NotificationTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HighPriority")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<Guid?>("TemplateTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TemplateTypeId");

                    b.ToTable("NotificationTemplates");
                });

            modelBuilder.Entity("NotificationCenter.Core.Domain.NotificationTemplateSendHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateRead")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateSent")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeveicesSentTo")
                        .HasColumnType("int");

                    b.Property<int>("DevicesCount")
                        .HasColumnType("int");

                    b.Property<int>("FailedDevices")
                        .HasColumnType("int");

                    b.Property<bool>("Ignored")
                        .HasColumnType("bit");

                    b.Property<Guid>("NotificationTemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Payload")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReadMethod")
                        .HasColumnType("int");

                    b.Property<Guid>("TokenUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("NotificationTemplateId");

                    b.HasIndex("TokenUserId");

                    b.ToTable("NotificationTemplateHistory");
                });

            modelBuilder.Entity("NotificationCenter.Core.Domain.OrderContactStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("OrderId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OrderContactStatuses");
                });

            modelBuilder.Entity("NotificationCenter.Core.Domain.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AppType")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("NotificationCenter.Core.Domain.TemplateType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(1024)")
                        .HasMaxLength(1024);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("ReadableId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ReadableId")
                        .IsUnique();

                    b.ToTable("NotificationTemplateTypes");
                });

            modelBuilder.Entity("NotificationCenter.Core.Domain.TokenUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("TokenUsers");
                });

            modelBuilder.Entity("NotificationCenter.Core.Models.NotificationDataDetails", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DataType")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TemplateId");

                    b.ToTable("NotificationDataDetails");
                });

            modelBuilder.Entity("NotificationCenter.Core.Domain.ClientAndRoom", b =>
                {
                    b.HasOne("NotificationCenter.Core.Domain.Client", "Client")
                        .WithMany("ClientAndRooms")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NotificationCenter.Core.Domain.Room", "Room")
                        .WithMany("ClientAndRooms")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NotificationCenter.Core.Domain.Device", b =>
                {
                    b.HasOne("NotificationCenter.Core.Domain.App", "App")
                        .WithMany("Devices")
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NotificationCenter.Core.Domain.TokenUser", "TokenUser")
                        .WithMany("Devices")
                        .HasForeignKey("TokenUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NotificationCenter.Core.Domain.NotificationTemplate", b =>
                {
                    b.HasOne("NotificationCenter.Core.Domain.TemplateType", "TemplateType")
                        .WithMany("Templates")
                        .HasForeignKey("TemplateTypeId");
                });

            modelBuilder.Entity("NotificationCenter.Core.Domain.NotificationTemplateSendHistory", b =>
                {
                    b.HasOne("NotificationCenter.Core.Domain.NotificationTemplate", "NotificationTemplate")
                        .WithMany("History")
                        .HasForeignKey("NotificationTemplateId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("NotificationCenter.Core.Domain.TokenUser", "TokenUser")
                        .WithMany("NotificationTemplate")
                        .HasForeignKey("TokenUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("NotificationCenter.Core.Models.NotificationDataDetails", b =>
                {
                    b.HasOne("NotificationCenter.Core.Domain.TemplateType", "Template")
                        .WithMany("Data")
                        .HasForeignKey("TemplateId");
                });
#pragma warning restore 612, 618
        }
    }
}
using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NotificationCenter.Core.Migrations
{
    public partial class _111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apps",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AppName = table.Column<string>(nullable: false),
                    PackageNames = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    ConnectionId = table.Column<string>(nullable: true),
                    AppType = table.Column<int>(nullable: false),
                    IsConnected = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTemplateTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReadableId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTemplateTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderContactStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderContactStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(nullable: false),
                    RoomId = table.Column<int>(nullable: false),
                    MessageContent = table.Column<string>(nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: false),
                    AppType = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TokenUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationDataDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<Guid>(nullable: true),
                    Key = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DataType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationDataDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationDataDetails_NotificationTemplateTypes_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "NotificationTemplateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Body = table.Column<string>(nullable: false),
                    HighPriority = table.Column<bool>(nullable: false),
                    TemplateTypeId = table.Column<Guid>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTemplates_NotificationTemplateTypes_TemplateTypeId",
                        column: x => x.TemplateTypeId,
                        principalTable: "NotificationTemplateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientAndRoom",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(nullable: false),
                    RoomId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAndRoom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientAndRoom_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientAndRoom_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    FcmToken = table.Column<string>(nullable: true),
                    ApnsToken = table.Column<string>(nullable: true),
                    DeviceIdentifier = table.Column<string>(nullable: true),
                    AppId = table.Column<Guid>(nullable: false),
                    TokenUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Apps_AppId",
                        column: x => x.AppId,
                        principalTable: "Apps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Devices_TokenUsers_TokenUserId",
                        column: x => x.TokenUserId,
                        principalTable: "TokenUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTemplateHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TokenUserId = table.Column<Guid>(nullable: false),
                    NotificationTemplateId = table.Column<Guid>(nullable: false),
                    DateSent = table.Column<DateTime>(nullable: false),
                    DateRead = table.Column<DateTime>(nullable: true),
                    ReadMethod = table.Column<int>(nullable: false),
                    Ignored = table.Column<bool>(nullable: false),
                    DevicesCount = table.Column<int>(nullable: false),
                    DeveicesSentTo = table.Column<int>(nullable: false),
                    FailedDevices = table.Column<int>(nullable: false),
                    Payload = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTemplateHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTemplateHistory_NotificationTemplates_NotificationTemplateId",
                        column: x => x.NotificationTemplateId,
                        principalTable: "NotificationTemplates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotificationTemplateHistory_TokenUsers_TokenUserId",
                        column: x => x.TokenUserId,
                        principalTable: "TokenUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apps_AppName",
                table: "Apps",
                column: "AppName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientAndRoom_ClientId",
                table: "ClientAndRoom",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAndRoom_RoomId",
                table: "ClientAndRoom",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_AppId",
                table: "Devices",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_FcmToken",
                table: "Devices",
                column: "FcmToken",
                unique: true,
                filter: "[FcmToken] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_TokenUserId_DeviceIdentifier",
                table: "Devices",
                columns: new[] { "TokenUserId", "DeviceIdentifier" },
                unique: true,
                filter: "[DeviceIdentifier] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDataDetails_TemplateId",
                table: "NotificationDataDetails",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplateHistory_NotificationTemplateId",
                table: "NotificationTemplateHistory",
                column: "NotificationTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplateHistory_TokenUserId",
                table: "NotificationTemplateHistory",
                column: "TokenUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplates_TemplateTypeId",
                table: "NotificationTemplates",
                column: "TemplateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplateTypes_ReadableId",
                table: "NotificationTemplateTypes",
                column: "ReadableId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TokenUsers_UserId",
                table: "TokenUsers",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientAndRoom");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "NotificationDataDetails");

            migrationBuilder.DropTable(
                name: "NotificationTemplateHistory");

            migrationBuilder.DropTable(
                name: "OrderContactStatuses");

            migrationBuilder.DropTable(
                name: "RoomMessages");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Apps");

            migrationBuilder.DropTable(
                name: "NotificationTemplates");

            migrationBuilder.DropTable(
                name: "TokenUsers");

            migrationBuilder.DropTable(
                name: "NotificationTemplateTypes");
        }
    }
}

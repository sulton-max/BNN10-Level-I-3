using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LocalIdentity.SimpleInfra.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TemplateType = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "character varying(129536)", maxLength: 129536, nullable: false),
                    Subject = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    LastName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    EmailAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsEmailAddressVerified = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSignInDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: false),
                    UserAgent = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSignInDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    ExpiryTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessToken_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiverUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "character varying(129536)", maxLength: 129536, nullable: false),
                    IsSuccessful = table.Column<bool>(type: "boolean", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    SendEmailAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ReceiverEmailAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Subject = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationHistories_NotificationTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "NotificationTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationHistories_Users_ReceiverUserId",
                        column: x => x.ReceiverUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationHistories_Users_SenderUserId",
                        column: x => x.SenderUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PreferredNotificationType = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VerificationCode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodeType = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ExpiryTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    VerificationLink = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerificationCode_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedTime", "DeletedTime", "IsActive", "IsDeleted", "ModifiedTime", "Type" },
                values: new object[,]
                {
                    { new Guid("0327f1ba-81cf-478f-98d4-04fec56fc10a"), new DateTimeOffset(new DateTime(2023, 12, 15, 21, 41, 45, 950, DateTimeKind.Unspecified).AddTicks(8246), new TimeSpan(0, 5, 0, 0, 0)), null, true, false, null, 0 },
                    { new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1a"), new DateTimeOffset(new DateTime(2023, 12, 15, 21, 41, 45, 950, DateTimeKind.Unspecified).AddTicks(8278), new TimeSpan(0, 5, 0, 0, 0)), null, true, false, null, 1 },
                    { new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1b"), new DateTimeOffset(new DateTime(2023, 12, 15, 21, 41, 45, 950, DateTimeKind.Unspecified).AddTicks(8280), new TimeSpan(0, 5, 0, 0, 0)), null, true, false, null, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessToken_UserId",
                table: "AccessToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationHistories_ReceiverUserId",
                table: "NotificationHistories",
                column: "ReceiverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationHistories_SenderUserId",
                table: "NotificationHistories",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationHistories_TemplateId",
                table: "NotificationHistories",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTemplates_Type_TemplateType",
                table: "NotificationTemplates",
                columns: new[] { "Type", "TemplateType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailAddress",
                table: "Users",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VerificationCode_UserId",
                table: "VerificationCode",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessToken");

            migrationBuilder.DropTable(
                name: "NotificationHistories");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "UserSignInDetails");

            migrationBuilder.DropTable(
                name: "VerificationCode");

            migrationBuilder.DropTable(
                name: "NotificationTemplates");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

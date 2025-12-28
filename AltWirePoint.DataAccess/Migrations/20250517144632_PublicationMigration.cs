using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AltWirePoint.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PublicationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Publications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Image64 = table.Column<string>(type: "text", nullable: true),
                    PostedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publications_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Publications_Publications_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Publications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: true),
                    PublicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsLiked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Likes_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3f1d2e4c-5b6a-7d8e-9f01-2a3b4c5d6e7f"),
                column: "ConcurrencyStamp",
                value: "3b221c7f-27df-421e-a427-1cfb6c3e90e0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4a5b6c7d-8e9f-0a1b-2c3d-4e5f6a7b8c9d"),
                column: "ConcurrencyStamp",
                value: "8450a9a5-20dd-4179-89f1-e623cbdc493d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("9c8b8e2e-4f3a-4d2e-bf4a-e5c8a1b2c3d4"),
                columns: new[] { "ConcurrencyStamp", "Description", "Logo", "PasswordHash", "SecurityStamp" },
                values: new object[] { "64d74ac8-2217-4239-920b-9593a951368c", null, null, "AQAAAAIAAYagAAAAECH4FoX7OID/Xk9zkMje61IjFDuAUEjvvEOVM9Q8n2Ns5vudbfi9MR0KDGT5yX5BZw==", "58e20f04-5026-42a2-ba96-790cca88923f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                columns: new[] { "ConcurrencyStamp", "Description", "Logo", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2aa4dbb1-c001-4fbb-b2d4-2b1cdffaee1f", null, null, "AQAAAAIAAYagAAAAEOxNXdAYbZfiudufyeHyRiULdrfRj+pKneNp9rueQVwZl7/IE27N9PCdhxJRsFBTNw==", "19f440f4-6794-4c3a-8041-d12caeacdeb7" });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_AuthorId",
                table: "Likes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_PublicationId",
                table: "Likes",
                column: "PublicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_AuthorId",
                table: "Publications",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_ParentId",
                table: "Publications",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Publications");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3f1d2e4c-5b6a-7d8e-9f01-2a3b4c5d6e7f"),
                column: "ConcurrencyStamp",
                value: "82faf5bb-89bd-4481-9901-81b29afaa675");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4a5b6c7d-8e9f-0a1b-2c3d-4e5f6a7b8c9d"),
                column: "ConcurrencyStamp",
                value: "7b8f227d-f667-43f6-b7af-7446fb0c9901");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("9c8b8e2e-4f3a-4d2e-bf4a-e5c8a1b2c3d4"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "46d48247-545f-4b31-bbc6-00a256029a63", "AQAAAAIAAYagAAAAEHyN9vBYsq1g30GLH0160VnBLRjI2JOOfJfgdzqrf9yAew5hv6LvWz/KX+vjZlbkcw==", "1a6b9830-7a4e-4075-9dbc-d4349fc12a1a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1739e1fa-9953-40dc-ae10-65327a26873c", "AQAAAAIAAYagAAAAEMyYuUGpY6H6qIqX3kxhhFCfNd0IV89ne6bany9UGkcoO0rt4PbxQmSIuS2aD9o72w==", "17d90e4c-dcde-42ea-83e6-96cd7ade1432" });
        }
    }
}

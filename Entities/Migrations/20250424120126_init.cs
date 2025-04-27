using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "UserId" },
                values: new object[,]
                {
                    { new Guid("7b858e14-d92d-43e0-afe9-261365d067ad"), "User", null },
                    { new Guid("8dfc85ca-f780-43b1-b908-97ee9c90ef42"), "Admin", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "RoleId", "Username" },
                values: new object[,]
                {
                    { new Guid("3625e573-9f81-46a1-80f9-1100306169f5"), "$2a$11$vsxnzp0pm20gT4glVu3VFuhyqz.no6xi/jN7V6kTLFKt/0QUpJOAK", new Guid("7b858e14-d92d-43e0-afe9-261365d067ad"), "user2" },
                    { new Guid("44c091c5-be82-4b3f-a9e0-eb195d2e62af"), "$2a$11$oXueGR2jikCaEpwhVv.uyuPg6Nzmz4whMO6jtOEfvqhF6KUID2UC.", new Guid("7b858e14-d92d-43e0-afe9-261365d067ad"), "user1" },
                    { new Guid("59dfec42-4c48-407f-b9de-1ab16a845624"), "$2a$11$9WQBfP7xxgZtu/3wCCiAFeQdWGi9auZpl9Kj0zN9NiHK29PTB2NSS", new Guid("7b858e14-d92d-43e0-afe9-261365d067ad"), "user3" },
                    { new Guid("fac962ac-e397-47e2-996f-cc8e728a7f8f"), "$2a$11$8OPZ4Ufsaq0vhpxOTqjMp.6cvOUY7jIe3Vt4vFItIOf.t6RMuOY0q", new Guid("8dfc85ca-f780-43b1-b908-97ee9c90ef42"), "admin" }
                });

            migrationBuilder.InsertData(
                table: "TodoItems",
                columns: new[] { "Id", "CreationDate", "DeleteBy", "DeleteDate", "Description", "DueDate", "Title", "UserId" },
                values: new object[,]
                {
                    { new Guid("4f1790de-f460-409d-8c27-67089bcbed2d"), new DateTime(2025, 4, 24, 15, 1, 25, 912, DateTimeKind.Local).AddTicks(5762), null, null, "This is the second task.", new DateTime(2025, 4, 26, 15, 1, 25, 912, DateTimeKind.Local).AddTicks(5761), "Second Task", new Guid("3625e573-9f81-46a1-80f9-1100306169f5") },
                    { new Guid("db2f25b9-2149-4f75-aca0-f5baab2df9f4"), new DateTime(2025, 4, 24, 15, 1, 25, 912, DateTimeKind.Local).AddTicks(5759), null, null, "This is the first task.", new DateTime(2025, 4, 25, 15, 1, 25, 912, DateTimeKind.Local).AddTicks(5742), "First Task", new Guid("44c091c5-be82-4b3f-a9e0-eb195d2e62af") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserId",
                table: "Roles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_UserId",
                table: "TodoItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_UserId",
                table: "Roles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_UserId",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "TodoItems");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}

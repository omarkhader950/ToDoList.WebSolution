using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTodoItemAttachmentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TodoItemAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TodoItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItemAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoItemAttachments_TodoItems_TodoItemId",
                        column: x => x.TodoItemId,
                        principalTable: "TodoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: new Guid("4f1790de-f460-409d-8c27-67089bcbed2d"),
                columns: new[] { "CreationDate", "DueDate" },
                values: new object[] { new DateTime(2025, 6, 22, 11, 5, 26, 77, DateTimeKind.Local).AddTicks(1056), new DateTime(2025, 6, 24, 11, 5, 26, 77, DateTimeKind.Local).AddTicks(1056) });

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: new Guid("db2f25b9-2149-4f75-aca0-f5baab2df9f4"),
                columns: new[] { "CreationDate", "DueDate" },
                values: new object[] { new DateTime(2025, 6, 22, 11, 5, 26, 77, DateTimeKind.Local).AddTicks(1053), new DateTime(2025, 6, 23, 11, 5, 26, 77, DateTimeKind.Local).AddTicks(1024) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3625e573-9f81-46a1-80f9-1100306169f5"),
                column: "PasswordHash",
                value: "$2a$11$nFjMNdH.l5uxnyieviI5J.RALzFAveeOF3lmWJdX.d2U5EvrG2Wju");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44c091c5-be82-4b3f-a9e0-eb195d2e62af"),
                column: "PasswordHash",
                value: "$2a$11$/rvYqZleFCAEp2pS8IZeTOZYMZRbXGxubQhICO4ZirC/nobK1eZqq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("59dfec42-4c48-407f-b9de-1ab16a845624"),
                column: "PasswordHash",
                value: "$2a$11$yeDci3Oe0WMaEBhdQo5U6uMipi7pxaSgiN6KeoGSiOJMP5UA5IP8e");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fac962ac-e397-47e2-996f-cc8e728a7f8f"),
                column: "PasswordHash",
                value: "$2a$11$W1aw9rPyihMf6abxNQaS5./hDRTqf359qg.F3EcJmkXXbBMHBJOWu");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItemAttachments_TodoItemId",
                table: "TodoItemAttachments",
                column: "TodoItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoItemAttachments");

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: new Guid("4f1790de-f460-409d-8c27-67089bcbed2d"),
                columns: new[] { "CreationDate", "DueDate" },
                values: new object[] { new DateTime(2025, 5, 18, 11, 1, 43, 869, DateTimeKind.Local).AddTicks(4297), new DateTime(2025, 5, 20, 11, 1, 43, 869, DateTimeKind.Local).AddTicks(4297) });

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: new Guid("db2f25b9-2149-4f75-aca0-f5baab2df9f4"),
                columns: new[] { "CreationDate", "DueDate" },
                values: new object[] { new DateTime(2025, 5, 18, 11, 1, 43, 869, DateTimeKind.Local).AddTicks(4295), new DateTime(2025, 5, 19, 11, 1, 43, 869, DateTimeKind.Local).AddTicks(4279) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3625e573-9f81-46a1-80f9-1100306169f5"),
                column: "PasswordHash",
                value: "$2a$11$fkZcB1FxEqWVGrWM5IejDuS9gAcpVplFbgpN6qQqVKicS4YFSrsFW");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44c091c5-be82-4b3f-a9e0-eb195d2e62af"),
                column: "PasswordHash",
                value: "$2a$11$l.bvaiwhc0jzE..YLqNsoOw38Maw4MHg5rlh3Liw5PF.nxtvH/0aK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("59dfec42-4c48-407f-b9de-1ab16a845624"),
                column: "PasswordHash",
                value: "$2a$11$gBwNmLprAUibp2Gnt5Wm3.JRytcWr1UlcZfMIyjOjH7p3n6pLgzsa");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fac962ac-e397-47e2-996f-cc8e728a7f8f"),
                column: "PasswordHash",
                value: "$2a$11$f3Csq/RSrxKFDzpVbXiEGuon7jxKeNeJncNWHmtrFrfJFv.7aolpq");
        }
    }
}

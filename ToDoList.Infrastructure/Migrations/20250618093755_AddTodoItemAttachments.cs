using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTodoItemAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: new Guid("4f1790de-f460-409d-8c27-67089bcbed2d"),
                columns: new[] { "CreationDate", "DueDate" },
                values: new object[] { new DateTime(2025, 6, 18, 12, 37, 55, 87, DateTimeKind.Local).AddTicks(6276), new DateTime(2025, 6, 20, 12, 37, 55, 87, DateTimeKind.Local).AddTicks(6275) });

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: new Guid("db2f25b9-2149-4f75-aca0-f5baab2df9f4"),
                columns: new[] { "CreationDate", "DueDate" },
                values: new object[] { new DateTime(2025, 6, 18, 12, 37, 55, 87, DateTimeKind.Local).AddTicks(6273), new DateTime(2025, 6, 19, 12, 37, 55, 87, DateTimeKind.Local).AddTicks(6253) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3625e573-9f81-46a1-80f9-1100306169f5"),
                column: "PasswordHash",
                value: "$2a$11$mG1rLswIIlRWjbFyGz1zGuj/hDT5nxD0MsQhjYdwlUsDmgApyrDca");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44c091c5-be82-4b3f-a9e0-eb195d2e62af"),
                column: "PasswordHash",
                value: "$2a$11$tVXfFCgUj610QSCzgPQpPuYXttwhjb2Zl1CCDZQtffX3Oa/1zL6mC");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("59dfec42-4c48-407f-b9de-1ab16a845624"),
                column: "PasswordHash",
                value: "$2a$11$r2SBVFohGhkVo0tZH8KHo.VTEyAY7Ffyh.Unf5RnpgSfwp3JD72JS");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fac962ac-e397-47e2-996f-cc8e728a7f8f"),
                column: "PasswordHash",
                value: "$2a$11$virg5SgyAxzq3xUl7qH53Oxcod0QqeWrnDtH.D5E1KIwjC.s12kgO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: new Guid("4f1790de-f460-409d-8c27-67089bcbed2d"),
                columns: new[] { "CreationDate", "DueDate" },
                values: new object[] { new DateTime(2025, 6, 18, 12, 30, 1, 507, DateTimeKind.Local).AddTicks(4923), new DateTime(2025, 6, 20, 12, 30, 1, 507, DateTimeKind.Local).AddTicks(4923) });

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: new Guid("db2f25b9-2149-4f75-aca0-f5baab2df9f4"),
                columns: new[] { "CreationDate", "DueDate" },
                values: new object[] { new DateTime(2025, 6, 18, 12, 30, 1, 507, DateTimeKind.Local).AddTicks(4920), new DateTime(2025, 6, 19, 12, 30, 1, 507, DateTimeKind.Local).AddTicks(4904) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3625e573-9f81-46a1-80f9-1100306169f5"),
                column: "PasswordHash",
                value: "$2a$11$MubmG/eHoZE27/Kj7UtoTu0K6TTxMwuXk1jNNB/KxRxtwMMEoFQT6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44c091c5-be82-4b3f-a9e0-eb195d2e62af"),
                column: "PasswordHash",
                value: "$2a$11$ZykMJMIc11vqn/f/JA7PeeC1lR9HRQ7eK86d2wDXoKej2xAvmm9dm");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("59dfec42-4c48-407f-b9de-1ab16a845624"),
                column: "PasswordHash",
                value: "$2a$11$.tnLz1ZLNrfpf6I90BXQ2.9dpcy7uJO1i.zJtbbkF8OZ0UEo.t.0u");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fac962ac-e397-47e2-996f-cc8e728a7f8f"),
                column: "PasswordHash",
                value: "$2a$11$f4O108K1Vp60dEghURMfGuzsD0JmCn3fyHTlFjwh.2p8m4Vmw/Wmu");
        }
    }
}

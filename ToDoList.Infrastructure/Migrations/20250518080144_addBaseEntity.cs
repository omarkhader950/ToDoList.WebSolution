using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: new Guid("4f1790de-f460-409d-8c27-67089bcbed2d"),
                columns: new[] { "CreationDate", "DueDate" },
                values: new object[] { new DateTime(2025, 5, 7, 12, 36, 18, 555, DateTimeKind.Local).AddTicks(4329), new DateTime(2025, 5, 9, 12, 36, 18, 555, DateTimeKind.Local).AddTicks(4328) });

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: new Guid("db2f25b9-2149-4f75-aca0-f5baab2df9f4"),
                columns: new[] { "CreationDate", "DueDate" },
                values: new object[] { new DateTime(2025, 5, 7, 12, 36, 18, 555, DateTimeKind.Local).AddTicks(4326), new DateTime(2025, 5, 8, 12, 36, 18, 555, DateTimeKind.Local).AddTicks(4310) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3625e573-9f81-46a1-80f9-1100306169f5"),
                column: "PasswordHash",
                value: "$2a$11$uQbMqNeOncdastLebJrlQuDFdZZOWv0v4Z.wpOlC6odDmge6QoeUm");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44c091c5-be82-4b3f-a9e0-eb195d2e62af"),
                column: "PasswordHash",
                value: "$2a$11$Tp46F5EW4WSCFNa7C1VJMeF44R8pAQLZQyNFSc6UlpWXKqCKHdRTK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("59dfec42-4c48-407f-b9de-1ab16a845624"),
                column: "PasswordHash",
                value: "$2a$11$fivZEvz8irFri9tJx5NQ6.VmtXSvDXwsWddwc3NXc00.DcXj27f32");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fac962ac-e397-47e2-996f-cc8e728a7f8f"),
                column: "PasswordHash",
                value: "$2a$11$4u1O8ZdyuphJn.7VPkaThe0zhK5u0COOYfPf4AicBXVvk3WBBXY9.");
        }
    }
}

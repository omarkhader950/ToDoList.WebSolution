using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_enumstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TodoItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: new Guid("4f1790de-f460-409d-8c27-67089bcbed2d"),
                columns: new[] { "CreationDate", "DueDate", "Status" },
                values: new object[] { new DateTime(2025, 5, 7, 12, 36, 18, 555, DateTimeKind.Local).AddTicks(4329), new DateTime(2025, 5, 9, 12, 36, 18, 555, DateTimeKind.Local).AddTicks(4328), 0 });

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: new Guid("db2f25b9-2149-4f75-aca0-f5baab2df9f4"),
                columns: new[] { "CreationDate", "DueDate", "Status" },
                values: new object[] { new DateTime(2025, 5, 7, 12, 36, 18, 555, DateTimeKind.Local).AddTicks(4326), new DateTime(2025, 5, 8, 12, 36, 18, 555, DateTimeKind.Local).AddTicks(4310), 0 });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "TodoItems");

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: new Guid("4f1790de-f460-409d-8c27-67089bcbed2d"),
                columns: new[] { "CreationDate", "DueDate" },
                values: new object[] { new DateTime(2025, 4, 29, 9, 28, 25, 309, DateTimeKind.Local).AddTicks(8235), new DateTime(2025, 5, 1, 9, 28, 25, 309, DateTimeKind.Local).AddTicks(8235) });

            migrationBuilder.UpdateData(
                table: "TodoItems",
                keyColumn: "Id",
                keyValue: new Guid("db2f25b9-2149-4f75-aca0-f5baab2df9f4"),
                columns: new[] { "CreationDate", "DueDate" },
                values: new object[] { new DateTime(2025, 4, 29, 9, 28, 25, 309, DateTimeKind.Local).AddTicks(8231), new DateTime(2025, 4, 30, 9, 28, 25, 309, DateTimeKind.Local).AddTicks(8208) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3625e573-9f81-46a1-80f9-1100306169f5"),
                column: "PasswordHash",
                value: "$2a$11$x9csDjmpRRFL66d0Gg1ll.ffOh73a67U.XWJdtRHkdEJVUZp5rWFi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44c091c5-be82-4b3f-a9e0-eb195d2e62af"),
                column: "PasswordHash",
                value: "$2a$11$i/oRxjN3TGyQlLhuyxKA1OB9kpGS0tCguIv4GobDWhO5mMy9jpgEK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("59dfec42-4c48-407f-b9de-1ab16a845624"),
                column: "PasswordHash",
                value: "$2a$11$6p714PXtRFvX7DB6/1C6V.v4h4cBr148BBZ/ct6rsXgHKxDzD12me");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("fac962ac-e397-47e2-996f-cc8e728a7f8f"),
                column: "PasswordHash",
                value: "$2a$11$tEM7JCl7lGj12yaXj7yMxOhmAGNYrARp08MmBgxQzygkmWG94e18O");
        }
    }
}

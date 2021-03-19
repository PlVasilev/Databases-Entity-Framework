using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFRelations.Infrastructure.Migrations
{
    public partial class test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 1,
                column: "RegisteredOn",
                value: new DateTime(2019, 3, 15, 20, 58, 48, 532, DateTimeKind.Local).AddTicks(7839));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 2,
                column: "RegisteredOn",
                value: new DateTime(2019, 3, 15, 20, 58, 48, 537, DateTimeKind.Local).AddTicks(842));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 3,
                column: "RegisteredOn",
                value: new DateTime(2019, 3, 15, 20, 58, 48, 537, DateTimeKind.Local).AddTicks(881));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 1,
                column: "RegisteredOn",
                value: new DateTime(2019, 3, 15, 20, 27, 33, 152, DateTimeKind.Local).AddTicks(1877));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 2,
                column: "RegisteredOn",
                value: new DateTime(2019, 3, 15, 20, 27, 33, 157, DateTimeKind.Local).AddTicks(3387));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 3,
                column: "RegisteredOn",
                value: new DateTime(2019, 3, 15, 20, 27, 33, 157, DateTimeKind.Local).AddTicks(3410));
        }
    }
}

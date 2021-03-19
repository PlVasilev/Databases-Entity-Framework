using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFRelations.Infrastructure.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    RegisteredOn = table.Column<DateTime>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "Birthday", "PhoneNumber", "RegisteredOn", "FirstName", "LastName" },
                values: new object[] { 1, new DateTime(1989, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "76876876868", new DateTime(2019, 3, 15, 20, 27, 33, 152, DateTimeKind.Local).AddTicks(1877), "Petar", "Ivanov" });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "Birthday", "PhoneNumber", "RegisteredOn", "FirstName", "LastName" },
                values: new object[] { 2, new DateTime(1989, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "76876876868", new DateTime(2019, 3, 15, 20, 27, 33, 157, DateTimeKind.Local).AddTicks(3387), "Ivan", "Ivanov" });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "Birthday", "PhoneNumber", "RegisteredOn", "FirstName", "LastName" },
                values: new object[] { 3, new DateTime(1989, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "76876876868", new DateTime(2019, 3, 15, 20, 27, 33, 157, DateTimeKind.Local).AddTicks(3410), "Ivan", "Petrov" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SMS.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Courses = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CourseCode", "CourseName", "EndDate", "StartDate", "TeacherName" },
                values: new object[,]
                {
                    { -5, "332", "Urdu", new DateTime(2023, 10, 7, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2024, 3, 7, 0, 0, 0, 0, DateTimeKind.Local), "Abdul" },
                    { -4, "322", "English", new DateTime(2023, 10, 7, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Local), "Jauna Halls" },
                    { -3, "125", "Chemistry", new DateTime(2023, 10, 7, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2024, 2, 7, 0, 0, 0, 0, DateTimeKind.Local), "Jhon Smith" },
                    { -2, "362", "Physics", new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2024, 4, 7, 0, 0, 0, 0, DateTimeKind.Local), "Jake Mathews" },
                    { -1, "112", "Maths", new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2024, 3, 7, 0, 0, 0, 0, DateTimeKind.Local), "Jhon Smith" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address1", "Address2", "Address3", "Courses", "DOB", "FirstName", "Gender", "SurName" },
                values: new object[,]
                {
                    { -3, "i10/2, Islamabad", "e11/2, Islamabad", "e11/3, Islamabad", "1", new DateTime(2034, 9, 21, 0, 0, 0, 0, DateTimeKind.Local), "Lisa", "Male", "Landry" },
                    { -2, "f10/2, Islamabad", "f11/2, Islamabad", "f11/3, Islamabad", "1,3", new DateTime(2054, 8, 19, 0, 0, 0, 0, DateTimeKind.Local), "Jhon", "Male", "Snow" },
                    { -1, "i10/2, Islamabad", "i11/2, Islamabad", "i11/3, Islamabad", "1,2,3", new DateTime(2049, 9, 21, 0, 0, 0, 0, DateTimeKind.Local), "Jake", "Male", "Brown" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}

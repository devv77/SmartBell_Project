using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBell.Migrations
{
    public partial class holidayextend1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BellRings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BellRingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Interval = table.Column<TimeSpan>(type: "time", nullable: false),
                    IntervalSeconds = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BellRings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutputPaths",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SequenceID = table.Column<int>(type: "int", nullable: false),
                    BellRingId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputPaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutputPaths_BellRings_BellRingId",
                        column: x => x.BellRingId,
                        principalTable: "BellRings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateElements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BellRingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Interval = table.Column<TimeSpan>(type: "time", nullable: false),
                    IntervalSeconds = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TemplateId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateElements_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Templates",
                columns: new[] { "Id", "Name" },
                values: new object[] { "1", "Tízperces csengetési rend" });

            migrationBuilder.InsertData(
                table: "Templates",
                columns: new[] { "Id", "Name" },
                values: new object[] { "2", "Tizenöt perces csengetési rend" });

            migrationBuilder.InsertData(
                table: "Templates",
                columns: new[] { "Id", "Name" },
                values: new object[] { "3", "Rövidített órák" });

            migrationBuilder.InsertData(
                table: "TemplateElements",
                columns: new[] { "Id", "BellRingTime", "FilePath", "Interval", "IntervalSeconds", "TemplateId", "Type" },
                values: new object[,]
                {
                    { "1", new DateTime(1, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 0 },
                    { "27", new DateTime(1, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 0 },
                    { "28", new DateTime(1, 1, 1, 12, 45, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 1 },
                    { "29", new DateTime(1, 1, 1, 13, 0, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 0 },
                    { "30", new DateTime(1, 1, 1, 13, 45, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 1 },
                    { "31", new DateTime(1, 1, 1, 13, 55, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 0 },
                    { "32", new DateTime(1, 1, 1, 14, 40, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 1 },
                    { "33", new DateTime(1, 1, 1, 14, 50, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 0 },
                    { "34", new DateTime(1, 1, 1, 15, 35, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 1 },
                    { "35", new DateTime(1, 1, 1, 15, 40, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 0 },
                    { "26", new DateTime(1, 1, 1, 11, 45, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 1 },
                    { "36", new DateTime(1, 1, 1, 16, 25, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 1 },
                    { "38", new DateTime(1, 1, 1, 8, 35, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "3", 1 },
                    { "39", new DateTime(1, 1, 1, 8, 45, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "3", 0 },
                    { "40", new DateTime(1, 1, 1, 9, 20, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "3", 1 },
                    { "41", new DateTime(1, 1, 1, 9, 25, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "3", 0 },
                    { "42", new DateTime(1, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "3", 1 },
                    { "43", new DateTime(1, 1, 1, 10, 5, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "3", 0 },
                    { "44", new DateTime(1, 1, 1, 10, 40, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "3", 1 },
                    { "45", new DateTime(1, 1, 1, 10, 45, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "3", 0 },
                    { "46", new DateTime(1, 1, 1, 11, 20, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "3", 1 },
                    { "37", new DateTime(1, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "3", 0 },
                    { "25", new DateTime(1, 1, 1, 11, 0, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 0 },
                    { "24", new DateTime(1, 1, 1, 10, 45, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 1 },
                    { "23", new DateTime(1, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 0 },
                    { "2", new DateTime(1, 1, 1, 8, 45, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 1 },
                    { "3", new DateTime(1, 1, 1, 8, 55, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 0 },
                    { "4", new DateTime(1, 1, 1, 9, 40, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 1 },
                    { "5", new DateTime(1, 1, 1, 9, 55, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 0 },
                    { "6", new DateTime(1, 1, 1, 10, 40, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 1 },
                    { "7", new DateTime(1, 1, 1, 10, 50, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 0 },
                    { "8", new DateTime(1, 1, 1, 11, 35, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 1 },
                    { "9", new DateTime(1, 1, 1, 11, 55, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 0 },
                    { "10", new DateTime(1, 1, 1, 12, 40, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 1 },
                    { "11", new DateTime(1, 1, 1, 12, 50, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 0 },
                    { "12", new DateTime(1, 1, 1, 13, 35, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 1 },
                    { "13", new DateTime(1, 1, 1, 13, 40, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 0 },
                    { "14", new DateTime(1, 1, 1, 14, 25, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 1 },
                    { "15", new DateTime(1, 1, 1, 14, 35, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 0 },
                    { "16", new DateTime(1, 1, 1, 15, 20, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 1 },
                    { "17", new DateTime(1, 1, 1, 15, 25, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 0 },
                    { "18", new DateTime(1, 1, 1, 16, 10, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "1", 1 }
                });

            migrationBuilder.InsertData(
                table: "TemplateElements",
                columns: new[] { "Id", "BellRingTime", "FilePath", "Interval", "IntervalSeconds", "TemplateId", "Type" },
                values: new object[,]
                {
                    { "19", new DateTime(1, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 0 },
                    { "20", new DateTime(1, 1, 1, 8, 45, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 1 },
                    { "21", new DateTime(1, 1, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 0 },
                    { "22", new DateTime(1, 1, 1, 9, 45, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "2", 1 },
                    { "47", new DateTime(1, 1, 1, 11, 25, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "3", 0 },
                    { "48", new DateTime(1, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "default.mp3", new TimeSpan(0, 0, 0, 10, 0), 10, "3", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutputPaths_BellRingId",
                table: "OutputPaths",
                column: "BellRingId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateElements_TemplateId",
                table: "TemplateElements",
                column: "TemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "OutputPaths");

            migrationBuilder.DropTable(
                name: "TemplateElements");

            migrationBuilder.DropTable(
                name: "BellRings");

            migrationBuilder.DropTable(
                name: "Templates");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "JobId", "PasswordHash" },
                values: new object[] { "9b5dec73-4197-4e40-9c36-776a081e3af8", 1, "AQAAAAIAAYagAAAAECmlPElairJHlJ1USrKU12F0BmrjV2EqZj76hmjBKLnR/twzWJ04KCxXRA3UiXlUCA==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2025, 4, 8, 10, 5, 29, 387, DateTimeKind.Local).AddTicks(3071));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2025, 4, 8, 10, 5, 29, 387, DateTimeKind.Local).AddTicks(3076));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2025, 4, 8, 10, 5, 29, 387, DateTimeKind.Local).AddTicks(3079));

            migrationBuilder.InsertData(
                table: "Job",
                columns: new[] { "Id", "CreatedTime", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 8, 8, 5, 29, 387, DateTimeKind.Utc).AddTicks(3512), "", "ادارة" },
                    { 2, new DateTime(2025, 4, 8, 8, 5, 29, 387, DateTimeKind.Utc).AddTicks(3515), "", "برمجة" },
                    { 3, new DateTime(2025, 4, 8, 8, 5, 29, 387, DateTimeKind.Utc).AddTicks(3518), "", "تركيبات" },
                    { 4, new DateTime(2025, 4, 8, 8, 5, 29, 387, DateTimeKind.Utc).AddTicks(3520), "", "تسويق" },
                    { 5, new DateTime(2025, 4, 8, 8, 5, 29, 387, DateTimeKind.Utc).AddTicks(3522), "", "مبيعات" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_JobId",
                table: "AspNetUsers",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Job_JobId",
                table: "AspNetUsers",
                column: "JobId",
                principalTable: "Job",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Job_JobId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_JobId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "040c0942-54e2-4432-817d-5d70f2a6200e", "AQAAAAIAAYagAAAAEAM5K/0NuEm3RAgzFR60J135KPAOoHM6PK/XsDRXf1PUFMPkIGxdi9/jmWNaK8utHA==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2025, 4, 7, 16, 14, 41, 661, DateTimeKind.Local).AddTicks(1128));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2025, 4, 7, 16, 14, 41, 661, DateTimeKind.Local).AddTicks(1132));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2025, 4, 7, 16, 14, 41, 661, DateTimeKind.Local).AddTicks(1135));
        }
    }
}

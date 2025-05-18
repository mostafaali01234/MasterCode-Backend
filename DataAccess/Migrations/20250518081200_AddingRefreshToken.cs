using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loan_AspNetUsers_EmpId",
                table: "Loan");

            migrationBuilder.AlterColumn<string>(
                name: "EmpId",
                table: "Loan",
                type: "nvarchar(450)",
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ce38d37d-d789-4ee0-9a86-fc012bf3bdf9", "AQAAAAIAAYagAAAAEOTfVJSIci0iwNsjDEiJbAsuIs9o3e+GT9pul4022IzrrfaXvdAScVbVHPzI+8pkyA==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 18, 11, 11, 59, 95, DateTimeKind.Local).AddTicks(4285));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 18, 11, 11, 59, 95, DateTimeKind.Local).AddTicks(4289));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 18, 11, 11, 59, 95, DateTimeKind.Local).AddTicks(4291));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 18, 8, 11, 59, 95, DateTimeKind.Utc).AddTicks(4542));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 18, 8, 11, 59, 95, DateTimeKind.Utc).AddTicks(4545));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 18, 8, 11, 59, 95, DateTimeKind.Utc).AddTicks(4547));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 18, 8, 11, 59, 95, DateTimeKind.Utc).AddTicks(4549));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 18, 8, 11, 59, 95, DateTimeKind.Utc).AddTicks(4550));

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_AspNetUsers_EmpId",
                table: "Loan",
                column: "EmpId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loan_AspNetUsers_EmpId",
                table: "Loan");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.AlterColumn<string>(
                name: "EmpId",
                table: "Loan",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "93d67422-ab23-49a9-8469-289edb5726ed", "AQAAAAIAAYagAAAAEPvkER8G34jzObWs404uMFhGsoIgLhKVjW6fFA9llxVrOC/a9cX2Qjyp/LkZtK+W2Q==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 19, 25, 43, 829, DateTimeKind.Local).AddTicks(7965));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 19, 25, 43, 829, DateTimeKind.Local).AddTicks(7969));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 19, 25, 43, 829, DateTimeKind.Local).AddTicks(7972));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 16, 25, 43, 829, DateTimeKind.Utc).AddTicks(8182));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 16, 25, 43, 829, DateTimeKind.Utc).AddTicks(8184));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 16, 25, 43, 829, DateTimeKind.Utc).AddTicks(8186));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 16, 25, 43, 829, DateTimeKind.Utc).AddTicks(8188));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 16, 25, 43, 829, DateTimeKind.Utc).AddTicks(8190));

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_AspNetUsers_EmpId",
                table: "Loan",
                column: "EmpId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

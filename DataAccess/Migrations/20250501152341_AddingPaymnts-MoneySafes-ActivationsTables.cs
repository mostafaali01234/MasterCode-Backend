using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingPaymntsMoneySafesActivationsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MoneySafes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpeningBalance = table.Column<double>(type: "float", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneySafes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoneySafes_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderActivations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderHeaderId = table.Column<int>(type: "int", nullable: true),
                    DeviceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivationCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderActivations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderActivations_OrderHeaders_OrderHeaderId",
                        column: x => x.OrderHeaderId,
                        principalTable: "OrderHeaders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderHeaderId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MoneySafeId = table.Column<int>(type: "int", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPayments_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerPayments_MoneySafes_MoneySafeId",
                        column: x => x.MoneySafeId,
                        principalTable: "MoneySafes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerPayments_OrderHeaders_OrderHeaderId",
                        column: x => x.OrderHeaderId,
                        principalTable: "OrderHeaders",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "882b03a1-271c-4260-8aa5-36677eff5f06", "AQAAAAIAAYagAAAAEKtnoIKi42PnrgVFhWyFXMIGNOEengeh0BJu5HH6LdVFNOKJK7hBbYA4JqpyOxJj0Q==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 18, 23, 39, 658, DateTimeKind.Local).AddTicks(9905));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 18, 23, 39, 658, DateTimeKind.Local).AddTicks(9909));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 18, 23, 39, 658, DateTimeKind.Local).AddTicks(9912));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 15, 23, 39, 659, DateTimeKind.Utc).AddTicks(193));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 15, 23, 39, 659, DateTimeKind.Utc).AddTicks(196));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 15, 23, 39, 659, DateTimeKind.Utc).AddTicks(198));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 15, 23, 39, 659, DateTimeKind.Utc).AddTicks(199));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedTime",
                value: new DateTime(2025, 5, 1, 15, 23, 39, 659, DateTimeKind.Utc).AddTicks(201));

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPayments_ApplicationUserId",
                table: "CustomerPayments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPayments_MoneySafeId",
                table: "CustomerPayments",
                column: "MoneySafeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPayments_OrderHeaderId",
                table: "CustomerPayments",
                column: "OrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneySafes_ApplicationUserId",
                table: "MoneySafes",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderActivations_OrderHeaderId",
                table: "OrderActivations",
                column: "OrderHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPayments");

            migrationBuilder.DropTable(
                name: "OrderActivations");

            migrationBuilder.DropTable(
                name: "MoneySafes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f346390f-6173-45fc-93b2-9edca4048a9d", "AQAAAAIAAYagAAAAEMssaE6Mh18Hk6VGSfSug1bGYD+LWKGvl7rBfTZSP7Vw1NzIvW83HrOufiBGZr/mNg==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2025, 4, 30, 16, 20, 11, 118, DateTimeKind.Local).AddTicks(6278));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2025, 4, 30, 16, 20, 11, 118, DateTimeKind.Local).AddTicks(6282));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2025, 4, 30, 16, 20, 11, 118, DateTimeKind.Local).AddTicks(6285));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2025, 4, 30, 13, 20, 11, 118, DateTimeKind.Utc).AddTicks(6513));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2025, 4, 30, 13, 20, 11, 118, DateTimeKind.Utc).AddTicks(6515));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2025, 4, 30, 13, 20, 11, 118, DateTimeKind.Utc).AddTicks(6517));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedTime",
                value: new DateTime(2025, 4, 30, 13, 20, 11, 118, DateTimeKind.Utc).AddTicks(6519));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedTime",
                value: new DateTime(2025, 4, 30, 13, 20, 11, 118, DateTimeKind.Utc).AddTicks(6521));
        }
    }
}

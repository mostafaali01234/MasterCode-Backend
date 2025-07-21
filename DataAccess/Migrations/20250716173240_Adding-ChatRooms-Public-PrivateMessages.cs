using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingChatRoomsPublicPrivateMessages : Migration
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
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "ChatRoom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoom", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrivateChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Seen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateChatMessages_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrivateChatMessages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PublicChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicChatMessages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PublicChatMessages_ChatRoom_RoomId",
                        column: x => x.RoomId,
                        principalTable: "ChatRoom",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2ff46ae0-c4e3-4515-85e2-3316c7443549", "AQAAAAIAAYagAAAAEKpSsOvWP/Tm2n2HSXpRi57TXLXPF6jsEnUC7I2F96ji5bRDmehXMtkoiiVrl6/NYg==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2025, 7, 16, 20, 32, 39, 216, DateTimeKind.Local).AddTicks(3297));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2025, 7, 16, 20, 32, 39, 216, DateTimeKind.Local).AddTicks(3301));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2025, 7, 16, 20, 32, 39, 216, DateTimeKind.Local).AddTicks(3304));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedTime",
                value: new DateTime(2025, 7, 16, 17, 32, 39, 216, DateTimeKind.Utc).AddTicks(3528));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedTime",
                value: new DateTime(2025, 7, 16, 17, 32, 39, 216, DateTimeKind.Utc).AddTicks(3530));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedTime",
                value: new DateTime(2025, 7, 16, 17, 32, 39, 216, DateTimeKind.Utc).AddTicks(3532));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedTime",
                value: new DateTime(2025, 7, 16, 17, 32, 39, 216, DateTimeKind.Utc).AddTicks(3534));

            migrationBuilder.UpdateData(
                table: "Job",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedTime",
                value: new DateTime(2025, 7, 16, 17, 32, 39, 216, DateTimeKind.Utc).AddTicks(3536));

            migrationBuilder.CreateIndex(
                name: "IX_PrivateChatMessages_ReceiverId",
                table: "PrivateChatMessages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateChatMessages_SenderId",
                table: "PrivateChatMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicChatMessages_RoomId",
                table: "PublicChatMessages",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicChatMessages_SenderId",
                table: "PublicChatMessages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_AspNetUsers_EmpId",
                table: "Loan",
                column: "EmpId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loan_AspNetUsers_EmpId",
                table: "Loan");

            migrationBuilder.DropTable(
                name: "PrivateChatMessages");

            migrationBuilder.DropTable(
                name: "PublicChatMessages");

            migrationBuilder.DropTable(
                name: "ChatRoom");

            migrationBuilder.AlterColumn<string>(
                name: "EmpId",
                table: "Loan",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_AspNetUsers_EmpId",
                table: "Loan",
                column: "EmpId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

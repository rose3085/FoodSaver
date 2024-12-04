using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PostLimit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "FoodModel",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "SellerPostLimitId",
                table: "FoodModel",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CanPost",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SellerPostLimit",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalPreviousAmount = table.Column<double>(type: "float", nullable: false),
                    DailyLimitReached = table.Column<bool>(type: "bit", nullable: false),
                    CommissionPaid = table.Column<bool>(type: "bit", nullable: false),
                    NewAmount = table.Column<double>(type: "float", nullable: false),
                    SellerIdId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerPostLimit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerPostLimit_AspNetUsers_SellerIdId",
                        column: x => x.SellerIdId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodModel_SellerPostLimitId",
                table: "FoodModel",
                column: "SellerPostLimitId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerPostLimit_SellerIdId",
                table: "SellerPostLimit",
                column: "SellerIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodModel_SellerPostLimit_SellerPostLimitId",
                table: "FoodModel",
                column: "SellerPostLimitId",
                principalTable: "SellerPostLimit",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodModel_SellerPostLimit_SellerPostLimitId",
                table: "FoodModel");

            migrationBuilder.DropTable(
                name: "SellerPostLimit");

            migrationBuilder.DropIndex(
                name: "IX_FoodModel_SellerPostLimitId",
                table: "FoodModel");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "FoodModel");

            migrationBuilder.DropColumn(
                name: "SellerPostLimitId",
                table: "FoodModel");

            migrationBuilder.DropColumn(
                name: "CanPost",
                table: "AspNetUsers");
        }
    }
}

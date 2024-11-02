using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addressMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodModel_AspNetUsers_UsersId",
                table: "FoodModel");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "FoodModel",
                newName: "SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_FoodModel_UsersId",
                table: "FoodModel",
                newName: "IX_FoodModel_SellerId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodModel_AspNetUsers_SellerId",
                table: "FoodModel",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodModel_AspNetUsers_SellerId",
                table: "FoodModel");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "FoodModel",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_FoodModel_SellerId",
                table: "FoodModel",
                newName: "IX_FoodModel_UsersId");

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodModel_AspNetUsers_UsersId",
                table: "FoodModel",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

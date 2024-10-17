using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderModelUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_BuyerIdId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_FoodModel_FoodIdId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "FoodIdId",
                table: "Orders",
                newName: "FoodId");

            migrationBuilder.RenameColumn(
                name: "BuyerIdId",
                table: "Orders",
                newName: "BuyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_FoodIdId",
                table: "Orders",
                newName: "IX_Orders_FoodId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_BuyerIdId",
                table: "Orders",
                newName: "IX_Orders_BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_BuyerId",
                table: "Orders",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_FoodModel_FoodId",
                table: "Orders",
                column: "FoodId",
                principalTable: "FoodModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_BuyerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_FoodModel_FoodId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "FoodId",
                table: "Orders",
                newName: "FoodIdId");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "Orders",
                newName: "BuyerIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_FoodId",
                table: "Orders",
                newName: "IX_Orders_FoodIdId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_BuyerId",
                table: "Orders",
                newName: "IX_Orders_BuyerIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_BuyerIdId",
                table: "Orders",
                column: "BuyerIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_FoodModel_FoodIdId",
                table: "Orders",
                column: "FoodIdId",
                principalTable: "FoodModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

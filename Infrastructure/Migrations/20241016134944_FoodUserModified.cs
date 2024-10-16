using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FoodUserModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "applicationUserFoodModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FoodsId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodModelId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicationUserFoodModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_applicationUserFoodModels_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_applicationUserFoodModels_FoodModel_FoodModelId",
                        column: x => x.FoodModelId,
                        principalTable: "FoodModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_applicationUserFoodModels_ApplicationUserId",
                table: "applicationUserFoodModels",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_applicationUserFoodModels_FoodModelId",
                table: "applicationUserFoodModels",
                column: "FoodModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applicationUserFoodModels");
        }
    }
}

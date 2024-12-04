using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class salesRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "SellerPostLimitId",
                table: "FoodModel");

            migrationBuilder.CreateTable(
                name: "SalesRecord",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalPreviousAmount = table.Column<double>(type: "float", nullable: false),
                    DailyLimitReached = table.Column<bool>(type: "bit", nullable: false),
                    CommissionPaid = table.Column<bool>(type: "bit", nullable: false),
                    NewAmount = table.Column<double>(type: "float", nullable: false),
                    SellerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesRecord_AspNetUsers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesRecord_SellerId",
                table: "SalesRecord",
                column: "SellerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesRecord");

            migrationBuilder.AddColumn<string>(
                name: "SellerPostLimitId",
                table: "FoodModel",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SellerPostLimit",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellerIdId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CommissionPaid = table.Column<bool>(type: "bit", nullable: false),
                    DailyLimitReached = table.Column<bool>(type: "bit", nullable: false),
                    NewAmount = table.Column<double>(type: "float", nullable: false),
                    TotalPreviousAmount = table.Column<double>(type: "float", nullable: false)
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
    }
}

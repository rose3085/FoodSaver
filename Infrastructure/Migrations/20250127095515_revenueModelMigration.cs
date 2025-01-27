using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class revenueModelMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RevenueModelId",
                table: "SalesRecord",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SellerRevenue",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PidX = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmountPaid = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerRevenue", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesRecord_RevenueModelId",
                table: "SalesRecord",
                column: "RevenueModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesRecord_SellerRevenue_RevenueModelId",
                table: "SalesRecord",
                column: "RevenueModelId",
                principalTable: "SellerRevenue",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesRecord_SellerRevenue_RevenueModelId",
                table: "SalesRecord");

            migrationBuilder.DropTable(
                name: "SellerRevenue");

            migrationBuilder.DropIndex(
                name: "IX_SalesRecord_RevenueModelId",
                table: "SalesRecord");

            migrationBuilder.DropColumn(
                name: "RevenueModelId",
                table: "SalesRecord");
        }
    }
}

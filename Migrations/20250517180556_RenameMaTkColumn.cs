using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechShop.Migrations
{
    /// <inheritdoc />
    public partial class RenameMaTkColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "MaTK",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MaTK",
                table: "Orders",
                column: "MaTK");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Accounts_MaTK",
                table: "Orders",
                column: "MaTK",
                principalTable: "Accounts",
                principalColumn: "MaTK",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Accounts_MaTK",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_MaTK",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MaTK",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

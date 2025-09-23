using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechShop.Migrations
{
    /// <inheritdoc />
    public partial class AddHinhAnhSanPhamRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HinhAnhSanPhams",
                table: "HinhAnhSanPhams");

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTao",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SoLuongBan",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "MaHh",
                table: "HinhAnhSanPhams",
                type: "nvarchar(10)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DuongDan",
                table: "HinhAnhSanPhams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__HinhAnhSanPham__3214EC07A8B5A3C3",
                table: "HinhAnhSanPhams",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhSanPhams_MaHh",
                table: "HinhAnhSanPhams",
                column: "MaHh");

            migrationBuilder.AddForeignKey(
                name: "FK_HinhAnhSanPham_Hanghoa",
                table: "HinhAnhSanPhams",
                column: "MaHh",
                principalTable: "Hanghoa",
                principalColumn: "MaHH",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HinhAnhSanPham_Hanghoa",
                table: "HinhAnhSanPhams");

            migrationBuilder.DropPrimaryKey(
                name: "PK__HinhAnhSanPham__3214EC07A8B5A3C3",
                table: "HinhAnhSanPhams");

            migrationBuilder.DropIndex(
                name: "IX_HinhAnhSanPhams_MaHh",
                table: "HinhAnhSanPhams");

            migrationBuilder.DropColumn(
                name: "NgayTao",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SoLuongBan",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "MaHh",
                table: "HinhAnhSanPhams",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)");

            migrationBuilder.AlterColumn<string>(
                name: "DuongDan",
                table: "HinhAnhSanPhams",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HinhAnhSanPhams",
                table: "HinhAnhSanPhams",
                column: "Id");
        }
    }
}

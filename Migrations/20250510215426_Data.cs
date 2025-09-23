using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechShop.Migrations
{
    /// <inheritdoc />
    public partial class Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    MaTK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsernameTK = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordTK = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RoleTK = table.Column<int>(type: "int", nullable: false),
                    Gmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Accounts__27250070C97282C8", x => x.MaTK);
                });

            migrationBuilder.CreateTable(
                name: "Nhacungcap",
                columns: table => new
                {
                    MaNCC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenNCC = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailNCC = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SdtNCC = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DiachiNCC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Nguoilienlac = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Nhacungc__3A185DEBBBD7F3FE", x => x.MaNCC);
                });

            migrationBuilder.CreateTable(
                name: "PhanloaiSP",
                columns: table => new
                {
                    MaPLSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenPLSP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTaPLSP = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Phanloai__BD3992AFC28AFCE5", x => x.MaPLSP);
                });

            migrationBuilder.CreateTable(
                name: "Khachhang",
                columns: table => new
                {
                    MaKH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FulltenKH = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SdtKH = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DiachiKH = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaTK = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Khachhan__2725CF1E72F421B0", x => x.MaKH);
                    table.ForeignKey(
                        name: "FK__Khachhang__MaTK__4D94879B",
                        column: x => x.MaTK,
                        principalTable: "Accounts",
                        principalColumn: "MaTK");
                });

            migrationBuilder.CreateTable(
                name: "Nhanvien",
                columns: table => new
                {
                    MaNV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FulltenNV = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailNV = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SdtNV = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DiachiNV = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaTK = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Nhanvien__2725D70A04C9B38A", x => x.MaNV);
                    table.ForeignKey(
                        name: "FK__Nhanvien__MaTK__5165187F",
                        column: x => x.MaTK,
                        principalTable: "Accounts",
                        principalColumn: "MaTK");
                });

            migrationBuilder.CreateTable(
                name: "Hanghoa",
                columns: table => new
                {
                    MaHH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenHH = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GiaHH = table.Column<int>(type: "int", nullable: false),
                    Soluongton = table.Column<int>(type: "int", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LoaiHH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaNCC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayXoa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Hanghoa__2725A6E45E5FAD4F", x => x.MaHH);
                    table.ForeignKey(
                        name: "FK__Hanghoa__MaNCC__160F4887",
                        column: x => x.MaNCC,
                        principalTable: "Nhacungcap",
                        principalColumn: "MaNCC");
                });

            migrationBuilder.CreateTable(
                name: "Hoadon",
                columns: table => new
                {
                    MaHD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NgaylapHD = table.Column<DateOnly>(type: "date", nullable: false),
                    MaKH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaNV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TongtienHD = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Ghichu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Hoadon__2725A6E0A90BC80A", x => x.MaHD);
                    table.ForeignKey(
                        name: "FK__Hoadon__MaKH__6754599E",
                        column: x => x.MaKH,
                        principalTable: "Khachhang",
                        principalColumn: "MaKH");
                    table.ForeignKey(
                        name: "FK__Hoadon__MaNV__68487DD7",
                        column: x => x.MaNV,
                        principalTable: "Nhanvien",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "Banphim",
                columns: table => new
                {
                    MaBP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenBP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HangBP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GiaBP = table.Column<int>(type: "int", nullable: false),
                    LoaiBP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KieuketnoiBP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DenledBP = table.Column<bool>(type: "bit", nullable: false),
                    XuatxuBP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgaysanxuatBP = table.Column<DateOnly>(type: "date", nullable: true),
                    TinhtrangBP = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNCC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaPLSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaHH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Banphim__272475AB37053E7E", x => x.MaBP);
                    table.ForeignKey(
                        name: "FK__Banphim__MaHH__18EBB532",
                        column: x => x.MaHH,
                        principalTable: "Hanghoa",
                        principalColumn: "MaHH");
                    table.ForeignKey(
                        name: "FK__Banphim__MaPLSP__70DDC3D8",
                        column: x => x.MaPLSP,
                        principalTable: "PhanloaiSP",
                        principalColumn: "MaPLSP");
                });

            migrationBuilder.CreateTable(
                name: "Laptop",
                columns: table => new
                {
                    MaLT = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenLT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HangLT = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    GiaLT = table.Column<int>(type: "int", nullable: false),
                    KichthuocLT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RamLT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OcungLT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    XuatxuLT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgaysanxuatLT = table.Column<DateOnly>(type: "date", nullable: true),
                    TinhtrangLT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNCC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaPLSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaHH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Laptop__2725C773763C2B6D", x => x.MaLT);
                    table.ForeignKey(
                        name: "FK__Laptop__MaHH__17036CC0",
                        column: x => x.MaHH,
                        principalTable: "Hanghoa",
                        principalColumn: "MaHH");
                    table.ForeignKey(
                        name: "FK__Laptop__MaPLSP__6EF57B66",
                        column: x => x.MaPLSP,
                        principalTable: "PhanloaiSP",
                        principalColumn: "MaPLSP");
                });

            migrationBuilder.CreateTable(
                name: "Loa",
                columns: table => new
                {
                    MaLoa = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenLoa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HangLoa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GiaLoa = table.Column<int>(type: "int", nullable: false),
                    CongsuatLoa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    KieuketnoiLoa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    XuatxuLoa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgaysanxuatLoa = table.Column<DateOnly>(type: "date", nullable: true),
                    TinhtrangLoa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNCC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaPLSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaHH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Loa__3B98D240FBD17876", x => x.MaLoa);
                    table.ForeignKey(
                        name: "FK__Loa__MaHH__17F790F9",
                        column: x => x.MaHH,
                        principalTable: "Hanghoa",
                        principalColumn: "MaHH");
                    table.ForeignKey(
                        name: "FK__Loa__MaPLSP__71D1E811",
                        column: x => x.MaPLSP,
                        principalTable: "PhanloaiSP",
                        principalColumn: "MaPLSP");
                });

            migrationBuilder.CreateTable(
                name: "Manhinh",
                columns: table => new
                {
                    MaMH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenMH = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HangMH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GiaMH = table.Column<int>(type: "int", nullable: false),
                    KichthuocMH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TansoMH = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DoPhanGiaiMH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    XuatxuMH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgaysanxuatMH = table.Column<DateOnly>(type: "date", nullable: true),
                    TinhtrangMH = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNCC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaPLSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaHH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Manhinh__2725DFD969530BD5", x => x.MaMH);
                    table.ForeignKey(
                        name: "FK_Manhinh_PhanloaiSP",
                        column: x => x.MaPLSP,
                        principalTable: "PhanloaiSP",
                        principalColumn: "MaPLSP");
                    table.ForeignKey(
                        name: "FK__Manhinh__MaHH__19DFD96B",
                        column: x => x.MaHH,
                        principalTable: "Hanghoa",
                        principalColumn: "MaHH");
                });

            migrationBuilder.CreateTable(
                name: "Tainghe",
                columns: table => new
                {
                    MaTN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenTN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HangTN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GiaTN = table.Column<int>(type: "int", nullable: false),
                    LoaiTN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KieuketnoiTN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PinTN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrongluongTN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    XuatxuTN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgaysanxuatTN = table.Column<DateOnly>(type: "date", nullable: true),
                    TinhtrangTN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaNCC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaPLSP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    MaHH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tainghe__27250073255F3EB0", x => x.MaTN);
                    table.ForeignKey(
                        name: "FK_Tainghe_Hanghoa",
                        column: x => x.MaHH,
                        principalTable: "Hanghoa",
                        principalColumn: "MaHH");
                    table.ForeignKey(
                        name: "FK__Tainghe__MaPLSP__6FE99F9F",
                        column: x => x.MaPLSP,
                        principalTable: "PhanloaiSP",
                        principalColumn: "MaPLSP");
                });

            migrationBuilder.CreateTable(
                name: "ChitietHD",
                columns: table => new
                {
                    MaCTHD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaHD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaHH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Soluong = table.Column<int>(type: "int", nullable: false),
                    Dongia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChitietH__1E4FA771E41E6CA7", x => x.MaCTHD);
                    table.ForeignKey(
                        name: "FK__ChitietHD__MaHD__2B0A656D",
                        column: x => x.MaHD,
                        principalTable: "Hoadon",
                        principalColumn: "MaHD");
                    table.ForeignKey(
                        name: "FK__ChitietHD__MaHH__2BFE89A6",
                        column: x => x.MaHH,
                        principalTable: "Hanghoa",
                        principalColumn: "MaHH");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Accounts__15DC27FC668419E0",
                table: "Accounts",
                column: "UsernameTK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banphim_MaHH",
                table: "Banphim",
                column: "MaHH");

            migrationBuilder.CreateIndex(
                name: "IX_Banphim_MaPLSP",
                table: "Banphim",
                column: "MaPLSP");

            migrationBuilder.CreateIndex(
                name: "IX_ChitietHD_MaHD",
                table: "ChitietHD",
                column: "MaHD");

            migrationBuilder.CreateIndex(
                name: "IX_ChitietHD_MaHH",
                table: "ChitietHD",
                column: "MaHH");

            migrationBuilder.CreateIndex(
                name: "IX_Hanghoa_MaNCC",
                table: "Hanghoa",
                column: "MaNCC");

            migrationBuilder.CreateIndex(
                name: "IX_Hoadon_MaKH",
                table: "Hoadon",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_Hoadon_MaNV",
                table: "Hoadon",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_Khachhang_MaTK",
                table: "Khachhang",
                column: "MaTK");

            migrationBuilder.CreateIndex(
                name: "UQ__Khachhan__D6512A83F236E621",
                table: "Khachhang",
                column: "FulltenKH",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Laptop_MaHH",
                table: "Laptop",
                column: "MaHH");

            migrationBuilder.CreateIndex(
                name: "IX_Laptop_MaPLSP",
                table: "Laptop",
                column: "MaPLSP");

            migrationBuilder.CreateIndex(
                name: "UQ__Laptop__4CF9A468F04DF093",
                table: "Laptop",
                column: "TenLT",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loa_MaHH",
                table: "Loa",
                column: "MaHH");

            migrationBuilder.CreateIndex(
                name: "IX_Loa_MaPLSP",
                table: "Loa",
                column: "MaPLSP");

            migrationBuilder.CreateIndex(
                name: "IX_Manhinh_MaHH",
                table: "Manhinh",
                column: "MaHH");

            migrationBuilder.CreateIndex(
                name: "IX_Manhinh_MaPLSP",
                table: "Manhinh",
                column: "MaPLSP");

            migrationBuilder.CreateIndex(
                name: "IX_Nhanvien_MaTK",
                table: "Nhanvien",
                column: "MaTK");

            migrationBuilder.CreateIndex(
                name: "IX_Tainghe_MaHH",
                table: "Tainghe",
                column: "MaHH");

            migrationBuilder.CreateIndex(
                name: "IX_Tainghe_MaPLSP",
                table: "Tainghe",
                column: "MaPLSP");

            // Cập nhật dữ liệu cho các bản ghi hiện có
            migrationBuilder.Sql("UPDATE Hanghoa SET NgayTao = GETDATE() WHERE NgayTao IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banphim");

            migrationBuilder.DropTable(
                name: "ChitietHD");

            migrationBuilder.DropTable(
                name: "Laptop");

            migrationBuilder.DropTable(
                name: "Loa");

            migrationBuilder.DropTable(
                name: "Manhinh");

            migrationBuilder.DropTable(
                name: "Tainghe");

            migrationBuilder.DropTable(
                name: "Hoadon");

            migrationBuilder.DropTable(
                name: "Hanghoa");

            migrationBuilder.DropTable(
                name: "PhanloaiSP");

            migrationBuilder.DropTable(
                name: "Khachhang");

            migrationBuilder.DropTable(
                name: "Nhanvien");

            migrationBuilder.DropTable(
                name: "Nhacungcap");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}

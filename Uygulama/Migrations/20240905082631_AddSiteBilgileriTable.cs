using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uygulama.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteBilgileriTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteBilgileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteAdi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteKullaniciAdi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteSifre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KullaniciId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteBilgileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteBilgileri_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteBilgileri_KullaniciId",
                table: "SiteBilgileri",
                column: "KullaniciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteBilgileri");
        }
    }
}

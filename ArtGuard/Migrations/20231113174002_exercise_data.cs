using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtGuard.Migrations
{
    /// <inheritdoc />
    public partial class exercise_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cwiczenia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cwiczenia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SesjeTreningowe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataCwiczenia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SesjeTreningowe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SesjeTreningowe_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DaneWykonanychCwiczen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSesjiTreningowej = table.Column<int>(type: "int", nullable: false),
                    IdCwiczenia = table.Column<int>(type: "int", nullable: false),
                    ObciazenieWCwiczeniu = table.Column<int>(type: "int", nullable: false),
                    LiczbaPowtorzen = table.Column<int>(type: "int", nullable: false),
                    LiczbaSerii = table.Column<int>(type: "int", nullable: false),
                    SejsaTreningowaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaneWykonanychCwiczen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DaneWykonanychCwiczen_Cwiczenia_IdCwiczenia",
                        column: x => x.IdCwiczenia,
                        principalTable: "Cwiczenia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DaneWykonanychCwiczen_SesjeTreningowe_IdSesjiTreningowej",
                        column: x => x.IdSesjiTreningowej,
                        principalTable: "SesjeTreningowe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DaneWykonanychCwiczen_SesjeTreningowe_SejsaTreningowaId",
                        column: x => x.SejsaTreningowaId,
                        principalTable: "SesjeTreningowe",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DaneWykonanychCwiczen_IdCwiczenia",
                table: "DaneWykonanychCwiczen",
                column: "IdCwiczenia");

            migrationBuilder.CreateIndex(
                name: "IX_DaneWykonanychCwiczen_IdSesjiTreningowej",
                table: "DaneWykonanychCwiczen",
                column: "IdSesjiTreningowej");

            migrationBuilder.CreateIndex(
                name: "IX_DaneWykonanychCwiczen_SejsaTreningowaId",
                table: "DaneWykonanychCwiczen",
                column: "SejsaTreningowaId");

            migrationBuilder.CreateIndex(
                name: "IX_SesjeTreningowe_UserId",
                table: "SesjeTreningowe",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DaneWykonanychCwiczen");

            migrationBuilder.DropTable(
                name: "Cwiczenia");

            migrationBuilder.DropTable(
                name: "SesjeTreningowe");
        }
    }
}

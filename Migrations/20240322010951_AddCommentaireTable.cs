using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalVioo.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentaireTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Commentaire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreePar = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdTache = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commentaire", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commentaire_AspNetUsers_CreePar",
                        column: x => x.CreePar,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commentaire_Tache_IdTache",
                        column: x => x.IdTache,
                        principalTable: "Tache",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commentaire_CreePar",
                table: "Commentaire",
                column: "CreePar");

            migrationBuilder.CreateIndex(
                name: "IX_Commentaire_IdTache",
                table: "Commentaire",
                column: "IdTache");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commentaire");
        }
    }
}

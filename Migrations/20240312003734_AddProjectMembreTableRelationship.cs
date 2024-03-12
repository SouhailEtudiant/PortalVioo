using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalVioo.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectMembreTableRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetTitre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetImage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MembreProjet",
                columns: table => new
                {
                    IdMembrePorjet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUtilisateur = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdProjet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembreProjet", x => x.IdMembrePorjet);
                    table.ForeignKey(
                        name: "FK_MembreProjet_AspNetUsers_IdUtilisateur",
                        column: x => x.IdUtilisateur,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MembreProjet_Projet_IdProjet",
                        column: x => x.IdProjet,
                        principalTable: "Projet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MembreProjet_IdProjet",
                table: "MembreProjet",
                column: "IdProjet");

            migrationBuilder.CreateIndex(
                name: "IX_MembreProjet_IdUtilisateur",
                table: "MembreProjet",
                column: "IdUtilisateur");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MembreProjet");

            migrationBuilder.DropTable(
                name: "Projet");
        }
    }
}

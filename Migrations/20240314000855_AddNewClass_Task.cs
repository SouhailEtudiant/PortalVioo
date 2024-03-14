using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalVioo.Migrations
{
    /// <inheritdoc />
    public partial class AddNewClass_Task : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tache",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TacheTitre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TacheDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChargeEstime = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChargeReele = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateDebut = table.Column<DateOnly>(type: "date", nullable: false),
                    DateFin = table.Column<DateOnly>(type: "date", nullable: false),
                    IdUtilisateur = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdPriorite = table.Column<int>(type: "int", nullable: false),
                    IdStatus = table.Column<int>(type: "int", nullable: false),
                    IdType = table.Column<int>(type: "int", nullable: false),
                    IdProjet = table.Column<int>(type: "int", nullable: false),
                    IdTacheParent = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tache", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tache_AspNetUsers_IdUtilisateur",
                        column: x => x.IdUtilisateur,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tache_ParamPriorite_IdPriorite",
                        column: x => x.IdPriorite,
                        principalTable: "ParamPriorite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tache_ParamStatus_IdStatus",
                        column: x => x.IdStatus,
                        principalTable: "ParamStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tache_ParamType_IdType",
                        column: x => x.IdType,
                        principalTable: "ParamType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tache_Projet_IdProjet",
                        column: x => x.IdProjet,
                        principalTable: "Projet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tache_Tache_IdTacheParent",
                        column: x => x.IdTacheParent,
                        principalTable: "Tache",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tache_IdPriorite",
                table: "Tache",
                column: "IdPriorite");

            migrationBuilder.CreateIndex(
                name: "IX_Tache_IdProjet",
                table: "Tache",
                column: "IdProjet");

            migrationBuilder.CreateIndex(
                name: "IX_Tache_IdStatus",
                table: "Tache",
                column: "IdStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Tache_IdTacheParent",
                table: "Tache",
                column: "IdTacheParent");

            migrationBuilder.CreateIndex(
                name: "IX_Tache_IdType",
                table: "Tache",
                column: "IdType");

            migrationBuilder.CreateIndex(
                name: "IX_Tache_IdUtilisateur",
                table: "Tache",
                column: "IdUtilisateur");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tache");
        }
    }
}

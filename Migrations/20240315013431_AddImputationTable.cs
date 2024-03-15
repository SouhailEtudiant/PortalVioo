using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalVioo.Migrations
{
    /// <inheritdoc />
    public partial class AddImputationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tache",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Projet",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ParamType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ParamStatus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ParamPriorite",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MembreProjet",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Imputation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    chargeEnHeure = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IdUtilisateur = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdTache = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imputation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Imputation_AspNetUsers_IdUtilisateur",
                        column: x => x.IdUtilisateur,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Imputation_Tache_IdTache",
                        column: x => x.IdTache,
                        principalTable: "Tache",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Imputation_IdTache",
                table: "Imputation",
                column: "IdTache");

            migrationBuilder.CreateIndex(
                name: "IX_Imputation_IdUtilisateur",
                table: "Imputation",
                column: "IdUtilisateur");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Imputation");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tache");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Projet");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ParamType");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ParamStatus");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ParamPriorite");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MembreProjet");
        }
    }
}

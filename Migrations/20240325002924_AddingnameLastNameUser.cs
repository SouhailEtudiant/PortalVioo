using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalVioo.Migrations
{
    /// <inheritdoc />
    public partial class AddingnameLastNameUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomUser",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrenomUser",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomUser",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrenomUser",
                table: "AspNetUsers");
        }
    }
}

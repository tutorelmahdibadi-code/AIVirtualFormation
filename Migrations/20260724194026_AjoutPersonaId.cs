using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VIRTUAL_CLASSE_FORMATION.Migrations
{
    /// <inheritdoc />
    public partial class AjoutPersonaId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonaId",
                table: "Formateurs",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonaId",
                table: "Formateurs");
        }
    }
}

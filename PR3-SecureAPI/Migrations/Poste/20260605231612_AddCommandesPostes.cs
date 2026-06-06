using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PR3_SecureAPI.Migrations.Poste
{
    /// <inheritdoc />
    public partial class AddCommandesPostes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommandesPostes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PosteId = table.Column<int>(type: "INTEGER", nullable: true),
                    MacAdress = table.Column<string>(type: "TEXT", nullable: false),
                    TypeCommande = table.Column<string>(type: "TEXT", nullable: false),
                    Portee = table.Column<string>(type: "TEXT", nullable: false),
                    Statut = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateExecution = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Resultat = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandesPostes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandesPostes");
        }
    }
}

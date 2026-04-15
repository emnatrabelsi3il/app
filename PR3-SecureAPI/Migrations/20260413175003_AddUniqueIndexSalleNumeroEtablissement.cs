using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PR3_SecureAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexSalleNumeroEtablissement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Salle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero = table.Column<string>(type: "TEXT", nullable: false),
                    EtablissementId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salle", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Salle_Numero_EtablissementId",
                table: "Salle",
                columns: new[] { "Numero", "EtablissementId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Salle");
        }
    }
}

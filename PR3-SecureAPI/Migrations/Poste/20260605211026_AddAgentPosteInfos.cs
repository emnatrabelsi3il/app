using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PR3_SecureAPI.Migrations.Poste
{
    /// <inheritdoc />
    public partial class AddAgentPosteInfos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomMachine",
                table: "Poste",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdresseIP",
                table: "Poste",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OsVersion",
                table: "Poste",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RamDisponibleMb",
                table: "Poste",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RamTotaleMb",
                table: "Poste",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DisqueTotalGb",
                table: "Poste",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DisqueLibreGb",
                table: "Poste",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeen",
                table: "Poste",
                type: "TEXT",
                nullable: true);
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomMachine",
                table: "Poste");

            migrationBuilder.DropColumn(
                name: "AdresseIP",
                table: "Poste");

            migrationBuilder.DropColumn(
                name: "OsVersion",
                table: "Poste");

            migrationBuilder.DropColumn(
                name: "RamDisponibleMb",
                table: "Poste");

            migrationBuilder.DropColumn(
                name: "RamTotaleMb",
                table: "Poste");

            migrationBuilder.DropColumn(
                name: "DisqueTotalGb",
                table: "Poste");

            migrationBuilder.DropColumn(
                name: "DisqueLibreGb",
                table: "Poste");

            migrationBuilder.DropColumn(
                name: "LastSeen",
                table: "Poste");
        }
    }
}

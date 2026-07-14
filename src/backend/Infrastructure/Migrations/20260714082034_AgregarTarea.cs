using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AplicacionTareas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTarea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tareas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    EstaCompletada = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    CreadoEnUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ActualizadoEnUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tareas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_EstaCompletada",
                table: "Tareas",
                column: "EstaCompletada");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_Titulo",
                table: "Tareas",
                column: "Titulo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tareas");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TareaAPI.Migrations
{
    /// <inheritdoc />
    public partial class RelacionTareaCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Tareas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_CategoriaId",
                table: "Tareas",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_Categorias_CategoriaId",
                table: "Tareas",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "categoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_Categorias_CategoriaId",
                table: "Tareas");

            migrationBuilder.DropIndex(
                name: "IX_Tareas_CategoriaId",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Tareas");
        }
    }
}

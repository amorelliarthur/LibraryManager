using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManager.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserindo os gêneros
            migrationBuilder.InsertData("Genres", new[] { "idGenre", "Name" }, new object[] { 1, "Romance" });
            migrationBuilder.InsertData("Genres", new[] { "idGenre", "Name" }, new object[] { 2, "Distopia" });
            migrationBuilder.InsertData("Genres", new[] { "idGenre", "Name" }, new object[] { 3, "Fantasia" });
            migrationBuilder.InsertData("Genres", new[] { "idGenre", "Name" }, new object[] { 4, "Fábula política" });
            migrationBuilder.InsertData("Genres", new[] { "idGenre", "Name" }, new object[] { 5, "Fábula filosófica" });

            // Inserindo os livros com os respectivos GenreId
            migrationBuilder.InsertData("Books", new[] { "Title", "Author", "GenreidGenre" }, new object[] { "1984", "George Orwell", 2 });
            migrationBuilder.InsertData("Books", new[] { "Title", "Author", "GenreidGenre" }, new object[] { "O Senhor dos Anéis", "J.R.R. Tolkien", 3 });
            migrationBuilder.InsertData("Books", new[] { "Title", "Author", "GenreidGenre" }, new object[] { "A Revolução dos Bichos", "George Orwell", 4 });
            migrationBuilder.InsertData("Books", new[] { "Title", "Author", "GenreidGenre" }, new object[] { "O Pequeno Príncipe", "Antoine de Saint-Exupéry", 5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Books");
        }
    }
}

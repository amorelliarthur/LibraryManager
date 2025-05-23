using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManager.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class DataEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Gêneros
            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "idGenre", "Name" },
                values: new object[,]
                {
                    { 1, "Romance" },
                    { 2, "Distopia" },
                    { 3, "Fantasia" },
                    { 4, "Fábula política" },
                    { 5, "Fábula filosófica" }
                });

            // 2) Editoras
            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "idPublisher", "Name" },
                values: new object[,]
                {
                    { 1, "Companhia das Letras" },
                    { 2, "HarperCollins" },
                    { 3, "Nova Fronteira" }
                });

            // 3) Livros (com idBook explícito!)
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "idBook", "Title", "Author", "GenreId", "PublisherId" },
                values: new object[,]
                {
                    { 1, "1984", "George Orwell", 2, 1 },
                    { 2, "O Senhor dos Anéis", "J.R.R. Tolkien", 3, 2 },
                    { 3, "A Revolução dos Bichos", "George Orwell", 4, 1 },
                    { 4, "O Pequeno Príncipe", "Antoine de Saint-Exupéry", 5, 3 }
                });

            // 4) Leitores (com idReader explícito!)
            migrationBuilder.InsertData(
                table: "Readers",
                columns: new[] { "idReader", "Name", "Email" },
                values: new object[,]
                {
                    { 1, "João Silva", "joao@email.com" },
                    { 2, "Maria Oliveira", "maria@email.com" },
                    { 3, "Carlos Lima", "carlos@email.com" }
                });

            // 5) Associação N:N (BookReader)
            migrationBuilder.InsertData(
                table: "BookReader",
                columns: new[] { "BooksidBook", "ReadersidReader" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove todas as associações N:N
            migrationBuilder.Sql("DELETE FROM BookReader");
            // Apaga livros
            migrationBuilder.Sql("DELETE FROM Books");
            // Apaga leitores
            migrationBuilder.Sql("DELETE FROM Readers");
            // Apaga editoras
            migrationBuilder.Sql("DELETE FROM Publishers");
            // Apaga gêneros
            migrationBuilder.Sql("DELETE FROM Genres");
        }
    }
}

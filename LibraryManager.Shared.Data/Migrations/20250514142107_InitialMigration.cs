using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManager.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    idGenre = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.idGenre);
                });

            migrationBuilder.CreateTable(
                name: "Readers",
                columns: table => new
                {
                    idReader = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readers", x => x.idReader);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    idBook = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GenreidGenre = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.idBook);
                    table.ForeignKey(
                        name: "FK_Books_Genres_GenreidGenre",
                        column: x => x.GenreidGenre,
                        principalTable: "Genres",
                        principalColumn: "idGenre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookReader",
                columns: table => new
                {
                    BooksidBook = table.Column<int>(type: "int", nullable: false),
                    ReadersidReader = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookReader", x => new { x.BooksidBook, x.ReadersidReader });
                    table.ForeignKey(
                        name: "FK_BookReader_Books_BooksidBook",
                        column: x => x.BooksidBook,
                        principalTable: "Books",
                        principalColumn: "idBook",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookReader_Readers_ReadersidReader",
                        column: x => x.ReadersidReader,
                        principalTable: "Readers",
                        principalColumn: "idReader",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookReader_ReadersidReader",
                table: "BookReader",
                column: "ReadersidReader");

            migrationBuilder.CreateIndex(
                name: "IX_Books_GenreidGenre",
                table: "Books",
                column: "GenreidGenre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookReader");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Readers");

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}

using LibraryManager.Data.BD;
using LibraryManager.Shared.Data.DB;
using LibraryManager_API.Requests;
using LibraryManager_API.Responses;
using LibraryManager_Console;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager_API.Endpoints
{
    public static class BookExtension
    {
        public static void AddEndPointsBook(this WebApplication app)
        {
            var group = app
                .MapGroup("/books")
                .RequireAuthorization()
                .WithTags("Books");

            // GET /books - Listar todos os Livros
            group.MapGet("", ([FromServices] DAL<Book> dal) =>
            {
                var list = dal.ReadAll();
                var response = list.Select(b => new BookResponse(
                    b.idBook,
                    b.Title,
                    b.Author,
                    b.Genre?.idGenre ?? 0,
                    b.Genre?.Name ?? "Sem gênero",
                    b.Publisher?.idPublisher ?? 0,
                    b.Publisher?.Name ?? "Sem editora"
                ));
                return Results.Ok(response);
            });

            // GET /books/{id} - Buscar um Livro específico
            group.MapGet("/{id}", (int id, [FromServices] DAL<Book> dal) =>
            {
                var book = dal.ReadBy(b => b.idBook == id);
                if (book == null) return Results.NotFound();

                var response = new BookResponse(
                    book.idBook,
                    book.Title,
                    book.Author,
                    book.Genre?.idGenre ?? 0,
                    book.Genre?.Name ?? "Sem gênero",
                    book.Publisher?.idPublisher ?? 0,
                    book.Publisher?.Name ?? "Sem editora"
                );
                return Results.Ok(response);
            });

            // POST /books - Cadastrar um Livro             
            group.MapPost("", (
                [FromServices] DAL<Book> bookDal,
                [FromServices] DAL<Genre> genreDal,
                [FromServices] DAL<Publisher> publisherDal,
                [FromBody] BookRequest dto) =>
            {
                // Verifica se o gênero informado existe
                var genre = genreDal.ReadBy(g => g.idGenre == dto.GenreId);
                if (genre == null)
                    return Results.BadRequest($"Gênero com id {dto.GenreId} não encontrado.");

                // Verifica se a editora informada existe
                var publisher = publisherDal.ReadBy(p => p.idPublisher == dto.PublisherId);
                if (publisher == null)
                    return Results.BadRequest($"Editora com id {dto.PublisherId} não encontrada.");

                // Cria o livro com os dados válidos
                var book = new Book(dto.Title, dto.Author, genre, publisher);
                bookDal.Create(book);

                var response = new BookResponse(
                    book.idBook,
                    book.Title,
                    book.Author,
                    genre.idGenre,
                    genre.Name,
                    publisher.idPublisher,
                    publisher.Name
                );

                return Results.Created($"/books/{book.idBook}", response);
            });



            // PUT /books - Atualizar um Livro
            group.MapPut("", ([FromServices] DAL<Book> bookDal, [FromServices] DAL<Genre> genreDal, [FromServices] DAL<Publisher> publisherDal, [FromBody] BookEditRequest request) =>
            {
                var book = bookDal.ReadBy(b => b.idBook == request.Id);
                if (book == null) return Results.NotFound();

                var genre = genreDal.ReadBy(g => g.idGenre == request.GenreId);
                if (genre == null) return Results.BadRequest("Gênero não encontrado.");

                var publisher = publisherDal.ReadBy(p => p.idPublisher == request.PublisherId);
                if (publisher == null) return Results.BadRequest("Editora não encontrada.");

                book.Title = request.Title;
                book.Author = request.Author;
                book.Genre = genre;
                book.Publisher = publisher;

                bookDal.Update(book);

                var response = new BookResponse(book.idBook, book.Title, book.Author, genre.idGenre, genre.Name, publisher.idPublisher, publisher.Name);
                return Results.Ok(response);
            });

            // DELETE /books/{id} - Deletar um Livro
            group.MapDelete("/{id}", (int id, [FromServices] DAL<Book> dal) =>
            {
                var book = dal.ReadBy(b => b.idBook == id);
                if (book == null) return Results.NotFound();

                dal.Delete(book);
                return Results.NoContent();
            });

            // GET /books/{id}/readers - Listar os Leitores de um Livro
            group.MapGet("/{id}/readers", (int id, [FromServices] DAL<Book> bookDal) =>
            {
                var book = bookDal.ReadBy(b => b.idBook == id);

                if (book == null)
                    return Results.NotFound();

                var readers = book.Readers?.Select(r =>
                    new ReaderResponse(r.idReader, r.Name, r.Email)).ToList();

                return Results.Ok(readers);
            });

        }
    }
}

using LibraryManager.Data.BD;
using LibraryManager.Shared.Data.DB;
using LibraryManager_API.Requests;
using LibraryManager_API.Responses;
using LibraryManager_Console;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager_API.Endpoints
{
    public static class ReaderExtension
    {
        public static void AddEndPointsReader(this WebApplication app)
        {
            var group = app
                .MapGroup("/readers")
                .RequireAuthorization()
                .WithTags("Readers");

            // POST /readers - Cadastrar novo leitor
            group.MapPost("", ([FromServices] DAL<Reader> dal, [FromBody] ReaderRequest req) =>
            {
                var existing = dal.ReadBy(r => r.Email.ToLower() == req.Email.ToLower());
                if (existing != null)
                    return Results.BadRequest("Leitor já cadastrado com este email.");

                var reader = new Reader(req.Name, req.Email);
                dal.Create(reader);

                return Results.Created($"/readers/{reader.idReader}", new ReaderResponse(reader.idReader, reader.Name, reader.Email));
            });

            // GET /readers - Listar leitores
            group.MapGet("", ([FromServices] DAL<Reader> dal) =>
            {
                var list = dal.Read().Select(r => new ReaderResponse(r.idReader, r.Name, r.Email)).ToList();
                return Results.Ok(list);
            });

            // GET /readers/{id} - Obter leitor por ID
            group.MapGet("/{id}", ([FromServices] DAL<Reader> dal, int id) =>
            {
                var reader = dal.ReadBy(r => r.idReader == id);
                if (reader == null) return Results.NotFound();

                return Results.Ok(new ReaderResponse(reader.idReader, reader.Name, reader.Email));
            });

            // PUT /readers - Editar leitor
            group.MapPut("", ([FromServices] DAL<Reader> dal, [FromBody] ReaderEditRequest req) =>
            {
                var reader = dal.ReadBy(r => r.idReader == req.Id);
                if (reader == null) return Results.NotFound();

                reader.Name = req.Name;
                reader.Email = req.Email;
                dal.Update(reader);

                return Results.Ok(new ReaderResponse(reader.idReader, reader.Name, reader.Email));
            });

            // DELETE /readers - Deletar leitor
            group.MapDelete("/{id}", ([FromServices] DAL<Reader> dal, int id) =>
            {
                var reader = dal.ReadBy(r => r.idReader == id);
                if (reader == null) return Results.NotFound();

                dal.Delete(reader);
                return Results.NoContent();
            });

            // POST - /readers/{readerId}/books/{bookId} - Associar leitor a um livro
            group.MapPost("/{readerId}/books/{bookId}", ([FromServices] DAL<Reader> readerDal, [FromServices] DAL<Book> bookDal, int readerId, int bookId) =>
            {
                var reader = readerDal.ReadBy(r => r.idReader == readerId);
                if (reader == null) return Results.NotFound("Leitor não encontrado.");

                using var context = new LibraryManagerContext();
                var book = context.Books.Find(bookId);
                if (book == null) return Results.NotFound("Livro não encontrado.");

                if (!reader.Books.Contains(book))
                {
                    reader.Books.Add(book);
                    readerDal.Update(reader);
                }

                return Results.Ok("Associação realizada.");
            });

            // GET /readers/{readerId}/books - Listar livros de um leitor
            group.MapGet("/{readerId}/books", ([FromServices] DAL<Reader> readerDal, int readerId) =>
            {
                var reader = readerDal.ReadBy(r => r.idReader == readerId);
                if (reader == null) return Results.NotFound();

                var list = reader.Books.Select(b =>
                new BookResponse(
                    b.idBook,
                    b.Title,
                    b.Author,
                    b.Genre?.idGenre ?? 0,
                    b.Genre?.Name ?? "Sem gênero",
                    b.Publisher?.idPublisher ?? 0,
                    b.Publisher?.Name ?? "Sem editora"
                )).ToList();


                return Results.Ok(list);
            });
        }
    }
}
    
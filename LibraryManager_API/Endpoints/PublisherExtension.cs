using LibraryManager.Data.BD;
using LibraryManager.Shared.Data.DB;
using LibraryManager_API.Requests;
using LibraryManager_API.Responses;
using LibraryManager_Console;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager_API.Endpoints
{
    public static class PublisherExtension
    {
        public static void AddEndPointsPublisher(this WebApplication app)
        {
            var group = app.MapGroup("/publishers")
                           .WithTags("Publishers");

            // GET /publishers - Listar todas as Editoras
            group.MapGet("", ([FromServices] DAL<Publisher> dal) =>
            {
                var list = dal.Read()
                    .Select(p => new PublisherResponse(p.idPublisher, p.Name))
                    .ToList();
                return Results.Ok(list);
            });

            // GET /publishers/{id} - Buscar uma Editora específica
            group.MapGet("/{id}", ([FromServices] DAL<Publisher> dal, int id) =>
            {
                var p = dal.ReadBy(x => x.idPublisher == id);
                if (p == null) return Results.NotFound();
                return Results.Ok(new PublisherResponse(p.idPublisher, p.Name));
            });

            // POST /publishers - Cadastrar uma Editora
            group.MapPost("", ([FromServices] DAL<Publisher> dal, [FromBody] PublisherRequest req) =>
            {
                if (dal.Read().Any(x => x.Name.ToLower() == req.Name.ToLower()))
                    return Results.BadRequest("Editora já existe.");

                var pub = new Publisher(req.Name);
                dal.Create(pub);
                return Results.Created($"/publishers/{pub.idPublisher}",
                    new PublisherResponse(pub.idPublisher, pub.Name));
            });

            // PUT /publishers - Atualizar uma Editora
            group.MapPut("", ([FromServices] DAL<Publisher> dal, [FromBody] PublisherResponse upd) =>
            {
                var p = dal.ReadBy(x => x.idPublisher == upd.idPublisher);
                if (p == null) return Results.NotFound();
                p.Name = upd.Name;
                dal.Update(p);
                return Results.Ok(new PublisherResponse(p.idPublisher, p.Name));
            });

            // DELETE /publishers/{id} - Deletar uma Editora
            group.MapDelete("/{id}", ([FromServices] DAL<Publisher> dal, int id) =>
            {
                var p = dal.ReadBy(x => x.idPublisher == id);
                if (p == null) return Results.NotFound();
                dal.Delete(p);
                return Results.NoContent();
            });

            // GET /publishers/{id}/books - Listar os Livros de uma Editora
            group.MapGet("/{id}/books", ([FromServices] DAL<Publisher> dal, int id) =>
            {
                var p = dal.ReadBy(x => x.idPublisher == id);
                if (p == null) return Results.NotFound();
                var list = p.Books.Select(b =>
                new BookResponse(
                    b.idBook,
                    b.Title,
                    b.Author,
                    b.Genre?.idGenre ?? 0,
                    b.Genre?.Name ?? "Sem gênero",
                    p.idPublisher,
                    p.Name
                )).ToList();
                return Results.Ok(list);
            });
        }
    }
}

using LibraryManager.Data.BD;
using LibraryManager_API.Requests;
using LibraryManager_API.Responses;
using LibraryManager_Console;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager_API.Endpoints
{
    public static class GenreExtension
    {
        public static void AddEndPointsGenre(this WebApplication app)
        {
            var group = app.MapGroup("/genres").WithTags("Genres");

            // POST /genres - Cadastrar um Gênero
            group.MapPost("", ([FromServices] DAL<Genre> dal, [FromBody] GenreRequest genreReq) =>
            {
                var existing = dal.ReadBy(g => g.Name.ToLower() == genreReq.Name.ToLower());
                if (existing != null)
                    return Results.BadRequest("Gênero já cadastrado.");

                var genre = new Genre(genreReq.Name);
                dal.Create(genre);
                return Results.Created($"/genres/{genre.idGenre}", new GenreResponse(genre.idGenre, genre.Name));
            });

            // GET /genres - Listar todos os Gêneros
            group.MapGet("", ([FromServices] DAL<Genre> dal) =>
            {
                var genres = dal.Read().Select(g => new GenreResponse(g.idGenre, g.Name)).ToList();
                return Results.Ok(genres);
            });

            // GET /genres/{id} - Buscar um Gênero específico
            group.MapGet("/{id}", ([FromServices] DAL<Genre> dal, int id) =>
            {
                var genre = dal.ReadBy(g => g.idGenre == id);
                if (genre == null)
                    return Results.NotFound();

                return Results.Ok(new GenreResponse(genre.idGenre, genre.Name));
            });

            // PUT /genres - Atualizar um Gênero
            group.MapPut("", ([FromServices] DAL<Genre> dal, [FromBody] GenreEditRequest genreEdit) =>
            {
                var genre = dal.ReadBy(g => g.idGenre == genreEdit.Id);
                if (genre == null)
                    return Results.NotFound();

                genre.Name = genreEdit.Name;
                dal.Update(genre);

                return Results.Ok(new GenreResponse(genre.idGenre, genre.Name));
            });

            // DELETE /genres/{id} - Deletar um Gênero
            group.MapDelete("/{id}", ([FromServices] DAL<Genre> dal, int id) =>
            {
                var genre = dal.ReadBy(g => g.idGenre == id);
                if (genre == null)
                    return Results.NotFound();

                dal.Delete(genre);
                return Results.NoContent();
            });

            // GET /genres/{id}/books - Listar os Livros de um Gênero
            group.MapGet("/{id}/books", ([FromServices] DAL<Genre> dal, int id) =>
            {
                var genre = dal.ReadBy(g => g.idGenre == id);
                if (genre == null)
                    return Results.NotFound();

                var books = genre.Books?.Select(b =>
                    new BookResponse(b.idBook, b.Title, b.Author, genre.idGenre, genre.Name)).ToList();

                return Results.Ok(books ?? new List<BookResponse>());
            });
        }
    }
}

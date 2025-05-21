namespace LibraryManager_API.Responses
{
    public record BookResponse(int idBook, string Title, string Author, int GenreId, string GenreName);
}

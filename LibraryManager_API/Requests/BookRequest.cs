namespace LibraryManager_API.Requests
{
    public record BookRequest(string Title, string Author, int GenreId, int PublisherId);
}

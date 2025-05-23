namespace LibraryManager_API.Requests
{
    public record BookEditRequest(int Id, string Title, string Author, int GenreId, int PublisherId);

}

using LibraryManager_Console;

internal class Genre
{
    public string Name { get; set; }
    private List<Book> Books = new();

    public Genre(string name)
    {
        Name = name;
    }

    public void AddBook(Book b)
    {
        Books.Add(b);
    }

    public override string ToString()
    {
        return $"Gênero: {Name}";
    }

    public void ShowBooks()
    {
        Console.WriteLine($"Livros do gênero {Name}:");
        if (Books.Count > 0)
        {
            foreach (Book b in Books)
            {
                Console.WriteLine(b);
            }
        }
        else
        {
            Console.WriteLine("Nenhum livro registrado neste gênero.");
        }
    }
}

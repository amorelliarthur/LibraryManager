using LibraryManager_Console;

internal class Program
{
    public static Dictionary<string, Book> BookList = new();
    public static Dictionary<string, Reader> ReaderList = new(); // chave: email


    private static void Main(string[] args)
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Bem-vindo ao LibraryManager!\n");
            Console.WriteLine("Digite 1 para registrar um livro");
            Console.WriteLine("Digite 2 para registrar o gênero de um livro");
            Console.WriteLine("Digite 3 para mostrar todos os livros");
            Console.WriteLine("Digite 4 para mostrar livros por gênero");
            Console.WriteLine("Digite 5 para registrar um leitor");
            Console.WriteLine("Digite 6 para associar livro a leitor");
            Console.WriteLine("Digite 7 para mostrar leitores de um livro");
            Console.WriteLine("Digite 8 para mostrar livros de um leitor");
            Console.WriteLine("Digite -1 para sair\n");

            Console.Write("Escolha sua opção: ");
            int opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    BookRegistration();
                    break;
                case 2:
                    GenreRegistration();
                    break;
                case 3:
                    BookGet();
                    break;
                case 4:
                    GenreGet();
                    break;
                case 5:
                    ReaderRegistration();
                    break;
                case 6:
                    AssociateBookToReader();
                    break;
                case 7:
                    ShowReadersOfBook();
                    break;
                case 8:
                    ShowBooksOfReader();
                    break;
                case -1:
                    Console.WriteLine("Até mais!");
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Opção inválida");
                    break;
            }
        }
    }

    private static void GenreGet()
    {
        Console.Clear();
        Console.WriteLine("Mostrar livros por gênero");
        Console.Write("Digite o nome do gênero: ");
        string genreName = Console.ReadLine();

        var livrosDoGenero = BookList.Values
            .Where(b => b.Genre != null && b.Genre.Name.Equals(genreName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (livrosDoGenero.Count == 0)
        {
            Console.WriteLine($"Nenhum livro encontrado com o gênero \"{genreName}\".");
        }
        else
        {
            Console.WriteLine($"\nLivros do gênero \"{genreName}\":");
            foreach (var livro in livrosDoGenero)
            {
                Console.WriteLine($"- {livro.Title} (Autor: {livro.Author})");
            }
        }
    }


    private static void BookGet()
    {
        Console.Clear();
        Console.WriteLine("Lista de livros:");
        foreach (var item in BookList.Values)
        {
            Console.WriteLine(item);
        }
    }

    private static void GenreRegistration()
    {
        Console.Clear();
        Console.WriteLine("Registro de gênero");
        Console.Write("Digite o título do livro que receberá o gênero: ");
        string title = Console.ReadLine();
        if (BookList.ContainsKey(title))
        {
            Console.Write($"Informe o nome do gênero para o livro \"{title}\": ");
            string name = Console.ReadLine();
            Book b = BookList[title];
            b.Genre = new Genre(name);
            Console.WriteLine($"O gênero \"{name}\" foi atribuído ao livro \"{title}\" com sucesso!");
        }
        else
        {
            Console.WriteLine($"O livro \"{title}\" não está registrado.");
        }
    }


    private static void BookRegistration()
    {
        Console.Clear();
        Console.WriteLine("Registro de livro");
        Console.Write("Digite o título do livro: ");
        string title = Console.ReadLine();
        Console.Write("Digite o autor do livro: ");
        string author = Console.ReadLine();
        Book b = new(title, author);
        BookList.Add(title, b);
        Console.WriteLine($"Livro \"{title}\" registrado com sucesso!");
    }

    private static void ReaderRegistration()
    {
        Console.Clear();
        Console.WriteLine("Registro de leitor");
        Console.Write("Digite o nome do leitor: ");
        string name = Console.ReadLine();
        Console.Write("Digite o email do leitor: ");
        string email = Console.ReadLine();

        if (ReaderList.ContainsKey(email))
        {
            Console.WriteLine("Já existe um leitor com este email.");
            return;
        }

        Reader r = new(name, email);
        ReaderList.Add(email, r);
        Console.WriteLine($"Leitor \"{name}\" registrado com sucesso!");
    }

    private static void AssociateBookToReader()
    {
        Console.Clear();
        Console.WriteLine("Associar livro a leitor");
        Console.Write("Digite o título do livro: ");
        string title = Console.ReadLine();
        Console.Write("Digite o email do leitor: ");
        string email = Console.ReadLine();

        if (!BookList.ContainsKey(title))
        {
            Console.WriteLine("Livro não encontrado.");
            return;
        }

        if (!ReaderList.ContainsKey(email))
        {
            Console.WriteLine("Leitor não encontrado.");
            return;
        }

        Book b = BookList[title];
        Reader r = ReaderList[email];
        r.AddBook(b); // isso atualiza os dois lados
        Console.WriteLine($"Leitor \"{r.Name}\" associado ao livro \"{b.Title}\" com sucesso!");
    }

    private static void ShowReadersOfBook()
    {
        Console.Clear();
        Console.Write("Digite o título do livro: ");
        string title = Console.ReadLine();

        if (!BookList.ContainsKey(title))
        {
            Console.WriteLine("Livro não encontrado.");
            return;
        }

        BookList[title].ShowReaders();
    }

    private static void ShowBooksOfReader()
    {
        Console.Clear();
        Console.Write("Digite o email do leitor: ");
        string email = Console.ReadLine();

        if (!ReaderList.ContainsKey(email))
        {
            Console.WriteLine("Leitor não encontrado.");
            return;
        }

        ReaderList[email].ShowBooks();
    }

}

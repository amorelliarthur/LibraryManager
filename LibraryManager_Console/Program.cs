using LibraryManager.Data.BD;
using LibraryManager.Shared.Data.DB;
using LibraryManager_Console;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    public static Dictionary<string, Book> BookList = new();
    public static Dictionary<string, Reader> ReaderList = new(); // chave: email

    public static DAL<Book> BookDAL = new();
    public static DAL<Genre> GenreDAL = new();


    private static void Main(string[] args)
    {
        
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\nBem-vindo ao LibraryManager!\n");

            Console.WriteLine("Digite 1 para registrar um livro");
            Console.WriteLine("Digite 2 para mostrar todos os livros");
            Console.WriteLine("Digite 3 para editar um livro");            
            Console.WriteLine("Digite 4 para excluir um livro\n");

            Console.WriteLine("Digite 5 para registrar um gênero");
            Console.WriteLine("Digite 6 para mostrar livros por gênero\n");

            Console.WriteLine("Digite 7 para registrar um leitor");
            Console.WriteLine("Digite 8 para associar livro a leitor");
            Console.WriteLine("Digite 9 para mostrar leitores de um livro");
            Console.WriteLine("Digite 10 para mostrar livros de um leitor\n");

            Console.WriteLine("Digite -1 para sair\n");
            Console.WriteLine("---------------------");

            Console.Write("Escolha sua opção: ");
            int opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    BookRegistration();
                    break;
                case 2:
                    BookGet();                    
                    break;
                case 3:
                    BookUpdate();
                    break;
                case 4:
                    BookDelete();
                    break;
                case 5:
                    GenreRegistration();
                    break;
                case 6:
                    GenreGet();
                    break;
                case 7:
                    ReaderRegistration();
                    break;
                case 8:
                    AssociateBookToReader();
                    break;
                case 9:
                    ShowReadersOfBook();
                    break;
                case 10:
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

    private static void BookRegistration()
    {
        Console.Clear();
        Console.WriteLine("Registro de livro");
        Console.Write("Digite o título do livro: ");
        string title = Console.ReadLine();
        Console.Write("Digite o autor do livro: ");
        string author = Console.ReadLine();
        Console.Write("Digite o gênero do livro: ");
        string genreName = Console.ReadLine();

        // Consulta usando o contexto para obter a instância real do gênero
        using (var context = new LibraryManagerContext())
        {
            var genre = context.Genres
                .FirstOrDefault(g => g.Name.ToLower() == genreName.ToLower());

            // Se não existir, cria e salva
            if (genre == null)
            {
                genre = new Genre { Name = genreName };
                context.Genres.Add(genre);
                //context.Update(genre); PAra utilizar Proxy
                context.SaveChanges(); // Salva para gerar o ID
                Console.WriteLine($"Novo gênero \"{genreName}\" criado.");
            }

            // Agora o gênero tem ID garantido
            Book book = new(title, author);
            book.Genre = genre;

            context.Books.Add(book);
            context.SaveChanges();
        }

        Console.WriteLine($"Livro \"{title}\" registrado com sucesso com o gênero \"{genreName}\"!");
    }
    private static void BookGet()
    {
        Console.Clear();
        Console.WriteLine("Lista de Livros:");

        using (var context = new LibraryManagerContext())
        {
            var books = context.Books.ToList();

            if (books.Count == 0)
            {
                Console.WriteLine("Nenhum book encontrado.");
                return;
            }

            foreach (var book in books)
            {
                string genreName = book.Genre != null ? book.Genre.Name : "Gênero não informado";
                Console.WriteLine($"Título: {book.Title} | Autor: {book.Author} | Gênero: {genreName}");
            }
        }
    }
    private static void BookUpdate()
    {
        Console.Clear();
        Console.WriteLine("Edição de livro");

        Console.Write("Digite o título do livro que deseja editar: ");
        string oldTitle = Console.ReadLine();

        using (var context = new LibraryManagerContext())
        {
            var book = context.Books.Include(b => b.Genre).FirstOrDefault(b => b.Title.ToLower() == oldTitle.ToLower());

            if (book == null)
            {
                Console.WriteLine($"Livro \"{oldTitle}\" não encontrado.");
                return;
            }

            Console.Write("Digite o novo título (ou pressione Enter para manter o atual): ");
            string newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                book.Title = newTitle;
            }

            Console.Write("Digite o novo autor (ou pressione Enter para manter o atual): ");
            string newAuthor = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newAuthor))
            {
                book.Author = newAuthor;
            }

            Console.Write("Digite o novo gênero (ou pressione Enter para manter o atual): ");
            string newGenreName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newGenreName))
            {
                var existingGenre = context.Genres.FirstOrDefault(g => g.Name.ToLower() == newGenreName.ToLower());

                if (existingGenre == null)
                {
                    existingGenre = new Genre { Name = newGenreName };
                    context.Genres.Add(existingGenre);
                    context.SaveChanges();
                    Console.WriteLine($"Novo gênero \"{newGenreName}\" criado.");
                }

                book.Genre = existingGenre;
            }

            context.SaveChanges();
            Console.WriteLine("Livro atualizado com sucesso!");
        }
    }
    private static void BookDelete()
    {
        Console.Clear();
        Console.WriteLine("Excluir livro");
        Console.Write("Digite o título do livro a ser excluído: ");
        string title = Console.ReadLine();

        using (var context = new LibraryManagerContext())
        {
            var book = context.Books
                .Include(b => b.Genre) // opcional, apenas se quiser ver o gênero no log
                .FirstOrDefault(b => b.Title.ToLower() == title.ToLower());

            if (book == null)
            {
                Console.WriteLine("Livro não encontrado.");
                return;
            }

            context.Books.Remove(book);
            context.SaveChanges();
            Console.WriteLine($"Livro \"{book.Title}\" excluído com sucesso!");
        }
    }

    private static void GenreRegistration()
    {
        Console.Clear();
        Console.WriteLine("Registro de Gênero");
        Console.Write("Digite o nome do gênero: ");
        string genreName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(genreName))
        {
            Console.WriteLine("Nome de gênero inválido.");
            return;
        }

        using (var context = new LibraryManagerContext())
        {
            var existingGenre = context.Genres
                .FirstOrDefault(g => g.Name.ToLower() == genreName.ToLower());

            if (existingGenre != null)
            {
                Console.WriteLine($"O gênero \"{genreName}\" já está registrado.");
                return;
            }

            var newGenre = new Genre { Name = genreName };
            context.Genres.Add(newGenre);
            context.SaveChanges();

            Console.WriteLine($"Gênero \"{genreName}\" registrado com sucesso!");
        }
    }
    private static void GenreGet()
    {
        Console.Clear();
        Console.WriteLine("Mostrar livros por gênero");
        Console.Write("Digite o nome do gênero: ");
        string genreName = Console.ReadLine();

        var booksOfGener = BookDAL.ReadAll()
            .Where(b => b.Genre != null && b.Genre.Name.Equals(genreName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (booksOfGener.Count == 0)
        {
            Console.WriteLine($"Nenhum livro encontrado com o gênero \"{genreName}\".");
        }
        else
        {
            Console.WriteLine($"\nLivros do gênero \"{genreName}\":");
            foreach (var book in booksOfGener)
            {
                Console.WriteLine($"- {book.Title} (Autor: {book.Author})");
            }
        }
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

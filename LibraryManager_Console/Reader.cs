using System;
using System.Collections.Generic;

namespace LibraryManager_Console
{
    internal class Reader
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Book> Books { get; set; } = new();

        public Reader(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public void AddBook(Book book)
        {
            if (!Books.Contains(book))
            {
                Books.Add(book);
                book.AddReader(this); // Mantém a relação nos dois lados
            }
        }

        public override string ToString()
        {
            return $"Leitor: {Name}, Email: {Email}, Livros lidos: {Books.Count}";
        }

        public void ShowBooks()
        {
            Console.WriteLine($"Livros lidos por {Name}:");
            foreach (var book in Books)
            {
                Console.WriteLine($"- {book.Title}");
            }
        }
    }
}

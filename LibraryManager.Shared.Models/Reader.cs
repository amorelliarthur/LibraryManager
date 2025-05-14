using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager_Console
{
    public class Reader
    {
        [Key]
        public int idReader { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();

        public Reader() { }

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

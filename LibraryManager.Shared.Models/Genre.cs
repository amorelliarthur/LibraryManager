using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager_Console
{
    public class Genre
    {
        [Key]
        public int idGenre { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();

        public Genre() { }
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
}

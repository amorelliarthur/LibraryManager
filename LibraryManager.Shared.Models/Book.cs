using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager_Console
{
    public class Book
    {
        [Key]
        public int idBook { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public int? GenreId { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual ICollection<Reader> Readers { get; set; } = new List<Reader>();
        public int? PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }

        public Book() { }

        public Book(string title, string author, Genre genre = null, Publisher publisher = null)
        {
            Title = title;
            Author = author;
            Genre = genre;
            Publisher = publisher;
        }

        public void AddReader(Reader reader)
        {
            if (!Readers.Contains(reader))
            {
                Readers.Add(reader);
            }
        }

        public override string ToString()
        {
            return $"Livro: {Title}, Autor: {Author}, Gênero: {Genre?.Name}, Leitores: {Readers.Count}";
        }

        public void ShowReaders()
        {
            Console.WriteLine($"Leitores do livro \"{Title}\":");
            foreach (var r in Readers)
            {
                Console.WriteLine($"- {r.Name} ({r.Email})");
            }
        }
    }

}

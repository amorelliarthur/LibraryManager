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

        // Propriedade de navegação virtual
        public virtual Genre Genre { get; set; }

        public virtual ICollection<Reader> Readers { get; set; } = new List<Reader>();

        public Book() { }

        public Book(string title, string author, Genre genre = null)
        {
            Title = title;
            Author = author;
            Genre = genre;
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

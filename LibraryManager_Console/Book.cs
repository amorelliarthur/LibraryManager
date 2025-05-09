using System;
using System.Collections.Generic;

namespace LibraryManager_Console
{
    internal class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public Genre Genre { get; set; }

        public List<Reader> Readers { get; set; } = new();

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager_Console
{
    public class Publisher
    {
        [Key]
        public int idPublisher { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();

        public Publisher() { }

        public Publisher(string name)
        {
            Name = name;
        }

        public override string ToString() => $"Editora: {Name} (Livros: {Books.Count})";
    }
}

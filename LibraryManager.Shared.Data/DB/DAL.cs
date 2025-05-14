using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManager.Shared.Data.DB;
using LibraryManager_Console;
using Microsoft.EntityFrameworkCore;


namespace LibraryManager.Data.BD
{
    public class DAL<T> where T : class
    {
        private readonly LibraryManagerContext context;

        public DAL()
        {
            context = new LibraryManagerContext();
        }
        public void Create(T value)
        {
            context.Set<T>().Add(value);
            context.SaveChanges();
        }
        public IEnumerable<T> Read()
        {
            return context.Set<T>().ToList();
        }
        public void Update(T value)
        {
            context.Set<T>().Update(value);
            context.SaveChanges();
        }
        public void Delete(T value)
        {
            context.Set<T>().Remove(value);
            context.SaveChanges();
        }
        public T? ReadBy(Func<T, bool> predicate)
        {
            return context.Set<T>().FirstOrDefault(predicate);
        }

        public List<T> ReadAll()
        {
            using var context = new LibraryManagerContext();

            if (typeof(T) == typeof(Book))
            {
                return context.Set<Book>()
                    .Include(b => b.Genre)
                    .Cast<T>()
                    .ToList();
            }

            return context.Set<T>().ToList();
        }


    }
}
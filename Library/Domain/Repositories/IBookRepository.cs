using Library.Domain.Entities;
using System.Collections.Generic;

namespace Library.Domain.Repositories
{
    internal interface IBookRepository
    {
        void Create(Book book);
        void Update(Book book);
        void Add(string isbn);
        void Delete(string isbn);
        int GetAmount(string isbn);
        Book Get(string isnb);
        IEnumerable<Book> GetAll();
    }
}

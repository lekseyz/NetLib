using Library.Domain.Dtos;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Abstraction
{
    internal interface ILibraryService
    {
        IEnumerable<Book> GetBooks();
        Book Add(string isbn, string author, string title, string description);
        Book Add(string isbn);
        void Delete(string isbn);
        Book Get(string isbn);
        Book Change(string isbn, string description);
        void BorrowBook(string isbn, Guid userId);
        void ReturnBook(string isbn, Guid userId);
    }
}

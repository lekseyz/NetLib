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
        IEnumerable<BookInventoryInfo> GetBooks();
        Book Add(string isbn, string author, string title, string description);
        void Delete(string isbn);
        Book Get(string isbn);
        Book Change(string isbn);
        void BorrowBook(string isbn, Guid userId);
        void ReturnBook(string isbn, Guid userId);
    }
}

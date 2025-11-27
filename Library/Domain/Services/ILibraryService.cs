using Library.Domain.Dtos;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Services
{
    public interface ILibraryService
    {
        IEnumerable<Book> GetBooks();
        IEnumerable<IssueDto> GetIssue(Guid userId);

        IEnumerable<IssueDto> GetIssue(string isbn);
        Book Add(string isbn, string author, string title, string description);
        Book Add(string isbn);
        void Delete(string isbn);
        Book Get(string isbn);
        Book Change(string isbn, string description);
        void BorrowBook(string isbn, Guid userId);
        void ReturnBook(string isbn, Guid userId);
    }
}

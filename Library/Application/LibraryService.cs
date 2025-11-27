using Library.Application.Abstraction;
using Library.Domain.Dtos;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Application
{
    public class LibraryService : ILibraryService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IIssuanceRepository _issuanceRepository;

        public LibraryService(IBookRepository bookRepository, 
                              IClientRepository clientRepository, 
                              IIssuanceRepository issuanceRepository)
        {
            _bookRepository = bookRepository;
            _clientRepository = clientRepository;
            _issuanceRepository = issuanceRepository;
        }

        Book ILibraryService.Add(string isbn, string author, string title, string description)
        {
            Book book = Book.CreateBook(isbn, title, author, description);
            _bookRepository.Create(book);
            return book;
        }

        Book ILibraryService.Add(string isbn)
        {
            var book = _bookRepository.Get(isbn);

            if (book == null)
            {
                throw new KeyNotFoundException($"There is no other books with Isbn = {isbn}");
            }
            book.Add();
            _bookRepository.Update(book);
            return book;
        }

        void ILibraryService.BorrowBook(string isbn, Guid userId)
        {
            var book = _bookRepository.Get(isbn);
            if (book == null)
            {
                throw new KeyNotFoundException(isbn);
            }
            if (book.Amount <= 0)
            {
                throw new InvalidOperationException("cant borrow book");
            }

            var client = _clientRepository.Get(userId);
            if (client == null)
            {
                throw new KeyNotFoundException(userId.ToString());
            }

            book.Get();
            _bookRepository.Update(book);
            _issuanceRepository.Issue(isbn, userId);
        }

        Book ILibraryService.Change(string isbn, string description)
        {
            var book = _bookRepository.Get(isbn);
            if (book == null)
            {
                throw new KeyNotFoundException(isbn);
            }

            book.ChangeDescription(description);
            _bookRepository.Update(book);
            return book;
        }

        void ILibraryService.Delete(string isbn)
        {
            var book = _bookRepository.Get(isbn);
            if (book == null)
            {
                throw new KeyNotFoundException(isbn);
            }

            book.Get();
            _bookRepository.Update(book);
        }

        Book ILibraryService.Get(string isbn)
        {
            var book = _bookRepository.Get(isbn);
            if (book == null)
            {
                throw new KeyNotFoundException(isbn);
            }

            return book;
        }

        IEnumerable<Book> ILibraryService.GetBooks()
        {
            return _bookRepository.GetAll();
        }

        void ILibraryService.ReturnBook(string isbn, Guid userId)
        {
            var book = _bookRepository.Get(isbn);
            if (book == null)
            {
                throw new KeyNotFoundException(isbn);
            }

            book.Add();
            _bookRepository.Update(book);
            _issuanceRepository.Return(isbn, userId);
        }
    }
}
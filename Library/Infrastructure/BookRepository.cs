using Dapper;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Misc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Infrastructure
{
    public class BookRepository : IBookRepository
    {
        public void Add(string isbn)
        {
            var sql = @"
UPDATE Books 
SET Amount = Amount+1
WHERE Isbn = @Isbn;";

            using (var connection = DataBase.CreateConnection())
            {
                var affected = connection.Execute(sql, new { Isbn = isbn });
                if (affected == 0)
                {
                    throw new InvalidOperationException($"Not found book with Isbn = {isbn}");
                }
            }
        }

        public void Create(Book book)
        {
            var sql = @"
INSERT INTO Books (Isbn, Title, Author, Description, Amount)
VALUES (@Isbn, @Title, @Author, @Description, @Amount);";

            using (var connection = DataBase.CreateConnection())
            {
                connection.Execute(sql, ToEntity(book));
            }
        }

        public void Delete(string isbn)
        {
            var sql = @"
UPDATE Books
SET Amount = Amount - 1
WHERE Isbn = @Isbn;";

            using (var connection = DataBase.CreateConnection())
            {
                connection.Execute(sql, new { Isbn = isbn });
            }
        }

        public Book Get(string isbn)
        {
            var sql = @"
SELECT Isbn, Title, Author, Description, Amount
FROM Books
WHERE Isbn = @Isbn;";

            using (var connection = DataBase.CreateConnection())
            {
                var entity = connection.QueryFirstOrDefault<BookEntity>(sql, new { Isbn = isbn });
                return ToDomain(entity);
            }
        }

        public IEnumerable<Book> GetAll()
        {
            var sql = @"
SELECT Isbn, Title, Author, Description, Amount
FROM Books;";

            using (var connection = DataBase.CreateConnection())
            {
                var entity = connection.Query<BookEntity>(sql);
                return entity.Select(ToDomain);
            }
        }

        public int GetAmount(string isbn)
        {
            var sql = @"
SELECT Isbn, Title, Author, Description, Amount
FROM Books
WHERE Isbn = @Isbn;";

            using (var connection = DataBase.CreateConnection())
            {
                var entity = connection.QueryFirst<BookEntity>(sql, new { Isbn = isbn });
                return entity.Amount;
            }
        }

        public void Update(Book book)
        {
            var sql = @"
UPDATE Books
SET Amount = @Amount,
    Title = @Title,
    Author = @Author,
    Description = @Description
WHERE Isbn = @Isbn;";

            using (var connection = DataBase.CreateConnection())
            {
                var affected = connection.Execute(sql, ToEntity(book));
                if (affected == 0)
                {
                    throw new InvalidOperationException($"Not found book with Isnb = {book.Isbn}");
                }
            }
        }

        private BookEntity ToEntity(Book book)
        {
            return new BookEntity
            {
                Isbn = book.Isbn,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Amount = book.Amount
            };
        }

        private Book ToDomain(BookEntity entity)
        {
            return new Book(entity.Isbn, entity.Title, entity.Author, entity.Description, entity.Amount);
        }
    }
}
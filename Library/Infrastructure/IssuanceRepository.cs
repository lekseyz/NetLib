using Dapper;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Infrastructure
{
    public class IssuanceRepository : IIssuanceRepository
    {
        public IEnumerable<IssueDto> GetAllIssues(string isbn)
        {
            var sql = @"
SELECT 
    i.Id, i.UserId, i.Isbn, i.IssueDate, i.DueDate, i.ReturnDate,
    c.Id, c.Name, c.PassportId, c.RegistrationDate,
    b.Isbn, b.Title, b.Author, b.Description, b.Amount
FROM Issues i
JOIN Clients c
    ON c.Id = i.UserId
JOIN Books b
    ON b.Isbn = i.Isbn
WHERE i.Isbn = @Isbn";

            using (var connection = DataBase.CreateConnection())
            {
                return connection.Query<IssuesEntity, ClientEntity, BookEntity, IssueDto>(sql,
                    (issue, client, book) =>
                    {
                        return new IssueDto()
                        {
                            Client = ToDomain(client),
                            Book = ToDomain(book),
                            DueDate = issue.DueDate,
                            IssueDate = issue.IssueDate,
                            ReturnDate = issue.ReturnDate
                        };
                    },
                    new { Isbn = isbn },
                    splitOn: "Id,Isbn");
            }
        }

        public IEnumerable<IssueDto> GetAllIssues(Guid userId)
        {
            var sql = @"
SELECT 
    i.Id, i.UserId, i.Isbn, i.IssueDate, i.DueDate, i.ReturnDate,
    c.Id, c.Name, c.PassportId, c.RegistrationDate,
    b.Isbn, b.Title, b.Author, b.Description, b.Amount
FROM Issues i
JOIN Clients c
    ON c.Id = i.UserId
JOIN Books b
    ON b.Isbn = i.Isbn
WHERE i.UserId = @UserId";

            using (var connection = DataBase.CreateConnection())
            {
                return connection.Query<IssuesEntity, ClientEntity, BookEntity, IssueDto>(sql,
                    (issue, client, book) =>
                    {
                        return new IssueDto()
                        {
                            Client = ToDomain(client),
                            Book = ToDomain(book),
                            DueDate = issue.DueDate,
                            IssueDate = issue.IssueDate,
                            ReturnDate = issue.ReturnDate
                        };
                    },
                    new { UserId = userId.ToString() },
                    splitOn: "Id,Isbn");
            }
        }

        public void Issue(string isbn, Guid userId)
        {
            var sql = @"
INSERT INTO Issues (Id, UserId, Isbn, IssueDate, DueDate, ReturnDate)
VALUES (@Id, @UserId, @Isbn, @IssueDate, @DueDate, NULL);";

            using (var connection = DataBase.CreateConnection())
            {
                connection.Execute(sql, new {Id = Guid.NewGuid().ToString(), UserId = userId.ToString(), Isbn = isbn, IssueDate = DateTime.Today,  DueDate = DateTime.Today.AddMonths(6)});
            }
        }

        public void Return(string isbn, Guid userId)
        {
            var sql0 = @"
SELECT Id
FROM Issues
WHERE UserId = @UserId AND Isbn = @Isbn;";
            var sql = @"
UPDATE Issues
SET ReturnDate = @ReturnDate
WHERE Id = @Id AND ReturnDate IS NULL;";

            using (var connection = DataBase.CreateConnection())
            {
                var id = connection.QueryFirst<string>(sql0, new { UserId = userId.ToString(), Isbn = isbn });
                connection.Execute(sql, new { Id = id, ReturnDate = DateTime.Today });
            }
        }

        private static ClientEntity ToEntity(Client client)
        {
            return new ClientEntity
            {
                Id = client.Id.ToString(),
                Name = client.Name,
                PassportId = client.PassportId,
                RegistrationDate = client.RegistrationDate
            };
        }

        private static Client ToDomain(ClientEntity entity)
        {
            return new Client(Guid.Parse(entity.Id), entity.Name, entity.PassportId, entity.RegistrationDate);
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
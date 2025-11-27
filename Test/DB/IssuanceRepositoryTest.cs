using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Infrastructure;
using Test.DB;

namespace Test.DB
{
    [TestFixture]
    public class IssuanceRepositoryTests : DatabaseTestBase
    {
        private IBookRepository CreateBookRepository()
            => new BookRepository();

        private IClientRepository CreateClientRepository()
            => new ClientRepository();

        private IIssuanceRepository CreateIssuanceRepository()
            => new IssuanceRepository();


        [Test]
        public void Return_SetsReturnDateAndIncrementsBookAmount()
        {
            var bookRepo = CreateBookRepository();
            var clientRepo = CreateClientRepository();
            var issuanceRepo = CreateIssuanceRepository();

            var book = Book.CreateBook("ISSUE-TEST-2", "Title", "Author", "Desc");
            bookRepo.Create(book);

            var client = Client.RegisterClient("Ivan", "1234");
            clientRepo.Add(client);

            issuanceRepo.Issue(book.Isbn, client.Id);

            issuanceRepo.Return(book.Isbn, client.Id);

            var amountAfterReturn = bookRepo.GetAmount(book.Isbn);
            Assert.That(amountAfterReturn, Is.EqualTo(1));

            var issuesByUser = issuanceRepo.GetAllIssues(client.Id).ToList();
            Assert.That(issuesByUser.Count, Is.EqualTo(1));

            var issue = issuesByUser.Single();
            Assert.That(issue.ReturnDate, Is.Not.Null);
            Assert.That(issue.ReturnDate!.Value.Date, Is.LessThanOrEqualTo(DateTime.Now.Date));
        }

        [Test]
        public void GetAllIssues_ByUserAndByIsbn_ReturnSameData()
        {
            var bookRepo = CreateBookRepository();
            var clientRepo = CreateClientRepository();
            var issuanceRepo = CreateIssuanceRepository();

            var book = Book.CreateBook("ISSUE-TEST-3", "Title", "Author", "Desc");
            bookRepo.Create(book);

            var client = Client.RegisterClient("Ivan", "1234");
            clientRepo.Add(client);

            issuanceRepo.Issue(book.Isbn, client.Id);

            var byIsbn = issuanceRepo.GetAllIssues(book.Isbn).ToList();
            var byUser = issuanceRepo.GetAllIssues(client.Id).ToList();

            Assert.That(byIsbn.Count, Is.EqualTo(1));
            Assert.That(byUser.Count, Is.EqualTo(1));

            var i1 = byIsbn.Single();
            var i2 = byUser.Single();

            Assert.That(i1.IssueDate, Is.EqualTo(i2.IssueDate));
            Assert.That(i1.Book.Isbn, Is.EqualTo(i2.Book.Isbn));
            Assert.That(i1.Client.Id, Is.EqualTo(i2.Client.Id));
        }
    }
}

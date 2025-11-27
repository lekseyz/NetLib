using Library.Domain.Entities;
using Library.Infrastructure;

namespace Test.DB
{
    [TestFixture]
    public class BookRepositoryTests : DatabaseTestBase
    {
        private BookRepository CreateRepository()
        {
            return new BookRepository();
        }

        [Test]
        public void Create_Then_Get_ReturnsSameBook()
        {
            var repo = CreateRepository();
            var book = Book.CreateBook("111-222-333", "Test Title", "Test Author", "Desc");


            repo.Create(book);
            var fromDb = repo.Get(book.Isbn);


            Assert.That(fromDb, Is.Not.Null);
            Assert.That(fromDb!.Isbn, Is.EqualTo(book.Isbn));
            Assert.That(fromDb.Title, Is.EqualTo(book.Title));
            Assert.That(fromDb.Author, Is.EqualTo(book.Author));
            Assert.That(fromDb.Description, Is.EqualTo(book.Description));
            Assert.That(fromDb.Amount, Is.EqualTo(book.Amount));
        }

        [Test]
        public void Add_IncrementsAmount()
        {
            var repo = CreateRepository();
            var book = Book.CreateBook("111-222-444", "Test Title", "Test Author", "Desc");
            repo.Create(book);


            repo.Add(book.Isbn);


            var amount = repo.GetAmount(book.Isbn);
            Assert.That(amount, Is.EqualTo(2));
        }

        [Test]
        public void Update_ChangesFields()
        {
            var repo = CreateRepository();
            var book = Book.CreateBook("111-222-555", "Old Title", "Old Author", "Old Desc");
            repo.Create(book);

            var updated = new Book(book.Isbn, "New Title", "New Author", "New Desc", 5);

            repo.Update(updated);
            var fromDb = repo.Get(book.Isbn);

            Assert.That(fromDb, Is.Not.Null);
            Assert.That(fromDb!.Title, Is.EqualTo("New Title"));
            Assert.That(fromDb.Author, Is.EqualTo("New Author"));
            Assert.That(fromDb.Description, Is.EqualTo("New Desc"));
            Assert.That(fromDb.Amount, Is.EqualTo(5));
        }

        [Test]
        public void Delete_RemovesBook()
        {
            var repo = CreateRepository();
            var book = Book.CreateBook("111-222-666", "Title", "Author", "Desc");
            repo.Create(book);

            repo.Delete(book.Isbn);
            var fromDb = repo.Get(book.Isbn);

            Assert.That(fromDb.Amount, Is.EqualTo(0));
        }

        [Test]
        public void GetAll_ReturnsAllBooks()
        {
            var repo = CreateRepository();
            repo.Create(Book.CreateBook("111-222-777", "T1", "A1", "D1"));
            repo.Create(Book.CreateBook("111-222-888", "T2", "A2", "D2"));

            var books = repo.GetAll().ToList();

            Assert.That(books.Count, Is.EqualTo(2));
            Assert.That(books.Select(b => b.Isbn),
                Is.EquivalentTo(new[] { "111-222-777", "111-222-888" }));
        }
    }
}

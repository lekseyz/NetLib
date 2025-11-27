using System;
using System.Linq;
using System.Web.Mvc;
using Library.Domain.Services;
using Library.Models.Books;


namespace Library.Controllers
{
    public class BookController : Controller
    {
        private readonly ILibraryService _libraryService;

        public BookController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        // GET: /Book
        [HttpGet]
        public ActionResult Index()
        {
            var books = _libraryService.GetBooks()
                .Select(b => new BookItemViewModel
                {
                    Isbn = b.Isbn,
                    Title = b.Title,
                    Author = b.Author,
                    Amount = b.Amount
                })
                .ToList();

            var model = new BookIndexViewModel
            {
                NewBook = new BookCreateViewModel(),
                Books = books,
                IsFormOpen = false
            };

            return View(model);
        }

        // POST: /Book  (создание книги на той же странице)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(BookIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // при ошибках — заново подтянуть список и открыть форму
                var books = _libraryService.GetBooks()
                    .Select(b => new BookItemViewModel
                    {
                        Isbn = b.Isbn,
                        Title = b.Title,
                        Author = b.Author,
                        Amount = b.Amount
                    })
                    .ToList();

                model.Books = books;
                model.IsFormOpen = true;

                return View(model);
            }

            // создание новой книги
            _libraryService.Add(
                model.NewBook.Isbn,
                model.NewBook.Author,
                model.NewBook.Title,
                model.NewBook.Description
            );

            return RedirectToAction("Index");
        }

        // GET: /Book/Details?isbn=...
        [HttpGet]
        public ActionResult Details(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return HttpNotFound();

            var book = _libraryService.Get(isbn);
            if (book == null)
                return HttpNotFound();

            // История по книге
            var issues = _libraryService.GetIssue(isbn);

            // Полагаюсь на то, что IssueDto содержит Client и даты (как в твоём репозитории)
            var history = issues.Select(i => new BookBorrowNoteModel
            {
                ClientId = i.Client.Id,
                ClientName = i.Client.Name,
                ClientPassportId = i.Client.PassportId,
                IssueDate = i.IssueDate,
                DueDate = i.DueDate,
                ReturnDate = i.ReturnDate
            }).ToList();

            var model = new BookDetailsViewModel
            {
                Isbn = book.Isbn,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Amount = book.Amount,
                History = history
            };

            return View(model);
        }
    }
}


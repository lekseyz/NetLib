using System.Collections.Generic;
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

        [HttpGet]
        public ActionResult Index()
        {
            var model = new BookIndexViewModel
            {
                NewBook = new BookCreateViewModel(),
                Books = GetBookItems(),
                IsFormOpen = false
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(BookIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Books = GetBookItems();
                model.IsFormOpen = true;

                return View(model);
            }

            _libraryService.Add(
                model.NewBook.Isbn,
                model.NewBook.Author,
                model.NewBook.Title,
                model.NewBook.Description
            );

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return HttpNotFound();

            var book = _libraryService.Get(isbn);
            if (book == null)
                return HttpNotFound();

            var model = new BookDetailsViewModel
            {
                Isbn = book.Isbn,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Amount = book.Amount,
                History = GetBookHistory(isbn),
                IsEditMode = false
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var book = _libraryService.Get(model.Isbn);
                if (book == null)
                    return HttpNotFound();

                model.Title = book.Title;
                model.Author = book.Author;
                model.Amount = book.Amount;
                model.History = GetBookHistory(model.Isbn);
                model.IsEditMode = true;

                return View("Details", model);
            }

            _libraryService.Change(model.Isbn, model.Description);

            return RedirectToAction("Details", new { isbn = model.Isbn });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IncreaseAmount(string isbn)
        {
            _libraryService.Add(isbn);

            return RedirectToAction("Details", new { isbn });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DecreaseAmount(string isbn)
        {
            if (_libraryService.Get(isbn).Amount <= 0)
                return RedirectToAction("Details", new { isbn });

            _libraryService.Delete(isbn);

            return RedirectToAction("Details", new { isbn });
        }

        // ----------------- хэлперы -----------------

        private List<BookItemViewModel> GetBookItems()
        {
            return _libraryService.GetBooks()
                .Select(b => new BookItemViewModel
                {
                    Isbn = b.Isbn,
                    Title = b.Title,
                    Author = b.Author,
                    Amount = b.Amount
                })
                .ToList();
        }

        private List<BookBorrowNoteModel> GetBookHistory(string isbn)
        {
            var issues = _libraryService.GetIssue(isbn);

            return issues
                .Select(i => new BookBorrowNoteModel
                {
                    ClientId = i.Client.Id,
                    ClientName = i.Client.Name,
                    ClientPassportId = i.Client.PassportId,
                    IssueDate = i.IssueDate,
                    DueDate = i.DueDate,
                    ReturnDate = i.ReturnDate
                })
                .ToList();
        }
    }
}

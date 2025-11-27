using Library.Domain.Dtos;
using Library.Domain.Entities;
using Library.Domain.Services;
using Library.Models.Books;
using Library.Models.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ILibraryService _libraryService;


        public ClientController(IClientService clientService, ILibraryService libraryService)
        {
            _clientService = clientService;
            _libraryService = libraryService;
        }

        private ClientDetailsViewModel BuildClientDetailsModel(Client client)
        {
            var history = _libraryService.GetIssue(client.Id);

            var notes = history.Select(h => new ClientRegistryNoteModel
            {
                Book = new BookItemModel
                {
                    Isbn = h.Book.Isbn,
                    Title = h.Book.Title
                },
                IssueDate = h.IssueDate,
                DueDate = h.DueDate,
                ReturnDate = h.ReturnDate
            }).ToList();

            var model = new ClientDetailsViewModel
            {
                Id = client.Id,
                Name = client.Name,
                PassportId = client.PassportId,
                RegistrationDate = client.RegistrationDate,
                Notes = notes,
                IsEditMode = false
            };

            return model;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var clients = _clientService.GetAll()
                .Select(c => new ClientItemViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    PassportId = c.PassportId,
                    RegistrationDate = c.RegistrationDate
                })
                .ToList();

            var model = new ClientIndexViewModel
            {
                IsFormOpen = false,
                NewClient = new ClientCreateViewModel(),
                Clients = clients
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ClientIndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var clients = _clientService.GetAll()
                    .Select(c => new ClientItemViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        PassportId = c.PassportId,
                        RegistrationDate = c.RegistrationDate
                    })
                    .ToList();
                model.IsFormOpen = true;
                model.Clients = clients;
                return View(model);
            }

            _clientService.Register(model.NewClient.Name, model.NewClient.PassportId);

            return RedirectToAction("Index");
        }

        public ActionResult Details(Guid id)
        {
            var client = _clientService.Get(id);
            if (client == null)
                return HttpNotFound();

            return View(BuildClientDetailsModel(client));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var client = _clientService.Get(model.Id);
                if (client == null)
                    return HttpNotFound();

                return View("Details", BuildClientDetailsModel(client));
            }

            var changeDto = new ChangeClientDto
            {
                Name = model.Name,
                PassportId = model.PassportId
            };

            _clientService.Change(model.Id, changeDto);

            return RedirectToAction("Details", new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Borrow(Guid clientId, string isbnBorrow)
        {
            if (clientId == Guid.Empty || string.IsNullOrWhiteSpace(isbnBorrow))
            {
                ModelState.AddModelError("", "Нужно указать ISBN.");
            }

            if (!ModelState.IsValid)
            {
                var client = _clientService.Get(clientId);

                var model = BuildClientDetailsModel(client);

                model.IsBorrowFormOpen = true;
                return View("Details", model);
            }

            try
            {
                var book = _libraryService.Get(isbnBorrow);
            }
            catch(KeyNotFoundException)
            {
                ModelState.AddModelError("", "Книга с таким ISBN не найдена.");
                var client = _clientService.Get(clientId);

                var model = BuildClientDetailsModel(client);

                model.IsBorrowFormOpen = true;
                return View("Details", model);
            }

            try
            {
                _libraryService.BorrowBook(isbnBorrow, clientId);
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("", "Нет больше доступных книг.");
                var client = _clientService.Get(clientId);

                var model = BuildClientDetailsModel(client);

                model.IsBorrowFormOpen = true;
                return View("Details", model);
            }

            return RedirectToAction("Details", new { id = clientId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Return(Guid clientId, string isbn)
        {
            if (clientId == Guid.Empty || string.IsNullOrWhiteSpace(isbn))
            {
                return RedirectToAction("Details", new { id = clientId });
            }

            _libraryService.ReturnBook(isbn, clientId);

            return RedirectToAction("Details", new { id = clientId });
        }
    }
}

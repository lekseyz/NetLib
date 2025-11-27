using Library.Domain.Dtos;
using Library.Domain.Services;
using Library.Models.Books;
using Library.Models.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

            var history = _libraryService.GetIssue(id);

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

            return View(model);
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

                var issues = _libraryService.GetIssue(model.Id);
                var notes = issues.Select(i => new ClientRegistryNoteModel
                {
                    Book = new BookItemModel
                    {
                        Isbn = i.Book.Isbn,
                        Title = i.Book.Title
                    },
                    IssueDate = i.IssueDate,
                    DueDate = i.DueDate,
                    ReturnDate = i.ReturnDate
                }).ToList();

                model.RegistrationDate = client.RegistrationDate;
                model.Notes = notes;
                model.IsEditMode = true;

                return View("Details", model);
            }

            var changeDto = new ChangeClientDto
            {
                Name = model.Name,
                PassportId = model.PassportId
            };

            _clientService.Change(model.Id, changeDto);

            return RedirectToAction("Details", new { id = model.Id });
        }
    }
}
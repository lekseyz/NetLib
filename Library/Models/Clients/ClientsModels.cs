using Library.Models.Books;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models.Clients
{
    public class ClientCreateViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Паспортные данные")]
        [StringLength(50)]
        public string PassportId { get; set; }
    }

    public class ClientItemViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Паспортные данные")]
        public string PassportId { get; set; }

        [Display(Name = "Дата регистрации")]
        public DateTime RegistrationDate { get; set; }
    }

    public class ClientIndexViewModel
    {

        public bool IsFormOpen { get; set; }
        public ClientCreateViewModel NewClient { get; set; }
        public IEnumerable<ClientItemViewModel> Clients { get; set; }
    }

    public class ClientRegistryNoteModel
    {
        public BookItemModel Book { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }

    public class ClientDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PassportId { get; set; }
        public DateTime RegistrationDate { get; set; }

        public IEnumerable<ClientRegistryNoteModel> Notes { get; set; }
    }
}
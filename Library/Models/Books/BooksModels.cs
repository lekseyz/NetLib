using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Models.Books
{
    public class BookItemModel
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
    }

    public class BookCreateViewModel
    {
        [Required]
        [Display(Name = "ISBN")]
        public string Isbn { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }
    }

    public class BookItemViewModel
    {
        [Display(Name = "ISBN")]
        public string Isbn { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Количество")]
        public int Amount { get; set; }
    }

    public class BookIndexViewModel
    {
        public BookCreateViewModel NewBook { get; set; }
        public IEnumerable<BookItemViewModel> Books { get; set; }

        public bool IsFormOpen { get; set; }
    }

    public class BookBorrowNoteModel
    {
        public Guid ClientId { get; set; }

        [Display(Name = "Клиент")]
        public string ClientName { get; set; }

        [Display(Name = "Паспорт")]
        public string ClientPassportId { get; set; }

        [Display(Name = "Дата выдачи")]
        public DateTime IssueDate { get; set; }

        [Display(Name = "Срок возврата")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Дата возврата")]
        public DateTime? ReturnDate { get; set; }
    }

    public class BookDetailsViewModel
    {
        [Required]
        public string Isbn { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Автор")]
        public string Author { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Количество")]
        public int Amount { get; set; }
        public IEnumerable<BookBorrowNoteModel> History { get; set; }

        public bool IsEditMode { get; set; }
    }
}
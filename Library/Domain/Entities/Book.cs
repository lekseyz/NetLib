using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Domain.Entities
{
    public class Book
    {
        public string Isbn { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Description { get; private set; }
        public int Amount { get; private set; }

        public Book(string isbn, string title, string author, string description, int amount)
        {
            Isbn = isbn;
            Title = title;
            Author = author;
            Description = description;
            Amount = amount;
        }

        public static Book CreateBook(string isbn, string title, string author, string description)
        {
            // TODO: maybe some checks
            return new Book(isbn, title, author, description, 1);
        }

        public void ChangeDescription(string newDescription)
        {
            Description = newDescription;
        }
        public void ChangeTitle(string newTitle) { Title = newTitle; }
        public void ChangeAuthor(string newAuthor) { Author = newAuthor; }
        public void Add()
        {
            Amount += 1;
        }
        public void Get()
        {
            if (Amount <= 0)
            {
                throw new InvalidOperationException("Cannot get book");
            }

            Amount -= 1;
        }
    }
}
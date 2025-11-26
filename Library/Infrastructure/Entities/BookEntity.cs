using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Infrastructure.Entities
{
    public class BookEntity
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
    }
}
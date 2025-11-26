using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Domain.Dtos
{
    public class BookInventoryInfo
    {
        public Book Book { get; set; }
        public int Amount { get; set; }
    }
}
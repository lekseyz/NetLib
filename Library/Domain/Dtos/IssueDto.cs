using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Domain.Entities
{
    public class IssueDto
    {
        public Client Client { get; set; }
        public Book Book { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
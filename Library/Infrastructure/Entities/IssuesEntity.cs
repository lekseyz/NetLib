using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Infrastructure.Entities
{
    public class IssuesEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Isbn { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
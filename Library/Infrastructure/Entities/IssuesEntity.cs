using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Infrastructure.Entities
{
    public class IssuesEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Isbn { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
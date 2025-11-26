using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Infrastructure.Entities
{
    public class ClientEntity
    {
        public Guid Id { get; set; } // Possibly can be used as bar-code on client's card
        public string Name { get; set; }
        public string PassportId { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
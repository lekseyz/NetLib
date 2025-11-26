using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Domain.Entities
{
    public class Client
    {
        public Guid Id { get; private set; } // Possibly can be used as bar-code on client's card
        public string Name { get; private set; }
        public string PassportId { get; private set; }
        public DateTime RegistrationDate { get; private set; }

        private Client(Guid id, string name, string passportId, DateTime registrationDate)
        {
            Id = id;
            Name = name;
            PassportId = passportId;
            RegistrationDate = registrationDate;
        }

        public static Client RegisterClient(string name, string passportId)
        {
            if (name.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("Client's name cannot be empty"); 
            }
            if (passportId.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("Client's passport data needed");
            }

            return new Client(Guid.NewGuid(), name, passportId, DateTime.Now);
        }
    }
}
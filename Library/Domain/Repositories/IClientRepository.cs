using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Abstraction
{
    public interface IClientRepository
    {
        void Add(Client client);
        void Update(Client client);
        void Delete(Guid id);
        Client Get(Guid id);
        IEnumerable<Client> GetAll();
    }
}

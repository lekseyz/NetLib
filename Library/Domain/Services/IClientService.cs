using Library.Domain.Dtos;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Library.Domain.Services
{
    public interface IClientService
    {
        Client Register(string name, string passwordId);
        Client Get(Guid id);
        IEnumerable<Client> GetAll();
        Client Change(Guid id, ChangeClientDto data);
    }
}

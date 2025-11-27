using Library.Domain.Dtos;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Application
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        Client IClientService.Change(Guid id, ChangeClientDto data)
        {
            var client = _clientRepository.Get(id);

            if (client == null)
            {
                throw new KeyNotFoundException("Cannot change not existing client");
            }

            if (data.PassportId != null)
            {
                client.UpdatePassport(data.PassportId);
            }
            if (data.Name != null)
            {
                client.Rename(data.Name);
            }

            _clientRepository.Update(client);
            return client;
        }

        Client IClientService.Get(Guid id)
        {
            var client = _clientRepository.Get(id);

            if (client == null)
            {
                throw new KeyNotFoundException("Cannot get not existing client");
            }
            return client;
        }

        IEnumerable<Client> IClientService.GetAll()
        {
            return _clientRepository.GetAll();
        }

        Client IClientService.Register(string name, string passportId)
        {
            var newClient = Client.RegisterClient(name, passportId);
            _clientRepository.Add(newClient);
            return newClient;
        }
    }
}
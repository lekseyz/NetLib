using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Dapper;

using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Misc;

namespace Library.Infrastructure
{
    public class ClientRepository : IClientRepository
    {
        public void Add(Client client)
        {
            var entity = ToEntity(client);

            const string sql = @"
INSERT INTO Clients (Id, Name, PassportId, RegistrationDate)
VALUES (@Id, @Name, @PassportId, @RegistrationDate);";

            using (IDbConnection connection = DataBase.CreateConnection())
            {
                connection.Execute(sql, entity);
            }
        }

        public void Update(Client client)
        {
            var entity = ToEntity(client);

            const string sql = @"
UPDATE Clients
SET Name = @Name,
    PassportId = @PassportId,
    RegistrationDate = @RegistrationDate
WHERE Id = @Id;";

            using (IDbConnection connection = DataBase.CreateConnection())
            {
                connection.Execute(sql, entity);
            }
        }

        public void Delete(Guid id)
        {
            const string sql = @"DELETE FROM Clients WHERE Id = @Id;";

            using (IDbConnection connection = DataBase.CreateConnection())
            {
                connection.Execute(sql, new { Id = id });
            }
        }

        public Client Get(Guid id)
        {
            const string sql = @"
SELECT Id, Name, PassportId, RegistrationDate
FROM Clients
WHERE Id = @Id;";

            using (IDbConnection connection = DataBase.CreateConnection())
            {
                var entity = connection
                    .QuerySingleOrDefault<ClientEntity>(sql, new { Id = id.ToString() });

                if (entity == null)
                    return null; // или бросить исключение, если так принято в домене

                return ToDomain(entity);
            }
        }

        public IEnumerable<Client> GetAll()
        {
            const string sql = @"
SELECT Id, Name, PassportId, RegistrationDate
FROM Clients;";

            using (IDbConnection connection = DataBase.CreateConnection())
            {
                var entities = connection.Query<ClientEntity>(sql);
                return entities.Select(ToDomain).ToList();
            }
        }

        private static ClientEntity ToEntity(Client client)
        {
            return new ClientEntity
            {
                Id = client.Id.ToString(),
                Name = client.Name,
                PassportId = client.PassportId,
                RegistrationDate = client.RegistrationDate
            };
        }

        private static Client ToDomain(ClientEntity entity)
        {
            return new Client(Guid.Parse(entity.Id), entity.Name, entity.PassportId, entity.RegistrationDate);
        }
    }
}

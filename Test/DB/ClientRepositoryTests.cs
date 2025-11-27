using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Infrastructure;
using Test.DB;

namespace Test.DB
{
    [TestFixture]
    public class ClientRepositoryTests : DatabaseTestBase
    {
        private IClientRepository CreateRepository()
            => new ClientRepository();

        [Test]
        public void Add_Then_Get_ReturnsSameClient()
        {
            var repo = CreateRepository();
            var client = Client.RegisterClient("Ivan", "1234 567890");

            repo.Add(client);
            var fromDb = repo.Get(client.Id);

            Assert.That(fromDb, Is.Not.Null);
            Assert.That(fromDb!.Id, Is.EqualTo(client.Id));
            Assert.That(fromDb.Name, Is.EqualTo(client.Name));
            Assert.That(fromDb.PassportId, Is.EqualTo(client.PassportId));
            Assert.That(fromDb.RegistrationDate.Date, Is.EqualTo(client.RegistrationDate.Date));
        }

        [Test]
        public void Update_ChangesFields()
        {
            var repo = CreateRepository();
            var client = Client.RegisterClient("Old Name", "1111 111111");
            repo.Add(client);

            var updated = new Client(client.Id, "New Name", "2222 222222", client.RegistrationDate);

            repo.Update(updated);
            var fromDb = repo.Get(client.Id);

            Assert.That(fromDb, Is.Not.Null);
            Assert.That(fromDb!.Name, Is.EqualTo("New Name"));
            Assert.That(fromDb.PassportId, Is.EqualTo("2222 222222"));
        }

        [Test]
        public void Delete_RemovesClient()
        {
            var repo = CreateRepository();
            var client = Client.RegisterClient("Name", "1234 567890");
            repo.Add(client);

            repo.Delete(client.Id);
            var fromDb = repo.Get(client.Id);

            Assert.That(fromDb, Is.Null);
        }

        [Test]
        public void GetAll_ReturnsAllClients()
        {
            var repo = CreateRepository();
            var c1 = Client.RegisterClient("Ivan", "123");
            var c2 = Client.RegisterClient("Petr", "456");

            repo.Add(c1);
            repo.Add(c2);

            var clients = repo.GetAll().ToList();

            Assert.That(clients.Count, Is.EqualTo(2));
            Assert.That(clients.Select(c => c.Id),
                Is.EquivalentTo(new[] { c1.Id, c2.Id }));
        }
    }
}

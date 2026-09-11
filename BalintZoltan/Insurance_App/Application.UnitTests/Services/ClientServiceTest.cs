using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.DTO.Clients;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Xunit;

namespace Application.UnitTests.Services
{
    public class ClientServiceTest
    {
        private class FakeClientRepository : IClientRepository
        {
            public readonly Dictionary<Guid, Client> Storage = new();

            public Task AddAsync(Client client)
            {
                Storage[client.Id] = client;
                return Task.CompletedTask;
            }

            public Task<bool> ExistsByIdentificationNumberAsync(string identificationNumber, Guid? excludedClientId = null)
            {
                var exists = Storage.Values.Any(c => c.IdentificationNumber == identificationNumber && c.Id != excludedClientId);
                return Task.FromResult(exists);
            }

            public Task<Client?> GetByIdAsync(Guid id)
            {
                Storage.TryGetValue(id, out var client);
                return Task.FromResult(client);
            }

            public Task<IReadOnlyCollection<Client>> SearchAsync(string? searchTerm)
            {
                var list = Storage.Values.ToList();
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    list = list.Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || c.IdentificationNumber.Contains(searchTerm)).ToList();
                }

                return Task.FromResult((IReadOnlyCollection<Client>)list);
            }

            public Task UpdateAsync(Client client)
            {
                Storage[client.Id] = client;
                return Task.CompletedTask;
            }
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Individual_With_Valid_CNP()
        {
            var repo = new FakeClientRepository();
            var service = new ClientService(repo);

            var request = new CreateClientRequest
            {
                ClientType = ClientType.Individual,
                Name = "John",
                IdentificationNumber = "1234567890123",
                Email = "a@b.com",
                Phone = "123"
            };

            var dto = await service.CreateAsync(request);

            Assert.NotNull(dto);
            Assert.Equal(request.Name, dto.Name);
            Assert.Equal(request.IdentificationNumber, dto.IdentificationNumber);
            Assert.Single(repo.Storage);
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Company_With_RO_Prefix()
        {
            var repo = new FakeClientRepository();
            var service = new ClientService(repo);

            var request = new CreateClientRequest
            {
                ClientType = ClientType.Company,
                Name = "ACME",
                IdentificationNumber = "RO12345",
                Email = "info@acme.com"
            };

            var dto = await service.CreateAsync(request);

            Assert.NotNull(dto);
            Assert.Equal(request.Name, dto.Name);
            Assert.Equal(request.IdentificationNumber, dto.IdentificationNumber);
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_Identification_Invalid_For_Individual()
        {
            var repo = new FakeClientRepository();
            var service = new ClientService(repo);

            var request = new CreateClientRequest
            {
                ClientType = ClientType.Individual,
                Name = "John",
                IdentificationNumber = "ABC"
            };

            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(request));
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_Duplicate_Identification()
        {
            var repo = new FakeClientRepository();
            var existing = new Client(ClientType.Individual, "Existing", "1234567890123");
            await repo.AddAsync(existing);

            var service = new ClientService(repo);

            var request = new CreateClientRequest
            {
                ClientType = ClientType.Individual,
                Name = "New",
                IdentificationNumber = "1234567890123"
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(request));
            Assert.Equal("A client with this identification number already exists.", ex.Message);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Dto_When_Found()
        {
            var repo = new FakeClientRepository();
            var client = new Client(ClientType.Individual, "John", "1234567890123");
            await repo.AddAsync(client);

            var service = new ClientService(repo);

            var dto = await service.GetByIdAsync(client.Id);

            Assert.NotNull(dto);
            Assert.Equal(client.Id, dto!.Id);
            Assert.Equal(client.Name, dto.Name);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
        {
            var repo = new FakeClientRepository();
            var service = new ClientService(repo);

            var dto = await service.GetByIdAsync(Guid.NewGuid());

            Assert.Null(dto);
        }

        [Fact]
        public async Task SearchAsync_Should_Return_Matching_Clients()
        {
            var repo = new FakeClientRepository();
            var c1 = new Client(ClientType.Individual, "Alice", "1111111111111");
            var c2 = new Client(ClientType.Company, "Acme", "RO22222");
            await repo.AddAsync(c1);
            await repo.AddAsync(c2);

            var service = new ClientService(repo);

            var all = await service.SearchAsync(null);
            Assert.Equal(2, all.Count);

            var filtered = await service.SearchAsync("Acme");
            Assert.Single(filtered);
            Assert.Equal(c2.Id, filtered.First().Id);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_When_Valid()
        {
            var repo = new FakeClientRepository();
            var client = new Client(ClientType.Individual, "John", "1234567890123");
            await repo.AddAsync(client);

            var service = new ClientService(repo);

            var update = new UpdateClientRequest
            {
                ClientType = ClientType.Company,
                Name = "John Updated",
                IdentificationNumber = "1234567890123",
                Email = "new@a.com",
                Phone = "999",
                Address = "Addr"
            };

            var dto = await service.UpdateAsync(client.Id, update);

            Assert.Equal(client.Id, dto.Id);
            Assert.Equal(update.Name, dto.Name);
            Assert.Equal(update.Email, dto.Email);
            Assert.Equal(ClientType.Company, dto.ClientType);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Client_Not_Found()
        {
            var repo = new FakeClientRepository();
            var service = new ClientService(repo);

            var update = new UpdateClientRequest
            {
                ClientType = ClientType.Individual,
                Name = "X",
                IdentificationNumber = "1234567890123"
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(Guid.NewGuid(), update));
            Assert.Equal("Client was not found.", ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Identification_Changed()
        {
            var repo = new FakeClientRepository();
            var client = new Client(ClientType.Individual, "John", "1234567890123");
            await repo.AddAsync(client);

            var service = new ClientService(repo);

            var update = new UpdateClientRequest
            {
                ClientType = ClientType.Individual,
                Name = "John",
                IdentificationNumber = "9999999999999"
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(client.Id, update));
            Assert.Equal("The client identification number cannot be changed.", ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Identification_Exists_For_Other()
        {
            var repo = new FakeClientRepository();
            var client1 = new Client(ClientType.Individual, "A", "1234567890123");
            var client2 = new Client(ClientType.Individual, "B", "9999999999999");
            await repo.AddAsync(client1);
            await repo.AddAsync(client2);

            var service = new ClientService(repo);

            var update = new UpdateClientRequest
            {
                ClientType = ClientType.Individual,
                Name = "A",
                IdentificationNumber = client2.IdentificationNumber // attempt to set to other client's id
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(client1.Id, update));
            Assert.Equal("A client with this identification number already exists.", ex.Message);
        }
    }
}

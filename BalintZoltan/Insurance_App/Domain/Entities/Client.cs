using Domain.Enums;

namespace Domain.Entities
{
    public class Client
    {
        public Guid Id { get; private set; }
        public ClientType Type { get; private set; }
        public string Name { get; private set; }
        public string IdentificationNumber { get; private set; }
        public string? Email { get; private set; }
        public string? Phone { get; private set; }
        public string? Address { get; private set; }
        public Client(
            ClientType type,
            string name,
            string identificationNumber,
            string? email = null,
            string? phone = null,
            string? address = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Client name is required.");

            if (string.IsNullOrWhiteSpace(identificationNumber))
                throw new ArgumentException(
                    "Identification number is required.");

            Id = Guid.NewGuid();
            Type = type;
            Name = name;
            IdentificationNumber = identificationNumber;
            Email = email;
            Phone = phone;
            Address = address;
        }
        public void UpdateContactDetails(
            string? email,
            string? phone,
            string? address)
        {
            Email = email;
            Phone = phone;
            Address = address;
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Client name is required.");

            Name = name;
        }
        public void ChangeType(ClientType type)
        {
            Type = type;
        }

        public void ChangeIdentificationNumber(string identificationNumber)
        {
            if (string.IsNullOrWhiteSpace(identificationNumber))
                throw new ArgumentException(
                    "Identification number is required.");

            IdentificationNumber = identificationNumber;
        }
    }
}

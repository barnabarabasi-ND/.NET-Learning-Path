using Domain.Enums;

namespace Domain.Entities
{
    public class Building
    {
        public Guid Id { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid CityId { get; private set; }
        public string Street { get; private set; }
        public string Number { get; private set; }
        public int ConstructionYear { get; private set; }
        public BuildingType Type { get; private set; }
        public int NumberOfFloors { get; private set; }
        public decimal SurfaceArea { get; private set; }
        public decimal InsuredValue { get; private set; }
        public bool IsFloodRiskZone { get; private set; }
        public bool IsEarthquakeRiskZone { get; private set; }
        public Building(
            Guid clientId,
            Guid cityId,
            string street,
            string number,
            int constructionYear,
            BuildingType type,
            int numberOfFloors,
            decimal surfaceArea,
            decimal insuredValue,
            bool isFloodRiskZone = false,
            bool isEarthquakeRiskZone = false)
        {
            if (clientId == Guid.Empty)
                throw new ArgumentException("Client is required.");

            if (cityId == Guid.Empty)
                throw new ArgumentException("City is required.");

            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street is required.");

            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("Building number is required.");

            if (surfaceArea <= 0)
                throw new ArgumentException(
                    "Surface area must be greater than zero.");

            if (insuredValue <= 0)
                throw new ArgumentException(
                    "Insured value must be greater than zero.");

            if (numberOfFloors < 0)
                throw new ArgumentException(
                    "Number of floors cannot be negative.");

            if (constructionYear < 0)
                throw new ArgumentException(
                    "Number of construction year cannot be negative.");

            if (constructionYear > DateTime.UtcNow.Year)
            {
                throw new ArgumentException(
                    "Construction year cannot be in the future.");
            }

            Id = Guid.NewGuid();
            ClientId = clientId;
            CityId = cityId;
            Street = street;
            Number = number;
            ConstructionYear = constructionYear;
            Type = type;
            NumberOfFloors = numberOfFloors;
            SurfaceArea = surfaceArea;
            InsuredValue = insuredValue;
            IsFloodRiskZone = isFloodRiskZone;
            IsEarthquakeRiskZone = isEarthquakeRiskZone;
        }

        public void UpdateAddress(
            Guid cityId,
            string street,
            string number)
        {
            if (cityId == Guid.Empty)
                throw new ArgumentException("City is required.");

            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street is required.");

            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("Building number is required.");

            CityId = cityId;
            Street = street;
            Number = number;
        }

        public void UpdateDetails(
            int constructionYear,
            BuildingType type,
            int numberOfFloors,
            decimal surfaceArea,
            decimal insuredValue)
        {
            if (surfaceArea <= 0)
                throw new ArgumentException(
                    "Surface area must be greater than zero.");

            if (insuredValue <= 0)
                throw new ArgumentException(
                    "Insured value must be greater than zero.");

            if (numberOfFloors < 0)
                throw new ArgumentException(
                    "Number of floors cannot be negative.");

            ConstructionYear = constructionYear;
            Type = type;
            NumberOfFloors = numberOfFloors;
            SurfaceArea = surfaceArea;
            InsuredValue = insuredValue;
        }

        public void UpdateRiskIndicators(
            bool isFloodRiskZone,
            bool isEarthquakeRiskZone)
        {
            IsFloodRiskZone = isFloodRiskZone;
            IsEarthquakeRiskZone = isEarthquakeRiskZone;
        }
    }
}

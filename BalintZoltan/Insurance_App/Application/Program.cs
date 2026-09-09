using Application.Abstractions;
using Application.DTO.Buildings;
using Application.DTO.Clients;
using Application.DTO.Geography;
using Application.Services;
using Domain.Entities;
using Domain.Enums;

Console.WriteLine("Insurance App - Application local test");
Console.WriteLine(new string('=', 60));

var clientRepository = new InMemoryClientRepository();
var buildingRepository = new InMemoryBuildingRepository();
var geographyRepository = new InMemoryGeographyRepository();

var clientService = new ClientService(clientRepository);
var buildingService = new BuildingService(buildingRepository,clientRepository,geographyRepository);
var geographyService = new GeographyService(geographyRepository);

try
{
    Console.WriteLine("\nGeography:");

    var countries = await geographyService.GetCountriesAsync();
    foreach (var country in countries)
    {
        Console.WriteLine($"Country: {country.Name} ({country.Id})");

        var counties = await geographyService.GetCountiesByCountryIdAsync(
            country.Id);

        foreach (var county in counties)
        {
            Console.WriteLine($"  County: {county.Name} ({county.Id})");

            var cities = await geographyService.GetCitiesByCountyIdAsync(
                county.Id);

            foreach (var city in cities)
            {
                Console.WriteLine(
                    $"    City: {city.Name}, postal code: {city.PostalCode} ({city.Id})");
            }
        }
    }

    var firstCity = (await geographyService.GetCitiesByCountyIdAsync(
        ((await geographyService.GetCountiesByCountryIdAsync(
            countries.First().Id)).First().Id))).First();

    Console.WriteLine("\nCreating client:");

    var client = await clientService.CreateAsync(
        new CreateClientRequest
        {
            ClientType = ClientType.Individual,
            //ClientType = ClientType.Company,
            Name = "Jon Doe",
            //IdentificationNumber = "190212319405",
            IdentificationNumber = "1902123194051",
            Email = "JD@email.com",
            Phone = "+407123431234",
            Address = "Targu Mures"
        });

    PrintClient(client);

    Console.WriteLine("\nCreating building:");

    var building = await buildingService.CreateAsync(
        new CreateBuildingRequest
        {
            ClientId = client.Id,
            //ClientId = Guid.NewGuid(),
            CityId = firstCity.Id,
            //CityId = Guid.NewGuid(),
            Street = "Calea Victoriei",
            Number = "10",
            ConstructionYear = 1985,
            Type = BuildingType.Hotel,
            NumberOfFloors = 5,
            SurfaceArea = 2500m,
            InsuredValue = 3_000_000m,
            IsFloodRiskZone = false,
            IsEarthquakeRiskZone = false
        });

    PrintBuilding(building);

    Console.WriteLine("\nSearching clients:");

    var clients = await clientService.SearchAsync("Jon");

    foreach (var item in clients)
    {
        PrintClient(item);
    }

    Console.WriteLine("\nUpdating client:");

    var updatedClient = await clientService.UpdateAsync(
        client.Id,
        new UpdateClientRequest
        {
            ClientType = ClientType.Company,
            Name = "Jonathan Doe SRL",
            IdentificationNumber = client.IdentificationNumber,
            Email = "office@jonathan-doe.ro",
            Phone = "+40722222222",
            Address = "Bucuresti"
        });

    PrintClient(updatedClient);

    Console.WriteLine("\nUpdating building:");

    var updatedBuilding = await buildingService.UpdateAsync(
        building.Id,
        new UpdateBuildingRequest
        {
            CityId = firstCity.Id,
            Street = "Strada Libertatii",
            Number = "42",
            ConstructionYear = 1990,
            Type = BuildingType.Administrative,
            NumberOfFloors = 6,
            SurfaceArea = 2700m,
            InsuredValue = 3_500_000m,
            IsFloodRiskZone = false,
            IsEarthquakeRiskZone = true
        });

    PrintBuilding(updatedBuilding);
}
catch (Exception exception)
{
    Console.WriteLine($"Unexpected error: {exception.Message}");
}

void PrintClient(ClientDto client)
{
    Console.WriteLine($"Client Id: {client.Id}");
    Console.WriteLine($"Name: {client.Name}");
    Console.WriteLine($"Type: {client.ClientType}");
    Console.WriteLine($"Identification number: {client.IdentificationNumber}");
    Console.WriteLine($"Email: {client.Email ?? "-"}");
    Console.WriteLine($"Phone: {client.Phone ?? "-"}");
    Console.WriteLine($"Address: {client.Address ?? "-"}");
}

void PrintBuilding(BuildingDto building)
{
    Console.WriteLine($"Building Id: {building.Id}");
    Console.WriteLine($"Owner ClientId: {building.ClientId}");
    Console.WriteLine($"CityId: {building.CityId}");
    Console.WriteLine($"Type: {building.Type}");
    Console.WriteLine($"Address: {building.Street} {building.Number}");
    Console.WriteLine($"Construction year: {building.ConstructionYear}");
    Console.WriteLine($"Number of floors: {building.NumberOfFloors}");
    Console.WriteLine($"Surface area: {building.SurfaceArea} m2");
    Console.WriteLine($"Insured value: {building.InsuredValue:N0}");
    Console.WriteLine($"Flood risk zone: {building.IsFloodRiskZone}");
    Console.WriteLine($"Earthquake risk zone: {building.IsEarthquakeRiskZone}");
}

public class InMemoryClientRepository : IClientRepository
{
    private readonly List<Client> _clients = new();

    public Task AddAsync(Client client)
    {
        _clients.Add(client);

        return Task.CompletedTask;
    }

    public Task<bool> ExistsByIdentificationNumberAsync(
        string identificationNumber,
        Guid? excludedClientId = null)
    {
        var exists = _clients.Any(client =>
            client.IdentificationNumber == identificationNumber
            && client.Id != excludedClientId);

        return Task.FromResult(exists);
    }

    public Task<Client?> GetByIdAsync(Guid id)
    {
        var client = _clients.FirstOrDefault(client => client.Id == id);

        return Task.FromResult(client);
    }

    public Task<IReadOnlyCollection<Client>> SearchAsync(string? searchTerm)
    {
        var query = _clients.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(client =>
                client.Name.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase)
                || client.IdentificationNumber.Contains(
                    searchTerm,
                    StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult<IReadOnlyCollection<Client>>(
            query.ToList());
    }

    public Task UpdateAsync(Client client)
    {
        return Task.CompletedTask;
    }
}

public class InMemoryBuildingRepository : IBuildingRepository
{
    private readonly List<Building> _buildings = new();

    public Task AddAsync(Building building)
    {
        _buildings.Add(building);

        return Task.CompletedTask;
    }

    public Task<Building?> GetByIdAsync(Guid id)
    {
        var building = _buildings.FirstOrDefault(building => building.Id == id);

        return Task.FromResult(building);
    }

    public Task<IReadOnlyCollection<Building>> GetByClientIdAsync(Guid clientId)
    {
        var buildings = _buildings
            .Where(building => building.ClientId == clientId)
            .ToList();

        return Task.FromResult<IReadOnlyCollection<Building>>(buildings);
    }

    public Task UpdateAsync(Building building)
    {
        return Task.CompletedTask;
    }
}

public class InMemoryGeographyRepository : IGeographyRepository
{
    private readonly List<Country> _countries = new();
    private readonly List<County> _counties = new();
    private readonly List<City> _cities = new();

    public InMemoryGeographyRepository()
    {
        var country = new Country("Romania");
        var county = new County(country.Id, "Mures");
        var city = new City(county.Id, "Targu Mures", "540000");

        _counties.Add(county);
        _cities.Add(city);

        county = new County(country.Id, "Harghita");
        city = new City(county.Id, "Ododrheiu Secuiesc", "535600");

        _countries.Add(country);
        _counties.Add(county);
        _cities.Add(city);
    }

    public Task<IReadOnlyCollection<Country>> GetCountriesAsync()
    {
        return Task.FromResult<IReadOnlyCollection<Country>>(_countries);
    }

    public Task<IReadOnlyCollection<County>> GetCountiesByCountryIdAsync(
        Guid countryId)
    {
        var counties = _counties
            .Where(county => county.CountryId == countryId)
            .ToList();

        return Task.FromResult<IReadOnlyCollection<County>>(counties);
    }

    public Task<IReadOnlyCollection<City>> GetCitiesByCountyIdAsync(
        Guid countyId)
    {
        var cities = _cities
            .Where(city => city.CountyId == countyId)
            .ToList();

        return Task.FromResult<IReadOnlyCollection<City>>(cities);
    }

    public Task<bool> CityExistsAsync(Guid cityId)
    {
        var exists = _cities.Any(city => city.Id == cityId);

        return Task.FromResult(exists);
    }
}
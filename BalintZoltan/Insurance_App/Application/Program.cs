using Domain.Entities;
using Domain.Enums;
Console.WriteLine("Insurance App - Client / Building local test");
Console.WriteLine(new string('=', 60));

try
{
    var client = new Client(
        ClientType.Individual,
        "Jon Doe",
        "1902123194051",
        "JD@email.com",
        "+407123431234",
        "Targu Mures");

    PrintClient(client);

    var firstCityId = Guid.NewGuid();
    var secondCityId = Guid.NewGuid();

    // A client can own multiple buildings.
    var buildings = new List<Building>
    {
        new Building(
            client.Id,
            firstCityId,
            "Calea Victoriei",
            "10",
            1985,
            BuildingType.Hotel,
            5,
            2500m,
            3_000_000m),

        new Building(
            client.Id,
            secondCityId,
            "Strada Florilor",
            "25A",
            2005,
            BuildingType.Residential,
            2,
            180m,
            450_000m,
            isFloodRiskZone: true)
    };

    Console.WriteLine("\nClient buildings:");

    foreach (var building in buildings)
    {
        PrintBuilding(building);

    }

    Console.WriteLine("\nUpdating client:");

    client.UpdateContactDetails(
        "jon.doe@updated-email.com",
        "+40722222222",
        "Bucurest");

    client.ChangeName("Jonathan Doe");
    client.ChangeType(ClientType.Company);
    client.ChangeIdentificationNumber("RO12345678");

    PrintClient(client);

    Console.WriteLine("\nUpdating building:");

    var buildingToUpdate = buildings[0];

    buildingToUpdate.UpdateAddress(
        secondCityId,
        "Strada Libertatii",
        "42");

    buildingToUpdate.UpdateDetails(
        1990,
        BuildingType.Administrative,
        6,
        2700m,
        3_500_000m);

    buildingToUpdate.UpdateRiskIndicators(
        isFloodRiskZone: false,
        isEarthquakeRiskZone: true);

    PrintBuilding(buildingToUpdate);


}
catch (ArgumentException exception)
{
    Console.WriteLine(
        $"Unexpected validation error: {exception.Message}");
}



void PrintClient(Client client)
{
    Console.WriteLine($"Client Id: {client.Id}");
    Console.WriteLine($"Name: {client.Name}");
    Console.WriteLine($"Type: {client.Type}");
    Console.WriteLine($"Identification number: {client.IdentificationNumber}");
    Console.WriteLine($"Email: {client.Email ?? "-"}");
    Console.WriteLine($"Phone: {client.Phone ?? "-"}");
    Console.WriteLine($"Address: {client.Address ?? "-"}");
}

void PrintBuilding(Building building)
{
    Console.WriteLine($"Building Id: {building.Id}");
    Console.WriteLine($"Owner ClientId: {building.ClientId}");
    Console.WriteLine($"CityId: {building.CityId}");
    Console.WriteLine($"Type: {building.Type}");
    Console.WriteLine($"Address: {building.Street} {building.Number}");
    Console.WriteLine($"Construction year: {building.ConstructionYear}");
    Console.WriteLine($"Number of floors: {building.NumberOfFloors}");
    Console.WriteLine($"Surface area: {building.SurfaceArea} m²");
    Console.WriteLine($"Insured value: {building.InsuredValue:N0}");
    Console.WriteLine($"Flood risk zone: {building.IsFloodRiskZone}");
    Console.WriteLine($"Earthquake risk zone: {building.IsEarthquakeRiskZone}");
}


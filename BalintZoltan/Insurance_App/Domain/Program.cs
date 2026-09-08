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
        "Bucharest");

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
    Console.Write($"Name: {client.Name,-20}");
    Console.Write($"Type: {client.Type,-20}");
    Console.Write($"GUI: {client.IdentificationNumber,-20}");
    Console.WriteLine($"Address: {client.Address,-20}");
}
void PrintBuilding(Building building)
{
    Console.Write($"Owner: {building.ClientId,-20}");
    Console.Write($"Type: {building.Type,-20}");
    Console.Write($"GUI: {building.Id,-20}");
    Console.WriteLine($"Address: {building.Street} {building.Number,-20}");
}

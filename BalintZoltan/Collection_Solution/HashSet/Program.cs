var workerCapabilities = new HashSet<string>(
    /*StringComparer.OrdinalIgnoreCase*/);

bool firstAdd = workerCapabilities.Add("Export");
bool secondAdd = workerCapabilities.Add("export");
bool thirdAdd = workerCapabilities.Add("Cleanup");

Console.WriteLine($"Added 'Export': {firstAdd}");
Console.WriteLine($"Added 'export': {secondAdd}");
Console.WriteLine($"Added 'Cleanup': {thirdAdd}");

Console.WriteLine();
Console.WriteLine("Worker capabilities:");

foreach (string capability in workerCapabilities)
{
    Console.WriteLine(capability);
}

bool canExport = workerCapabilities.Contains("EXPORT");

Console.WriteLine();
Console.WriteLine($"Can export: {canExport}");
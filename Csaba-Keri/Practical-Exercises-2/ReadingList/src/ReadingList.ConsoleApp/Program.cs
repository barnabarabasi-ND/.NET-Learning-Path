using Microsoft.Extensions.DependencyInjection;
using ReadingList.Application.Interfaces;
using ReadingList.Domain.Entities;
using ReadingList.Infrastructure.FileSystem;
using ReadingList.Infrastructure.Importing;
using ReadingList.Infrastructure.Parsing;
using ReadingList.Infrastructure.Repositories;

namespace ReadingList.ConsoleApp;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddSingleton<IRepository<Book, int>>(
            _ => new InMemoryRepository<Book, int>(book => book.Id)
        );

        services.AddSingleton<IFileReader, FileReader>();
        services.AddSingleton<IBookCsvParser, BookCsvParser>();
        services.AddSingleton<IBookImportService, BookImportService>();

        services.AddSingleton<ReadingListConsoleApp>();

        using var serviceProvider = services.BuildServiceProvider();

        var app = serviceProvider.GetRequiredService<ReadingListConsoleApp>();

        using var cancellationTokenSource = new CancellationTokenSource();

        Console.CancelKeyPress += (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cancellationTokenSource.Cancel();
        };

        try
        {
            await app.RunAsync(cancellationTokenSource.Token);
        }
        catch(OperationCanceledException)
        {
            Console.WriteLine();
            Console.WriteLine("Application cancelled.");
        }
    }
}

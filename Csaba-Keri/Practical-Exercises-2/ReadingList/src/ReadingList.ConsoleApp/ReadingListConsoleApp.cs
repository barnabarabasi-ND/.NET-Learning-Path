using ReadingList.Application.Interfaces;
using ReadingList.Application.Models;
using ReadingList.ConsoleApp.Commands;

namespace ReadingList.ConsoleApp;

internal sealed class ReadingListConsoleApp
{
    private const string FilePathsSeparator = "|";

    private readonly IBookImportService _bookImportService;
    private readonly Dictionary<string, ConsoleCommand> _commands;

    public ReadingListConsoleApp(IBookImportService bookImportService)
    {
        ArgumentNullException.ThrowIfNull(bookImportService);

        _bookImportService = bookImportService;
        _commands = CreateCommands();
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        PrintHelp();

        var shouldContinue = true;

        while (shouldContinue &&
            !cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine();

            var commandName = ReadUserInput(prompt: "> ");

            if (commandName is null)
            {
                return;
            }

            if (!_commands.TryGetValue(commandName, out var command))
            {
                Console.WriteLine(
                    $"Unknown command '{commandName}'. Type 'help' to see available commands."
                );

                continue;
            }

            shouldContinue = await command.Handler(cancellationToken);
        }
    }

    private Dictionary<string, ConsoleCommand> CreateCommands()
    {
        return new(StringComparer.OrdinalIgnoreCase)
        {
            ["import"] = new(
                "Import books from one or more CSV files",
                HandleImportAsync
            ),

            ["help"] = new(
                "Show available commands",
                HandleHelpAsync
            ),

            ["exit"] = new(
                "Exit the application",
                HandleExitAsync
            )
        };
    }

    private async Task<bool> HandleImportAsync(CancellationToken cancellationToken)
    {
        var input = ReadUserInput($"Enter CSV file path(s), separated by {FilePathsSeparator}: ");

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("No file path was provided.");

            return true;
        }

        var filePaths = input.Split(
            FilePathsSeparator,
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries
        );

        if (filePaths.Length == 0)
        {
            Console.WriteLine("No valid file path was provided.");

            return true;
        }

        var summary = await _bookImportService.ImportAsync(filePaths, cancellationToken);

        PrintImportSummary(summary);

        return true;
    }

    private Task<bool> HandleHelpAsync(CancellationToken cancellationToken)
    {
        PrintHelp();

        return Task.FromResult(true);
    }

    private static Task<bool> HandleExitAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(false);
    }

    private static string? ReadUserInput(string prompt)
    {
        Console.Write(prompt);

        return Console.ReadLine()?.Trim();
    }

    private void PrintHelp()
    {
        Console.WriteLine("Reading List Application");
        Console.WriteLine();
        Console.WriteLine("Available commands:");
        
        foreach (var command in _commands)
        {
            Console.WriteLine($"\t{command.Key, -10} {command.Value.Description}");
        }
    }

    private static void PrintImportSummary(BookImportSummary summary)
    {
        Console.WriteLine();
        Console.WriteLine("Import completed.");
        Console.WriteLine($"Imported: {summary.ImportedCount}");
        Console.WriteLine($"Duplicates: {summary.DuplicateCount}");
        Console.WriteLine($"Malformed rows: {summary.MalformedCount}");
        Console.WriteLine($"Failed files: {summary.FailedFileCount}");

        if (summary.SkippedDuplicateIds.Count > 0)
        {
            Console.WriteLine(
                $"Skipped duplicate IDs: {string.Join(", ", summary.SkippedDuplicateIds)}"
            );
        }

        if (summary.Warnings.Count == 0)
        {
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Warnings:");

        foreach (var warning in summary.Warnings)
        {
            var location = warning.LineNumber.HasValue
                ? $"line {warning.LineNumber.Value}"
                : "file";

            Console.WriteLine($"- {warning.FilePath} ({location}): {warning.Message}");
        }
    }
}

namespace ReadingList.ConsoleApp.Commands;

internal record ConsoleCommand(
    string Description,
    Func<CancellationToken, Task<bool>> Handler
);

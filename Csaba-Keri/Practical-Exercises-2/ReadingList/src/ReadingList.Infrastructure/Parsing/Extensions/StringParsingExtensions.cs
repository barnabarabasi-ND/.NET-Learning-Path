namespace ReadingList.Infrastructure.Parsing.Extensions;

public static class StringParsingExtensions
{
    public static bool TryParseFinished(this string value, out bool finished)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        switch (value.Trim().ToLowerInvariant())
        {
            case "y" or "yes" or "true":
                finished = true;
                return true;

            case "n" or "no" or "false":
                finished = false;
                return true;

            default:
                finished = default;
                return false;
        }
    }
}

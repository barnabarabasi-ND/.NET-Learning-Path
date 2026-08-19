using ReadingList.Application.Common;
using System.Text;

namespace ReadingList.Infrastructure.Parsing;

public static class CsvLineParser
{
    public static Result<IReadOnlyList<string>> Parse(string line)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(line);

        var fields = new List<string>();
        var currentField = new StringBuilder();
        var insideQuotes = false;

        for (var i = 0; i < line.Length; i++)
        {
            var currentCharacter = line[i];

            if (currentCharacter == '"')
            {
                if (insideQuotes &&
                    i + 1 < line.Length &&
                    line[i + 1] == '"')
                {
                    currentField.Append(currentCharacter);
                    i++;
                    continue;
                }

                insideQuotes = !insideQuotes;
                continue;
            }

            if (currentCharacter == ',' && !insideQuotes)
            {
                fields.Add(currentField.ToString().Trim());
                currentField.Clear();
                continue;
            }

            currentField.Append(currentCharacter);
        }

        if (insideQuotes)
        {
            return Result<IReadOnlyList<string>>.Failure(
                "The CSV row contains an unclosed quoted field."
            );
        }

        fields.Add(currentField.ToString().Trim());

        return Result<IReadOnlyList<string>>.Success(fields);
    }
}

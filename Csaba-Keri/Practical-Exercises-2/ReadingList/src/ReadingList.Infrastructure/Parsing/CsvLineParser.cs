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

        for (var currentCharIdx = 0; currentCharIdx < line.Length; currentCharIdx++)
        {
            var currentChar = line[currentCharIdx];

            if (currentChar == '"')
            {
                if (insideQuotes &&
                    currentCharIdx + 1 < line.Length &&
                    line[currentCharIdx + 1] == '"')
                {
                    currentField.Append(currentChar);
                    currentCharIdx++;
                    continue;
                }

                insideQuotes = !insideQuotes;
                continue;
            }

            if (currentChar == ',' && !insideQuotes)
            {
                fields.Add(currentField.ToString().Trim());
                currentField.Clear();
                continue;
            }

            currentField.Append(currentChar);
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

namespace ReadingList.Tests.Common.TestData;

public static class BookCsvTestData
{
    public const string ValidBookRow =
        "1,\"Clean Code\",\"Robert C. Martin\",2008,464,\"software\",y,5";

    public const string RowWithCommaInTitle =
        "1,\"Clean Code, Second Edition\",\"Robert C. Martin\",2008,464,\"software\",yes,4.5";

    public const string InvalidRatingRow =
        "1,\"Clean Code\",\"Robert C. Martin\",2008,464,\"software\",yes,7";

    public const string MalformedYearRow =
        "1,\"Clean Code\",\"Robert C. Martin\",not-a-year,464,\"software\",yes,5";
}

namespace ReadingList.Domain.Entities;

public class Book
{
    public const decimal MinimumRating = 0m;
    public const decimal MaximumRating = 5m;

    public int Id { get; }
    public string Title { get; }
    public string Author { get; }
    public int Year { get; }
    public int Pages { get; }
    public string Genre { get; }

    public bool Finished { get; private set; }
    public decimal Rating { get; private set; }

    public Book(int id, string title, string author, int year, int pages, string genre, bool finished, decimal rating)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(year, nameof(year));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pages, nameof(pages));

        ArgumentException.ThrowIfNullOrWhiteSpace(title, nameof(title));
        ArgumentException.ThrowIfNullOrWhiteSpace(author, nameof(author));
        ArgumentException.ThrowIfNullOrWhiteSpace(genre, nameof(genre));

        ValidateRating(rating);

        Id = id;
        Title = title.Trim();
        Author = author.Trim();
        Year = year;
        Pages = pages;
        Genre = genre.Trim();
        Finished = finished;
        Rating = rating;
    }

    public void MarkAsFinished()
    {
        Finished = true;
    }

    public void SetRating(decimal rating)
    {
        ValidateRating(rating);

        Rating = rating;
    }

    private static void ValidateRating(decimal rating)
    {
        if (rating is < MinimumRating or > MaximumRating)
        {
            throw new ArgumentOutOfRangeException(
                nameof(rating),
                rating,
                $"Rating must be between {MinimumRating} and {MaximumRating}."
            );
        }
    }
}

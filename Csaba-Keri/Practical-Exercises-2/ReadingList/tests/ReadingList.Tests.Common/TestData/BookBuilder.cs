using ReadingList.Domain.Entities;

namespace ReadingList.Tests.Common.TestData;

public class BookBuilder
{
    private int _id = 1;
    private string _title = "Test Book";
    private string _author = "Test Author";
    private int _year = 2026;
    private int _pages = 100;
    private string _genre = "Test Genre";
    private bool _finished = false;
    private decimal _rating = Book.MinimumRating;

    public BookBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public BookBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public BookBuilder WithAuthor(string author)
    {
        _author = author;
        return this;
    }

    public BookBuilder WithYear(int year)
    {
        _year = year;
        return this;
    }

    public BookBuilder WithPages(int pages)
    {
        _pages = pages;
        return this;
    }

    public BookBuilder WithGenre(string genre)
    {
        _genre = genre;
        return this;
    }

    public BookBuilder WithFinished(bool finished = true)
    {
        _finished = finished;
        return this;
    }

    public BookBuilder WithRating(decimal rating)
    {
        _rating = rating;
        return this;
    }

    public Book Build()
    {
        return new(
            id: _id,
            title: _title,
            author: _author,
            year: _year,
            pages: _pages,
            genre: _genre,
            finished: _finished,
            rating: _rating
        );
    }
}

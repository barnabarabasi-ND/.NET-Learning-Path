namespace InsuranceApp.Application.Geography.Results;

public record CityGeographyResult
{
    public CityResult City { get; }
    public CountyResult County { get; }
    public CountryResult Country { get; }

    public CityGeographyResult(CityResult city, CountyResult county, CountryResult country)
    {
        City = city;
        County = county;
        Country = country;
    }
}

namespace AirBnb.Api.Models.Dtos;

/// <summary>
/// Represents a country in the application.
/// </summary>
public class CountryDto
{
    /// <summary>
    /// Gets city Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the name of the country.
    /// </summary>
    public string Name { get; init; } = default!;

    /// <summary>
    /// Gets the code of the country.
    /// </summary>
    public string Code { get; init; } = default!;

    /// <summary>
    /// Gets the cities in the country.
    /// </summary>
    public ICollection<CityDto>? Cities { get; init; }
}
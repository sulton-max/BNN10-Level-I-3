namespace AirBnb.Api.Models.Dtos;

/// <summary>
/// Represents a city in the application.
/// </summary>
public class CityDto
{
    /// <summary>
    /// Gets city Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets the name of the city.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets country of the city
    /// </summary>
    public CountryDto? Country { get; set; } = default!;
}
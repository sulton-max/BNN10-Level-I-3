namespace AirBnb.Domain.Entities;

public class ListingLocation
{
    public string Country { get; set; } = default!;

    public string City { get; set; } = default!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}
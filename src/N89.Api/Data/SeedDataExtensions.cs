using AirBnb.Domain.Entities;
using AirBnb.Domain.Enums;
using AirBnb.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AirBnb.Api.Data;

/// <summary>
/// Provides extension methods for seeding data.
/// </summary>
public static class SeedDataExtensions
{
    /// <summary>
    /// Seeds the database with data.
    /// </summary>
    /// <param name="serviceProvider">Service provider</param>
    public static async ValueTask SeedDataAsync(this IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();

        if (!await dbContext.Locations.AnyAsync())
            await SeedLocationsAsync(dbContext, webHostEnvironment);
    }

    /// <summary>
    /// Seeds the database with locations.
    /// </summary>
    /// <param name="dbContext">Database context to seed data</param>
    /// <param name="webHostEnvironment">Web application environment</param>
    private static async ValueTask SeedLocationsAsync(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
    {
        var countriesFileName = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "SeedData", "Countries.json");
        var citiesFileName = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "SeedData", "Cities.json");

        // Retrieve countries
        var countries = JsonConvert.DeserializeObject<List<Location>>(await File.ReadAllTextAsync(countriesFileName))!;
        countries.ForEach(country => country.Type = LocationType.Country);

        // Retrieve cities
        var cities = JsonConvert.DeserializeObject<List<Location>>(await File.ReadAllTextAsync(citiesFileName))!;
        cities.ForEach(city => city.Type = LocationType.City);

        // Add countries and cities
        await dbContext.Locations.AddRangeAsync(countries);
        await dbContext.Locations.AddRangeAsync(cities);

        await dbContext.SaveChangesAsync();
    }
}
using AirBnb.Api.Models.Dtos;
using AirBnb.Application.Locations.Models;
using AirBnb.Application.Locations.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AirBnb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController(IMapper mapper, ILocationService locationService) : ControllerBase
{
    [HttpGet("countries")]
    public async Task<IActionResult> GetCountries([FromQuery] CountryFilter cityFilter)
    {
        var result = await locationService.GetAsync(cityFilter.ToQuerySpecification());
        return result.Any() ? Ok(mapper.Map<IEnumerable<CountryDto>>(result)) : NoContent();
    }

    [HttpGet("cities")]
    public async ValueTask<IActionResult> GetCities([FromQuery] CityFilter cityFilter)
    {
        var result = await locationService.GetAsync(cityFilter.ToQuerySpecification());
        return result.Any() ? Ok(mapper.Map<IEnumerable<CityDto>>(result)) : NoContent();
    }
}
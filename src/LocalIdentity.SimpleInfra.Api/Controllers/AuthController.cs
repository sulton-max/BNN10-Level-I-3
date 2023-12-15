using AutoMapper;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Models;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocalIdentity.SimpleInfra.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMapper mapper, IAuthAggregationService authAggregationService) : ControllerBase
{
    [HttpPost("sign-up")]
    public async ValueTask<IActionResult> SignUp([FromBody] SignUpDetails signUpDetails, CancellationToken cancellationToken)
    {
        var result = await authAggregationService.SignUpAsync(signUpDetails, cancellationToken);
        return result ? Ok() : BadRequest();
    }
}
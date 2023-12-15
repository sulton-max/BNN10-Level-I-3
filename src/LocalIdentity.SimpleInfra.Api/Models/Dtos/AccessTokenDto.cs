namespace LocalIdentity.SimpleInfra.Api.Models.Dtos;

public class AccessTokenDto
{
    public string Token { get; set; } = default!;

    public DateTimeOffset ExpiryTime { get; set; }
}
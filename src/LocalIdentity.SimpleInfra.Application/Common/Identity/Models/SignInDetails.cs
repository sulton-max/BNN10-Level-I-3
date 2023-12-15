namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Models;

public class SignInDetails
{
    public string EmailAddress { get; set; } = default!;

    public string Password { get; set; } = default!;
}
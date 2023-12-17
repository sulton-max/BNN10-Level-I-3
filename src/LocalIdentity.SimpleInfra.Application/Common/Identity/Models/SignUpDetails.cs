namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Models;

public class SignUpDetails
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string EmailAddress { get; set; } = default!;

    public int Age { get; set; }

    public string? Password { get; set; }

    public bool AutoGeneratePassword { get; set; }
}
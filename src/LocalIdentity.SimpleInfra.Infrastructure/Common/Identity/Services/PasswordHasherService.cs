using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using BC = BCrypt.Net.BCrypt;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class PasswordHasherService : IPasswordHasherService
{
    public string HashPassword(string password)
    {
        return BC.HashPassword(password);
    }

    public bool ValidatePassword(string password, string hashedPassword)
    {
        return BC.Verify(password, hashedPassword);
    }
}
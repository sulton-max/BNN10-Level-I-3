using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Services;

public interface IPasswordGeneratorService
{
    string GeneratePassword();

    string GetValidatedPassword(string password, User user);
}
namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;

public class ValidationSettings
{
    public string EmailAddressRegexPattern { get; set; } = default!;

    public string NameRegexPattern { get; set; } = default!;

    public string UrlRegexPattern { get; set; } = default!;
}
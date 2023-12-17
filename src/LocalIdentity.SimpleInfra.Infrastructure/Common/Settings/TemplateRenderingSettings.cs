namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;

public class TemplateRenderingSettings
{
    public string PlaceholderRegexPattern { get; set; } = default!;

    public string PlaceholderValueRegexPattern { get; set; } = default!;

    public int RegexMatchTimeoutInSeconds { get; set; }
}
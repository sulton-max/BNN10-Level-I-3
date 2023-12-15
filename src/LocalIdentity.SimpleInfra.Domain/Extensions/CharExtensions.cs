namespace LocalIdentity.SimpleInfra.Domain.Extensions;

public static class CharExtensions
{
    public static char GetRandomDigit(Random? random = null) => (char)(random ?? new Random()).Next('0', '9' + 1);

    public static char GetRandomUppercase(Random? random = null) => (char)(random ?? new Random()).Next('A', 'Z' + 1);

    public static char GetRandomLowercase(Random? random = null) => (char)(random ?? new Random()).Next('a', 'z' + 1);

    public static char GetRandomNonAlphanumeric(Random? random = null) =>
        "!@#$%^&*()_-+=<>?"[(random ?? new Random()).Next(16)];

    public static char GetRandomCharacter(Random? random) => (char)(random ?? new Random()).Next(32, 126);
}
namespace time_abstraction;

public class FakeTimeProvider : TimeProvider
{
    public override DateTimeOffset GetUtcNow()
    {
        return new DateTimeOffset(2023, 11, 27, 6, 0, 0, TimeSpan.Zero);
    }
}
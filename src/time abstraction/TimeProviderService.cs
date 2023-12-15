namespace time_abstraction;

public class TimeProviderService(TimeProvider timeProvider)
{
    public bool IsMorning()
    {
        var now = timeProvider.GetLocalNow();
        return now.Hour < 12;
    }
}
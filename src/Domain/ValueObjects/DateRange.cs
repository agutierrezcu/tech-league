using Domain.DDD;

namespace Domain.ValueObjects;

public class DateRange : ValueObject
{
    public static DateRange From(DateTime start, DateTime end)
        => new() { Start = start, End = end };

    public static DateRange FromTodayTo(DateTime end)
       => From(DateTime.Today, end);

    public required DateTime Start { get; init; }

    public required DateTime End { get; init; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Start;
        yield return End;
    }

    internal bool Contains(DateTime dateTime)
        => dateTime >= Start && dateTime <= End;
}

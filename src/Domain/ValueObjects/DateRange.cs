using Domain.DDD;
using SharedKernel;

namespace Domain.ValueObjects;

public class DateRange : ValueObject
{
    public required DateTime Start { get; init; }

    public required DateTime End { get; init; }

    public static Result<DateRange> Create(DateTime start, DateTime end)
    {
        if (start >= end)
        {
            return Result<DateRange>.ValidationFailure(
                Error.Invalid("InvalidDateRange", "Start date must be earlier than end date."));
        }

        return new DateRange { Start = start, End = end };
    }

    public static Result<DateRange> FromTodayTo(DateTime end)
        => Create(DateTime.Today, end);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Start;
        yield return End;
    }

    internal bool Contains(DateTime dateTime)
        => dateTime >= Start && dateTime <= End;
}

using Domain.DDD;
using SharedKernel;

namespace Domain.ValueObjects;

public class DateRange : ValueObject
{
    public static readonly DateRange None = new()
    {
        Start = DateTime.MinValue,
        End = DateTime.MinValue
    };

    public required DateTime Start { get; init; }

    public required DateTime End { get; init; }

    public static Result<DateRange> Create(DateTime start, DateTime end)
    {
        if (start >= end)
        {
            return Result<DateRange>.ValidationFailure(
                Error.Invalid("InvalidDateRange", "Start date must be earlier than end date."));
        }

        return Result.Success(
            new DateRange
            {
                Start = start,
                End = end
            });
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Start;
        yield return End;
    }

    internal bool Contains(DateTime dateTime)
    {
        return dateTime >= Start && dateTime <= End;
    }
}

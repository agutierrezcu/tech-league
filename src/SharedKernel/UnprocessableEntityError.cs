namespace SharedKernel;

public sealed record UnprocessableEntityError : Error
{
    public UnprocessableEntityError(Error[] errors)
        : base(
            "Invalid Unprocessable Entity",
            "One or more business logic errors occurred",
            ErrorType.UnprocessableEntity)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    public static UnprocessableEntityError FromResults(IEnumerable<Result> results)
    {
        return new([.. results.Where(r => r.IsFailure).Select(r => r.Error)]);
    }
}

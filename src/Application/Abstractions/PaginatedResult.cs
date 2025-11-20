namespace Application.Abstractions;

public record PaginatedResult<TData>
{
    public required TData[] Data { get; init; }

    public required int TotalCount { get; init; }

    public required int PageIndex { get; init; }

    public required int PageSize { get; init; }

    public required int TotalPages { get; init; }

    public bool HasNextPage => PageIndex < TotalPages;

    public bool HasPreviousPage => PageIndex > 1;
}

namespace Application.Abstractions;

public record PaginationInfo<TData>(TData[] Data, int TotalCount, 
    int PageIndex, int PageSize, int TotalPages)
{
    public bool HasNextPage => PageIndex < TotalPages;

    public bool HasPreviousPage => PageIndex > 1;
}

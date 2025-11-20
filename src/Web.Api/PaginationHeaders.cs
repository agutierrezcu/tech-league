using Application.Abstractions;
using static System.Globalization.CultureInfo;

namespace Web.Api;

internal static class PaginationHeaders
{
    public const string TotalCount = "X-Total-Count";

    public const string PageNumber = "X-Page-Number";

    public const string PageSize = "X-Page-Size";

    public const string TotalPages = "X-Total-Pages";

    public const string HasNext = "X-Has-Next-Page";

    public const string HasPrevious = "X-Has-Previous-Page";

    public static void AddPaginationHeaders<TData>(this IHeaderDictionary headers, PaginatedResult<TData> paginationInfo)
    {
        ArgumentNullException.ThrowIfNull(headers);
        ArgumentNullException.ThrowIfNull(paginationInfo);

        headers.Append(TotalCount, paginationInfo.TotalCount.ToString(CurrentCulture));
        headers.Append(PageNumber, paginationInfo.PageIndex.ToString(CurrentCulture));
        headers.Append(PageSize, paginationInfo.PageSize.ToString(CurrentCulture));
        headers.Append(TotalPages, paginationInfo.TotalPages.ToString(CurrentCulture));
        headers.Append(HasNext, paginationInfo.HasNextPage.ToString(CurrentCulture).ToLower(CurrentCulture));
        headers.Append(HasPrevious, paginationInfo.HasPreviousPage.ToString(CurrentCulture).ToLower(CurrentCulture));
    }
}

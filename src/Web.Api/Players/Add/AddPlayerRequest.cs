namespace Web.Api.Players.Add;

internal sealed record AddPlayerRequest(string FullName, string? NickName, DateOnly? BirthDate);

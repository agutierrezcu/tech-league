namespace Application.Clubs.Players.Get;

public sealed record PlayerByClub(PlayerId Id, string FullName, string? NickName, DateOnly? BirthDate);

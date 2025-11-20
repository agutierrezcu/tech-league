using Application.Abstractions.Messaging;

namespace Application.Players.Add;

public sealed record AddPlayerCommand(string FullName, string? NickName, DateOnly? BirthDate)
    : ICommand<PlayerId>;

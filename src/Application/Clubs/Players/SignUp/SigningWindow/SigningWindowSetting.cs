using Domain.ValueObjects;

namespace Application.Clubs.Players.SignUp.SigningWindow;

public sealed class SigningWindowSetting : DateRange
{
    public const string DefaultConfigurationSection = "SigningWindow";
}

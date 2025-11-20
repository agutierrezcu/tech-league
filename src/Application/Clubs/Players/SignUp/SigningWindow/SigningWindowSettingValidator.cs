using Application.Validators;
using FluentValidation;

namespace Application.Clubs.Players.SignUp.SigningWindow;

public class SigningWindowSettingValidator : DateRangeValidator<SigningWindowSetting>
{
    public const int MaximumWindowTimeInDays = 180;

    public SigningWindowSettingValidator()
    {
        RuleFor(s => s)
            .Must(s => (s.End - s.Start).TotalDays <= MaximumWindowTimeInDays)
            .WithMessage($"Signing window can not exceed {MaximumWindowTimeInDays} days");
    }
}
